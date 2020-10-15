// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System;
using System.IO;
using CubismFramework;
using osu.Framework.Extensions.ShaderExtensions;
using osu.Framework.Graphics.OpenGL;
using osu.Framework.Graphics.OpenGL.Buffers;
using osu.Framework.Graphics.OpenGL.Vertices;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Textures;
using osuTK;
using osuTK.Graphics.ES30;

namespace osu.Framework.Graphics.Cubism
{
    public partial class CubismSprite
    {
        protected class CubismShaderManager
        {
            private readonly IShader maskDrawingShader;
            private readonly IShader unmaskedMeshDrawShader;
            private readonly IShader maskedMeshDrawShader;
            private readonly IShader maskedInvertedMeshDrawShader;
            private readonly IShader unmaskedPremultAlphaMeshDrawShader;
            private readonly IShader maskedPremultAlphaMeshDrawShader;
            private readonly IShader maskedInvertedPremultAlphaMeshDrawShader;

            public CubismShaderManager(ShaderManager shaderManager)
            {
                maskDrawingShader = shaderManager.Load("SetupMaskVertex", "SetupMaskFragment");
                unmaskedMeshDrawShader = shaderManager.Load("UnmaskedVertex", "UnmaskedFragment");
                maskedMeshDrawShader = shaderManager.Load("MaskedVertex", "MaskedFragment");
                maskedInvertedMeshDrawShader = shaderManager.Load("MaskedVertex", "MaskedInvertedFragment");
                unmaskedPremultAlphaMeshDrawShader = shaderManager.Load("UnmaskedVertex", "UnmaskedPremultipliedAlphaFragment");
                maskedPremultAlphaMeshDrawShader = shaderManager.Load("MaskedVertex", "MaskedPremultipliedAlphaFragment");
                maskedInvertedPremultAlphaMeshDrawShader = shaderManager.Load("MaskedVertex", "MaskedInvertedPremultipliedAlphaFragment");
            }

            public IShader GetDrawMaskShader() => maskDrawingShader;

            public IShader GetDrawMeshShader(bool useClippingMask, bool usePremultipliedAlpha, bool useInvertedMask)
            {
                if (!useClippingMask)
                    return (!usePremultipliedAlpha) ? unmaskedMeshDrawShader : unmaskedPremultAlphaMeshDrawShader;
                else
                    if (!usePremultipliedAlpha)
                        return (!useInvertedMask) ? maskedMeshDrawShader : maskedInvertedMeshDrawShader;
                    else
                        return (!useInvertedMask) ? maskedPremultAlphaMeshDrawShader : maskedInvertedPremultAlphaMeshDrawShader;
            }
        }

        private class CubismSpriteDrawNode : DrawNode
        {
            private readonly CubismRenderingManager renderingManager;

            public CubismSpriteDrawNode(CubismSprite source)
                : base(source)
            {
                renderingManager = new CubismRenderingManager(new CubismRenderer(source), source.asset);
            }

            protected new CubismSprite Source => (CubismSprite)base.Source;

            public override void Draw(Action<TexturedVertex2D> vertexAction)
            {
                base.Draw(vertexAction);
                renderingManager.Draw(Matrix4.Identity);
            }

            protected override void Dispose(bool isDisposing)
            {
                base.Dispose(isDisposing);

                // We are handling disposal here to ensure that all draw calls have been performed to avoid race conditions
                renderingManager.Dispose();
            }
        }

        private unsafe class CubismRenderer : ICubismRenderer
        {
            private const int mask_size = 256;
            private readonly CubismShaderManager shaders;
            private readonly CubismRendererState state;
            private readonly CubismSprite sprite;
            private Matrix4 projectionMatrix;
            private BlendModeType blendMode;
            private bool useCulling;

            public CubismRenderer(CubismSprite sprite)
            {
                this.sprite = sprite;
                shaders = sprite.cubismShaders;
                state = new CubismRendererState();
            }

            public bool UsePremultipliedAlpha { get; set; }

            public BlendModeType BlendMode
            {
                get => blendMode;
                set
                {
                    blendMode = value;
                    var parameters = blendMode switch
                    {
                        BlendModeType.Normal => new BlendingParameters
                        {
                            Source = BlendingType.One,
                            Destination = BlendingType.OneMinusSrcAlpha,
                            SourceAlpha = BlendingType.One,
                            DestinationAlpha = BlendingType.OneMinusSrcAlpha,
                        },
                        BlendModeType.Add => new BlendingParameters
                        {
                            Source = BlendingType.One,
                            Destination = BlendingType.One,
                            SourceAlpha = BlendingType.Zero,
                            DestinationAlpha = BlendingType.One,
                        },
                        BlendModeType.Multiply => new BlendingParameters
                        {
                            Source = BlendingType.DstColor,
                            Destination = BlendingType.OneMinusSrcAlpha,
                            SourceAlpha = BlendingType.Zero,
                            DestinationAlpha = BlendingType.One,
                        },
                        _ => throw new ArgumentException($"{nameof(value)} is not a valid BlendMode."),
                    };
                    GLWrapper.SetBlend(parameters);
                }
            }

            public bool UseCulling
            {
                get => useCulling;
                set
                {
                    useCulling = value;

                    if (useCulling)
                        GL.Enable(EnableCap.CullFace);
                    else
                        GL.Disable(EnableCap.CullFace);
                }
            }

            public ICubismClippingMask CreateClippingMask()
            {
                return new CubismClippingMask { Size = new Vector2(mask_size) };
            }

            public ICubismTexture CreateTexture(byte[] textureData)
            {
                var upload = new TextureUpload(new MemoryStream(textureData));
                var texture = new CubismTexture(upload.Width, upload.Height);
                texture.SetData(upload);
                return texture;
            }

            public void DisposeClippingMask(ICubismClippingMask clippingMask) => (clippingMask as CubismClippingMask).Dispose();

            public void DisposeTexture(ICubismTexture texture) => (texture as CubismTexture).Dispose();

            public void DrawMask(ICubismTexture itexture, float[] vertexBuffer, float[] uvBuffer, short[] indexBuffer, ICubismClippingMask clippingMask, Matrix4 clippingMatrix, bool useCulling, bool isInvertedMask)
            {
                var texture = (CubismTexture)itexture;

                UseCulling = useCulling;

                var shader = (Shader)shaders.GetDrawMaskShader();
                shader.Bind();

                texture.Bind();

                shader.GetUniform<int>("s_texture0").Value = 0;

                // Set other parameters
                shader.GetUniform<Vector4>("u_channelFlag").Value = new Vector4(1.0f, 0.0f, 0.0f, 0.0f);
                shader.GetUniform<Matrix4>("u_clipMatrix").Value = clippingMatrix;
                shader.GetUniform<Vector4>("u_baseColor").Value = new Vector4(-1.0f, -1.0f, 1.0f, 1.0f);

                // Set the vertex buffer
                GL.EnableVertexAttribArray(shader.GetAttributeLocation("a_position"));
                fixed (float* pinnedVertexBuffer = vertexBuffer)
                    GL.VertexAttribPointer(shader.GetAttributeLocation("a_position"), 2, VertexAttribPointerType.Float, false, sizeof(float) * 2, (IntPtr)pinnedVertexBuffer);

                // Set the UV buffer
                GL.EnableVertexAttribArray(shader.GetAttributeLocation("a_texCoord"));
                fixed (float* pinnedUVBuffer = uvBuffer)
                    GL.VertexAttribPointer(shader.GetAttributeLocation("a_texCoord"), 2, VertexAttribPointerType.Float, false, sizeof(float) * 2, (IntPtr)pinnedUVBuffer);

                // Draw
                fixed (short* pinnedIndexBuffer = indexBuffer)
                    GL.DrawElements(PrimitiveType.Triangles, indexBuffer.Length, DrawElementsType.UnsignedShort, (IntPtr)pinnedIndexBuffer);

                shader.Unbind();
            }

            public void DrawMesh(ICubismTexture itexture, float[] vertexBuffer, float[] uvBuffer, short[] indexBuffer, ICubismClippingMask clippingMask, Matrix4 clippingMatrix, BlendModeType blendMode, bool useCulling, bool isInvertedMask, double opacity)
            {
                var texture = (CubismTexture)itexture;
                var mask = clippingMask as CubismClippingMask;
                bool useClippingMask = mask != null;

                UseCulling = useCulling;
                BlendMode = blendMode;

                // BUG: Using Inverted Masks causes eyes to be missing
                var shader = (Shader)shaders.GetDrawMeshShader(useClippingMask, UsePremultipliedAlpha, false);
                shader.Bind();

                if (useClippingMask == true)
                {
                    mask.Texture.Bind(TextureUnit.Texture1);
                    shader.GetUniform<int>("s_texture1").Value = 1;
                    shader.GetUniform<Matrix4>("u_clipMatrix").Value = clippingMatrix;
                    shader.GetUniform<Vector4>("u_channelFlag").Value = new Vector4(1.0f, 0.0f, 0.0f, 0.0f);
                }
                else
                {
                    GLWrapper.BindTexture(null, TextureUnit.Texture1);
                }

                texture.Bind();

                shader.GetUniform<int>("s_texture0").Value = 0;
                shader.GetUniform<Matrix4>("u_matrix").Value = projectionMatrix;

                var c = (Colour4)sprite.Colour;
                var color = new Vector4(c.R, c.G, c.B, c.A);
                color.W *= (float)opacity;

                if (UsePremultipliedAlpha)
                    color.Xyz *= color.W;

                shader.GetUniform<Vector4>("u_baseColor").Value = color;

                GL.EnableVertexAttribArray(shader.GetAttributeLocation("a_position"));
                fixed (float* pinnedVertexBuffer = vertexBuffer)
                    GL.VertexAttribPointer(shader.GetAttributeLocation("a_position"), 2, VertexAttribPointerType.Float, false, sizeof(float) * 2, (IntPtr)pinnedVertexBuffer);

                GL.EnableVertexAttribArray(shader.GetAttributeLocation("a_texCoord"));
                fixed (float* pinnedUVBuffer = uvBuffer)
                    GL.VertexAttribPointer(shader.GetAttributeLocation("a_texCoord"), 2, VertexAttribPointerType.Float, false, sizeof(float) * 2, (IntPtr)pinnedUVBuffer);

                fixed (short* pinnedIndexBuffer = indexBuffer)
                    GL.DrawElements(PrimitiveType.Triangles, indexBuffer.Length, DrawElementsType.UnsignedShort, (IntPtr)pinnedIndexBuffer);

                shader.Unbind();
            }

            public void EndDrawingMask(ICubismClippingMask clippingMask)
            {
                GLWrapper.PopViewport();

                var mask = (CubismClippingMask)clippingMask;
                mask.Unbind();
            }

            public void EndDrawingModel()
            {
                GLWrapper.PopScissorState();
                GLWrapper.PopDepthInfo();
                state.Restore();
            }

            public void StartDrawingMask(ICubismClippingMask clippingMask)
            {
                var mask = (CubismClippingMask)clippingMask;
                mask.Bind();
                GLWrapper.PushViewport(new RectangleI(0, 0, mask.Texture.Width, mask.Texture.Height));
                GLWrapper.Clear(new ClearInfo(Colour4.White));

                GLWrapper.SetBlend(new BlendingParameters
                {
                    Source = BlendingType.Zero,
                    Destination = BlendingType.OneMinusSrcColor,
                    SourceAlpha = BlendingType.Zero,
                    DestinationAlpha = BlendingType.OneMinusSrcAlpha,
                });
            }

            public void StartDrawingModel(float[] color, Matrix4 mvpMatrix)
            {
                state.Save();

                GL.FrontFace(FrontFaceDirection.Ccw);
                GLWrapper.PushScissorState(false);
                GLWrapper.PushDepthInfo(new DepthInfo(false));

                GLWrapper.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                GLWrapper.BindBuffer(BufferTarget.ArrayBuffer, 0);

                Matrix4 translation = Matrix4.CreateTranslation(Vector3.Zero);
                Matrix4 zoom = Matrix4.CreateScale(new Vector3(sprite.DrawScale));

                projectionMatrix = mvpMatrix * zoom * translation;
            }

            private class CubismRendererState
            {
                private int lastArrayBufferBind;
                private int lastElementArrayBufferBind;
                private bool lastCullFace;
                private int lastFrontFace;

                public void Save()
                {
                    GL.GetInteger(GetPName.ArrayBufferBinding, out lastArrayBufferBind);
                    GL.GetInteger(GetPName.ElementArrayBufferBinding, out lastElementArrayBufferBind);
                    GL.GetInteger(GetPName.FrontFace, out lastFrontFace);

                    lastCullFace = GL.IsEnabled(EnableCap.CullFace);
                }

                public void Restore()
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, (uint)lastArrayBufferBind);
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, (uint)lastElementArrayBufferBind);

                    GL.FrontFace((FrontFaceDirection)lastFrontFace);

                    setEnabled(EnableCap.CullFace, lastCullFace);
                }

                private static void setEnabled(EnableCap cap, bool enabled)
                {
                    if (enabled)
                        GL.Enable(cap);
                    else
                        GL.Disable(cap);
                }
            }
        }

        private class CubismTexture : Texture, ICubismTexture
        {
            public CubismTexture(int width, int height)
                : base(width, height, false, All.Linear)
            {
            }

            public bool Bind(TextureUnit unit = TextureUnit.Texture0) => TextureGL.Bind(unit);
        }

        private class CubismClippingMask : FrameBuffer, ICubismClippingMask
        {
            public CubismClippingMask()
                : base(new[] { RenderbufferInternalFormat.DepthComponent16 })
            {
            }
        }
    }
}

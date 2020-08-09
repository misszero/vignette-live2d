using System;
using System.Collections.Generic;
using System.IO;
using CubismFramework;
using osu.Framework.Graphics.OpenGL;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Textures;
using osuTK;
using osuTK.Graphics.ES30;

namespace osu.Framework.Graphics.Cubism
{
    public unsafe class CubismRenderer : ICubismRenderer, IDisposable
    {
        private const int clipping_mask_size = 256;
        private Matrix4 projectionMatrix;
        private Colour4 modelColor = new Colour4(1.0f, 1.0f, 1.0f, 1.0f);
        private List<CubismTexture> textures = new List<CubismTexture>();
        private List<CubismClippingMask> masks = new List<CubismClippingMask>();
        private CubismRendererState rendererState = new CubismRendererState();
        private CubismShaderManager shaderManager;

        private BlendModeType blendMode;
        public BlendModeType BlendMode
        {
            get => blendMode;
            set
            {
                blendMode = value;

                BlendingParameters parameters;
                switch (blendMode)
                {
                    case BlendModeType.Normal:
                        parameters = new BlendingParameters
                        {
                            Source = BlendingType.One,
                            Destination = BlendingType.OneMinusSrcAlpha,
                            SourceAlpha = BlendingType.One,
                            DestinationAlpha = BlendingType.OneMinusSrcAlpha
                        };
                        break;
                    case BlendModeType.Add:
                        parameters = new BlendingParameters
                        {
                            Source = BlendingType.One,
                            Destination = BlendingType.One,
                            SourceAlpha = BlendingType.Zero,
                            DestinationAlpha = BlendingType.One
                        };
                        break;
                    case BlendModeType.Multiply:
                        parameters = new BlendingParameters
                        {
                            Source = BlendingType.DstColor,
                            Destination = BlendingType.OneMinusSrcAlpha,
                            SourceAlpha = BlendingType.Zero,
                            DestinationAlpha = BlendingType.One
                        };
                        break;
                    default:
                        throw new ArgumentException();
                }

                GLWrapper.SetBlend(parameters);
            }
        }

        private bool useCulling;
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

        private bool isDisposed = false;

        public CubismRenderer(CubismShaderManager shaderManager)
        {
            this.shaderManager = shaderManager;
        }

        ~CubismRenderer()
        {
            Dispose(false);
        }

        public bool UsePremultipliedAlpha { get; set; } = false;

        public ICubismClippingMask CreateClippingMask()
        {
            var mask = new CubismClippingMask();
            mask.Size = new Vector2(clipping_mask_size);

            masks.Add(mask);
            return mask;
        }

        public ICubismTexture CreateTexture(byte[] textureData)
        {
            var upload = new TextureUpload(new MemoryStream(textureData));
            var texture = new CubismTexture(upload.Width, upload.Height);
            texture.SetData(upload);

            textures.Add(texture);
            return texture;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public void DisposeClippingMask(ICubismClippingMask clippingMask)
        {
            var mask = (CubismClippingMask)clippingMask;
            mask.Dispose();
            masks.Remove(mask);
        }

        public void DisposeTexture(ICubismTexture itexture)
        {
            var texture = (CubismTexture)itexture;
            texture.Dispose();
            textures.Remove(texture);
        }

        public void DrawMask(ICubismTexture itexture, float[] vertexBuffer, float[] uvBuffer, short[] indexBuffer, ICubismClippingMask clippingMask, Matrix4 clippingMatrix, bool useCulling)
        {
            var texture = (CubismTexture)itexture;

            UseCulling = useCulling;

            var shader = (Shader)shaderManager.GetDrawMaskShader();
            shader.Bind();

            if (texture.Bind())
            {
                GL.Uniform1(shader.GetUniformLocation("s_texture0"), 0);

                GL.EnableVertexAttribArray(shader.GetAttributeLocation("a_position"));
                fixed (float* pinnedVertexBuffer = vertexBuffer)
                    GL.VertexAttribPointer(shader.GetAttributeLocation("a_position"), 2, VertexAttribPointerType.Float, false, sizeof(float) * 2, (IntPtr)pinnedVertexBuffer);
                
                GL.EnableVertexAttribArray(shader.GetAttributeLocation("a_texCoord"));
                fixed (float* pinnedUVBuffer = uvBuffer)
                    GL.VertexAttribPointer(shader.GetAttributeLocation("a_texCoord"), 2, VertexAttribPointerType.Float, false, sizeof(float) * 2, (IntPtr)pinnedUVBuffer);
                
                GL.Uniform4(shader.GetUniformLocation("u_channelFlag"), 1.0f, 0.0f, 0.0f, 0.0f);
                GL.UniformMatrix4(shader.GetUniformLocation("u_clipMatrix"), false, ref clippingMatrix);
                GL.Uniform4(shader.GetUniformLocation("u_baseColor"), -1.0f, -1.0f, 1.0f, 1.0f);

                fixed (short* pinnedIndexBuffer = indexBuffer)
                    GL.DrawElements(PrimitiveType.Triangles, indexBuffer.Length, DrawElementsType.UnsignedShort, (IntPtr)pinnedIndexBuffer);
            }

            shader.Unbind();
        }

        public void DrawMesh(ICubismTexture itexture, float[] vertexBuffer, float[] uvBuffer, short[] indexBuffer, ICubismClippingMask clippingMask, Matrix4 clippingMatrix, BlendModeType blendMode, bool useCulling, double opacity)
        {
            var texture = (CubismTexture)itexture;
            var mask = clippingMask as CubismClippingMask;
            bool useClippingMask = (clippingMask != null);

            UseCulling = useCulling;
            BlendMode = blendMode;

            var shader = (Shader)shaderManager.GetDrawMeshShader(useClippingMask, UsePremultipliedAlpha);
            shader.Bind();

            GL.EnableVertexAttribArray(shader.GetAttributeLocation("a_position"));
            fixed (float* pinnedVertexBuffer = vertexBuffer)
                GL.VertexAttribPointer(shader.GetAttributeLocation("a_position"), 2, VertexAttribPointerType.Float, false, sizeof(float) * 2, (IntPtr)pinnedVertexBuffer);

            GL.EnableVertexAttribArray(shader.GetAttributeLocation("a_texCoord"));
            fixed (float* pinnedUVBuffer = uvBuffer)
                GL.VertexAttribPointer(shader.GetAttributeLocation("a_texCoord"), 2, VertexAttribPointerType.Float, false, sizeof(float) * 2, (IntPtr)pinnedUVBuffer);

            if (useClippingMask == true)
            {
                if (mask.Texture.Bind(TextureUnit.Texture1))
                {
                    GL.Uniform1(shader.GetUniformLocation("s_texture1"), 1);
                    GL.UniformMatrix4(shader.GetUniformLocation("u_clipMatrix"), false, ref clippingMatrix);
                    GL.Uniform4(shader.GetUniformLocation("u_channelFlag"), 1.0f, 0.0f, 0.0f, 0.0f);
                }
            }
            else
                GLWrapper.BindTexture(0, TextureUnit.Texture1);

            if (texture.Bind())
            {
                GL.Uniform1(shader.GetUniformLocation("s_texture0"), 0);
                GL.UniformMatrix4(shader.GetUniformLocation("u_matrix"), false, ref projectionMatrix);

                float[] color = new float[] { modelColor.R, modelColor.G, modelColor.B, modelColor.A };
                color[3] *= (float)opacity;

                if (UsePremultipliedAlpha)
                {
                    color[0] *= color[3];
                    color[1] *= color[3];
                    color[2] *= color[3];
                }

                GL.Uniform4(shader.GetUniformLocation("u_baseColor"), color[0], color[1], color[2], color[3]);
                fixed (short* pinnedIndexBuffer = indexBuffer)
                    GL.DrawElements(PrimitiveType.Triangles, indexBuffer.Length, DrawElementsType.UnsignedShort, (IntPtr)pinnedIndexBuffer);
            }

            shader.Unbind();
        }

        public void EndDrawingMask(ICubismClippingMask clipping_mask)
        {
            GLWrapper.PopViewport();
        }

        public void EndDrawingModel()
        {
            GLWrapper.PopScissorState();
            GLWrapper.PopDepthInfo();
            rendererState.Restore();
        }

        public void StartDrawingMask(ICubismClippingMask clippingMask)
        {
            var mask = (CubismClippingMask)clippingMask;
            mask.Bind();

            GLWrapper.PushViewport(new RectangleI(0, 0, mask.Texture.Width, mask.Texture.Height));
            GLWrapper.Clear(new ClearInfo());

            mask.Unbind();

            var shader = shaderManager.GetDrawMaskShader();
            shader.Bind();

            GLWrapper.SetBlend(new BlendingParameters
            {
                Source = BlendingType.Zero,
                Destination = BlendingType.OneMinusSrcColor,
                SourceAlpha = BlendingType.Zero,
                DestinationAlpha = BlendingType.OneMinusSrcAlpha
            });

            shader.Unbind();
        }

        public void StartDrawingModel(float[] color, Matrix4 mvpMatrix)
        {
            rendererState.Save();

            GL.FrontFace(FrontFaceDirection.Ccw);
            GLWrapper.PushScissorState(false);
            GLWrapper.PushDepthInfo(new DepthInfo(false));

            GLWrapper.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GLWrapper.BindBuffer(BufferTarget.ArrayBuffer, 0);

            if ((color != null) && (color.Length == 4))
                modelColor = new Colour4(color[0], color[1], color[2], color[3]);

            projectionMatrix = mvpMatrix;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
                return;

            isDisposed = true;
        }
    }
}
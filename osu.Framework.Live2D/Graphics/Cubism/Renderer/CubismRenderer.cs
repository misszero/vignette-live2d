// Copyright (c) Nitrous <n20gaming2000@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.IO;
using CubismFramework;
using osu.Framework.Extensions.ShaderExtensions;
using osu.Framework.Graphics.OpenGL;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Textures;
using osuTK;
using osuTK.Graphics.ES30;

namespace osu.Framework.Graphics.Cubism.Renderer
{
    public unsafe partial class CubismRenderer : ICubismRenderer
    {
        private const int mask_size = 256;
        private Matrix4 projectionMatrix;
        private CubismRendererState rendererState = new CubismRendererState();
        public CubismShaderManager ShaderManager;
        public bool UsePremultipliedAlpha { get; set; }

        public float DrawWidth;
        public float DrawHeight;
        public Vector2 DrawSize
        {
            get => new Vector2(DrawWidth, DrawHeight);
            set
            {
                if (value == DrawSize) return;

                DrawWidth = value.X;
                DrawHeight = value.Y;
            }
        }

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

        public ICubismClippingMask CreateClippingMask()
        {
            var mask = new CubismClippingMask();
            mask.Size = new Vector2(mask_size);

            return mask;
        }

        public ICubismTexture CreateTexture(byte[] textureData)
        {
            var upload = new TextureUpload(new MemoryStream(textureData));
            var texture = new CubismTexture(upload.Width, upload.Height);
            texture.SetData(upload);

            return texture;
        }

        public void DisposeClippingMask(ICubismClippingMask clippingMask) => (clippingMask as CubismClippingMask)?.Dispose();

        public void DisposeTexture(ICubismTexture texture) => (texture as CubismTexture)?.Dispose();

        public void DrawMask(ICubismTexture itexture, float[] vertexBuffer, float[] uvBuffer, short[] indexBuffer, ICubismClippingMask clippingMask, Matrix4 clippingMatrix, bool useCulling, bool isInvertedMask)
        {
            var texture = (CubismTexture)itexture;

            UseCulling = useCulling;

            var shader = (Shader)ShaderManager.GetDrawMaskShader();
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
            bool useClippingMask = (mask != null);

            UseCulling = useCulling;
            BlendMode = blendMode;

            var shader = (Shader)ShaderManager.GetDrawMeshShader(useClippingMask, UsePremultipliedAlpha);
            shader.Bind();

            if (useClippingMask == true)
            {
                mask.Texture.Bind(TextureUnit.Texture1);

                shader.GetUniform<int>("s_texture1").Value = 1;
                shader.GetUniform<Matrix4>("u_clipMatrix").Value = clippingMatrix;
                shader.GetUniform<Vector4>("u_channelFlag").Value = new Vector4(1.0f, 0.0f, 0.0f, 0.0f);
            }
            else
                GLWrapper.BindTexture(null, TextureUnit.Texture1);

            texture.Bind();

            shader.GetUniform<int>("s_texture0").Value = 0;
            shader.GetUniform<Matrix4>("u_matrix").Value = projectionMatrix;

            Vector4 color = new Vector4(Colour.R, Colour.G, Colour.B, Colour.A);
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
            rendererState.Restore();
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
                DestinationAlpha = BlendingType.OneMinusSrcAlpha
            });
        }

        public void StartDrawingModel(float[] color, Matrix4 mvpMatrix)
        {
            if (ShaderManager == null)
                throw new NullReferenceException();

            rendererState.Save();

            GL.FrontFace(FrontFaceDirection.Ccw);
            GLWrapper.PushScissorState(false);
            GLWrapper.PushDepthInfo(new DepthInfo(false));

            GLWrapper.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GLWrapper.BindBuffer(BufferTarget.ArrayBuffer, 0);

            Matrix4 translation = Matrix4.CreateTranslation(new Vector3(X / DrawWidth, -Y / DrawHeight, 0));
            Matrix4 zoom = Matrix4.CreateScale(Scale.X * (DrawHeight / DrawWidth), Scale.Y, 0);

            projectionMatrix = mvpMatrix * zoom * translation;
        }
    }
}
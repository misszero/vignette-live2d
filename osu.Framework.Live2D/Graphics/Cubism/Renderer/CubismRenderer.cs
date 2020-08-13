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
    internal unsafe class CubismRenderer : ICubismRenderer
    {
        private const int clipping_mask_size = 256;
        private Matrix4 projectionMatrix;
        private Colour4 modelColor = new Colour4(1.0f, 1.0f, 1.0f, 1.0f);
        private CubismRendererState rendererState = new CubismRendererState();
        private CubismShaderManager shaderManager;
        public bool UsePremultipliedAlpha { get; set; }

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

        public CubismRenderer(CubismShaderManager shaderManager)
        {
            this.shaderManager = shaderManager;
        }

        public ICubismClippingMask CreateClippingMask()
        {
            var mask = new CubismClippingMask();
            mask.Size = new Vector2(clipping_mask_size);

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

        public void DrawMask(ICubismTexture itexture, float[] vertexBuffer, float[] uvBuffer, short[] indexBuffer, ICubismClippingMask clippingMask, Matrix4 clippingMatrix, bool useCulling)
        {
            var texture = (CubismTexture)itexture;

            UseCulling = useCulling;

            var shader = (Shader)shaderManager.GetDrawMaskShader();
            shader.Bind();

            if (texture.Bind())
            {
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
            }

            shader.Unbind();
        }

        public void DrawMesh(ICubismTexture itexture, float[] vertexBuffer, float[] uvBuffer, short[] indexBuffer, ICubismClippingMask clippingMask, Matrix4 clippingMatrix, BlendModeType blendMode, bool useCulling, double opacity)
        {
            var texture = (CubismTexture)itexture;
            var mask = clippingMask as CubismClippingMask;
            bool useClippingMask = (mask != null);

            UseCulling = useCulling;
            BlendMode = blendMode;

            var shader = (Shader)shaderManager.GetDrawMeshShader(useClippingMask, UsePremultipliedAlpha);
            shader.Bind();

            if (useClippingMask == true)
            {
                if (mask.Texture.Bind(TextureUnit.Texture1))
                {
                    shader.GetUniform<int>("s_texture1").Value = 1;
                    shader.GetUniform<Matrix4>("u_clipMatrix").Value = clippingMatrix;
                    shader.GetUniform<Vector4>("u_channelFlag").Value = new Vector4(1.0f, 0.0f, 0.0f, 0.0f);
                }
            }
            else
                GLWrapper.BindTexture(null, TextureUnit.Texture1);

            if (texture.Bind())
            {
                shader.GetUniform<int>("s_texture0").Value = 0;
                shader.GetUniform<Matrix4>("u_matrix").Value = projectionMatrix;

                Vector4 color = new Vector4(modelColor.R, modelColor.G, modelColor.B, modelColor.A);
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
            GLWrapper.Clear(new ClearInfo(Colour4.White));
            mask.Unbind();

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
    }
}
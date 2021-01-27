// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System;
using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Graphics.OpenGL;
using osu.Framework.Graphics.OpenGL.Buffers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Textures;
using osuTK;
using osuTK.Graphics.ES30;
using Vignette.Application.Live2D.Model;

namespace Vignette.Application.Live2D.Graphics
{
    public unsafe class CubismRendererOpenGL : CubismRenderer
    {
        private readonly RendererProfile profile = new RendererProfile();

        private readonly List<IShader> shaders = new List<IShader>();

        public CubismRendererOpenGL(ShaderManager shaders, CubismModel model, IEnumerable<Texture> textures)
            : base(model, textures)
        {
            this.shaders.Add(shaders.Load(@"VertShaderSrcSetupMask", @"FragShaderSrcSetupMask"));

            this.shaders.Add(shaders.Load(@"VertShaderSrc", @"FragShaderSrc"));
            this.shaders.Add(shaders.Load(@"VertShaderSrcMasked", @"FragShaderSrcMask"));
            this.shaders.Add(shaders.Load(@"VertShaderSrcMasked", @"FragShaderSrcMaskInverted"));
            this.shaders.Add(shaders.Load(@"VertShaderSrc", @"FragShaderSrcPremultipliedAlpha"));
            this.shaders.Add(shaders.Load(@"VertShaderSrcMasked", @"FragShaderSrcMaskPremultipliedAlpha"));
            this.shaders.Add(shaders.Load(@"VertShaderSrcMasked", @"FragShaderSrcMaskInvertedPremultipliedAlpha"));
        }

        protected override void DrawMask(CubismDrawable drawable, Matrix4 clippingMatrix)
        {
            var shader = shaders[0];

            if (drawable.ConstantFlags.HasFlag(ConstantDrawableFlags.IsDoubleSided))
                GL.Enable(EnableCap.CullFace);
            else
                GL.Disable(EnableCap.CullFace);

            shader.Bind();

            shader.GetUniform<int>("s_texture0").Value = 0;

            shader.GetUniform<Vector4>("u_channelFlag").Value = new Vector4(1.0f, 0.0f, 0.0f, 0.0f);
            shader.GetUniform<Matrix4>("u_clipMatrix").Value = clippingMatrix;
            shader.GetUniform<Vector4>("u_baseColor").Value = new Vector4(-1.0f, -1.0f, 1.0f, 1.0f);

            GL.EnableVertexAttribArray(0);
            fixed (float* pinnedVertexBuffer = drawable.Vertices)
                GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, sizeof(float) * 2, (IntPtr)pinnedVertexBuffer);

            GL.EnableVertexAttribArray(1);
            fixed (float* pinnedUVBuffer = drawable.TextureCoordinates)
                GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, sizeof(float) * 2, (IntPtr)pinnedUVBuffer);

            fixed (short* pinnedIndexBuffer = drawable.Indices)
                GL.DrawElements(PrimitiveType.Triangles, drawable.Indices.Length, DrawElementsType.UnsignedShort, (IntPtr)pinnedIndexBuffer);

            shader.Unbind();
        }

        protected override void DrawMesh(CubismDrawable drawable, Texture texture, FrameBuffer clippingMask, Matrix4 drawMatrix)
        {
            int offset = 1 + (clippingMask != null ? (drawable.ConstantFlags.HasFlag(ConstantDrawableFlags.IsInvertedMask) ? 2 : 1) : 0) + (UsePremultipliedAlpha ? 3 : 0);

            IShader shader;
            BlendingParameters blendingParameters;
            
            if (drawable.ConstantFlags.HasFlag(ConstantDrawableFlags.BlendMultiplicative))
            {
                shader = shaders[offset];
                blendingParameters = new BlendingParameters
                {
                    Source = BlendingType.DstColor,
                    Destination = BlendingType.OneMinusSrcAlpha,
                    SourceAlpha = BlendingType.Zero,
                    DestinationAlpha = BlendingType.One,
                };
            }
            else if (drawable.ConstantFlags.HasFlag(ConstantDrawableFlags.BlendAdditive))
            {
                shader = shaders[offset];
                blendingParameters = new BlendingParameters
                {
                    Source = BlendingType.One,
                    Destination = BlendingType.One,
                    SourceAlpha = BlendingType.Zero,
                    DestinationAlpha = BlendingType.One,
                };
            }
            else
            {
                shader = shaders[offset];
                blendingParameters = new BlendingParameters
                {
                    Source = BlendingType.One,
                    Destination = BlendingType.OneMinusSrcAlpha,
                    SourceAlpha = BlendingType.One,
                    DestinationAlpha = BlendingType.OneMinusSrcAlpha,
                };
            }

            GLWrapper.SetBlend(blendingParameters);
            shader.Bind();

            if (clippingMask != null)
            {
                clippingMask.Texture.Bind(TextureUnit.Texture1);

                shader.GetUniform<int>("s_texture1").Value = 1;
                shader.GetUniform<Matrix4>("u_clipMatrix").Value = drawMatrix;
                shader.GetUniform<Vector4>("u_channelFlag").Value = new Vector4(1.0f, 0.0f, 0.0f, 0.0f);
            }
            else
            {
                GLWrapper.BindTexture(null, TextureUnit.Texture1);
            }

            texture.TextureGL.Bind();

            shader.GetUniform<int>("s_texture0").Value = 0;
            shader.GetUniform<Matrix4>("u_matrix").Value = MvpMatrix;

            var color = new Vector4(Color.R, Color.G, Color.B, Color.A);
            color.W *= drawable.Opacity;

            if (UsePremultipliedAlpha)
            {
                color.X *= drawable.Opacity;
                color.Y *= drawable.Opacity;
                color.Z *= drawable.Opacity;
            }

            shader.GetUniform<Vector4>("u_baseColor").Value = color;

            GL.EnableVertexAttribArray(0);
            fixed (float* pinnedVertexBuffer = drawable.Vertices)
                GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, sizeof(float) * 2, (IntPtr)pinnedVertexBuffer);

            GL.EnableVertexAttribArray(1);
            fixed (float* pinnedUVBuffer = drawable.TextureCoordinates)
                GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, sizeof(float) * 2, (IntPtr)pinnedUVBuffer);

            fixed (short* pinnedIndexBuffer = drawable.Indices)
                GL.DrawElements(PrimitiveType.Triangles, drawable.Indices.Length, DrawElementsType.UnsignedShort, (IntPtr)pinnedIndexBuffer);

            shader.Unbind();
        }

        protected override void PostDrawMask(FrameBuffer clippingMask)
        {
            GLWrapper.PopViewport();
            clippingMask.Unbind();
        }

        protected override void PostDrawMesh()
        {
            GLWrapper.PopScissorState();
            GLWrapper.PopDepthInfo();
            profile.Restore();
        }

        protected override void PreDrawMask(FrameBuffer clippingMask)
        {
            clippingMask.Bind();
            GLWrapper.PushViewport(new RectangleI(0, 0, clippingMask.Texture.Width, clippingMask.Texture.Height));
            GLWrapper.Clear(new ClearInfo(Colour4.White));
            GLWrapper.SetBlend(new BlendingParameters
            {
                Source = BlendingType.Zero,
                Destination = BlendingType.OneMinusSrcColor,
                SourceAlpha = BlendingType.Zero,
                DestinationAlpha = BlendingType.OneMinusSrcAlpha,
            });
        }

        protected override void PreDrawMesh()
        {
            profile.Save();
            GL.FrontFace(FrontFaceDirection.Ccw);
            GLWrapper.PushScissorState(false);
            GLWrapper.PushDepthInfo(new DepthInfo(false));
            GLWrapper.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GLWrapper.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        private class RendererProfile
        {
            private bool lastCullFace;

            private int lastFrontFace;

            private int lastArrayBufferBind;

            private int lastElementArrayBufferBind;

            public void Save()
            {
                GL.GetInteger(GetPName.ArrayBufferBinding, out lastArrayBufferBind);
                GL.GetInteger(GetPName.ElementArrayBufferBinding, out lastElementArrayBufferBind);
                GL.GetInteger(GetPName.FrontFace, out lastFrontFace);

                lastCullFace = GL.IsEnabled(EnableCap.CullFace);
            }

            public void Restore()
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, lastArrayBufferBind);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, lastElementArrayBufferBind);
                GL.FrontFace((FrontFaceDirection)lastFrontFace);

                if (lastCullFace)
                    GL.Enable(EnableCap.CullFace);
                else
                    GL.Disable(EnableCap.CullFace);
            }
        }
    }
}

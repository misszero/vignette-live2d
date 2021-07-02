// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.OpenGL;
using osu.Framework.Graphics.OpenGL.Buffers;
using osu.Framework.Graphics.OpenGL.Vertices;
using osu.Framework.Graphics.Primitives;
using osuTK;
using osuTK.Graphics.ES30;

namespace Vignette.Game.Live2D.Graphics
{
    internal unsafe partial class CubismRenderer
    {
        protected class CubismRendererDrawNode : TexturedShaderDrawNode
        {
            protected new CubismRenderer Source => (CubismRenderer)base.Source;

            private CubismShaders shaders;
            private List<CubismDrawable> drawables;
            private List<MaskingContext> contexts;

            public CubismRendererDrawNode(CubismRenderer source)
                : base(source)
            {
            }

            public override void ApplyState()
            {
                base.ApplyState();
                shaders = Source.shaders;
                contexts = Source.model.MaskingContexts.ToList();
                drawables = Source.model.Drawables.OrderBy(d => d.RenderOrder).ToList();
            }

            public override void Draw(Action<TexturedVertex2D> vertexAction)
            {
                base.Draw(vertexAction);

                // Ensure that OpenGL is in a state where we can perform drawing
                using (prepareRenderer())
                {
                    // Draw our masks to the frame buffer
                    foreach (var context in contexts)
                    {
                        using (bindFrameBuffer(context.FrameBuffer))
                        {
                            var shader = shaders.SetupMask;
                            shader.Bind();

                            shader.GetUniform<int>("s_texture0").Value = 0;
                            shader.GetUniform<Vector4>("u_channelFlag").Value = new Vector4(1.0f, 0.0f, 0.0f, 0.0f);
                            shader.GetUniform<Matrix4>("u_clipMatrix").Value = context.MaskMatrix;
                            shader.GetUniform<Vector4>("u_baseColor").Value = new Vector4(-1.0f, -1.0f, 1.0f, 1.0f);

                            foreach (var mask in context.Masks)
                            {
                                fixed (Vector2* pinnedPositions = mask.Positions)
                                fixed (Vector2* pinnedTexturePositions = mask.TexturePositions)
                                fixed (short* pinnedIndices = mask.Indices)
                                {
                                    GL.EnableVertexAttribArray(0);
                                    GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, sizeof(Vector2), (IntPtr)pinnedPositions);

                                    GL.EnableVertexAttribArray(1);
                                    GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, sizeof(Vector2), (IntPtr)pinnedTexturePositions);

                                    GL.DrawElements(PrimitiveType.Triangles, mask.Indices.Length, DrawElementsType.UnsignedShort, (IntPtr)pinnedIndices);
                                }
                            }

                            shader.Unbind();
                        }
                    }

                    // Draw our drawables
                    foreach (var drawable in drawables)
                    {
                        bool hasMask = drawable.MaskingContext != null;

                        var shader = shaders.GetShaderFor(hasMask, drawable.IsInvertedMask, false);

                        shader.Bind();

                        if (hasMask)
                        {
                            drawable.MaskingContext.FrameBuffer.Texture?.Bind(TextureUnit.Texture1);
                            shader.GetUniform<int>("s_texture1").Value = 1;
                            shader.GetUniform<Matrix4>("u_clipMatrix").Value = drawable.MaskingContext.DrawMatrix;
                            shader.GetUniform<Vector4>("u_channelFlag").Value = new Vector4(1.0f, 0.0f, 0.0f, 0.0f);
                        }
                        else
                        {
                            GLWrapper.BindTexture(null, TextureUnit.Texture1);
                        }

                        drawable.Texture?.TextureGL.Bind();
                        GLWrapper.SetBlend(drawable.Blending);

                        shader.GetUniform<int>("s_texture0").Value = 0;
                        shader.GetUniform<Matrix4>("u_matrix").Value = Matrix4.Identity;

                        Colour4 colour = drawable.Colour;
                        shader.GetUniform<Vector4>("u_baseColor").Value = new Vector4(colour.R, colour.G, colour.B, colour.A);

                        fixed (Vector2* pinnedPositions = drawable.Positions)
                        fixed (Vector2* pinnedTexturePositions = drawable.TexturePositions)
                        fixed (short* pinnedIndices = drawable.Indices)
                        {
                            GL.EnableVertexAttribArray(0);
                            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, sizeof(Vector2), (IntPtr)pinnedPositions);

                            GL.EnableVertexAttribArray(1);
                            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, sizeof(Vector2), (IntPtr)pinnedTexturePositions);

                            GL.DrawElements(PrimitiveType.Triangles, drawable.Indices.Length, DrawElementsType.UnsignedShort, (IntPtr)pinnedIndices);
                        }

                        GLWrapper.SetBlend(BlendingParameters.None);
                        shader.Unbind();
                    }
                }
            }

            private IDisposable bindFrameBuffer(FrameBuffer frameBuffer)
            {
                frameBuffer.Bind();
                GLWrapper.PushViewport(new RectangleI(0, 0, frameBuffer.Texture.Width, frameBuffer.Texture.Height));
                GLWrapper.Clear(new ClearInfo(Colour4.White));
                GLWrapper.SetBlend(new BlendingParameters
                {
                    Source = BlendingType.Zero,
                    Destination = BlendingType.OneMinusSrcColor,
                    SourceAlpha = BlendingType.Zero,
                    DestinationAlpha = BlendingType.OneMinusSrcAlpha,
                });

                return new ValueInvokeOnDisposal<FrameBuffer>(frameBuffer, f => unbindFrameBuffer(f));
            }

            private void unbindFrameBuffer(FrameBuffer frameBuffer)
            {
                GLWrapper.SetBlend(BlendingParameters.None);
                GLWrapper.PopViewport();
                frameBuffer.Unbind();
            }

            private bool lastCullFace;
            private int lastFrontFace;
            private int lastArrayBufferBind;
            private int lastElementArrayBufferBind;

            private IDisposable prepareRenderer()
            {
                GL.GetInteger(GetPName.ArrayBufferBinding, out lastArrayBufferBind);
                GL.GetInteger(GetPName.ElementArrayBufferBinding, out lastElementArrayBufferBind);
                GL.GetInteger(GetPName.FrontFace, out lastFrontFace);
                lastCullFace = GL.IsEnabled(EnableCap.CullFace);

                GLWrapper.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                GLWrapper.BindBuffer(BufferTarget.ArrayBuffer, 0);

                return new ValueInvokeOnDisposal<CubismRendererDrawNode>(this, d => d.restoreRenderer());
            }

            private void restoreRenderer()
            {
                GLWrapper.BindBuffer(BufferTarget.ArrayBuffer, lastArrayBufferBind);
                GLWrapper.BindBuffer(BufferTarget.ElementArrayBuffer, lastElementArrayBufferBind);
                GL.FrontFace((FrontFaceDirection)lastFrontFace);

                if (lastCullFace)
                    GL.Enable(EnableCap.CullFace);
                else
                    GL.Disable(EnableCap.CullFace);
            }
        }
    }
}

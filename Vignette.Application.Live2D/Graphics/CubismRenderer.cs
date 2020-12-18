// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics;
using osu.Framework.Graphics.OpenGL.Buffers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Textures;
using osuTK;
using Vignette.Application.Live2D.Model;

namespace Vignette.Application.Live2D.Graphics
{
    public abstract class CubismRenderer : IDisposable
    {
        private CubismModel model;

        private List<Texture> textures;

        private readonly List<ClippingContext> allClippingContexts = new List<ClippingContext>();

        private readonly List<ClippingContext> drawableClippingContexts = new List<ClippingContext>();

        private IEnumerable<CubismDrawable> renderOrderedDrawables;

        private bool isDisposed;

        public bool UsePremultipliedAlpha { get; set; }

        public Matrix4 MvpMatrix { get; set; }

        public Colour4 Color { get; set; }

        public CubismRenderer(CubismModel model, IEnumerable<Texture> textures)
        {
            this.model = model;
            this.textures = textures.ToList();

            foreach (var drawable in model.Drawables)
            {
                if (drawable.Masks.Length <= 0)
                {
                    drawableClippingContexts.Add(null);
                    continue;
                }

                ClippingContext newClippingContext = null;
                int[] maskIds = drawable.Masks.Distinct().OrderBy(x => x).ToArray();
                foreach (var target in allClippingContexts)
                {
                    if (maskIds.SequenceEqual(target.ClippingIds))
                    {
                        newClippingContext = target;
                        break;
                    }
                }

                if (newClippingContext == null)
                {
                    newClippingContext = new ClippingContext(maskIds);
                    allClippingContexts.Add(newClippingContext);
                }

                newClippingContext.ClippedDrawables.Add(drawable);
                drawableClippingContexts.Add(newClippingContext);
            }
        }

        public void Draw()
        {
            if (model == null)
                return;

            PreDrawMesh();

            foreach (var context in allClippingContexts)
            {
                var bounds = calcClippedDrawTotalBounds(context);
                if (bounds.IsEmpty)
                    continue;

                const float margin = 0.05f;
                var inflatedBounds = bounds.Inflate(margin);

                float scaleX = 1.0f / inflatedBounds.Width;
                float scaleY = 1.0f / inflatedBounds.Height;

                // These matrices are already transposed
                var matrixForMask = Matrix4.Identity;
                matrixForMask[0, 0] = 2.0f * scaleX;
                matrixForMask[1, 1] = 2.0f * scaleY;
                matrixForMask[3, 0] = -2.0f * scaleX * inflatedBounds.X - 1.0f;
                matrixForMask[3, 1] = -2.0f * scaleY * inflatedBounds.Y - 1.0f;

                context.MatrixForMask = matrixForMask;

                var matrixForDraw = Matrix4.Identity;
                matrixForDraw[0, 0] = scaleX;
                matrixForDraw[1, 1] = scaleY;
                matrixForDraw[3, 0] = -scaleX * inflatedBounds.X;
                matrixForDraw[3, 1] = -scaleY * inflatedBounds.Y;

                context.MatrixForDraw = matrixForDraw;

                PreDrawMask(context.Target);

                foreach (int i in context.ClippingIds)
                {
                    var drawable = model.Drawables[i];

                    DrawMask(drawable, context.MatrixForMask);
                }

                PostDrawMask(context.Target);
            }

            if (renderOrderedDrawables == null || model.Drawables.Any(d => d.DynamicFlags.HasFlag(DynamicDrawableFlags.RenderOrderChanged)))
            {
                renderOrderedDrawables = model.Drawables.OrderBy(d => d.RenderOrder);
            }

            foreach (var drawable in renderOrderedDrawables)
            {
                if (drawable.Indices.Length <= 0)
                    continue;

                if (!drawable.DynamicFlags.HasFlag(DynamicDrawableFlags.Visible))
                    continue;

                var context = drawableClippingContexts[drawable.Id];
                var matrix = context != null ? context.MatrixForDraw : Matrix4.Identity;

                DrawMesh(drawable, textures[drawable.TextureId], context?.Target, matrix);
            }

            PostDrawMesh();
        }

        private RectangleF calcClippedDrawTotalBounds(ClippingContext context)
        {
            var result = RectangleF.Empty;

            foreach (var drawable in context.ClippedDrawables)
            {
                if (!drawable.Bounds.IsEmpty)
                    result = RectangleF.Union(result, drawable.Bounds);
            }

            return result;
        }

        protected abstract void PreDrawMask(FrameBuffer clippingMask);

        protected abstract void DrawMask(CubismDrawable drawable, Matrix4 clippingMatrix);

        protected abstract void PostDrawMask(FrameBuffer clippingMask);

        protected abstract void PreDrawMesh();

        protected abstract void DrawMesh(CubismDrawable drawable, Texture texture, FrameBuffer clippingMask, Matrix4 drawMatrix);

        protected abstract void PostDrawMesh();

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
                return;

            foreach (var texture in textures)
                texture.Dispose();

            foreach (var context in allClippingContexts)
                context.Target.Dispose();

            isDisposed = true;
        }

        ~CubismRenderer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private class ClippingContext
        {
            public int[] ClippingIds { get; private set; }

            public Matrix4 MatrixForMask { get; set; } = Matrix4.Identity;

            public Matrix4 MatrixForDraw { get; set; } = Matrix4.Identity;

            public readonly FrameBuffer Target = new FrameBuffer() { Size = new Vector2(256) };

            public readonly List<CubismDrawable> ClippedDrawables = new List<CubismDrawable>();

            public ClippingContext(int[] clippingIds)
            {
                ClippingIds = clippingIds;
            }
        }
    }
}

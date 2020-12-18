// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System;
using osu.Framework.Graphics.Primitives;
using Vignette.Application.Live2D.Id;

namespace Vignette.Application.Live2D.Model
{
    public class CubismDrawable : CubismId
    {
        public readonly int TextureId;

        public readonly int[] Masks;

        public readonly float[] TextureCoordinates;

        public readonly short[] Indices;

        public int RenderOrder { get; private set; }

        public float[] Vertices { get; private set; }

        public float Opacity { get; private set; }

        public ConstantDrawableFlags ConstantFlags { get; private set; }

        public DynamicDrawableFlags DynamicFlags { get; private set; }

        public RectangleF Bounds
        {
            get
            {
                if (Vertices == null)
                    return RectangleF.Empty;

                int vertexCount = Vertices.Length / 2;
                float minX = float.MaxValue;
                float minY = float.MaxValue;
                float maxX = float.MinValue;
                float maxY = float.MinValue;

                for (int i = 0; i < vertexCount; i++)
                {
                    float x = Vertices[2 * i];
                    float y = Vertices[2 * i + 1];
                    minX = MathF.Min(minX, x);
                    maxX = MathF.Max(maxX, x);
                    minY = MathF.Min(minY, y);
                    maxY = MathF.Max(maxY, y);
                }

                var rect = RectangleF.Empty;
                if ((minX < maxX) && (minY < maxY))
                {
                    rect.X = minX;
                    rect.Y = minY;
                    rect.Width = maxX - minX;
                    rect.Height = maxY - minY;
                }

                return rect;
            }
        }

        internal CubismDrawable(int index, string name, int textureId, int[] masks, float[] textureCoords, short[] indices, ConstantDrawableFlags flags)
            : base(index, name)
        {
            Masks = masks;
            Indices = indices;
            TextureId = textureId;
            ConstantFlags = flags;
            TextureCoordinates = textureCoords;
        }

        public void Update(float opacity, int renderOrder, float[] vertices,  DynamicDrawableFlags flags)
        {
            Opacity = opacity;
            Vertices = vertices;
            RenderOrder = renderOrder;
            DynamicFlags = flags;
        }
    }
}

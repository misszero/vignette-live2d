// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System.Collections.Generic;
using System.Linq;
using osuTK;
using Vignette.Application.Live2D.Id;

namespace Vignette.Application.Live2D.Model
{
    public class CubismDrawable : CubismId
    {
        public readonly int TextureId;

        public readonly int[] Masks;

        public readonly bool Culling;

        public readonly bool Inverse;

        // I don't think Vertices, TextureCoordinates, and Indices should be exposed like this.
        // But for now, since we dont know what to do with it, might as well leave it as is.
        public List<Vector2> Vertices { get; private set; }

        public List<Vector2> TextureCoordinates { get; private set; }

        public short[] Indices { get; private set; }

        public bool Visible { get; private set; }

        public float Opacity { get; private set; }

        public bool OpacityChanged { get; private set; }

        public bool PositionsChanged { get; private set; }

        public bool DrawOrderChanged { get; private set; }

        public bool VisibilityChanged { get; private set; }

        public bool RenderOrderChanged { get; private set; }

        internal CubismDrawable(int index, string name, int textureId, int[] masks, ConstantDrawableFlags constantFlags)
            : base(index, name)
        {
            Masks = masks;
            TextureId = textureId;
            Culling = (constantFlags & ConstantDrawableFlags.IsDoubleSided) == 0;
            Inverse = (constantFlags & ConstantDrawableFlags.IsInvertedMask) == 0;
        }

        public void Update(float opacity, CubismVector2[] vertices, CubismVector2[] textureCoords, short[] indices, DynamicDrawableFlags flags)
        {
            Opacity = opacity;
            Indices = indices;
            Vertices = vertices.Select(v => v.ToVector2()).ToList();
            TextureCoordinates = textureCoords.Select(v => v.ToVector2()).ToList();

            Visible = (flags & DynamicDrawableFlags.Visible) != 0;
            VisibilityChanged = (flags & DynamicDrawableFlags.VisibilityChanged) != 0;
            OpacityChanged = (flags & DynamicDrawableFlags.OpacityChanged) != 0;
            PositionsChanged = (flags & DynamicDrawableFlags.VertexPositionsChanged) != 0;
            DrawOrderChanged = (flags & DynamicDrawableFlags.DrawOrderChanged) != 0;
            RenderOrderChanged = (flags & DynamicDrawableFlags.RenderOrderChanged) != 0;
        }
    }
}

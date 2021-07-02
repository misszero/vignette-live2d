// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace Vignette.Game.Live2D.Graphics
{
    /// <summary>
    /// A drawable that represents a single mesh.
    /// </summary>
    public class CubismDrawable : ICubismId
    {
        /// <summary>
        /// The vertex positions of the drawable used in drawing.
        /// </summary>
        public Vector2[] Positions { get; internal set; }

        /// <summary>
        /// The coordinates of the drawable used in drawing.
        /// </summary>
        public Vector2[] TexturePositions { get; init; }

        /// <summary>
        /// The indices of the drawable used in drawing.
        /// </summary>
        public short[] Indices { get; init; }

        /// <summary>
        /// The render order of this drawable.
        /// </summary>
        public int RenderOrder { get; internal set; }

        /// <summary>
        /// The texture of this drawable.
        /// </summary>
        public Texture Texture { get; internal set; }

        /// <summary>
        /// The rectangular bounds of the drawable.
        /// </summary>
        public RectangleF VertexBounds
        {
            get
            {
                if (Positions == null || Positions.Length <= 0)
                    return RectangleF.Empty;

                float minX = 0;
                float minY = 0;
                float maxX = 0;
                float maxY = 0;

                foreach (var v in Positions)
                {
                    minX = Math.Min(minX, v.X);
                    minY = Math.Min(minY, v.Y);
                    maxX = Math.Max(maxX, v.X);
                    maxY = Math.Max(maxY, v.Y);
                }

                return new RectangleF(minX, minY, maxX - minX, maxY - minY);
            }
        }

        /// <summary>
        /// Whether this drawable is double-sided.
        /// </summary>
        public bool IsDoubleSided { get; internal set; }

        /// <summary>
        /// Whether this drawable uses an inverted mask.
        /// </summary>
        public bool IsInvertedMask { get; internal set; }

        /// <summary>
        /// The blending parameters used in drawing this drawable.
        /// </summary>
        public BlendingParameters Blending { get; internal set; }

        /// <summary>
        /// The alpha component of this drawable.
        /// </summary>
        public float Alpha
        {
            get => Colour.A;
            set => Colour = Colour.Opacity(value);
        }

        private Colour4 colour = Colour4.White;

        internal event Action ColourChanged;

        /// <summary>
        /// The colour of this drawable.
        /// </summary>
        public Colour4 Colour
        {
            get => colour;
            set
            {
                if (Colour.Equals(value))
                    return;

                // Used to check whether it is necessary to invoke the event by ugnoring the alpha value
                // as the cubism framework handles invoking invalidations on alpha change.
                var prev = colour.Opacity(1);
                var next = value.Opacity(1);

                colour = value;

                if (!prev.Equals(next))
                    ColourChanged?.Invoke();
            }
        }

        /// <summary>
        /// The context used to mask this drawable.
        /// </summary>
        internal MaskingContext MaskingContext { get; set; }

        public string Name { get; set; }

        public int ID { get; set; }
    }
}

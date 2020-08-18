// Copyright (c) Nitrous <n20gaming2000@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osuTK;

namespace osu.Framework.Graphics.Cubism.Renderer
{
    public partial class CubismRenderer
    {
        public float X;
        public float Y;
        public Vector2 Translation
        {
            get => new Vector2(X, Y);
            set
            {
                if (value == Translation) return;

                X = value.X;
                Y = value.Y;
            }
        }

        public Vector2 Scale = Vector2.One;

        public Colour4 Colour = Colour4.White;
    }
}
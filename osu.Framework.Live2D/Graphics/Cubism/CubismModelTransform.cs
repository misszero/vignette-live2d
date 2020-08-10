using System;
using System.Diagnostics.CodeAnalysis;
using osuTK;

namespace osu.Framework.Graphics.Cubism
{
    public struct CubismModelTransform : IEquatable<CubismModelTransform>
    {
        public Vector2 Scale;
        public Vector2 Position;

        public bool Equals([AllowNull] CubismModelTransform other) =>
            Scale == other.Scale && Position == other.Position;
    }
}
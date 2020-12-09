// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System.Runtime.InteropServices;
using osuTK;

namespace Vignette.Application.Live2D
{
    // TODO: Test if we can use osuTK Vector2 instead
    [StructLayout(LayoutKind.Sequential)]
    public struct CubismVector2
    {
        public float X;

        public float Y;
    }

    public static class CubismVector2Extensions
    {
        public static Vector2 ToVector2(this CubismVector2 v) => new Vector2(v.X, v.Y);
    }
}

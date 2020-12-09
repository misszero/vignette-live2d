// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System;

namespace Vignette.Application.Live2D.Utils
{
    public static class CubismMath
    {
        public static float EaseSine(float t) => Math.Clamp(0.5f - 0.5f * MathF.Cos(MathF.PI * t), 0, 1);
    }
}

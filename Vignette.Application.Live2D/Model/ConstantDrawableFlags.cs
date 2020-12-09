// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System;

namespace Vignette.Application.Live2D.Model
{
    [Flags]
    public enum ConstantDrawableFlags : byte
    {
        BlendNormal = 0x0,

        BlendAdditive = 0x1,

        BlendMultiplicative = 0x2,

        IsDoubleSided = 0x4,

        IsInvertedMask = 0x8,
    }
}

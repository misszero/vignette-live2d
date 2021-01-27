// Copyright (c) Vignette Project.
// Licensed under MIT. Please see LICENSE for more details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

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

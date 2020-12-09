// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System;

namespace Vignette.Application.Live2D.Model
{
    [Flags]
    public enum DynamicDrawableFlags : byte
    {
        Visible = 0x1,

        VisibilityChanged = 0x2,

        OpacityChanged = 0x4,

        DrawOrderChanged = 0x8,

        RenderOrderChanged = 0x10,

        VertexPositionsChanged = 0x20
    }
}

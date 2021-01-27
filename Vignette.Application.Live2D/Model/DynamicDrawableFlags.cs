// Copyright (c) Vignette Project.
// Licensed under MIT. Please see LICENSE for more details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

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

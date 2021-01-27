// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System.Collections.Generic;
using Vignette.Application.Live2D.Model;

namespace Vignette.Application.Live2D.Motion.Pose
{
    public class PartInfo
    {
        public CubismPart Parent { get; private set; }

        public IEnumerable<CubismPart> Children { get; set; }

        public PartInfo(CubismPart part)
        {
            Parent = part;
            Parent.Target = 1.0f;
        }
    }
}

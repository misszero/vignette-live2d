// Copyright (c) Vignette Project.
// Licensed under MIT. Please see LICENSE for more details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

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

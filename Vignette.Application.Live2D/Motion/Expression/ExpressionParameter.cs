// Copyright (c) Vignette Project.
// Licensed under MIT. Please see LICENSE for more details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vignette.Application.Live2D.Model;

namespace Vignette.Application.Live2D.Motion.Expression
{
    public struct ExpressionParameter
    {
        public CubismParameter Parameter { get; set; }

        public ExpressionBlending Blending { get; set; }

        public double Value { get; set; }
    }
}

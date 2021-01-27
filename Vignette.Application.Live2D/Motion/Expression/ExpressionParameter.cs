// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

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

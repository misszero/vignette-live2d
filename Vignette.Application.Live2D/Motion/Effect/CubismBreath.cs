// Copyright (c) Vignette Project.
// Licensed under MIT. Please see LICENSE for more details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using System;
using System.Collections.Generic;
using Vignette.Application.Live2D.Model;

namespace Vignette.Application.Live2D.Motion.Effect
{
    public class CubismBreath
    {
        private readonly List<(CubismParameter, float, float, float, float)> settings = new List<(CubismParameter, float, float, float, float)>();

        public void SetParameter(CubismParameter parameter, float offset, float peak, float cycle, float weight)
        {
            settings.Add((parameter, offset, peak, cycle, weight));
        }

        public void Update(double time)
        {
            float phase = (float)time * 2.0f * MathF.PI;
            foreach (var setting in settings)
            {
                (CubismParameter param, float offset, float peak, float cycle, float weight) = setting;
                param.Value += offset + peak * MathF.Sin(phase / cycle) * weight;
            }
        }
    }
}

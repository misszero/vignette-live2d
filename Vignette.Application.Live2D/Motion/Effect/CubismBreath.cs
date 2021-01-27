// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

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

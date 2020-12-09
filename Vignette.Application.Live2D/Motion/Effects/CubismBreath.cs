// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System;
using System.Collections.Generic;
using Vignette.Application.Live2D.Model;

namespace Vignette.Application.Live2D.Motion.Effect
{
    public class CubismBreath : ICanUpdateParameter
    {
        public double Weight { get; set; }

        private readonly List<CubismBreathParameter> breathParameters = new List<CubismBreathParameter>();

        public CubismBreath(CubismModel model)
        {
            addParameter(new CubismBreathParameter(model.Parameters.Get("ParamAngleX"), 0, 15, 6.5345f, 0.5f));
            addParameter(new CubismBreathParameter(model.Parameters.Get("ParamAngleY"), 0, 8, 3.5345f, 0.5f));
            addParameter(new CubismBreathParameter(model.Parameters.Get("ParamAngleZ"), 0, 10, 5.5345f, 0.5f));
            addParameter(new CubismBreathParameter(model.Parameters.Get("ParamBreath"), 0.5f, 0.5f, 3.2345f, 0.5f));
            addParameter(new CubismBreathParameter(model.Parameters.Get("ParamBodyAngleX"), 0, 4, 15.5345f, 0.5f));
        }

        private void addParameter(CubismBreathParameter parameter)
        {
            if (!breathParameters.Contains(parameter))
                breathParameters.Add(parameter);
        }

        public void Update(float time, bool loop)
        {
            float phase = time * 2 * MathF.PI;
            foreach (var param in breathParameters)
            {
                float value = param.Offset + param.Peak * MathF.Sin(phase / param.Cycle);
                float current = param.Parameter.Value;
                param.Parameter.Value = current + value * param.Weight;
            }
        }
    }
}

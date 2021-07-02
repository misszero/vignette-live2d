// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using osu.Framework.Allocation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Vignette.Game.Live2D.Graphics.Controllers
{
    public class CubismBreathController : CubismController
    {
        private readonly List<(CubismParameter, float, float, float, float)> settings = new List<(CubismParameter, float, float, float, float)>();

        [BackgroundDependencyLoader]
        private void load()
        {
            SetParameter(Model.Parameters.FirstOrDefault(p => p.Name == "ParamAngleX"), 0.0f, 15.0f, 6.5345f, 0.5f);
            SetParameter(Model.Parameters.FirstOrDefault(p => p.Name == "ParamAngleY"), 0.0f, 8.0f, 3.5345f, 0.5f);
            SetParameter(Model.Parameters.FirstOrDefault(p => p.Name == "ParamAngleZ"), 0.0f, 10.0f, 5.5345f, 0.5f);
            SetParameter(Model.Parameters.FirstOrDefault(p => p.Name == "ParamBodyAngleX"), 0.0f, 4.0f, 15.5345f, 0.5f);
            SetParameter(Model.Parameters.FirstOrDefault(p => p.Name == "ParamBreath"), 0.5f, 0.5f, 3.2345f, 0.5f);
        }

        public void SetParameter(CubismParameter parameter, float offset, float peak, float cycle, float weight)
        {
            if (parameter == null)
                return;

            settings.Add((parameter, offset, peak, cycle, weight));
        }

        protected override void Update()
        {
            float phase = (float)Clock.ElapsedFrameTime / 1000 * 2.0f * MathF.PI;
            foreach (var setting in settings)
            {
                (CubismParameter p, float offset, float peak, float cycle, float weight) = setting;
                float sin = MathF.Sin(phase / cycle);
                p.Value += (offset + peak * sin) * weight;
            }
        }
    }
}

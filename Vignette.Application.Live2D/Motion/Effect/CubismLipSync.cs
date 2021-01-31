// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using osu.Framework.Audio.Sample;
using Vignette.Application.Live2D.Model;

namespace Vignette.Application.Live2D.Motion.Effect
{
    public class CubismLipSync
    {
        public ISampleChannel Sample { get; set; }

        private readonly CubismParameter parameter;

        public CubismLipSync(CubismParameter parameter)
        {
            this.parameter = parameter;
        }

        public void Update()
        {
            parameter.Value = parameter.Maximum * (Sample?.CurrentAmplitudes.Average ?? 0.0f);
        }
    }
}

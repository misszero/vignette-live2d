// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System.Linq;
using osu.Framework.Audio.Sample;
using Vignette.Application.Live2D.Json;
using Vignette.Application.Live2D.Model;

namespace Vignette.Application.Live2D.Motion.Effect
{
    public class CubismLipSync : ICanUpdateParameter
    {
        // TODO: Should we really use SampleChannel here?
        public SampleChannel Sample { get; set; }

        public double Weight { get; set; }

        private readonly CubismParameter parameter;

        public CubismLipSync(CubismModel model, CubismModelSetting settings)
        {
            parameter = model.Parameters.Get(settings.Groups.FirstOrDefault(g => g.Name == "LipSync").Ids.First());
        }

        public CubismLipSync(CubismParameter parameter)
        {
            this.parameter = parameter;
        }

        public void Update(float time, bool loop)
        {
            if (parameter == null)
                return;

            parameter.Value = Sample.CurrentAmplitudes.Average;
        }
    }
}

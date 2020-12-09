// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using Vignette.Application.Live2D.Model;

namespace Vignette.Application.Live2D.Motion.Effect
{
    public class CubismBreathParameter
    {
        public CubismParameter Parameter { get; private set; }

        public float Offset { get; private set; }

        public float Peak { get; private set; }

        public float Cycle { get; private set; }

        public float Weight { get; private set; }

        public CubismBreathParameter(CubismParameter parameter, float offset, float peak, float cycle, float weight)
        {
            Parameter = parameter;
            Offset = offset;
            Peak = peak;
            Cycle = cycle;
            Weight = weight;
        }
    }
}

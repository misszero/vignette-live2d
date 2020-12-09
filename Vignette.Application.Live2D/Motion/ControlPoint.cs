// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

namespace Vignette.Application.Live2D.Motion
{
    public struct ControlPoint
    {
        public float Time { get; set; }

        public float Value { get; set; }

        public ControlPoint(float time, float value)
        {
            Time = time;
            Value = value;
        }
    }
}

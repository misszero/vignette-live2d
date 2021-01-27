// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

namespace Vignette.Application.Live2D.Motion
{
    public struct ControlPoint
    {
        public double Time { get; set; }

        public double Value { get; set; }

        public ControlPoint(double time, double value)
        {
            Time = time;
            Value = value;
        }
    }
}

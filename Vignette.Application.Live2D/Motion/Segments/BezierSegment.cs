// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System;

namespace Vignette.Application.Live2D.Motion.Segments
{
    public class BezierSegment : Segment
    {
        public BezierSegment()
            : base(4)
        {
        }

        public override float Evaluate(float time)
        {
            float t = Math.Max((time - Points[0].Time) / (Points[3].Time - Points[0].Time), 0);
            ControlPoint p01 = lerp(t, Points[0], Points[1]);
            ControlPoint p12 = lerp(t, Points[1], Points[2]);
            ControlPoint p23 = lerp(t, Points[2], Points[3]);
            ControlPoint p012 = lerp(t, p01, p12);
            ControlPoint p123 = lerp(t, p12, p23);
            return lerp(t, p012, p123).Value;
        }

        private ControlPoint lerp(float time, ControlPoint a, ControlPoint b) => new ControlPoint
        {
            Time = a.Time + ((b.Time - a.Time) * time),
            Value = a.Value + ((b.Value - a.Value) * time),
        };
    }
}

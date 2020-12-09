// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System;

namespace Vignette.Application.Live2D.Motion.Segments
{
    public class LinearSegment : Segment
    {
        public LinearSegment()
            : base(2)
        {
        }

        public override float Evaluate(float time)
        {
            float t = Math.Max((time - Points[0].Time) / (Points[1].Time - Points[0].Time), 0);
            return Points[0].Value + ((Points[1].Value - Points[0].Value) * t);
        }
    }
}

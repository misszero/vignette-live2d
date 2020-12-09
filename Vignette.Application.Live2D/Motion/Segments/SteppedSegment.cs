// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

namespace Vignette.Application.Live2D.Motion.Segments
{
    public class SteppedSegment : Segment
    {
        public SteppedSegment()
            : base(2)
        {
        }

        public override float Evaluate(float time) => Points[0].Value;
    }
}

// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

namespace Vignette.Application.Live2D.Motion.Segments
{
    public abstract class Segment : ISegment
    {
        public ControlPoint[] Points { get; protected set; }

        public Segment(int points)
        {
            Points = new ControlPoint[points];
        }

        public abstract double ValueAt(double time);
    }
}

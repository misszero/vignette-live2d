// Copyright (c) Vignette Project.
// Licensed under MIT. Please see LICENSE for more details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

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

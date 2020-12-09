// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System.Linq;
using Vignette.Application.Live2D.Motion.Segments;

namespace Vignette.Application.Live2D.Motion
{
    public abstract class Curve
    {
        public MotionTarget Target { get; set; }

        public Segment[] Segments { get; set; }

        public float FadeInTime { get; set; }

        public float FadeOutTime { get; set; }

        public float Evaluate(float time)
        {
            foreach (var segment in Segments)
            {
                if (time <= segment.Points.Last().Time)
                {
                    if (segment.Points[0].Time <= time)
                        return segment.Evaluate(time);
                    else
                        return segment.Points[0].Value;
                }
            }

            return Segments.Last().Points.Last().Value;
        }
    }

    public class Curve<T> : Curve
        where T : class
    {
        public T Effect { get; set; }
    }
}

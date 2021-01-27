// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

namespace Vignette.Application.Live2D.Motion.Segments
{
    public interface ISegment
    {
        double ValueAt(double time);
    }
}

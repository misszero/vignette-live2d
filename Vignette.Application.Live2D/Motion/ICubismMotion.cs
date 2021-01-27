// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

namespace Vignette.Application.Live2D.Motion
{
    public interface ICubismMotion
    {
        public double GlobalFadeInSeconds { get; set; }

        public double GlobalFadeOutSeconds { get; set; }

        public double Weight { get; set; }

        public void Update(double time, bool loop = false);
    }
}

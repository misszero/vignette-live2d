// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

namespace Vignette.Application.Live2D.Motion
{
    public interface ICubismMotion
    {
        float GlobalFadeInSeconds { get; set; }

        float GlobalFadeOutSeconds { get; set; }

        float Weight { get; set; }
    }
}

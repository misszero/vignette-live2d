// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

namespace Vignette.Application.Live2D.Json
{
    public class CubismExpressionSetting : ICubismJsonSetting
    {
        public int Version { get; set; }

        public string Type { get; set; }

        public float FadeInTime { get; set; } = 1.0f;

        public float FadeOutTime { get; set; } = 1.0f;

        public Parameter[] Parameters { get; set; }

        public class Parameter
        {
            public string Id { get; set; }

            public double Value { get; set; }

            public string Blend { get; set; }
        }
    }
}

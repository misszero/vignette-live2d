// Copyright (c) Vignette Project.
// Licensed under MIT. Please see LICENSE for more details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using System.Collections.Generic;

namespace Vignette.Application.Live2D.Json
{
    public class CubismExpressionSetting : ICubismJsonSetting
    {
        public int Version { get; set; }

        public string Type { get; set; }

        public float FadeInTime { get; set; } = 1.0f;

        public float FadeOutTime { get; set; } = 1.0f;

        public List<Parameter> Parameters { get; set; }

        public class Parameter
        {
            public string Id { get; set; }

            public double Value { get; set; }

            public string Blend { get; set; }
        }
    }
}

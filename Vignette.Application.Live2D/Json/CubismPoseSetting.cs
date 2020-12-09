// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System.Collections.Generic;

namespace Vignette.Application.Live2D.Json
{
    public class CubismPoseSetting : ICubismJsonSetting
    {
        public int Version { get; set; }

        public string Type { get; set; }

        public float FadeInTime { get; set; }

        public List<List<string>> Groups { get; set; } = new List<List<string>>();

        public class Group
        {
            public string Id { get; set; }

            public List<string> Link { get; set; } = new List<string>();
        }
    }
}

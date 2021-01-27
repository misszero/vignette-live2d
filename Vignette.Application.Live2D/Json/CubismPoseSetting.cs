// Copyright (c) Vignette Project.
// Licensed under MIT. Please see LICENSE for more details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using System.Collections.Generic;

namespace Vignette.Application.Live2D.Json
{
    public class CubismPoseSetting : ICubismJsonSetting
    {
        public int Version { get; set; }

        public string Type { get; set; }

        public float FadeInTime { get; set; }

        public List<List<Group>> Groups { get; set; } = new List<List<Group>>();

        public class Group
        {
            public string Id { get; set; }

            public List<string> Link { get; set; } = new List<string>();
        }
    }
}

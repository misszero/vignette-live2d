﻿// Copyright (c) Vignette Project.
// Licensed under MIT. Please see LICENSE for more details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using System.Collections.Generic;

namespace Vignette.Application.Live2D.Json
{
    public class CubismModelSetting : ICubismJsonSetting
    {
        public int Version { get; set; }

        public List<Group> Groups { get; set; }

        public FileReference FileReferences { get; set; }

        public List<HitArea> HithAreas { get; set; }

        public Dictionary<string, double> Layout { get; set; }

        public class FileReference
        {
            public string Moc { get; set; }

            public string DisplayInfo { get; set; }

            public Dictionary<string, List<Motion>> Motions { get; set; }

            public Dictionary<string, List<Expression>> Expressions { get; set; }

            public List<string> Textures { get; set; }

            public string Physics { get; set; }

            public string Pose { get; set; }

            public string UserData { get; set; }

            public class Motion
            {
                public string File { get; set; }

                public float FadeInTime { get; set; }

                public float FadeOutTime { get; set; }
            }

            public class Expression
            {
                public string Name { get; set; }

                public string File { get; set; }
            }
        }

        public class Group
        {
            public string Target { get; set; }

            public string Name { get; set; }

            public List<string> Ids { get; set; }
        }

        public class HitArea
        {
            public string Name { get; set; }

            public string Id { get; set; }
        }
    }
}

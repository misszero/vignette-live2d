// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

namespace Vignette.Application.Live2D.Json
{
    public class CubismUserDataSetting : ICubismJsonSetting
    {
        public int Version { get; set; }

        public Metadata Meta { get; set; }

        public Userdata UserData { get; set; }

        public class Metadata
        {
            public int UserDataCount { get; set; }

            public int TotalUserDataSize { get; set; }
        }

        public class Userdata
        {
            public string Target { get; set; }

            public string Id { get; set; }

            public string Value { get; set; }
        }
    }
}

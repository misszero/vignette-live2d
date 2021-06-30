// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using System.IO;
using osu.Framework.IO.Stores;

namespace Vignette.Game.Live2D.Tests.Resources
{
    public static class TestResources
    {
        public static DllResourceStore GetStore() => new DllResourceStore(typeof(TestResources).Assembly);

        public static Stream GetStream(string name) => GetStore().GetStream($"Resources/{name}");

        public static Stream GetModelResource(string name) => GetStream($"Model/{name}");
    }
}

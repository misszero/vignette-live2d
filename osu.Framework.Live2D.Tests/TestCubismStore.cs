// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System.IO;
using osu.Framework.Graphics.Cubism;
using osu.Framework.IO.Stores;

namespace osu.Framework.Live2D.Tests
{
    public class TestCubismStore : CubismStore
    {
        public TestCubismStore(IResourceStore<byte[]> store = null)
            : base(store)
        {
        }

        protected override Stream GetResource(string path) => GetStream(path);
    }
}

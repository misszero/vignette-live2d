// Copyright (c) Nitrous <n20gaming2000@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.IO;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Cubism;
using osu.Framework.IO.Stores;

namespace osu.Framework.Live2D.Tests
{
    public class TestGame : Game
    {
        private DependencyContainer dependencies;
        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        [BackgroundDependencyLoader]
        private void load()
        {
            // Make sure to add resources included in the library as it contains necessary shaders to run Live2D assets.
            Resources.AddStore(new NamespacedResourceStore<byte[]>(new DllResourceStore(CubismResources.ResourceAssembly), "Resources"));
            Resources.AddStore(new NamespacedResourceStore<byte[]>(new DllResourceStore(typeof(TestGame).Assembly), "Resources"));

            dependencies.CacheAs<CubismStore>(new TestCubismStore(new NamespacedResourceStore<byte[]>(Resources, "Models")));
        }

        private class TestCubismStore : CubismStore
        {
            public TestCubismStore(IResourceStore<byte[]> store = null)
                : base(store)
            {
            }

            protected override Stream GetResource(string path) => GetStream(path);
        }
    }
}
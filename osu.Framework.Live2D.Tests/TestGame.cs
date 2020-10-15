// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

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
    }
}

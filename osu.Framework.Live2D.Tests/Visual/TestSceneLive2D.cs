using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cubism;
using osu.Framework.Testing;
using osuTK;

namespace osu.Framework.Live2D.Tests.Visuals
{
    public class TestSceneLive2D : TestScene
    {
        [BackgroundDependencyLoader]
        private void load(CubismAssetStore cubismAssets)
        {
            Add(new CubismModelContainer(cubismAssets.Get("hiyori.hiyori_free_t06.model3.json"))
            {
                Size = new Vector2(512),
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
            });
        }
    }
}
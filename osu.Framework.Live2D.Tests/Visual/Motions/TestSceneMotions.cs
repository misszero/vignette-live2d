using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cubism;
using osu.Framework.Testing;
using osuTK;

namespace osu.Framework.Live2D.Tests.Visual.Motions
{
    public abstract class TestSceneMotions : TestScene
    {
        protected CubismSprite Sprite;

        [BackgroundDependencyLoader]
        private void load(CubismAssetStore cubismAssets)
        {
            Add(Sprite = new CubismSprite
            {
                Size = new Vector2(512),
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
                Asset = cubismAssets.Get("hiyori.hiyori_free_t06.model3.json"),
                ModelScale = 2.5f,
                ModelOffsetY = 50
            });
        }
    }
}
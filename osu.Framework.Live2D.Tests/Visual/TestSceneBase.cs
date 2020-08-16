using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cubism;
using osu.Framework.Graphics.Cubism.Renderer;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Testing;
using osuTK;

namespace osu.Framework.Live2D.Tests.Visual
{
    public abstract class TestSceneBase : TestScene
    {
        protected Container<Drawable> Container;
        protected CubismSprite Sprite;
        protected virtual Colour4 BackgroundColour => Colour4.Transparent;

        [BackgroundDependencyLoader]
        private void load(CubismAssetStore cubismAssets)
        {
            Add(Container = new Container<Drawable>
            {
                Size = new Vector2(684),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = BackgroundColour
                    },
                    Sprite = new CubismSprite
                    {
                        RelativeSizeAxes = Axes.Both,
                        Asset = cubismAssets.Get("hiyori.hiyori_free_t06.model3.json"),
                        Renderer = new CubismRenderer
                        {
                            Scale = new Vector2(2),
                            Y = 384
                        }
                    }
                }
            });
        }
    }
}
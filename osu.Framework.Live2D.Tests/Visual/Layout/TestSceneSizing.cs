using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cubism;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Testing;
using osuTK;

namespace osu.Framework.Live2D.Tests.Visual.Layout
{
    public class TestSceneSizing : TestScene
    {
        private Container<Drawable> container;
        private CubismSprite sprite;

        [BackgroundDependencyLoader]
        private void load(CubismAssetStore cubismAssets)
        {
            Add(container = new Container<Drawable>
            {
                Size = new Vector2(512),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Colour4.Red
                    },
                    sprite = new CubismSprite
                    {
                        RelativeSizeAxes = Axes.Both,
                        Asset = cubismAssets.Get("hiyori.hiyori_free_t06.model3.json"),
                        CanBreathe = false,
                        CanEyeBlink = false
                    }
                }
            });

            AddLabel("container");

            AddSliderStep<float>("width", 128, 684, 512, (float w) => container.Width = w);
            AddSliderStep<float>("height", 128, 684, 512, (float h) => container.Height = h);

            AddLabel("model");

            AddSliderStep<float>("scale", 0.5f, 2, 1, (float s) => sprite.ModelScale = s);
            AddSliderStep<float>("x position", -128, 128, 0, (float x) => sprite.ModelOffsetX = x);
            AddSliderStep<float>("y position", -128, 128, 0, (float y) => sprite.ModelOffsetY = y);
        }
    }
}
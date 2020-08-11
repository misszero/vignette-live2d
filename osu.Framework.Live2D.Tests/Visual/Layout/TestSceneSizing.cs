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
                        Origin = Anchor.Centre,
                        Anchor = Anchor.Centre,
                        Asset = cubismAssets.Get("hiyori.hiyori_free_t06.model3.json"),
                        CanBreathe = false,
                        CanEyeBlink = false
                    }
                }
            });

            AddSliderStep<float>("width", 128, 684, 512, (float w) => container.Width = w);
            AddSliderStep<float>("height", 128, 684, 512, (float h) => container.Height = h);

            AddSliderStep<float>("model scale", 0.5f, 2, 1, (float s) =>
                sprite.ModelTransform = new CubismModelTransform { Scale = new Vector2(s), Position = sprite.ModelTransform.Position });

            AddSliderStep<float>("model x position", -128, 128, 0, (float x) =>
                sprite.ModelTransform = new CubismModelTransform 
                { 
                    Scale = sprite.ModelTransform.Scale,
                    Position = new Vector2(x, sprite.ModelTransform.Position.Y)
                }
            );

            AddSliderStep<float>("model y position", -128, 128, 0, (float y) =>
                sprite.ModelTransform = new CubismModelTransform 
                { 
                    Scale = sprite.ModelTransform.Scale,
                    Position = new Vector2(sprite.ModelTransform.Position.X, y)
                }
            );
        }
    }
}
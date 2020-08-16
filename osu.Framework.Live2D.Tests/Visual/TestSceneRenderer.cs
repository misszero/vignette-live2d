using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cubism;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace osu.Framework.Live2D.Tests.Visual
{
    public class TestSceneRenderer : TestSceneBase
    {
        protected override Colour4 BackgroundColour => Colour4.Red;

        [BackgroundDependencyLoader]
        private void load(CubismAssetStore cubismAssets)
        {
            Container.Size = new Vector2(512);
            Sprite.ModelPositionY = 0;
            Sprite.Scale = Vector2.One;

            AddLabel("container");

            AddSliderStep<float>("width", 128, 684, 512, (float w) => Container.Width = w);
            AddSliderStep<float>("height", 128, 684, 512, (float h) => Container.Height = h);

            AddLabel("model");

            AddSliderStep<float>("scale", 0.5f, 2, 1, (float s) => Sprite.ModelScale = new Vector2(s));
            AddSliderStep<float>("x position", -128, 128, 0, (float x) => Sprite.ModelPositionX = x);
            AddSliderStep<float>("y position", -128, 128, 0, (float y) => Sprite.ModelPositionY = y);
        }
    }
}
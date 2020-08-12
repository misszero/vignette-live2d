using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cubism;
using osu.Framework.Testing;
using osuTK;

namespace osu.Framework.Live2D.Tests.Visual.Motions
{
    [System.ComponentModel.Description("Breathing and Eyeblinking")]
    public class TestSceneMotionEffects : TestSceneMotions
    {

        [BackgroundDependencyLoader]
        private void load(CubismAssetStore cubismAssets)
        {
            AddToggleStep("toggle breathing", (bool enabled) => Sprite.CanBreathe = enabled);
            AddToggleStep("toggle blinking", (bool enabled) => Sprite.CanEyeBlink = enabled);
        }
    }
}
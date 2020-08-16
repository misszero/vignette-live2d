using osu.Framework.Allocation;

namespace osu.Framework.Live2D.Tests.Visual
{
    public class TestSceneMotionEffects : TestSceneBase
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            AddToggleStep("toggle breathing state", (bool s) => Sprite.CanBreathe = s);
            AddToggleStep("toggle eyeblink state", (bool s) => Sprite.CanEyeBlink = s);
        }
    }
}
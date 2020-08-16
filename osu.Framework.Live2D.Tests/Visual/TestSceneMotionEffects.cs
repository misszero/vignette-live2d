using osu.Framework.Allocation;
using osu.Framework.Audio;

namespace osu.Framework.Live2D.Tests.Visual
{
    [System.ComponentModel.Description("Motions independent from queues")]
    public class TestSceneMotionEffects : TestSceneBase
    {
        [BackgroundDependencyLoader]
        private void load(AudioManager audio)
        {
            AddLabel("Breathing");
            AddStep("set breathing true", () => Sprite.CanBreathe = true);
            AddAssert("is moving", () => Sprite.IsMoving);
            AddStep("set breathing false", () => Sprite.CanBreathe = false);
            AddAssert("is not moving", () => !Sprite.IsMoving);

            AddLabel("Blinking");
            AddStep("set eyeblink true", () => Sprite.CanEyeBlink = true);
            AddAssert("is moving", () => Sprite.IsMoving);
            AddStep("set eyeblink false", () => Sprite.CanEyeBlink = false);
            AddAssert("is not moving", () => !Sprite.IsMoving);
        }
    }
}
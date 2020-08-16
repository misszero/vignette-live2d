using CubismFramework;
using osu.Framework.Allocation;

namespace osu.Framework.Live2D.Tests.Visual
{
    public class TestSceneMotionQueue : TestSceneBase
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            AddStep("enqueue motion", () => Sprite.StartMotion("", 0, CubismAsset.MotionType.Base));
            AddRepeatStep("enqueue motions", () => Sprite.StartMotion("", 0, CubismAsset.MotionType.Base), 5);
            AddRepeatStep("skip motions", () => Sprite.NextMotion(), 3);
            AddStep("enqueue forced motion", () => Sprite.StartMotion("", 1, CubismAsset.MotionType.Base, true));
            AddStep("enqueue looping motion", () => Sprite.StartMotion("", 2, CubismAsset.MotionType.Base, true, true));
            AddStep("stop motion", () => Sprite.StopMotion());
        }
    }
}
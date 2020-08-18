using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cubism;
using osu.Framework.Input.Events;
using osuTK;

namespace osu.Framework.Live2D.Tests.Visual
{
    [System.ComponentModel.Description("Look at a point")]
    public class TestSceneLook : TestSceneBase
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            Sprite.LookType = CubismLookType.Hover;
            Sprite.CanBreathe = true;
            Add(new ParameterMonitor(Sprite, CubismSprite.PARAMS_BREATH));
        }
    }
}
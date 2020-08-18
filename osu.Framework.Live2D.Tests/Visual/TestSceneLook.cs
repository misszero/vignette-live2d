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
            Add(new ParameterMonitor(Sprite, CubismSprite.PARAMS_BREATH));

            AddStep("effects", () => Sprite.CanBreathe = Sprite.CanEyeBlink = true);
            AddStep("disable", () => Sprite.LookType = CubismLookType.None);
            AddStep("drag mode", () => Sprite.LookType = CubismLookType.Drag);
            AddStep("hover mode", () => Sprite.LookType = CubismLookType.Hover);
        }
    }
}
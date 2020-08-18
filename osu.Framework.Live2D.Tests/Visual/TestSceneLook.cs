// Copyright (c) Nitrous <n20gaming2000@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics.Cubism;

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
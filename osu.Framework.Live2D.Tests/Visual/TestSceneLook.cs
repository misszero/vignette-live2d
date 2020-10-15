// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using osu.Framework.Graphics.Cubism;

namespace osu.Framework.Live2D.Tests.Visual
{
    [System.ComponentModel.Description("Look at a point")]
    public class TestSceneLook : CubismTestScene
    {
        protected override void LoadComplete()
        {
            AddParameterTracker(CubismSprite.PARAMS_BREATH);
            AddStep("disable", () => Sprite.LookType = CubismLookType.None);
            AddStep("drag mode", () => Sprite.LookType = CubismLookType.Drag);
            AddStep("hover mode", () => Sprite.LookType = CubismLookType.Hover);
        }
    }
}

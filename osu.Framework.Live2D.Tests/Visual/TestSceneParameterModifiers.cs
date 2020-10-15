// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System.Linq;

namespace osu.Framework.Live2D.Tests.Visual
{
    [System.ComponentModel.Description("Sandbox for modifying a model's parameters")]
    public class TestSceneParameterModifiers : CubismTestScene
    {
        protected override void LoadComplete()
        {
            foreach (var param in Parameters.Where(p => Sprite.HasParameter(p)))
                AddSliderStep<float>(param, 0, 1, 0, (newValue) => Sprite.SetParameterValue(param, newValue));
        }
    }
}

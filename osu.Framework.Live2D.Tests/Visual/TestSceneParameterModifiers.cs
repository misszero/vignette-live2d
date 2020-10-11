// Copyright (c) Nitrous <n20gaming2000@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;

namespace osu.Framework.Live2D.Tests.Visual
{
    [System.ComponentModel.Description("Modify a model's parameters")]
    public class TestSceneParameterModifiers : CubismTestScene
    {
        protected override void LoadComplete()
        {
            foreach (var param in Parameters.Where(p => Sprite.HasParameter(p)))
                AddSliderStep<float>(param, 0, 1, 0, (newValue) => Sprite.SetParameterValue(param, newValue));
        }
    }
}
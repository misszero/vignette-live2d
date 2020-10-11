// Copyright (c) Nitrous <n20gaming2000@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics.Cubism;

namespace osu.Framework.Live2D.Tests.Visual
{
    [System.ComponentModel.Description("Motions independent from queues")]
    public class TestSceneMotionEffects : CubismTestScene
    {
        [BackgroundDependencyLoader]
        private void load(AudioManager audio)
        {
            AddLabel("Breathing");
            AddStep("set breathing true", () => Sprite.Breathing = true);
            AddAssert("is moving", () => Sprite.IsMoving);
            AddStep("set breathing false", () => Sprite.Breathing = false);
            AddAssert("is not moving", () => !Sprite.IsMoving);

            AddLabel("Blinking");
            AddStep("set eyeblink true", () => Sprite.Blinking = true);
            AddAssert("is moving", () => Sprite.IsMoving);
            AddStep("set eyeblink false", () => Sprite.Blinking = false);
            AddAssert("is not moving", () => !Sprite.IsMoving);

            string[] parameters = CubismSprite.PARAMS_EYE.Concat(CubismSprite.PARAMS_BREATH).ToArray();
            Add(new ParameterMonitor(Sprite, parameters));
        }
    }
}
// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System.Linq;
using NUnit.Framework;
using osu.Framework.Graphics.Cubism;

namespace osu.Framework.Live2D.Tests.Visual
{
    [System.ComponentModel.Description("Motions independent from queues")]
    public class TestSceneMotionEffects : CubismTestScene
    {
        protected override void LoadComplete()
        {
            AddParameterTracker(CubismSprite.PARAMS_EYE.Concat(CubismSprite.PARAMS_BREATH).ToArray());
        }

        [Test]
        public void TestBreathing()
        {
            AddStep("set breathing true", () => Sprite.Breathing = true);
            AddAssert("is moving", () => Sprite.IsMoving);
            AddStep("set breathing false", () => Sprite.Breathing = false);
            AddAssert("is not moving", () => !Sprite.IsMoving);
        }

        [Test]
        public void TestBlinking()
        {
            AddStep("set eyeblink true", () => Sprite.Blinking = true);
            AddAssert("is moving", () => Sprite.IsMoving);
            AddStep("set eyeblink false", () => Sprite.Blinking = false);
            AddAssert("is not moving", () => !Sprite.IsMoving);
        }
    }
}

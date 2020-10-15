// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using CubismFramework;
using NUnit.Framework;

namespace osu.Framework.Live2D.Tests.Visual
{
    [System.ComponentModel.Description("Add or remove motions in queue")]
    public class TestSceneMotionQueue : CubismTestScene
    {
        protected override void LoadComplete()
        {
            AddParameterTracker(Parameters);
        }

        [Test]
        public void TestSimple()
        {
            AddStep("enqueue motion", () => Sprite.StartMotion("Idle", 0, MotionType.Base));
            AddAssert("check motion", () => Sprite.IsMoving);
            AddStep("stop queue", () => Sprite.StopMotion());
        }

        [Test]
        public void TestQueueing()
        {
            AddRepeatStep("enqueue motions", () => Sprite.StartMotion("Idle", 0, MotionType.Base), 5);
            AddAssert("check queue count", () => Sprite.BaseMotionsQueued == 4);
            AddRepeatStep("skip motions", () => Sprite.NextMotion(), 4);
            AddAssert("check queue if empty", () => Sprite.BaseMotionsQueued == 0);
        }

        [Test]
        public void TestForced()
        {
            AddRepeatStep("enqueue motions", () => Sprite.StartMotion("Idle", 0, MotionType.Base), 5);
            AddStep("enqueue forced motion", () => Sprite.StartMotion("Idle", 1, MotionType.Base, true));
            AddAssert("check queue if empty", () => Sprite.BaseMotionsQueued == 0);
        }

        [Test]
        public void TestLooping()
        {
            AddStep("enqueue looping motion", () => Sprite.StartMotion("Idle", 2, MotionType.Base, true, true));
            AddWaitStep("wait", 30);
            AddAssert("check motion", () => Sprite.IsMoving);
            AddStep("stop motion", () => Sprite.StopMotion());
            AddAssert("check if stopped", () => !Sprite.IsMoving);
        }
    }
}

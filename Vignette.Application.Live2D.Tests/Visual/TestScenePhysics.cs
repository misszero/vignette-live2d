// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using osu.Framework.Graphics.Textures;
using Vignette.Application.Live2D.Graphics;
using Vignette.Application.Live2D.Json;
using Vignette.Application.Live2D.Model;
using Vignette.Application.Live2D.Motion;
using Vignette.Application.Live2D.Physics;
using Vignette.Application.Live2D.Tests.Resources;

namespace Vignette.Application.Live2D.Tests.Visual
{
    public class TestScenePhysics : TestSceneCubismSprite
    {
        protected override float SpriteSize => 1024.0f;

        protected override CubismSprite CreateSprite(CubismModel model, IEnumerable<Texture> textures) => new CubismSpriteWithPhysics(model, textures);

        private class CubismSpriteWithPhysics : CubismSprite
        {
            private readonly CubismPhysics physics;

            private readonly CubismMotion motion;

            public CubismSpriteWithPhysics(CubismModel model, IEnumerable<Texture> textures, bool disposeModel = false)
                : base(model, textures, disposeModel)
            {
                using var mots = new StreamReader(TestResources.GetModelResource(@"motions/Hiyori_m02.motion3.json"));
                using var phys = new StreamReader(TestResources.GetModelResource(@"Hiyori.physics3.json"));

                physics = new CubismPhysics(model, JsonSerializer.Deserialize<CubismPhysicsSetting>(phys.ReadToEnd()));
                motion = new CubismMotion(model, JsonSerializer.Deserialize<CubismMotionSetting>(mots.ReadToEnd()))
                {
                    Weight = 1.0,
                    LoopFading = true,
                    GlobalFadeInSeconds = 0.5,
                    GlobalFadeOutSeconds = 0.5,
                };
            }

            protected override void Update()
            {
                base.Update();

                motion.Update(Clock.CurrentTime / 1000);
                physics.Update(Clock.ElapsedFrameTime / 1000);
            }
        }
    }
}

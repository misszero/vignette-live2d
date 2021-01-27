﻿// Copyright (c) Vignette Project.
// Licensed under MIT. Please see LICENSE for more details.
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
using Vignette.Application.Live2D.Tests.Resources;

namespace Vignette.Application.Live2D.Tests.Visual
{
    public class TestSceneMotion : TestSceneCubismSprite
    {
        protected override CubismSprite CreateSprite(CubismModel model, IEnumerable<Texture> textures) => new CubismSpriteWithMotion(model, textures);

        private class CubismSpriteWithMotion : CubismSprite
        {
            public CubismMotion Motion { get; private set; }

            public CubismSpriteWithMotion(CubismModel model, IEnumerable<Texture> textures, bool disposeModel = false)
                : base(model, textures, disposeModel)
            {
                using var reader = new StreamReader(TestResources.GetModelResource(@"motions/Hiyori_m02.motion3.json"));
                var setting = JsonSerializer.Deserialize<CubismMotionSetting>(reader.ReadToEnd());

                Motion = new CubismMotion(model, setting)
                {
                    Weight = 1,
                    LoopFading = true,
                    GlobalFadeInSeconds = 0.5,
                    GlobalFadeOutSeconds = 0.5,
                };
            }

            protected override void Update()
            {
                base.Update();

                Motion.Update(Clock.CurrentTime / 1000, true);
            }
        }
    }
}

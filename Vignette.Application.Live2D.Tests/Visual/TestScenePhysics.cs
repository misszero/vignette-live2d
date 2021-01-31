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
using Vignette.Application.Live2D.Physics;
using Vignette.Application.Live2D.Tests.Resources;

namespace Vignette.Application.Live2D.Tests.Visual
{
    public class TestScenePhysics : TestSceneMotion
    {
        protected override CubismSprite CreateSprite(CubismModel model, IEnumerable<Texture> textures) => new CubismSpriteWithPhysics(model, textures);

        private class CubismSpriteWithPhysics : CubismSpriteWithMotion
        {
            private readonly CubismPhysics physics;

            public CubismSpriteWithPhysics(CubismModel model, IEnumerable<Texture> textures, bool disposeModel = false)
                : base(model, textures, disposeModel)
            {
                using var reader = new StreamReader(TestResources.GetModelResource(@"Hiyori.physics3.json"));
                physics = new CubismPhysics(model, JsonSerializer.Deserialize<CubismPhysicsSetting>(reader.ReadToEnd()));
            }

            protected override void Update()
            {
                base.Update();

                physics.Update(Clock.ElapsedFrameTime / 1000);
            }
        }
    }
}

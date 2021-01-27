// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using osu.Framework.Graphics.Textures;
using Vignette.Application.Live2D.Graphics;
using Vignette.Application.Live2D.Json;
using Vignette.Application.Live2D.Model;
using Vignette.Application.Live2D.Motion.Expression;
using Vignette.Application.Live2D.Tests.Resources;

namespace Vignette.Application.Live2D.Tests.Visual
{
    public class TestSceneExpression : TestSceneCubismSprite
    {
        protected override CubismSprite CreateSprite(CubismModel model, IEnumerable<Texture> textures) => new CubismSpriteWithExpression(model, textures);

        private class CubismSpriteWithExpression : CubismSprite
        {
            public CubismExpression Expression { get; private set; }

            public CubismSpriteWithExpression(CubismModel model, IEnumerable<Texture> textures, bool disposeModel = false)
                : base(model, textures, disposeModel)
            {
                //using var reader = new StreamReader(TestResources.GetModelResource(@"expressions/.json"));
                //var setting = JsonSerializer.Deserialize<CubismExpressionSetting>(reader.ReadToEnd());

                //Expression = new CubismExpression(model, setting)
                //{
                //    Weight = 1,
                //    GlobalFadeInSeconds = 0.5,
                //    GlobalFadeOutSeconds = 0.5,
                //};
            }

            protected override void Update()
            {
                base.Update();

                //Expression.Update(Clock.CurrentTime / 1000, true);
            }
        }
    }
}

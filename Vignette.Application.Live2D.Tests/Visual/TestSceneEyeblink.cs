// Copyright (c) Vignette Project.
// Licensed under MIT. Please see LICENSE for more details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;
using Vignette.Application.Live2D.Graphics;
using Vignette.Application.Live2D.Model;
using Vignette.Application.Live2D.Motion.Effect;

namespace Vignette.Application.Live2D.Tests.Visual
{
    public class TestSceneEyeblink : TestSceneCubismSprite
    {
        private readonly SpriteText statusText;

        public TestSceneEyeblink()
        {
            statusText = new SpriteText { Margin = new MarginPadding(10) };

            Add(statusText);
        }

        protected override void Update()
        {
            base.Update();

            statusText.Text = $"{((CubismSpriteWithEyeblinkEffect)Sprite).Eyeblink.State}";
        }

        protected override CubismSprite CreateSprite(CubismModel model, IEnumerable<Texture> textures) => new CubismSpriteWithEyeblinkEffect(model, textures)
        {
            ScaleAdjust = 7.5f,
            PositionAdjust = new Vector2(0, 1750),
        };

        private class CubismSpriteWithEyeblinkEffect : CubismSprite
        {
            public CubismEyeblink Eyeblink { get; private set; }

            public CubismSpriteWithEyeblinkEffect(CubismModel model, IEnumerable<Texture> textures, bool disposeModel = false)
                : base(model, textures, disposeModel)
            {
                var parameters = new[]
                {
                    model.Parameters.Get("ParamEyeLOpen"),
                    model.Parameters.Get("ParamEyeROpen"),
                };

                Eyeblink = new CubismEyeblink(parameters)
                {
                    BlinkInterval = 2.0,
                    OpeningDuration = 0.15,
                    ClosingDuration = 0.1,
                    ClosedDuration = 0.05,
                };
            }

            protected override void Update()
            {
                base.Update();

                Eyeblink.Update(Clock.CurrentTime / 1000);
            }
        }
    }
}

// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Textures;
using osu.Framework.Testing;
using osuTK;
using Vignette.Application.Live2D.Graphics;
using Vignette.Application.Live2D.Json;
using Vignette.Application.Live2D.Model;
using Vignette.Application.Live2D.Tests.Resources;

namespace Vignette.Application.Live2D.Tests.Visual
{
    public class TestSceneCubismSpriteSandbox : TestScene
    {
        public TestSceneCubismSpriteSandbox()
        {
            var moc = new CubismMoc(TestResources.GetModelResource("Hiyori.moc3"));
            var model = new CubismModel(moc);
            var textures = new[]
            {
                Texture.FromStream(TestResources.GetModelResource("textures/texture_00.png")),
                Texture.FromStream(TestResources.GetModelResource("textures/texture_01.png")),
            };

            var sprite = new CubismSprite(model, textures)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(512),
                ScaleAdjust = 7.5f,
                PositionAdjust = new Vector2(0, 1750),
            };

            Add(sprite);

            foreach (var param in model.Parameters)
                AddSliderStep<float>(param.Name, param.Minimum, param.Maximum, param.Value, (v) => param.Value = v);
        }
    }
}

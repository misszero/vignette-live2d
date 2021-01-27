// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Textures;
using osu.Framework.Testing;
using osuTK;
using Vignette.Application.Live2D.Graphics;
using Vignette.Application.Live2D.Model;
using Vignette.Application.Live2D.Tests.Resources;

namespace Vignette.Application.Live2D.Tests.Visual
{
    public abstract class TestSceneCubismSprite : TestScene
    {
        protected CubismSprite Sprite;

        protected CubismModel Model;

        public TestSceneCubismSprite()
        {
            var moc = new CubismMoc(TestResources.GetModelResource("Hiyori.moc3"));
            var textures = new[]
            {
                Texture.FromStream(TestResources.GetModelResource("textures/texture_00.png")),
                Texture.FromStream(TestResources.GetModelResource("textures/texture_01.png")),
            };

            Model = new CubismModel(moc);
            Sprite = CreateSprite(Model, textures);
            Sprite.Size = new Vector2(512);
            Sprite.Anchor = Anchor.Centre;
            Sprite.Origin = Anchor.Centre;
            Add(Sprite);
        }

        protected virtual CubismSprite CreateSprite(CubismModel model, IEnumerable<Texture> textures) => new CubismSprite(model, textures, true);
    }
}

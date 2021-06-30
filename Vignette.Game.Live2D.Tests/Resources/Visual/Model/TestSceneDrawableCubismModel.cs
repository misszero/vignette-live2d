// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using osu.Framework.Graphics;
using osu.Framework.Testing;
using osuTK;
using Vignette.Game.Live2D.Graphics;
using Vignette.Game.Live2D.Model;
using Vignette.Game.Live2D.Tests.Resources;

namespace Vignette.Game.Live2D.Tests.Visual.Model
{
    public class TestSceneDrawableCubismModel : TestScene
    {
        private CubismMoc moc;

        public TestSceneDrawableCubismModel()
        {
            moc = new CubismMoc(TestResources.GetModelResource("Hiyori.moc3"));
            Add(new CubismModel(moc)
            {
                Size = new Vector2(512),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            });
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
                moc?.Dispose();

            base.Dispose(isDisposing);
        }
    }
}

// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using osu.Framework.Graphics;
using osu.Framework.Testing;
using osuTK;
using Vignette.Live2D.Graphics;
using Vignette.Live2D.Tests.Resources;

namespace Vignette.Live2D.Tests.Visual
{
    public class TestSceneCubismModel : TestScene
    {
        public TestSceneCubismModel()
        {
            Add(new CubismModel(TestResources.GetModelResourceStore())
            {
                Size = new Vector2(1000),
                Anchor = Anchor.CentreRight,
                Origin = Anchor.CentreRight,
            });
        }
    }
}

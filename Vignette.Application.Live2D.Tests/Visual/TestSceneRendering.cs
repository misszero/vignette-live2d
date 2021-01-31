// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using NUnit.Framework;
using osu.Framework.Graphics;

namespace Vignette.Application.Live2D.Tests.Visual
{
    [Description(@"Draw Node Testing")]
    public class TestSceneRendering : TestSceneCubismSprite
    {
        public TestSceneRendering()
        {
            Sprite.Canvas.RelativePositionAxes = Axes.Both;
        }

        [Test]
        public void SetupContainer()
        {
            AddSliderStep(@"width", 0.0f, 1024.0f, 512.0f, value => Sprite.Width = value);
            AddSliderStep(@"height", 0.0f, 1024.0f, 512.0f, value => Sprite.Height = value);
        }

        [Test]
        public void SetupDrawing()
        {
            AddSliderStep(@"scale", 0.5f, 10.0f, 1.0f, value => Sprite.Canvas.Scale = value);
            AddSliderStep(@"x", -1.0f, 1.0f, 0.0f, value => Sprite.Canvas.X = value);
            AddSliderStep(@"y", -1.0f, 1.0f, 0.0f, value => Sprite.Canvas.Y = value);
        }
    }
}

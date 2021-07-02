// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.Testing;
using osuTK;
using Vignette.Game.Live2D.Graphics;
using Vignette.Game.Live2D.Tests.Resources;

namespace Vignette.Game.Live2D.Tests.Visual.Model
{
    public class TestSceneCubismDrawable : TestScene
    {
        [Cached(typeof(CubismModel))]
        private readonly CubismModel model;

        public TestSceneCubismDrawable()
        {
            Add(model = new TestCubismModel
            {
                Size = new Vector2(1500),
                Anchor = Anchor.CentreRight,
                Origin = Anchor.CentreRight,
            });

            Add(new CubismInspector(model)
            {
                Height = 300,
                RelativeSizeAxes = Axes.X,
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.BottomCentre,
            });
        }

        private class TestCubismModel : CubismModel
        {
            public TestCubismModel()
                : base(TestResources.GetModelResourceStore())
            {
            }

            protected override bool OnMouseDown(MouseDownEvent e) => true;

            protected override bool OnDragStart(DragStartEvent e) => true;

            protected override void OnDrag(DragEvent e)
            {
                base.OnDrag(e);
                Position += e.Delta;
            }

            protected override bool OnScroll(ScrollEvent e)
            {
                Scale += new Vector2(e.ScrollDelta.Y * 0.1f);
                return base.OnScroll(e);
            }
        }
    }
}

// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using System;
using Vignette.Game.Live2D.Graphics;

namespace Vignette.Game.Live2D.Tests
{
    public class CubismInspector : Container
    {
        [Cached]
        private readonly CubismModel target;

        private readonly TabControl<Tab> tabControl;
        private readonly Container tabContentContainer;
        private readonly DrawablePropertyDisplay drawablePropertyDisplay;

        public CubismInspector(CubismModel target)
        {
            this.target = target;

            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.X,
                    Colour = FrameworkColour.GreenDarker,
                    Height = 30,
                },
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = FrameworkColour.GreenDark,
                    Margin = new MarginPadding { Top = 30 },
                },
                tabControl = new BasicTabControl<Tab>
                {
                    Height = 30,
                    RelativeSizeAxes = Axes.X,
                    Margin = new MarginPadding { Top = 5, Left = 5 },
                    Items = Enum.GetValues<Tab>(),
                },
                tabContentContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding { Top = 30, Horizontal = 5 },
                    Children = new Drawable[]
                    {
                        drawablePropertyDisplay = new DrawablePropertyDisplay(),
                    }
                },
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            tabControl.Current.BindValueChanged(showTab, true);
        }

        private void showTab(ValueChangedEvent<Tab> tab)
        {
            tabContentContainer.Children.ForEach(t => t.Hide());

            switch (tab.NewValue)
            {
                case Tab.Drawables:
                    drawablePropertyDisplay.Show();
                    break;
            }
        }

        private enum Tab
        {
            Drawables,
        }
    }
}

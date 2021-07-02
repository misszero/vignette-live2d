// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using System;
using Vignette.Game.Live2D.Graphics;

namespace Vignette.Game.Live2D.Tests
{
    public class DrawablePropertyDisplay : Container
    {
        private readonly FillFlowContainer flow;
        private CubismDrawableItem selected;

        public DrawablePropertyDisplay()
        {
            RelativeSizeAxes = Axes.Both;
            Child = new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                ColumnDimensions = new[]
                {
                    new Dimension(),
                    new Dimension(),
                },
                Content = new Drawable[][]
                {
                    new Drawable[]
                    {
                        new BasicScrollContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                            Child = flow = new FillFlowContainer
                            {
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                            }
                        }
                    },
                },
            };
        }

        private void selectDrawable(CubismDrawableItem item)
        {
            if (selected != null && selected.Target.ID == item.Target.ID)
                return;

            item.IsSelected = true;

            if (selected != null)
                selected.IsSelected = false;

            selected = item;
        }

        [BackgroundDependencyLoader]
        private void load(CubismModel model)
        {
            foreach (var drawable in model.Drawables)
            {
                var item = new CubismDrawableItem(drawable);
                item.Select += selectDrawable;
                flow.Add(item);
            }
        }

        private class CubismDrawableItem : Container
        {
            public readonly CubismDrawable Target;

            private readonly Box background;

            private bool isSelected;

            public bool IsSelected
            {
                get => isSelected;
                set
                {
                    isSelected = value;

                    if (IsSelected)
                    {
                        background.Alpha = 0.4f;
                        Target.Colour = Colour4.Red;
                    }
                    else
                    {
                        background.Alpha = 0.0f;
                        Target.Colour = Colour4.White;
                    }
                }
            }

            public event Action<CubismDrawableItem> Select;

            public CubismDrawableItem(CubismDrawable target)
            {
                Target = target;

                Height = 35;
                RelativeSizeAxes = Axes.X;

                Children = new Drawable[]
                {
                    background = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        AlwaysPresent = true,
                        Alpha = 0,
                    },
                    new SpriteText
                    {
                        Text = target.Name,
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Margin = new MarginPadding { Left = 5 },
                    },
                };
            }

            protected override bool OnHover(HoverEvent e)
            {
                if (!IsSelected)
                {
                    Target.Colour = Colour4.Red;
                    background.Alpha = 0.2f;
                }

                return base.OnHover(e);
            }

            protected override void OnHoverLost(HoverLostEvent e)
            {
                base.OnHoverLost(e);

                if (!IsSelected)
                {
                    Target.Colour = Colour4.White;
                    background.Alpha = 0;
                }
            }

            protected override bool OnClick(ClickEvent e)
            {
                Select?.Invoke(this);
                return base.OnClick(e);
            }
        }
    }
}

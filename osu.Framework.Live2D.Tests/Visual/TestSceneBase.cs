// Copyright (c) Nitrous <n20gaming2000@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cubism;
using osu.Framework.Graphics.Cubism.Renderer;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Testing;
using osuTK;

namespace osu.Framework.Live2D.Tests.Visual
{
    public abstract class TestSceneBase : TestScene
    {
        protected Container<Drawable> Container;
        protected CubismSprite Sprite;
        protected virtual Colour4 BackgroundColour => Colour4.Transparent;

        [BackgroundDependencyLoader]
        private void load(CubismAssetStore cubismAssets)
        {
            Add(Container = new Container<Drawable>
            {
                Size = new Vector2(684),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = BackgroundColour
                    },
                    Sprite = new CubismSprite
                    {
                        RelativeSizeAxes = Axes.Both,
                        Asset = cubismAssets.Get("Hiyori.Hiyori.model3.json"),
                        Renderer = new CubismRenderer
                        {
                            Scale = new Vector2(2),
                            Y = 384
                        }
                    }
                }
            });
        }

        protected class ParameterMonitor : FillFlowContainer
        {
            private CubismSprite sprite;
            private Dictionary<string, Box> bars = new Dictionary<string, Box>();

            public ParameterMonitor(CubismSprite sprite, string[] parameters)
            {
                Width = 350;
                RelativeSizeAxes = Axes.Y;
                Direction = FillDirection.Full;

                this.sprite = sprite;
                foreach (var param in parameters)
                {
                    if (sprite.Asset.Model.GetParameter(param) == null)
                        continue;

                    var bar = new Box
                    {
                        RelativeSizeAxes = Axes.X,
                        Colour = Colour4.White,
                        Height = 5
                    };

                    bars.Add(param, bar);

                    Add(new FillFlowContainer
                    {
                        Direction = FillDirection.Vertical,
                        Width = 150,
                        Height = 30,
                        Margin = new MarginPadding(5),
                        Children = new Drawable[]
                        {
                            new SpriteText { Text = param, Scale = new Vector2(0.75f) },
                            bar
                        }
                    });
                }
            }

            protected override void Update()
            {
                base.Update();

                foreach (var (name, bar) in bars)
                {
                    var param = sprite.Asset.Model.GetParameter(name);
                    bar.Width = (float)(MathHelper.Clamp(param.Value, 0, 1));
                }
            }
        }
    }
}
// Copyright (c) Nitrous <n20gaming2000@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CubismFramework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cubism;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Testing;
using osuTK;

namespace osu.Framework.Live2D.Tests.Visual
{
    public abstract class CubismTestScene : TestScene
    {
        protected Container<Drawable> Container;
        protected TestCubismSprite Sprite;
        protected static string[] Parameters => typeof(CubismDefaultParameterId)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Select(f => f.GetValue(null) as string).ToArray();

        [BackgroundDependencyLoader]
        private void load(CubismStore cubismAssets)
        {
            Add(Container = new Container<Drawable>
            {
                Size = new Vector2(684),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Child = Sprite = new TestCubismSprite(cubismAssets.Get("Hiyori.model3.json"))
                {
                    RelativeSizeAxes = Axes.Both
                }
            });
        }

        protected class TestCubismSprite : CubismSprite
        {
            public int BaseMotionsQueued => BaseMotionQueue.Queued;
            public int ExpressionsQueued => ExpressionQueue.Queued;
            public int EffectsQueued => EffectQueue.Queued;

            public TestCubismSprite(CubismAsset asset)
                : base(asset)
            {
            }
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
                    if (!sprite.HasParameter(param))
                        continue;

                    var bar = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Anchor = Anchor.Centre,
                        Colour = Colour4.White,
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
                            new Container
                            {
                                RelativeSizeAxes = Axes.X,
                                Height = 5,
                                Children = new Drawable[]
                                {
                                    new Box
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = Colour4.White.Opacity(0.25f),
                                    },
                                    bar
                                }
                            }
                        }
                    });
                }
            }

            protected override void Update()
            {
                base.Update();

                foreach (var (name, bar) in bars)
                {
                    var param = map((float)sprite.GetParameterValue(name), 0, 1, -0.5f, 0.5f);
                    bar.Width = Math.Clamp(param, -0.5f, 0.5f);
                    bar.Colour = param > 0 ? Colour4.White : Colour4.Red;
                }

                float map(float value, float fromSource, float toSource, float fromDestination, float toDestination) =>
                    (value - fromSource) / (toSource - fromSource) * (toDestination - fromDestination) + fromDestination;
            }
        }
    }
}
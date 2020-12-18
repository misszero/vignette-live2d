// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.IO.Stores;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Testing;
using Vignette.Application.Live2D.Resources;

namespace Vignette.Application.Live2D.Tests
{
    internal class VisualTestGame : Game
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            Resources.AddStore(new NamespacedResourceStore<byte[]>(new DllResourceStore(CubismResources.ResourceAssembly), "Resources"));
            CubismCore.csmSetLogFunction((string message) => Logger.Log(message, LoggingTarget.Information, LogLevel.Debug));
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            Child = new SafeAreaContainer
            {
                RelativeSizeAxes = Axes.Both,
                Child = new DrawSizePreservingFillContainer
                {
                    Children = new Drawable[]
                    {
                        new TestBrowser("Vignette"),
                        new CursorContainer(),
                    },
                }
            };
        }

        public override void SetHost(GameHost host)
        {
            base.SetHost(host);
            host.Window.CursorState |= CursorState.Hidden;
        }
    }
}

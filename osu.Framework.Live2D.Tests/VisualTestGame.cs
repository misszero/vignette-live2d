// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Platform;
using osu.Framework.Testing;

namespace osu.Framework.Live2D.Tests
{
    public class VisualTestGame : TestGame
    {
        public override void SetHost(GameHost host)
        {
            base.SetHost(host);
            host.Window.CursorState |= CursorState.Hidden;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Child = new DrawSizePreservingFillContainer
            {
                Children = new Drawable[]
                {
                    new TestBrowser(),
                    new CursorContainer(),
                },
            };
        }
    }
}

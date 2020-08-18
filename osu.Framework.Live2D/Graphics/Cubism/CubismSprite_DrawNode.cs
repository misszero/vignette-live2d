// Copyright (c) Nitrous <n20gaming2000@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using CubismFramework;
using osu.Framework.Graphics.OpenGL.Vertices;
using osuTK;

namespace osu.Framework.Graphics.Cubism
{
    public partial class CubismSprite
    {
        private class CubismSpriteDrawNode : DrawNode
        {
            protected new CubismSprite Source => (CubismSprite)base.Source;

            private CubismAsset asset;
            private CubismRenderingManager renderingManager;

            public CubismSpriteDrawNode(CubismSprite source)
                : base(source)
            {
            }

            public override void ApplyState()
            {
                base.ApplyState();

                asset = Source.Asset;
                renderingManager = Source.RenderingManager;
            }

            public override void Draw(Action<TexturedVertex2D> vertexAction)
            {
                base.Draw(vertexAction);
                renderingManager.Draw(Matrix4.Identity);
            }

            protected override void Dispose(bool isDisposing)
            {
                // We are handling disposal here to ensure that all draw calls have been performed to avoid race conditions
                renderingManager?.Dispose();
                asset?.Dispose();

                base.Dispose(isDisposing);
            }
        }
    }
}
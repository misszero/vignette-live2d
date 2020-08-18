// Copyright (c) Nitrous <n20gaming2000@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using CubismFramework;
using osu.Framework.Graphics.OpenGL.Buffers;
using osuTK.Graphics.ES30;

namespace osu.Framework.Graphics.Cubism.Renderer
{
    public partial class CubismRenderer
    {
        private class CubismClippingMask : FrameBuffer, ICubismClippingMask
        {
            public CubismClippingMask()
                : base(new[] { RenderbufferInternalFormat.DepthComponent16 })
            {
            }
        }
    }
}
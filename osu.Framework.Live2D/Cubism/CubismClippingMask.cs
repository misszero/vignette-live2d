using System;
using CubismFramework;
using osu.Framework.Graphics.OpenGL.Buffers;
using osuTK;

namespace osu.Framework.Live2D.Cubism
{
    public class CubismClippingMask : ICubismClippingMask, IDisposable
    {
        private FrameBuffer buffer;
        private bool isDisposed = false;

        public CubismClippingMask(int width, int height)
        {
            buffer = new FrameBuffer();
            buffer.Size = new Vector2(width, height);
            buffer.Bind();
        }

        ~CubismClippingMask()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
                return;

            buffer.Dispose();
            isDisposed = true;
        }
    }
}
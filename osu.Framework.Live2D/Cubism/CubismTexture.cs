using System;
using CubismFramework;
using osu.Framework.Graphics.Textures;

namespace osu.Framework.Live2D.Cubism
{
    public class CubismTexture : ICubismTexture, IDisposable
    {
        private Texture texture;
        private bool isDisposed = false;

        public CubismTexture(int width, int height)
        {
            texture = new Texture(width, height);
        }

        public CubismTexture(Texture texture)
        {
            this.texture = texture;
        }

        ~CubismTexture()
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

            texture.Dispose();
            isDisposed = true;
        }
    }
}
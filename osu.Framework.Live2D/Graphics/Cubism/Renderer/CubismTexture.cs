using CubismFramework;
using osu.Framework.Graphics.Textures;
using osuTK.Graphics.ES30;

namespace osu.Framework.Graphics.Cubism.Renderer
{
    public partial class CubismRenderer
    {
        private class CubismTexture : Texture, ICubismTexture
        {
            public CubismTexture(int width, int height)
                : base(width, height, false, All.Linear)
            {
            }

            public bool Bind(TextureUnit unit = TextureUnit.Texture0) => TextureGL.Bind(unit);
        }
    }
}
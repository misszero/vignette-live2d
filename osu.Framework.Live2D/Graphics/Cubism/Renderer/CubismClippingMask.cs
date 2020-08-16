using CubismFramework;
using osu.Framework.Graphics.OpenGL.Buffers;
using osuTK.Graphics.ES30;

namespace osu.Framework.Graphics.Cubism.Renderer
{
    internal class CubismClippingMask : FrameBuffer, ICubismClippingMask
    {
        public CubismClippingMask()
            : base(new[] { RenderbufferInternalFormat.DepthComponent16 })
        {
        }
    }
}
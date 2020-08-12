using osuTK.Graphics.ES20;

namespace osu.Framework.Graphics.Cubism.Renderer
{
    internal class CubismRendererState
    {
        private int lastArrayBufferBind;
        private int lastElementArrayBufferBind;
        private bool lastCullFace;
        private int lastFrontFace;

        public void Save()
        {
            GL.GetInteger(GetPName.ArrayBufferBinding, out lastArrayBufferBind);
            GL.GetInteger(GetPName.ElementArrayBufferBinding, out lastElementArrayBufferBind);
            GL.GetInteger(GetPName.FrontFace, out lastFrontFace);

            lastCullFace = GL.IsEnabled(EnableCap.CullFace);
        }

        public void Restore()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, (uint)lastArrayBufferBind);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, (uint)lastElementArrayBufferBind);

            GL.FrontFace((FrontFaceDirection)lastFrontFace);

            SetEnabled(EnableCap.CullFace, lastCullFace);
        }

        private static void SetEnabled(EnableCap cap, bool enabled)
        {
            if (enabled)
                GL.Enable(cap);
            else
                GL.Disable(cap);
        }
    }
}
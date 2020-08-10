using osuTK.Graphics.ES20;

namespace osu.Framework.Graphics.Cubism.Renderer
{
    internal class CubismRendererState
    {
        private int lastArrayBufferBind;
        private int lastElementArrayBufferBind;
        private bool lastCullFace;
        private int lastFrontFace;
        private int[] lastVertexAttribArrayEnabled = new int[4];

        public void Save()
        {
            GL.GetInteger(GetPName.ArrayBufferBinding, out lastArrayBufferBind);
            GL.GetInteger(GetPName.ElementArrayBufferBinding, out lastElementArrayBufferBind);
            GL.GetInteger(GetPName.FrontFace, out lastFrontFace);

            GL.GetVertexAttrib(0, VertexAttribParameter.VertexAttribArrayEnabled, out lastVertexAttribArrayEnabled[0]);
            GL.GetVertexAttrib(1, VertexAttribParameter.VertexAttribArrayEnabled, out lastVertexAttribArrayEnabled[1]);
            GL.GetVertexAttrib(2, VertexAttribParameter.VertexAttribArrayEnabled, out lastVertexAttribArrayEnabled[2]);
            GL.GetVertexAttrib(3, VertexAttribParameter.VertexAttribArrayEnabled, out lastVertexAttribArrayEnabled[3]);

            lastCullFace = GL.IsEnabled(EnableCap.CullFace);
        }

        public void Restore()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, (uint)lastArrayBufferBind);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, (uint)lastElementArrayBufferBind);

            GL.FrontFace((FrontFaceDirection)lastFrontFace);

            SetEnabledVertexAttribArray(0, lastVertexAttribArrayEnabled[0] != 0);
            SetEnabledVertexAttribArray(1, lastVertexAttribArrayEnabled[1] != 0);
            SetEnabledVertexAttribArray(2, lastVertexAttribArrayEnabled[2] != 0);
            SetEnabledVertexAttribArray(3, lastVertexAttribArrayEnabled[3] != 0);

            SetEnabled(EnableCap.CullFace, lastCullFace);
        }

        private static void SetEnabled(EnableCap cap, bool enabled)
        {
            if (enabled)
                GL.Enable(cap);
            else
                GL.Disable(cap);
        }

        private static void SetEnabledVertexAttribArray(int index, bool enabled)
        {
            if (enabled)
                GL.EnableVertexAttribArray((uint)index);
            else
                GL.DisableVertexAttribArray((uint)index);
        }
    }
}
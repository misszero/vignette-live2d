// Copyright (c) Nitrous <n20gaming2000@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osuTK.Graphics.ES30;

namespace osu.Framework.Graphics.Cubism.Renderer
{
    public partial class CubismRenderer
    {
        private class CubismRendererState
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
}
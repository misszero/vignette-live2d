// Copyright (c) Vignette Project.
// Licensed under MIT. Please see LICENSE for more details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using System;
using System.IO;
using System.Runtime.InteropServices;
using Vignette.Application.Live2D.Utils;

namespace Vignette.Application.Live2D.Model
{
    public class CubismMoc : IDisposable
    {
        private IntPtr bufferPtr;

        public bool IsDisposed { get; private set; }

        public IntPtr Handle { get; private set; }

        public CubismMocVersion Version { get; private set; }

        public CubismMoc(Stream moc)
        {
            using var memory = new MemoryStream();
            moc.CopyTo(memory);

            var bytes = memory.ToArray();
            bufferPtr = Marshal.AllocHGlobal(bytes.Length + CubismCore.ALIGN_OF_MOC - 1);
            var aligned = CubismUtils.Align(bufferPtr, CubismCore.ALIGN_OF_MOC);

            Marshal.Copy(bytes, 0, aligned, bytes.Length);
            Handle = CubismCore.csmReviveMocInPlace(aligned, bytes.Length);

            Version = CubismCore.csmGetMocVersion(aligned);

            if (Handle == IntPtr.Zero)
                throw new ArgumentException($"{nameof(moc)} is not a valid Cubism Model.");

            if (Version > CubismCore.LatestMocVersion)
                throw new ArgumentException($"{nameof(moc)} has version '{Version}' while Core can only support up to '{CubismCore.LatestMocVersion}'.");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;

            Marshal.FreeHGlobal(bufferPtr);

            IsDisposed = true;
        }

        ~CubismMoc()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

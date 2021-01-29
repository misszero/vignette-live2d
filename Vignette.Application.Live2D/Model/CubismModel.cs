// Copyright (c) Vignette Project.
// Licensed under MIT. Please see LICENSE for more details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using osuTK;
using Vignette.Application.Live2D.Id;
using Vignette.Application.Live2D.Utils;

namespace Vignette.Application.Live2D.Model
{
    public class CubismModel : IDisposable
    {
        private readonly CubismMoc moc;

        private readonly IntPtr bufferPtr;

        private readonly List<CubismIdManager> managers = new List<CubismIdManager>();

        private bool isDisposed;

        public IntPtr Handle { get; private set; }

        public Vector2 Size { get; private set; }

        public CubismPartManager Parts { get; private set; }

        public CubismDrawableManager Drawables { get; private set; }

        public CubismParameterManager Parameters { get; private set; }

        public CubismModel(CubismMoc moc)
        {
            this.moc = moc;
            int size = CubismCore.csmGetSizeofModel(moc.Handle);

            bufferPtr = Marshal.AllocHGlobal(size + CubismCore.ALIGN_OF_MODEL - 1);
            Handle = CubismCore.csmInitializeModelInPlace(moc.Handle, CubismUtils.Align(bufferPtr, CubismCore.ALIGN_OF_MODEL), size);

            managers.AddRange(new CubismIdManager[]
            {
                Parts = new CubismPartManager(Handle),
                Drawables = new CubismDrawableManager(Handle),
                Parameters = new CubismParameterManager(Handle),
            });

            float[] sizeInPixels = new float[2];
            float[] originInPixels = new float[2];
            CubismCore.csmReadCanvasInfo(Handle, sizeInPixels, originInPixels, out float pixelsPerUnit);

            Size = new Vector2(sizeInPixels[0], sizeInPixels[1]) / pixelsPerUnit;

            // Obtain default values first
            foreach (var manager in managers)
                manager.PostModelUpdate();
        }

        public void Update()
        {
            if (moc.IsDisposed)
                throw new ObjectDisposedException($"{nameof(moc)} has been disposed while a model is active.");

            CubismCore.csmResetDrawableDynamicFlags(Handle);

            foreach (var manager in managers)
                manager.PreModelUpdate();

            CubismCore.csmUpdateModel(Handle);

            foreach (var manager in managers)
                manager.PostModelUpdate();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
                return;

            Marshal.FreeHGlobal(bufferPtr);

            isDisposed = true;
        }

        ~CubismModel()
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

// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System;

namespace Vignette.Application.Live2D.Model
{
    public class CubismModel : IDisposable
    {
        private bool isDisposed;

        public CubismPartManager Parts { get; private set; }

        public CubismDrawableManager Drawables { get; private set; }

        public CubismParameterManager Parameters { get; private set; }

        internal CubismModel(IntPtr mocPtr)
        {

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                isDisposed = true;
            }
        }

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
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

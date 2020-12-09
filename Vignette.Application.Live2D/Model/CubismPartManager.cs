// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System;
using System.Linq;
using System.Runtime.InteropServices;
using Vignette.Application.Live2D.Id;

namespace Vignette.Application.Live2D.Model
{
    public class CubismPartManager : CubismIdManager<CubismPart>
    {
        private readonly IntPtr handle;

        public CubismPartManager(IntPtr model)
            : base(model)
        {
            handle = CubismCore.csmGetPartOpacities(model);

            int count = CubismCore.csmGetPartCount(model);
            string[] names = CubismCore.csmGetPartIds(model);

            for (int i = 0; i < count; i++)
                Add(new CubismPart(i, names[i]));
        }

        public override void PreModelUpdate()
        {
            float[] values = this.Select(part => part.Value).ToArray();
            Marshal.Copy(values, 0, handle, Count);
        }

        public override void PostModelUpdate()
        {
            float[] values = new float[Count];
            Marshal.Copy(handle, values, 0, Count);

            for (int i = 0; i < Count; i++)
                this[i].Value = values[i];
        }
    }
}

// Copyright (c) Vignette Project.
// Licensed under MIT. Please see LICENSE for more details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using System;
using System.Linq;
using System.Runtime.InteropServices;
using Vignette.Application.Live2D.Id;
using Vignette.Application.Live2D.Utils;

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
            string[] names = CubismUtils.PointerToStringArray(CubismCore.csmGetPartIds(model), count);

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

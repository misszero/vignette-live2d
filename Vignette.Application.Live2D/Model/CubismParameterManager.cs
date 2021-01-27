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
    public class CubismParameterManager : CubismIdManager<CubismParameter>
    {
        private readonly IntPtr handle;

        public CubismParameterManager(IntPtr model)
            : base(model)
        {
            handle = CubismCore.csmGetParameterValues(model);

            int count = CubismCore.csmGetParameterCount(model);
            float[] def = CubismUtils.PointerToFloatArray(CubismCore.csmGetParameterDefaultValues(model), count);
            float[] min = CubismUtils.PointerToFloatArray(CubismCore.csmGetParameterMinimumValues(model), count);
            float[] max = CubismUtils.PointerToFloatArray(CubismCore.csmGetParameterMaximumValues(model), count);
            string[] names = CubismUtils.PointerToStringArray(CubismCore.csmGetParameterIds(model), count);

            for (int i = 0; i < count; i++)
                Add(new CubismParameter(i, names[i], min[i], max[i], def[i]));
        }

        public override void PreModelUpdate()
        {
            float[] values = this.Select(param => param.Value).ToArray();
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

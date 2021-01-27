// Copyright (c) Vignette Project.
// Licensed under MIT. Please see LICENSE for more details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using System;
using Vignette.Application.Live2D.Id;

namespace Vignette.Application.Live2D.Model
{
    public class CubismParameter : CubismId
    {
        public readonly float Minimum;

        public readonly float Maximum;

        public readonly float Default;

        private float val;

        public float Value
        {
            get => val;
            set => val = Math.Clamp(value, Minimum, Maximum);
        }

        public CubismParameter(int index, string name, float min, float max, float def)
            : base(index, name)
        {
            Minimum = min;
            Maximum = max;
            Default = def;
        }
    }
}

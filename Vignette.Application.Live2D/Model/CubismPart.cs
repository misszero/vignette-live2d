// Copyright (c) Vignette Project.
// Licensed under MIT. Please see LICENSE for more details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using System;
using Vignette.Application.Live2D.Id;

namespace Vignette.Application.Live2D.Model
{
    public class CubismPart : CubismId
    {
        public float Target { get; set; }

        private float val;

        public float Value
        {
            get => val;
            set => val = Math.Clamp(value, 0, 1);
        }

        public CubismPart(int index, string name)
            : base(index, name)
        {
        }
    }
}

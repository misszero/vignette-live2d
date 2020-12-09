// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using Vignette.Application.Live2D.Id;

namespace Vignette.Application.Live2D.Model
{
    public class CubismPart : CubismId
    {
        public float Target { get; set; }

        public float Value { get; set; }

        public CubismPart(int index, string name)
            : base(index, name)
        {
        }
    }
}

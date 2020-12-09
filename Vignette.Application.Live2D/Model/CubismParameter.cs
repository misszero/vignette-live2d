// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using Vignette.Application.Live2D.Id;

namespace Vignette.Application.Live2D.Model
{
    public class CubismParameter : CubismId
    {
        public readonly float Minimum;

        public readonly float Maximum;

        public readonly float Default;

        public float Value { get; set; }

        public CubismParameter(int index, string name, float min, float max, float def)
            : base(index, name)
        {
            Minimum = min;
            Maximum = max;
            Default = def;
        }
    }
}

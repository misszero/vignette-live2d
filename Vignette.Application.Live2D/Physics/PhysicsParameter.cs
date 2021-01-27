// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using Vignette.Application.Live2D.Model;

namespace Vignette.Application.Live2D.Physics
{
    public struct PhysicsParameter
    {
        public CubismParameter Parameter { get; set; }

        public PhysicsTarget TargetType { get; set; }
    }
}

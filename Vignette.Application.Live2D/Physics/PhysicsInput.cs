// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using osuTK;
using Vignette.Application.Live2D.Model;

namespace Vignette.Application.Live2D.Physics
{
    public class PhysicsInput
    {
        public PhysicsParameter Source { get; set; }

        public int SourceParameterIndex { get; set; }

        public float Weight { get; set; }

        public PhysicsSource Type { get; set; }

        public bool Reflect { get; set; }

        public NormalizedPhysicsParameterValueGetter GetNormalizedPhysicsParameterValue { get; set; }

        public delegate void NormalizedPhysicsParameterValueGetter(
            ref Vector2 TargetTranslation,
            ref float TargetAngle,
            CubismParameter parameter,
            PhysicsNormalization normalizationPosition,
            PhysicsNormalization normalizationAngle,
            bool isInverted,
            float weight
        );
    }
}

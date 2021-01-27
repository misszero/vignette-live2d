// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System.Collections.Generic;
using osuTK;

namespace Vignette.Application.Live2D.Physics
{
    public class PhysicsOutput
    {
        public PhysicsParameter Destination { get; set; }

        public int DestinationParameterIndex { get; set; }

        public int VertexIndex { get; set; }

        public Vector2 TranslationScale { get; set; }

        public float AngleScale { get; set; }

        public float Weight { get; set; }

        public PhysicsSource Type { get; set; }

        public bool Reflect { get; set; }

        public float ValueBelowMinimum { get; set; }

        public float ValueExceededMaximum { get; set; }

        public PhysicsValueGetter GetValue { get; set; }

        public PhysicsScaleGetter GetScale { get; set; }

        public delegate float PhysicsValueGetter(
            Vector2 translation,
            List<PhysicsParticle> particles,
            int particleIndex,
            bool isInverted,
            Vector2 parentGravity
        );

        public delegate float PhysicsScaleGetter(
            Vector2 translationScale,
            float angleScale
        );
    }
}

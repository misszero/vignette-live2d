// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

namespace Vignette.Application.Live2D.Physics
{
    public class PhysicsSubRig
    {
        public int InputCount { get; set; }

        public int OutputCount { get; set; }

        public int ParticleCount { get; set; }

        public int BaseInputIndex { get; set; }

        public int BaseOutputIndex { get; set; }

        public int BaseParticleIndex { get; set; }

        public PhysicsNormalization NormalizationPosition { get; set; }

        public PhysicsNormalization NormalizationAngle { get; set; }
    }
}

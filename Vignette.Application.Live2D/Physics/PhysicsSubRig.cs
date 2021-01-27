// Copyright (c) Vignette Project.
// Licensed under MIT. Please see LICENSE for more details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

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

// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using osuTK;

namespace Vignette.Application.Live2D.Physics
{
    public class PhysicsRig
    {
        public int SubRigCount { get; set; }

        public PhysicsSubRig[] Settings { get; set; }

        public PhysicsInput[] Inputs { get; set; }

        public PhysicsOutput[] Outputs { get; set; }

        public PhysicsParticle[] Particles { get; set; }

        public Vector2 Gravity { get; set; }

        public Vector2 Wind { get; set; }
    }
}

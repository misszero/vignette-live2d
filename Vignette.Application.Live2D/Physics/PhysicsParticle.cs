// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osuTK;

namespace Vignette.Application.Live2D.Physics
{
    public class PhysicsParticle
    {
        public Vector2 InitialPosition { get; set; }

        public float Mobility { get; set; }

        public float Delay { get; set; }

        public float Acceleration { get; set; }

        public float Radius { get; set; }

        public Vector2 Position { get; set; }

        public Vector2 LastPosition { get; set; }

        public Vector2 LastGravity { get; set; }

        public Vector2 Force { get; set; }

        public Vector2 Velocity { get; set; }
    }
}

// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using osuTK;

namespace Vignette.Application.Live2D.Physics2
{
    public struct CubismPhysicsParticle
    {
        public Vector2 InitialPosition;

        public Vector2 Mobility;

        public float Delay;

        public float Acceleration;

        public float Radius;

        public Vector2 Position;

        public Vector2 LastPosition;

        public Vector2 LastGravity;

        public Vector2 Force;

        public Vector2 Velocity;
    }
}

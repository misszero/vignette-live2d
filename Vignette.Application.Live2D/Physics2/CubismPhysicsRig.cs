// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using osuTK;

namespace Vignette.Application.Live2D.Physics2
{
    public class CubismPhysicsRig
    {
        public CubismPhysicsSubRig[] SubRigs;

        public Vector2 Gravity = CubismPhysics.Gravity;

        public Vector2 Wind = CubismPhysics.Wind;
    }
}

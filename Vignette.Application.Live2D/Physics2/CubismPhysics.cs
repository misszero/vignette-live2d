// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using osuTK;

namespace Vignette.Application.Live2D.Physics2
{
    public class CubismPhysics
    {
        public static Vector2 Gravity = new Vector2(0, -1);

        public static Vector2 Wind = Vector2.Zero;

        public static float AirResistance = 5.0f;

        public static float MaximumWeight = 100.0f;

        public const float MovementTreshold = 0.000f;
    }
}

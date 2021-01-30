// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using System;
using osuTK;
using Vignette.Application.Live2D.Model;
using Vignette.Application.Live2D.Utils;

namespace Vignette.Application.Live2D.Physics2
{
    public struct CubismPhysicsInput
    {
        //FIXME: stub boolean, this will be replaced once we can check if the drawable is inverted.
        internal bool IsInverted;

        public delegate void NormalizedParameterGetValue(
            ref Vector2 targetTranslation,
            ref float targetAngle,
            CubismParameter parameter,
            CubismPhysicsNormalization normalization,
            float weight
            );

        private void getInputTranslationXFromNormalizedParameterValue(
            ref Vector2 targetTranslation,
            ref float targetAngle,
            CubismParameter parameter,
            CubismPhysicsNormalization normalization,
            float weight
            )
        {
            targetTranslation.X = CubismMath.Normalize(
                parameter,
                normalization.Position.Minimum,
                normalization.Position.Maximum,
                normalization.Position.Default,
                IsInverted
                ) * weight;
        }
    }
}

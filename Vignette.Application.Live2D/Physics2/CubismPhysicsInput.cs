// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using osuTK;
using Vignette.Application.Live2D.Model;
using Vignette.Application.Live2D.Utils;

namespace Vignette.Application.Live2D.Physics2
{
    public struct CubismPhysicsInput
    {

        #region Delegates

        public delegate void NormalizedParameterValueGetter(
            ref Vector2 targetTranslation,
            ref float targetAngle,
            CubismParameter parameter,
            CubismPhysicsNormalization normalization,
            float weight
            );

        #endregion

        #region Methods

        private void getInputTranslationXFromNormalizedParameterValue(
            ref Vector2 targetTranslation,
            ref float targetAngle,
            CubismParameter parameter,
            CubismPhysicsNormalization normalization,
            float weight
            ) => targetTranslation.X = CubismMath.Normalize(
                parameter,
                normalization.Position.Minimum,
                normalization.Position.Maximum,
                normalization.Position.Default,
                IsInverted
                ) * weight;

        private void getInputTranslationYFromNormalizedParameterValue(
            ref Vector2 targetTranslation,
            ref float targetAngle,
            CubismParameter parameter,
            CubismPhysicsNormalization normalization,
            float weight
            ) => targetTranslation.Y += CubismMath.Normalize(
                parameter,
                normalization.Position.Minimum,
                normalization.Position.Maximum,
                normalization.Position.Default,
                IsInverted
                ) * weight;

        private void getInputAngleFromNormalizedParameterValue(
            ref Vector2 targetTranslation,
            ref float targetAngle,
            CubismParameter parameter,
            CubismPhysicsNormalization normalization,
            float weight
            ) => targetAngle += CubismMath.Normalize(
                parameter,
                normalization.Angle.Minimum,
                normalization.Angle.Maximum,
                normalization.Angle.Default,
                IsInverted) * weight;

        public void InitializeGetter()
        {
            switch(SourceComponent)
            {
                case CubismPhysicsSourceComponent.X:
                {
                    GetNormalizedParameterValue = getInputTranslationXFromNormalizedParameterValue;
                }

                break;

                case CubismPhysicsSourceComponent.Y:
                {
                    GetNormalizedParameterValue = getInputTranslationYFromNormalizedParameterValue;
                }
                break;

                case CubismPhysicsSourceComponent.Angle:
                {
                    GetNormalizedParameterValue = getInputAngleFromNormalizedParameterValue;
                }
                break;
            }
        }

        #endregion

        #region Fields

        public CubismPhysicsSourceComponent SourceComponent;

        public bool IsInverted;

        public CubismParameter Source;

        public float Weight;

        public float AngleScale;

        public Vector2 ScaleOfTranslation;

        public string SourceId;

        public NormalizedParameterValueGetter GetNormalizedParameterValue;

        #endregion
    }
}

// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using osuTK;
using Vignette.Application.Live2D.Model;
using Vignette.Application.Live2D.Utils;

namespace Vignette.Application.Live2D.Physics2
{
    public struct CubismPhysicsOutput
    {
        #region Delegates

        public delegate float ValueGetter(
            Vector2 translation,
            CubismParameter parameter,
            CubismPhysicsParticle[] particles,
            int particleIndex,
            Vector2 gravity
            );

        public delegate float ScaleGetter();

        #endregion

        #region Methods

        private float getOutputTranslationX(
            Vector2 translation,
            CubismParameter parameter,
            CubismPhysicsParticle[] particles,
            int particleIndex,
            Vector2 gravity
            )
        {
            var outputValue = translation.X;

            if (IsInverted)
            {
                outputValue *= 1.0f;
            }

            return outputValue;
        }

        private float getOutputTranslationY(
            Vector2 translation,
            CubismParameter parameter,
            CubismPhysicsParticle[] particles,
            int particleIndex,
            Vector2 gravity
            )
        {
            var outputValue = translation.Y;

            if (IsInverted)
            {
                outputValue *= -1.0f;
            }

            return outputValue;
        }

        private float getOutputAngle(
            Vector2 translation,
            CubismParameter parameter,
            CubismPhysicsParticle[] particles,
            int particleIndex,
            Vector2 gravity
            )
        {
            var parentGravity = Vector2.Zero;

            if (CubismPhysics.UseAngleCorrection)
            {
                if (particleIndex < 2)
                {
                    parentGravity = gravity;
                    parentGravity.Y *= -1.0f;
                }
                else
                {
                    parentGravity = particles[particleIndex - 1].Position - particles[particleIndex - 2].Position;
                }
            }
            else
            {
                parentGravity = gravity;
                parentGravity.Y *= -1.0f;
            }

            var outputValue = CubismMath.DirectionToRadian(parentGravity, translation);

            if (IsInverted)
            {
                outputValue *= -1.0f;
            }

            return outputValue;
        }

        private float getOutputScaleTranslationX() => TranslationScale.X;

        private float getOutputScaleTranslationY() => TranslationScale.Y;

        private float getOuputScaleAngle() => AngleScale;


        public void InitializeGetter()
        {
            switch (SourceComponent)
            {
                case CubismPhysicsSourceComponent.X:
                {
                    GetScale = getOutputScaleTranslationX;

                    GetValue = getOutputTranslationX;
                }
                break;
                case CubismPhysicsSourceComponent.Y:
                {
                    GetScale = getOutputScaleTranslationY;

                    GetValue = getOutputTranslationY;
                }
                break;
                case CubismPhysicsSourceComponent.Angle:
                {
                    GetScale = getOuputScaleAngle;

                    GetValue = getOutputAngle;
                }
                break;
            }
        }
        #endregion

        #region Fields

        public string DestinationId;

        public int ParticleIndex;

        public Vector2 TranslationScale;

        public float AngleScale;

        public float Weight;

        public CubismPhysicsSourceComponent SourceComponent;

        public bool IsInverted;

        public float ValueExceededMaximum;

        public CubismParameter Destination;

        public ValueGetter GetValue;

        public ScaleGetter GetScale;

        #endregion

    }
}

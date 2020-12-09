// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System;
using System.Linq;
using Vignette.Application.Live2D.Json;
using Vignette.Application.Live2D.Model;

namespace Vignette.Application.Live2D.Motion
{
    public class CubismExpression : ICubismMotion, ICanUpdateParameter
    {
        public float GlobalFadeInSeconds { get; set; }

        public float GlobalFadeOutSeconds { get; set; }

        public float Weight { get; set; }

        private ExpressionParameter[] parameters;

        public CubismExpression(CubismExpressionSetting json, CubismModel model)
        {
            GlobalFadeInSeconds = Math.Min(Math.Max(json.FadeInTime, 0), 1);
            GlobalFadeOutSeconds = Math.Min(Math.Max(json.FadeOutTime, 0), 1);

            parameters = new ExpressionParameter[json.Parameters.Length];
            for (int i = 0; i < json.Parameters.Length; i++)
            {
                parameters[i] = new ExpressionParameter
                {
                    Blending = (Blending)Enum.Parse(typeof(Blending), json.Parameters[i].Blend, true),
                    Parameter = model.Parameters.FirstOrDefault(p => p.Name == json.Parameters[i].Id),
                };
            }
        }

        public void Update(float time, bool loop)
        {
            foreach (var expressionParameter in parameters)
            {
                var parameter = expressionParameter.Parameter;
                switch (expressionParameter.Blending)
                {
                    case Blending.Add:
                        parameter.Value += parameter.Value + expressionParameter.Value * Weight;
                        break;

                    case Blending.Multiply:
                        parameter.Value *= (expressionParameter.Value - 1.0f) * Weight + 1.0f;
                        break;

                    case Blending.Overwrite:
                        parameter.Value = expressionParameter.Value * (1.0f - Weight) + expressionParameter.Value * Weight;
                        break;
                }
            }
        }

        public enum Blending
        {
            Add,

            Multiply,

            Overwrite,
        }

        private struct ExpressionParameter
        {
            public CubismParameter Parameter;

            public Blending Blending;

            public float Value;
        }
    }
}

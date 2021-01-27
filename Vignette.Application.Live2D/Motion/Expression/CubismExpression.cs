// Copyright (c) Vignette Project.
// Licensed under MIT. Please see LICENSE for more details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using System;
using System.Collections.Generic;
using Vignette.Application.Live2D.Json;
using Vignette.Application.Live2D.Model;

namespace Vignette.Application.Live2D.Motion.Expression
{
    public class CubismExpression : ICubismMotion
    {
        private double globalFadeInSeconds;

        public double GlobalFadeInSeconds
        {
            get => globalFadeInSeconds;
            set => globalFadeInSeconds = Math.Min(Math.Max(value, 0), 1);
        }

        private double globalFadeOutSeconds;

        public double GlobalFadeOutSeconds
        {
            get => globalFadeOutSeconds;
            set => globalFadeOutSeconds = Math.Min(Math.Max(value, 0), 1);
        }

        public double Weight { get; set; }

        private List<ExpressionParameter> parameters = new List<ExpressionParameter>();

        public CubismExpression(CubismModel model, CubismExpressionSetting setting)
        {
            GlobalFadeInSeconds = setting.FadeInTime;
            GlobalFadeOutSeconds = setting.FadeInTime;

            for (int i = 0; i < setting.Parameters.Count; i++)
            {
                var param = new ExpressionParameter();
                var item = setting.Parameters[i];
                param.Parameter = model.Parameters.Get(item.Id);
                param.Blending = Enum.Parse<ExpressionBlending>(item.Blend);
                param.Value = item.Value;
                parameters.Add(param);
            }
        }

        public void Update(double time, bool loop = false)
        {
            foreach (var expressionParam in parameters)
            {
                var param = expressionParam.Parameter;
                switch (expressionParam.Blending)
                {
                    case ExpressionBlending.Add:
                        param.Value += (float)(param.Value + expressionParam.Value * Weight);
                        break;

                    case ExpressionBlending.Multiply:
                        param.Value *= (float)((expressionParam.Value - 1) * Weight + 1.0);
                        break;

                    case ExpressionBlending.Overwrite:
                        param.Value = (float)(expressionParam.Value * (1 - Weight) + expressionParam.Value * Weight);
                        break;
                }
            }
        }
    }
}

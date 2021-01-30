// Copyright (c) Vignette Project.
// Licensed under MIT. Please see LICENSE for more details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using System;
using osuTK;
using Vignette.Application.Live2D.Model;

namespace Vignette.Application.Live2D.Utils
{
    public static class CubismMath
    {
        public static float Maximum(float left, float right) => (left > right) ? left : right;

        public static float Minimum(float left, float right) => (left < right) ? left : right;

        public static double EaseSine(double t) => Math.Clamp(0.5f - 0.5f * Math.Cos(Math.PI * t), 0.0f, 1.0f);

        public static float DegreesToRadian(float degrees) => (degrees / 180.0f) * MathF.PI;

        public static float RadianToDegrees(float radians) => (radians * 180.0f) * MathF.PI;

        public static float DirectionToRadian(Vector2 from, Vector2 to)
        {
            float q1, q2, ret;

            q1 = MathF.Atan2(to.Y, to.X);
            q2 = MathF.Atan2(from.Y, from.X);
            ret = q1 - q2;

            while (ret < -MathF.PI)
                ret += MathF.PI * 2.0f;

            while (ret > MathF.PI)
                ret -= MathF.PI * 2.0f;

            return ret;
        }

        public static float DirectionToDegrees(Vector2 from, Vector2 to)
        {
            float radian, degree;

            radian = DirectionToRadian(from, to);
            degree = RadianToDegrees(radian);

            if ((to.X - from.X) > 0)
                degree = -degree;

            return degree;
        }



        public static Vector2 RadianToDirection(float totalAngle) => new Vector2(MathF.Sin(totalAngle), MathF.Cos(totalAngle));

        public static float GetRangeValue(float min, float max)
        {
            var maxValue = MathF.Max(min, max);
            var minValue = MathF.Min(min, max);

            return MathF.Abs(maxValue - minValue);
        }

        public static float GetDefaultValue(float min, float max)
        {
            var minValue = MathF.Min(min, max);
            return minValue + (GetRangeValue(min, max) / 2.0f);
        }

        public static float Normalize(CubismParameter parameter, float normalizedMinimum, float normalizedMaximum, float normalizedDefault, bool isInverted = false)
        {
            var result = 0.0f;

            var maxValue = MathF.Max(parameter.Maximum, parameter.Minimum);

            if (maxValue < parameter.Value)
            {
                parameter.Value = maxValue;
            }

            var minValue = MathF.Min(parameter.Maximum, parameter.Minimum);

            if (minValue > parameter.Value)
            {
                parameter.Value = minValue;
            }

            var minNormValue = MathF.Min(normalizedMinimum, normalizedMaximum);
            var maxNormValue = MathF.Max(normalizedMinimum, normalizedMaximum);
            var middleNormValue = normalizedDefault;

            var middleValue = GetDefaultValue(minValue, maxValue);
            var paramValue = parameter.Value - middleValue;

            switch((int)MathF.Sign(paramValue))
            {
                case 1:
                {
                    var nLength = maxNormValue - middleNormValue;
                    var pLength = maxValue - middleValue;

                    if (pLength != 0.0f)
                    {
                        result = paramValue * (nLength / pLength);
                        result += middleNormValue;
                    }

                    break;
                }

                case -1:
                {
                    var nLength = minNormValue - middleNormValue;
                    var pLength = minValue - middleValue;

                    if (pLength != 0.0f)
                    {
                        result = paramValue * (nLength / pLength);
                        result += middleNormValue;
                    }

                    break;
                }

                case 0:
                {
                    result = middleNormValue;

                    break;
                }
            }

            return (isInverted) ? result : (result * -1.0f);
        }

    }
}

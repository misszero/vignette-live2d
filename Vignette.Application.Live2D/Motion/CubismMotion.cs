// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System;
using System.Collections.Generic;
using System.Linq;
using Vignette.Application.Live2D.Id;
using Vignette.Application.Live2D.Json;
using Vignette.Application.Live2D.Model;
using Vignette.Application.Live2D.Motion.Segments;
using Vignette.Application.Live2D.Utils;
using MotionSetting = Vignette.Application.Live2D.Json.CubismModelSetting.FileReference.Motion;
using CurveSetting = Vignette.Application.Live2D.Json.CubismMotionSetting.Curve;

namespace Vignette.Application.Live2D.Motion
{
    public class CubismMotion : ICubismMotion, ICanUpdateParameter
    {
        public bool CanLoop { get; protected set; }

        public float Duration { get; protected set; }

        public bool LoopFadingEnabled { get; set; } = true;

        public float GlobalFadeInSeconds { get; set; }

        public float GlobalFadeOutSeconds { get; set; }

        public float Weight { get; set; }

        private Curve[] curves;

        private MotionEvent[] events;

        private Dictionary<CubismId, Curve> effectCurves = new Dictionary<CubismId, Curve>();

        private List<(CubismId, CubismParameter)> effectUnusedParameters = new List<(CubismId, CubismParameter)>();

        public CubismMotion(CubismMotionSetting json, CubismModel model, MotionSetting setting)
        {
            Duration = json.Meta.Duration;
            CanLoop = json.Meta.Loop;

            GlobalFadeInSeconds = setting?.FadeInTime ?? 0;
            GlobalFadeInSeconds = !float.IsNaN(GlobalFadeInSeconds) ? GlobalFadeInSeconds : json.Meta.FadeInTime;
            GlobalFadeInSeconds = !float.IsNaN(GlobalFadeInSeconds) ? GlobalFadeInSeconds : 0;

            GlobalFadeOutSeconds = setting?.FadeOutTime ?? 0;
            GlobalFadeOutSeconds = !float.IsNaN(GlobalFadeOutSeconds) ? GlobalFadeOutSeconds : json.Meta.FadeOutTime;
            GlobalFadeOutSeconds = !float.IsNaN(GlobalFadeOutSeconds) ? GlobalFadeOutSeconds : 0;

            var curves = new List<Curve>();
            foreach (var item in json.Curves)
            {
                var curve = parseCurve(model, item);

                if (curve == null)
                    continue;

                if (curve.Target == MotionTarget.Model)
                    effectCurves[((Curve<CubismId>)curve).Effect] = curve;
                else
                    curves.Add(curve);
            }

            this.curves = curves.ToArray();

            if ((json.UserData != null) && (json.Meta.UserDataCount == json.UserData.Count))
            {
                events = new MotionEvent[json.Meta.UserDataCount];
                for (int i = 0; i < json.Meta.UserDataCount; i++)
                {
                    events[i] = new MotionEvent
                    {
                        Time = json.UserData[i].Time,
                        Value = json.UserData[i].Value,
                    };
                }
            }
            else
                events = Array.Empty<MotionEvent>();
        }

        public string[] GetFiredEvent(double time, double previousTime, bool looping)
        {
            var result = new List<string>();
            if (!looping)
            {
                foreach (var data in events)
                {
                    if ((previousTime < data.Time) && (data.Time <= time))
                        result.Add(data.Value);
                }
            }
            else
            {
                double timeInMotion = time % Duration;
                double previousTimeInMotion = previousTime % Duration;
                foreach (var data in events)
                {
                    if ((previousTimeInMotion < data.Time) && (data.Time <= timeInMotion) ||
                        (timeInMotion < previousTimeInMotion) && ((data.Time <= timeInMotion) || (previousTimeInMotion <= data.Time)))
                        result.Add(data.Value);
                }
            }

            return result.ToArray();
        }

        public void Update(float time, bool looping)
        {
            float timeInMotion = time % Duration;
            float globalFadeInWeight = calculateFadeInWeight(time, Duration, GlobalFadeInSeconds, looping, LoopFadingEnabled);
            float globalFadeOutWeight = calculateFadeOutWeight(time, Duration, GlobalFadeOutSeconds, looping, LoopFadingEnabled);

            Dictionary<CubismId, float> values = new Dictionary<CubismId, float>();
            foreach (var curve in effectCurves)
                values[curve.Key] = curve.Value.Evaluate(time);

            int i;
            for (i = 0; i < curves.Length; i++)
            {
                var curve = curves[i];
                float value, weight;
                if (curve.Target == MotionTarget.Parameter)
                {
                    var curveParameter = curve as Curve<CubismParameter>;
                    value = curve.Evaluate(timeInMotion);

                    if (curveParameter.Effect != null)
                    {
                        if (values.TryGetValue(curveParameter.Effect, out float effectValue))
                            value *= effectValue;
                    }

                    if (double.IsNaN(curve.FadeInTime) && double.IsNaN(curve.FadeOutTime))
                        weight = globalFadeInWeight * globalFadeOutWeight;
                    else
                    {
                        weight = Weight;

                        if (double.IsNaN(curve.FadeInTime))
                            weight *= globalFadeInWeight;
                        else
                            weight *= calculateFadeInWeight(time, Duration, curve.FadeInTime, looping, LoopFadingEnabled);

                        if (double.IsNaN(curve.FadeOutTime))
                            weight *= globalFadeOutWeight;
                        else
                            weight *= calculateFadeOutWeight(time, Duration, curve.FadeInTime, looping, LoopFadingEnabled);
                    }

                    float currentValue = curveParameter.Effect.Value;
                    float newValue = currentValue + (value - currentValue) * weight;

                    curveParameter.Effect.Value = newValue;
                }
                else if (curve.Target == MotionTarget.PartOpacity)
                {
                    value = curve.Evaluate(timeInMotion);
                    // Not sure why weight was set as its unused
                    // weight = 1.0;
                    ((Curve<CubismPart>)curve).Effect.Target = value;
                }
            }

            foreach (var (id, target) in effectUnusedParameters)
            {
                if (values.TryGetValue(id, out float value))
                {
                    float weight = globalFadeInWeight * globalFadeOutWeight;
                    float currentValue = target.Value;
                    float newValue = currentValue + (value - currentValue) * weight;
                    target.Value = newValue;
                }

                i++;
            }
        }

        private float calculateFadeInWeight(float time, float duration, float fadeTime, bool loopEnabled, bool loopFading)
        {
            if (fadeTime <= 0)
                return 1;

            return (!loopEnabled || loopFading)
                ? CubismMath.EaseSine(time / fadeTime)
                : CubismMath.EaseSine(time % duration / fadeTime);
        }

        private float calculateFadeOutWeight(float time, float duration, float fadeTime, bool loopEnabled, bool loopFading)
        {
            if (fadeTime <= 0)
                return 0;

            return (!loopEnabled)
                ? CubismMath.EaseSine((duration - time) / fadeTime)
                : (!loopFading) ? 1 : CubismMath.EaseSine((duration - (time % duration)) / fadeTime);
        }

        private Curve parseCurve(CubismModel model, CurveSetting setting)
        {
            Curve curve;
            switch (setting.Target)
            {
                case "Model":
                {
                    curve = new Curve<CubismId>
                    {
                        Target = MotionTarget.Model,
                        Effect = new CubismId(setting.Id),
                    };
                    break;
                }

                case "Parameter":
                {
                    curve = new Curve<CubismParameter>
                    {
                        Target = MotionTarget.Parameter,
                        Effect = model.Parameters.FirstOrDefault(p => p.Name == setting.Id),
                    };

                    if (((Curve<CubismParameter>)curve).Effect == null)
                        return null;

                    break;
                }

                case "PartOpacity":
                {
                    curve = new Curve<CubismPart>
                    {
                        Target = MotionTarget.PartOpacity,
                        Effect = model.Parts.FirstOrDefault(p => p.Name == setting.Id),
                    };

                    if (((Curve<CubismPart>)curve).Effect == null)
                        return null;

                    break;
                }

                default:
                    return null;
            }

            curve.FadeInTime = setting.FadeInTime;
            curve.FadeOutTime = setting.FadeOutTime;

            ControlPoint last = new ControlPoint(setting.Segments[0], setting.Segments[1]);

            var segments = new List<Segment>();
            for (int i = 2; i < setting.Segments.Count;)
            {
                Segment segment;
                switch ((SegmentType)setting.Segments[i])
                {
                    case SegmentType.Linear:
                    {
                        segment = new LinearSegment();
                        segment.Points[0] = last;
                        segment.Points[1] = new ControlPoint(setting.Segments[i + 1], setting.Segments[i + 2]);
                        last = segment.Points[1];
                        i += 3;
                        break;
                    }

                    case SegmentType.Bezier:
                    {
                        segment = new BezierSegment();
                        segment.Points[0] = last;
                        segment.Points[1] = new ControlPoint(setting.Segments[i + 1], setting.Segments[i + 2]);
                        segment.Points[2] = new ControlPoint(setting.Segments[i + 3], setting.Segments[i + 4]);
                        segment.Points[3] = new ControlPoint(setting.Segments[i + 5], setting.Segments[i + 6]);
                        last = segment.Points[3];
                        i += 7;
                        break;
                    }

                    case SegmentType.Stepped:
                    {
                        segment = new SteppedSegment();
                        segment.Points[0] = last;
                        segment.Points[1] = new ControlPoint(setting.Segments[i + 1], setting.Segments[i + 2]);
                        last = segment.Points[1];
                        i += 3;
                        break;
                    }

                    case SegmentType.InverseStepped:
                    {
                        segment = new InverseSteppedSegment();
                        segment.Points[0] = last;
                        segment.Points[1] = new ControlPoint(setting.Segments[i + 1], setting.Segments[i + 2]);
                        last = segment.Points[1];
                        i += 3;
                        break;
                    }

                    default:
                        throw new ArgumentException("Error parsing motion.");
                }

                segments.Add(segment);
            }

            curve.Segments = segments.ToArray();
            return curve;
        }
    }
}

// Copyright (c) Vignette Project.
// Licensed under MIT. Please see LICENSE for more details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using System;
using System.Collections.Generic;
using System.Linq;
using Vignette.Application.Live2D.Id;
using Vignette.Application.Live2D.Json;
using Vignette.Application.Live2D.Model;
using Vignette.Application.Live2D.Motion.Segments;
using Vignette.Application.Live2D.Utils;

namespace Vignette.Application.Live2D.Motion
{
    public class CubismMotion : ICubismMotion
    {
        public double GlobalFadeInSeconds { get; set; }

        public double GlobalFadeOutSeconds { get; set; }

        public double Duration { get; private set; }

        public double Weight { get; set; }

        public bool LoopFading { get; set; }

        public bool CanLoop { get; private set; }

        private List<Curve> curves;

        private List<MotionEvent> events = new List<MotionEvent>();

        private readonly Dictionary<CubismId, Curve> effectCurves = new Dictionary<CubismId, Curve>();

        private readonly List<(CubismId, CubismParameter)> effectUnusedParameters = new List<(CubismId, CubismParameter)>();

        public CubismMotion(CubismModel model, CubismMotionSetting setting, CubismModelSetting.FileReference.Motion modelMotionSetting = null)
        {
            Duration = setting.Meta.Duration;
            CanLoop = setting.Meta.Loop;

            if (GlobalFadeInSeconds <= 0)
                GlobalFadeInSeconds = modelMotionSetting?.FadeInTime ?? setting.Meta.FadeInTime;

            if (GlobalFadeOutSeconds <= 0)
                GlobalFadeOutSeconds = modelMotionSetting?.FadeOutTime ?? setting.Meta.FadeOutTime;

            var curves = new List<Curve>();

            foreach (var item in setting.Curves)
            {
                var curve = parseCurve(item, model);
                if (curve == null)
                    continue;

                if (curve.TargetType == MotionTarget.Model)
                    effectCurves[curve.Effect] = curve;
                else
                    curves.Add(curve);
            }

            this.curves = curves;
            if (setting.Meta.UserDataCount == (setting.UserData?.Count ?? 0))
            {
                for (int i = 0; i < setting.Meta.UserDataCount; i++)
                {
                    events.Add(new MotionEvent
                    {
                        Time = setting.UserData[i].Time,
                        Value = setting.UserData[i].Value,
                    });
                }
            }
        }

        public void Update(double time, bool loop = false)
        {
            double timeInMotion = time % Duration;

            double globalFadeInWeight = calculateFadeInWeight(time, Duration, GlobalFadeInSeconds, loop, LoopFading);
            double globalFadeOutWeight = calculateFadeOutWeight(time, Duration, GlobalFadeOutSeconds, loop, LoopFading);

            var effectValues = new Dictionary<CubismId, double>();
            foreach ((var effect, var curve) in effectCurves)
                effectValues[effect] = curve.ValueAt(time);

            int i;
            for (i = 0; i < curves.Count; i++)
            {
                var curve = curves[i];

                if (curve.TargetType == MotionTarget.Parameter)
                {
                    double value, weight;
                    value = curve.ValueAt(timeInMotion);

                    if (curve.Effect != null)
                    {
                        if (effectValues.TryGetValue(curve.Effect, out double effectValue))
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
                            weight *= calculateFadeOutWeight(time, Duration, curve.FadeInTime, loop, LoopFading);

                        if (double.IsNaN(curve.FadeOutTime))
                            weight *= globalFadeOutWeight;
                        else
                            weight *= calculateFadeOutWeight(time, Duration, curve.FadeOutTime, loop, LoopFading);
                    }

                    double currentValue = curve.Parameter.Value;
                    curve.Parameter.Value = (float)(currentValue + (value - currentValue) * weight);
                }
                else if (curve.TargetType == MotionTarget.PartOpacity)
                    curve.Part.Value = (float)curve.ValueAt(timeInMotion);
            }

            foreach ((var effectId, var target) in effectUnusedParameters)
            {
                if (effectValues.TryGetValue(effectId, out var value))
                {
                    double weight = globalFadeInWeight * globalFadeOutWeight;
                    double currentValue = target.Value;
                    target.Value = (float)(currentValue + (value - currentValue) * weight);
                }

                i++;
            }
        }

        public CubismId GetEffect(string id)
        {
            foreach (var effect in effectCurves.Keys)
            {
                if (effect.Equals(id))
                    return effect;
            }

            return null;
        }

        public bool SetEffectParameters(CubismId effectId, IEnumerable<CubismParameter> parameters)
        {
            if (!effectCurves.ContainsKey(effectId))
                return false;

            var parameterArray = parameters.ToArray();
            bool[] usedIds = new bool[parameterArray.Length];
            foreach (var curve in curves)
            {
                curve.Effect = null;
                for (int i = 0; i < parameters.Count(); i++)
                {
                    if (curve.Parameter == parameterArray[i])
                    {
                        curve.Effect = effectId;
                        usedIds[i] = true;
                        break;
                    }
                }
            }

            effectUnusedParameters.RemoveAll(p => p.Item1 == effectId);
            for (int i = 0; i < parameters.Count(); i++)
            {
                if (!usedIds[i])
                    effectUnusedParameters.Add((effectId, parameterArray[i]));
            }

            return true;
        }

        public IEnumerable<string> GetFiredEvents(double time, double previousTime, bool loop)
        {
            var result = new List<string>();

            if (!loop)
            {
                foreach (var @event in events)
                {
                    if ((previousTime < @event.Time) && (@event.Time <= time))
                        result.Add(@event.Value);
                }
            }
            else
            {
                double timeInMotion = time % Duration;
                double previousTimeInMotion = previousTime % Duration;
                foreach (var @event in events)
                {
                    if (((previousTimeInMotion < @event.Time) && (@event.Time <= timeInMotion)) ||
                        ((timeInMotion < previousTimeInMotion) && ((@event.Time <= timeInMotion) || (previousTimeInMotion <= @event.Time))))
                        result.Add(@event.Value);
                }
            }

            return result;
        }

        private Curve parseCurve(CubismMotionSetting.Curve item, CubismModel model)
        {
            var curve = new Curve();

            switch (item.Target)
            {
                case "Model":
                    curve.TargetType = MotionTarget.Model;
                    curve.Effect = new CubismId(item.Id);
                    break;

                case "Parameter":
                    if (!model.Parameters.Has(item.Id))
                        return null;

                    curve.TargetType = MotionTarget.Parameter;
                    curve.Parameter = model.Parameters.Get(item.Id);
                    break;

                case "PartOpacity":
                    if (!model.Parts.Has(item.Id))
                        return null;

                    curve.TargetType = MotionTarget.PartOpacity;
                    curve.Part = model.Parts.Get(item.Id);
                    break;

                default:
                    return null;
            }

            curve.FadeInTime = item.FadeInTime;
            curve.FadeOutTime = item.FadeOutTime;

            var segments = item.Segments;
            int segmentCount = item.Segments.Count();

            var last = new ControlPoint(segments[0], segments[1]);
            var segmentList = new List<Segment>();

            for (int i = 2; i < segmentCount;)
            {
                switch ((SegmentType)segments[i])
                {
                    case SegmentType.Linear:
                    {
                        var segment = new LinearSegment();
                        segment.Points[0] = last;
                        segment.Points[1] = new ControlPoint(segments[i + 1], segments[i + 2]);
                        segmentList.Add(segment);
                        last = segment.Points[1];

                        i += 3;
                        break;
                    }

                    case SegmentType.Bezier:
                    {
                        var segment = new BezierSegment();
                        segment.Points[0] = last;
                        segment.Points[1] = new ControlPoint(segments[i + 1], segments[i + 2]);
                        segment.Points[2] = new ControlPoint(segments[i + 3], segments[i + 4]);
                        segment.Points[3] = new ControlPoint(segments[i + 5], segments[i + 6]);
                        segmentList.Add(segment);
                        last = segment.Points[3];

                        i += 7;
                        break;
                    }

                    case SegmentType.Stepped:
                    {
                        var segment = new SteppedSegment();
                        segment.Points[0] = last;
                        segment.Points[1] = new ControlPoint(segments[i + 1], segments[i + 2]);
                        segmentList.Add(segment);
                        last = segment.Points[1];

                        i += 3;
                        break;
                    }

                    case SegmentType.InverseStepped:
                    {
                        var segment = new InverseSteppedSegment();
                        segment.Points[0] = last;
                        segment.Points[1] = new ControlPoint(segments[i + 1], segments[i + 2]);
                        segmentList.Add(segment);
                        last = segment.Points[1];

                        i += 3;
                        break;
                    }

                    default:
                        throw new ArgumentOutOfRangeException($"Segment (index: {i}) is out of range of supported values.");
                }
            }

            curve.Segments = segmentList;
            return curve;
        }

        private static double calculateFadeInWeight(double time, double duration, double fadeTime, bool loop, bool loopFading)
        {
            if (fadeTime <= 0)
                return 1;

            return (!loop || !loopFading)
                ? CubismMath.EaseSine(time / fadeTime)
                : CubismMath.EaseSine(time % duration / fadeTime);
        }

        private double calculateFadeOutWeight(double time, double duration, double fadeTime, bool loop, bool loopFading)
        {
            if (fadeTime <= 0)
                return 1;

            if (!loop)
                return CubismMath.EaseSine((duration - time) / fadeTime);
            else
                return (!loopFading) ? 1 : CubismMath.EaseSine((duration - (time % duration)) / fadeTime);
        }
    }
}

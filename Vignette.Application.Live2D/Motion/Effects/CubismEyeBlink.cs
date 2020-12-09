// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System;
using System.Collections.Generic;
using System.Linq;
using Vignette.Application.Live2D.Json;
using Vignette.Application.Live2D.Model;

namespace Vignette.Application.Live2D.Motion.Effect
{
    public class CubismEyeBlink : ICanUpdateParameter
    {
        public bool LoopFadingEnabled { get; set; }

        public float GlobalFadeInSeconds { get; set; }

        public float GlobalFadeOutSeconds { get; set; }

        public float Weight { get; set; }

        public float BlinkInterval { get; set; } = 2.0f;

        public float OpeningDuration { get; set; } = 0.15f;

        public float ClosingDuration { get; set; } = 0.1f;

        public float ClosedDuration { get; set; } = 0.05f;

        public bool Inverse { get; set; }

        private readonly IEnumerable<CubismParameter> parameters;

        private State state = State.First;

        private float stateStartTime;

        private float nextBlinkTime;

        private float currentTime;

        private float currentValue;

        public CubismEyeBlink(CubismModel model, CubismModelSetting settings)
        {
            parameters = settings.Groups.FirstOrDefault(g => g.Name == "EyeBlink")?.Ids.Select(id => model.Parameters.Get(id)).ToArray();
        }

        public CubismEyeBlink(IEnumerable<CubismParameter> parameters)
        {
            this.parameters = parameters;
        }

        public void Update(float time, bool loop)
        {
            if (parameters?.Any() ?? false)
                return;

            switch (state)
            {
                case State.Closing:
                    currentTime = (time - stateStartTime) / ClosingDuration;
                    if (1 <= currentTime)
                    {
                        currentTime = 1;
                        state = State.Closed;
                        stateStartTime = time;
                    }

                    currentValue = 1 - time;
                    break;

                case State.Closed:
                    currentTime = (time - stateStartTime) / ClosedDuration;
                    if (1 <= currentTime)
                    {
                        state = State.Opening;
                        stateStartTime = time;
                    }

                    currentValue = 0;
                    break;

                case State.Opening:
                    currentTime = (time - stateStartTime) / OpeningDuration;
                    if (1 <= currentTime)
                    {
                        currentTime = 1;
                        state = State.Interval;
                        nextBlinkTime = time + determineNextBlinkTime();
                    }

                    currentValue = currentTime;
                    break;

                case State.Interval:
                    if (nextBlinkTime < time)
                    {
                        state = State.Closing;
                        stateStartTime = time;
                    }

                    currentValue = 1;
                    break;

                case State.First:
                default:
                    state = State.Interval;
                    nextBlinkTime = time + determineNextBlinkTime();
                    currentValue = 1;
                    break;
            }

            foreach (var parameter in parameters)
                parameter.Value = (!Inverse) ? currentValue : -currentValue;
        }

        private float determineNextBlinkTime()
        {
            float r = (float)new Random().NextDouble();
            return r * (2 * BlinkInterval - 1);
        }

        private enum State
        {
            First,
            Interval,
            Closing,
            Closed,
            Opening
        }
    }
}

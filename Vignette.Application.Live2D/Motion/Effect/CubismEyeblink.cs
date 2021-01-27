// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System;
using System.Collections.Generic;
using System.Linq;
using Vignette.Application.Live2D.Model;

namespace Vignette.Application.Live2D.Motion.Effect
{
    public class CubismEyeblink
    {
        public EyeState State { get; private set; }

        public double BlinkInterval { get; set; }

        public double ClosedDuration { get; set; }

        public double ClosingDuration { get; set; }

        public double OpeningDuration { get; set; }

        public IEnumerable<CubismParameter> Parameters => parameters;

        private double nextBlinkTime;

        private double currentStateStartTime;

        private List<CubismParameter> parameters;

        public CubismEyeblink(IEnumerable<CubismParameter> parameters)
        {
            this.parameters = parameters.ToList();
        }

        public void Update(double time)
        {
            double value = 0.0f;
            double t;

            switch (State)
            {
                case EyeState.Closing:
                    t = (time - currentStateStartTime) / ClosingDuration;

                    if (1.0f <= t)
                    {
                        t = 1.0f;
                        State = EyeState.Closed;
                        currentStateStartTime = time;
                    }

                    value = 1.0f - t;
                    break;

                case EyeState.Closed:
                    t = (time - currentStateStartTime) / ClosedDuration;

                    if (1.0f <= t)
                    {
                        State = EyeState.Opening;
                        currentStateStartTime = time;
                    }

                    value = 0;
                    break;

                case EyeState.Opening:
                    t = (time - currentStateStartTime) / OpeningDuration;

                    if (1.0f <= t)
                    {
                        t = 1.0f;
                        State = EyeState.Open;
                        nextBlinkTime = time + determineNextBlinkTime();
                    }

                    value = t;
                    break;

                case EyeState.Open:
                    if (nextBlinkTime < time)
                    {
                        State = EyeState.Closing;
                        currentStateStartTime = time;
                    }

                    value = 1.0f;
                    break;

                case EyeState.Initial:
                    State = EyeState.Open;
                    nextBlinkTime = time + determineNextBlinkTime();
                    value = 1.0f;
                    break;
            }

            foreach (var param in parameters)
                param.Value = (float)value;
        }

        private double determineNextBlinkTime()
        {
            double r = new Random().NextDouble();
            return r * (2.0f * BlinkInterval - 1.0f);
        }

        public enum EyeState
        {
            Initial,

            Open,

            Opening,

            Closed,

            Closing,
        }
    }
}

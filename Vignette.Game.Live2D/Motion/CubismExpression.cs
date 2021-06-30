// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using Vignette.Game.Live2D.Graphics;

namespace Vignette.Game.Live2D.Motion
{
    public class CubismExpression : ICubismMotion
    {
        public double GlobalFadeInSeconds { get; set; }
        public double GlobalFadeOutSeconds { get; set; }
        public double Weight { get; set; }

        public void Update(double time, bool loop = false)
        {
        }

        private enum CubismExpressionBlending
        {
            Add,
            Multiply,
            Overwrite,
        }

        private struct CubismExpressionParameter
        {
            public CubismParameter Parameter { get; set; }

            public CubismExpressionBlending Blending { get; set; }

            public double Value { get; set; }
        }
    }
}

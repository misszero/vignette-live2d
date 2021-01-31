// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using osu.Framework.Graphics;
using osuTK;

namespace Vignette.Application.Live2D.Graphics
{
    public class CanvasOptions
    {
        public float Scale { get; set; } = 1.0f;

        public Axes RelativePositionAxes { get; set; }

        public float X
        {
            get => position.X;
            set => position.X = value;
        }

        public float Y
        {
            get => position.Y;
            set => position.Y = value;
        }
        
        public Vector2 Position
        {
            get => position;
            set => position = value;
        }

        private Vector2 position;
    }
}

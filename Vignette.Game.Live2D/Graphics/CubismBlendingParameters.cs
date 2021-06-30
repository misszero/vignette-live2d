// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using osu.Framework.Graphics;

namespace Vignette.Game.Live2D.Graphics
{
    public static class CubismBlendingParameters
    {
        public static BlendingParameters Multiply = new BlendingParameters
        {
            Source = BlendingType.DstColor,
            Destination = BlendingType.OneMinusSrcAlpha,
            SourceAlpha = BlendingType.Zero,
            DestinationAlpha = BlendingType.One,
        };

        public static BlendingParameters Add = new BlendingParameters
        {
            Source = BlendingType.One,
            Destination = BlendingType.One,
            SourceAlpha = BlendingType.Zero,
            DestinationAlpha = BlendingType.One,
        };

        public static BlendingParameters Normal = new BlendingParameters
        {
            Source = BlendingType.One,
            Destination = BlendingType.OneMinusSrcAlpha,
            SourceAlpha = BlendingType.One,
            DestinationAlpha = BlendingType.OneMinusSrcAlpha,
        };

        public static BlendingParameters Mask = new BlendingParameters
        {
            Source = BlendingType.Zero,
            Destination = BlendingType.OneMinusSrcColor,
            SourceAlpha = BlendingType.Zero,
            DestinationAlpha = BlendingType.OneMinusSrcAlpha,
        };
    }
}

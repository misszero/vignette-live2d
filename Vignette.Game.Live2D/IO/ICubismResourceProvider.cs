// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using Vignette.Game.Live2D.IO.Serialization;

namespace Vignette.Game.Live2D.IO
{
    /// <summary>
    /// An interface that exposes itself as a resource provider
    /// </summary>
    public interface ICubismResourceProvider
    {
        CubismModelSetting Settings { get; }

        LargeTextureStore Textures { get; }

        IResourceStore<byte[]> Resources { get; }
    }
}

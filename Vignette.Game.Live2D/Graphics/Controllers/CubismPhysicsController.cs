// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using Vignette.Game.Live2D.IO.Serialization;
using Vignette.Game.Live2D.Physics;

namespace Vignette.Game.Live2D.Graphics.Controllers
{
    /// <summary>
    /// A controller that manages the physics of a Live2D model.
    /// </summary>
    public class CubismPhysicsController : CubismController
    {
        private CubismPhysics physics;

        public CubismPhysicsController(CubismPhysicsSetting setting)
        {
        }
    }
}

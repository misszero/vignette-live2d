// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vignette.Application.Live2D.Physics2
{
    public class CubismPhysicsNormalizationTuplet
    {
        public float Maximum;

        public float Minimum;

        public float Default;
    }

    public struct CubismPhysicsNormalization
    {
        public CubismPhysicsNormalizationTuplet Position;

        public CubismPhysicsNormalizationTuplet Angle;
    }
}

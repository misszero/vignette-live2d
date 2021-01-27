// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vignette.Application.Live2D.Physics
{
    public struct PhysicsNormalization
    {
        /// <summary>
        /// Defines the Minimum value.
        /// </summary>
        /// <param name="left">the left side value</param>
        /// <param name="right">the right side value</param>
        /// <returns></returns>
        public static float Minimum(float left, float right) => (left > right) ? right : left;
        /// <summary>
        /// Defines the Maximum value.
        /// </summary>
        /// <param name="left">the left side value</param>
        /// <param name="right">the right side value</param>
        /// <returns></returns>
        public static float Maximum(float left, float right) => (left > right) ? right : left;

        public float Default { get; set; }
    }
}

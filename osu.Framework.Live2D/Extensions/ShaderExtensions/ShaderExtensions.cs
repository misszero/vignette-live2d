// Copyright (c) Nitrous <n20gaming2000@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics.Shaders;
using osuTK.Graphics.ES20;

namespace osu.Framework.Extensions.ShaderExtensions
{
    internal static class ShaderExtensions
    {
        public static int GetAttributeLocation(this Shader shader, string attribute)
        {
            int location = GL.GetAttribLocation((int)shader, attribute);

            if (location < 0)
                throw new InvalidOperationException($"Attribute '{attribute}' was not found.");

            return location;
        }
    }
}
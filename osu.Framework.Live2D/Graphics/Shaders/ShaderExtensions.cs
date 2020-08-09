using System;
using osuTK.Graphics.ES20;

namespace osu.Framework.Graphics.Shaders
{
    internal static class ShaderExtensions
    {
        public static int GetUniformLocation(this Shader shader, string uniform)
        {
            int location = GL.GetUniformLocation((int)shader, uniform);

            if (location < 0)
                throw new InvalidOperationException($"Uniform '{uniform}' was not found.");

            return location;
        }

        public static int GetAttributeLocation(this Shader shader, string attribute)
        {
            int location = GL.GetAttribLocation((int)shader, attribute);

            if (location < 0)
                throw new InvalidOperationException($"Attribute '{attribute}' was not found.");

            return location;
        }
    }
}
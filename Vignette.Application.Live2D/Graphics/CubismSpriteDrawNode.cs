// Copyright (c) Vignette Project.
// Licensed under MIT. Please see LICENSE for more details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.OpenGL.Vertices;
using osuTK;

namespace Vignette.Application.Live2D.Graphics
{
    public class CubismSpriteDrawNode : TexturedShaderDrawNode
    {
        protected new CubismSprite Source => (CubismSprite)base.Source;

        private readonly CubismRenderer renderer;

        public CubismSpriteDrawNode(CubismSprite source)
            : base(source)
        {
            renderer = source.Renderer;
            renderer.UsePremultipliedAlpha = source.UsePremultipliedAlpha;
        }

        public override void Draw(Action<TexturedVertex2D> vertexAction)
        {
            base.Draw(vertexAction);

            if (renderer != null)
            {
                float x = Source.Canvas.RelativePositionAxes.HasFlag(Axes.X)
                    ? Source.Canvas.X * Source.Width
                    : Source.Canvas.X;

                float y = Source.Canvas.RelativePositionAxes.HasFlag(Axes.Y)
                    ? Source.Canvas.Y * Source.Height
                    : Source.Canvas.Y;

                float canvasWidth = Source.Width / 2 * Source.Canvas.Scale;
                float canvasHeight = Source.Height / 2 * Source.Canvas.Scale;

                var matrix = Matrix4.Identity;
                matrix[0, 0] = Source.Canvas.Scale;
                matrix[1, 1] = Source.Canvas.Scale * (Source.Width / Source.Height);
                matrix[3, 0] = map(x * Source.Canvas.Scale, -canvasWidth, canvasWidth, -1.0f, 1.0f);
                matrix[3, 1] = map(-y * Source.Canvas.Scale, -canvasHeight, canvasHeight, -1.0f, 1.0f);

                renderer.MvpMatrix = matrix;
                renderer.Color = Source.Colour;
                renderer.Draw();
            }
        }

        private static float map(float value, float fromSource, float toSource, float fromTarget, float toTarget)
        {
            return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            renderer.Dispose();
        }
    }
}

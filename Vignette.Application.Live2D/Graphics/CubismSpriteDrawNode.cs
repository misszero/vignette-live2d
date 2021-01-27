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
                var matrix = Matrix4.Identity;
                matrix[0, 0] = Source.ScaleAdjust;
                matrix[1, 1] = Source.ScaleAdjust * (Source.Width / Source.Height);
                matrix[3, 0] = Source.PositionXAdjust / Source.DrawWidth;
                matrix[3, 1] = -Source.PositionYAdjust / Source.DrawHeight;

                renderer.MvpMatrix = matrix;
                renderer.Color = Source.Colour;
                renderer.Draw();
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            renderer.Dispose();
        }
    }
}

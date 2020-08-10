using System;
using CubismFramework;
using osu.Framework.Graphics.Cubism.Renderer;
using osu.Framework.Graphics.OpenGL.Vertices;
using osuTK;

namespace osu.Framework.Graphics.Cubism
{
    internal class CubismSpriteDrawNode : DrawNode
    {
        private CubismRenderer renderer;
        private CubismRenderingManager renderingManager;

        public CubismSpriteDrawNode(CubismSprite source)
            : base(source)
        {
            renderer = source.Renderer;
            renderingManager = source.RenderingManager;
        }

        public override void Draw(Action<TexturedVertex2D> vertexAction)
        {
            base.Draw(vertexAction);

            var source = (CubismSprite)Source;
            var transform = source.ModelTransform;

            Matrix4 projection = Matrix4.Identity;
            Matrix4 zoom = Matrix4.CreateScale(
                transform.Scale.X * source.DrawHeight / source.DrawWidth, 
                transform.Scale.Y, 0.0f);
            
            projection *= zoom;
            renderingManager.Draw(projection);
        }
    }
}
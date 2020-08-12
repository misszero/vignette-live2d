using System;
using osu.Framework.Graphics.OpenGL.Vertices;
using osuTK;

namespace osu.Framework.Graphics.Cubism
{
    internal class CubismSpriteDrawNode : DrawNode
    {
        public CubismSpriteDrawNode(CubismSprite source)
            : base(source)
        {
        }

        public override void Draw(Action<TexturedVertex2D> vertexAction)
        {
            base.Draw(vertexAction);

            var source = (CubismSprite)Source;

            Matrix4 projection = Matrix4.Identity;
            Matrix4 translate = Matrix4.CreateTranslation(new Vector3(source.ModelOffsetX, -source.ModelOffsetY, 0) / 100);
            Matrix4 zoom = Matrix4.CreateScale(source.ModelScale * source.DrawHeight / source.DrawWidth, source.ModelScale, 0.0f);
            
            projection *= zoom * translate;
            source.RenderingManager.Draw(projection);
        }

        protected override void Dispose(bool isDisposing)
        {
            var source = (CubismSprite)Source;

            // We are handling disposal here to ensure that it gets disposed after all draw calls have been performed
            source.RenderingManager.Dispose();
            source.Asset.Dispose();

            base.Dispose(isDisposing);
        }
    }
}
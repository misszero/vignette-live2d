using System;
using CubismFramework;
using osu.Framework.Graphics.OpenGL.Vertices;
using osuTK;

namespace osu.Framework.Graphics.Cubism
{
    internal class CubismSpriteDrawNode : DrawNode
    {
        protected new CubismSprite Source => (CubismSprite)base.Source;

        private CubismAsset asset;
        private CubismRenderingManager renderingManager;

        public CubismSpriteDrawNode(CubismSprite source)
            : base(source)
        {
        }

        public override void ApplyState()
        {
            base.ApplyState();

            asset = Source.Asset;
            renderingManager = Source.RenderingManager;
        }

        public override void Draw(Action<TexturedVertex2D> vertexAction)
        {
            base.Draw(vertexAction);

            Matrix4 projection = Matrix4.Identity;
            Matrix4 translate = Matrix4.CreateTranslation(new Vector3(Source.ModelPositionX / Source.DrawWidth, -Source.ModelPositionY / Source.DrawHeight, 0));
            Matrix4 zoom = Matrix4.CreateScale(Source.ModelScale.X * Source.DrawHeight / Source.DrawWidth, Source.ModelScale.Y, 0.0f);
            
            projection *= zoom * translate;
            renderingManager.Draw(projection);
        }

        protected override void Dispose(bool isDisposing)
        {
            // We are handling disposal here to ensure that it gets disposed after all draw calls have been performed
            renderingManager?.Dispose();
            asset?.Dispose();

            base.Dispose(isDisposing);
        }
    }
}
using System;
using CubismFramework;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Cubism;
using osu.Framework.Graphics.OpenGL;
using osu.Framework.Graphics.OpenGL.Vertices;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shaders;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Graphics.Containers
{
    public class CubismModelContainer : Drawable, IBufferedDrawable
    {
        public CubismAsset Asset;
        public CubismRenderer Renderer;
        public CubismRenderingManager RenderingManager;
        public CubismShaderManager shaderManager;

        public Color4 BackgroundColour => new Color4(0, 0, 0, 0);

        public DrawColourInfo? FrameBufferDrawColour => base.DrawColourInfo;

        public Vector2 FrameBufferScale => Vector2.One;

        public IShader TextureShader { get; private set; }

        public IShader RoundedTextureShader { get; private set; }

        protected override DrawNode CreateDrawNode() =>
            new BufferedDrawNode(this, new CubismModelContainerDrawNode(this), new BufferedDrawNodeSharedData());

        public CubismModelContainer(CubismAsset asset)
        {
            Asset = asset;
        }

        [BackgroundDependencyLoader]
        private void load(ShaderManager shaderManager)
        {
            this.shaderManager = new CubismShaderManager(shaderManager);

            TextureShader = shaderManager.Load(VertexShaderDescriptor.TEXTURE_2, FragmentShaderDescriptor.TEXTURE);
            RoundedTextureShader = shaderManager.Load(VertexShaderDescriptor.TEXTURE_2, FragmentShaderDescriptor.TEXTURE_ROUNDED);

            Renderer = new CubismRenderer(this.shaderManager);
            RenderingManager = new CubismRenderingManager(Renderer, Asset);
        }

        protected override void Update()
        {
            base.Update();

            Asset.Update(Clock.TimeInfo.Elapsed);
        }

        protected override void Dispose(bool isDisposing)
        {
            RenderingManager.Dispose();
            Renderer.Dispose();
            Asset.Dispose();

            base.Dispose(isDisposing);
        }

        private class CubismModelContainerDrawNode : DrawNode
        {
            private CubismRenderer renderer;
            private CubismRenderingManager renderingManager;
            private RectangleF screenSpaceDrawRect;

            public CubismModelContainerDrawNode(CubismModelContainer source)
                : base(source)
            {
            }

            public override void ApplyState()
            {
                base.ApplyState();

                var source = (CubismModelContainer)Source;
                renderer = source.Renderer;
                renderingManager = source.RenderingManager;
                screenSpaceDrawRect = source.ScreenSpaceDrawQuad.AABB;
            }

            public override void Draw(Action<TexturedVertex2D> vertexAction)
            {
                base.Draw(vertexAction);

                var source = (CubismModelContainer)Source;

                Matrix4 projectionMatrix = Matrix4.Identity;
                projectionMatrix[0, 0] = 2.0f;
                projectionMatrix[1, 1] = 2.0f * source.DrawWidth / source.DrawHeight;
                renderingManager.Draw(projectionMatrix);
            }
        }
    }
}
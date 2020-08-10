using System;
using CubismFramework;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Cubism.Renderer;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Timing;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Graphics.Cubism
{
    public class CubismSprite : Drawable, IBufferedDrawable
    {
        internal CubismRenderer Renderer;
        internal CubismShaderManager ShaderManager;
        public CubismAsset Asset;
        public CubismRenderingManager RenderingManager { get; private set; }
        public CubismMotionQueueEntry LastMotion { get; private set; }
        private CubismModelTransform modelTransform;
        public CubismModelTransform ModelTransform
        {
            get => modelTransform;
            set
            {
                if (modelTransform.Equals(value))
                    return;

                modelTransform = value;

                Invalidate(Invalidation.DrawNode);
            }
        }

        [BackgroundDependencyLoader]
        private void load(ShaderManager shaderManager)
        {
            this.ShaderManager = new CubismShaderManager(shaderManager);

            TextureShader = shaderManager.Load(VertexShaderDescriptor.TEXTURE_2, FragmentShaderDescriptor.TEXTURE);
            RoundedTextureShader = shaderManager.Load(VertexShaderDescriptor.TEXTURE_2, FragmentShaderDescriptor.TEXTURE_ROUNDED);

            var eye_blink_controller = new CubismEyeBlink(Asset.ParameterGroups["EyeBlink"]);
            Asset.StartMotion(CubismAsset.MotionType.Effect, eye_blink_controller);

            Renderer = new CubismRenderer(this.ShaderManager);
            RenderingManager = new CubismRenderingManager(Renderer, Asset);
        }

        protected override void Update()
        {
            base.Update();

            if ((LastMotion == null) || (LastMotion.Finished == true))
            {
                var motion_group = Asset.MotionGroups[""];
                int number = new Random().Next() % motion_group.Length;
                var motion = (CubismMotion)motion_group[number];
                LastMotion = Asset.StartMotion(CubismAsset.MotionType.Base, motion, false);
            }

            Asset.Update(Clock.ElapsedFrameTime / 1000);
            Invalidate(Invalidation.DrawNode);
        }

        public Color4 BackgroundColour => new Color4(0, 0, 0, 0);
        public DrawColourInfo? FrameBufferDrawColour => base.DrawColourInfo;
        public Vector2 FrameBufferScale => Vector2.One;
        public IShader TextureShader { get; private set; }
        public IShader RoundedTextureShader { get; private set; }
        protected override DrawNode CreateDrawNode() =>
            new BufferedDrawNode(this, new CubismSpriteDrawNode(this), new BufferedDrawNodeSharedData());

        protected override void Dispose(bool isDisposing)
        {
            RenderingManager.Dispose();
            Renderer.Dispose();
            Asset.Dispose();

            base.Dispose(isDisposing);
        }
    }
}
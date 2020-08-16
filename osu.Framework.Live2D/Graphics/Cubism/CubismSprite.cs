using System;
using CubismFramework;
using osu.Framework.Allocation;
using osu.Framework.Audio.Sample;
using osu.Framework.Graphics.Cubism.Renderer;
using osu.Framework.Graphics.Shaders;
using osuTK;
using osuTK.Graphics;
using osuTK.Graphics.ES30;

namespace osu.Framework.Graphics.Cubism
{
    public partial class CubismSprite : Drawable, IBufferedDrawable
    {
        public CubismRenderer Renderer = new CubismRenderer();
        public CubismRenderingManager RenderingManager { get; private set; }

        private CubismAsset asset;
        public CubismAsset Asset
        {
            get => asset;
            set
            {
                if (value == asset) return;

                RenderingManager?.Dispose();
                Asset?.Dispose();
                asset = value;

                if (IsLoaded)
                    initialize();
            }
        }

        public bool IsMoving
        {
            get
            {
                if (Voice?.Playing ?? false)
                    return true;

                if (CanBreathe || CanEyeBlink)
                    return true;

                if ((baseMotionQueue?.IsActive ?? false) || (expressionQueue?.IsActive ?? false) || (effectQueue?.IsActive ?? false))
                    return true;

                return false;
            }
        }

        public int BaseMotionsQueued => baseMotionQueue?.Queued ?? 0;
        public int ExpressionsQueued => expressionQueue?.Queued ?? 0;
        public int EffectsQueued => effectQueue?.Queued ?? 0;

        public SampleChannel Voice;
        private CubismMotionQueueEntry eyeBlinkMotion;
        private CubismMotionQueueEntry breatheMotion;
        private CubismMotionQueue baseMotionQueue;
        private CubismMotionQueue expressionQueue;
        private CubismMotionQueue effectQueue;

        private bool canBreathe;
        public bool CanBreathe
        {
            get => canBreathe;
            set
            {
                if (canBreathe == value) return;

                canBreathe = value;
                breatheMotion?.Suspend(!canBreathe);
            }
        }

        private bool canEyeBlink;
        public bool CanEyeBlink
        {
            get => canEyeBlink;
            set
            {
                if (canEyeBlink == value) return;

                canEyeBlink = value;
                eyeBlinkMotion?.Suspend(!canEyeBlink);
            }
        }

        [BackgroundDependencyLoader]
        private void load(ShaderManager shaderManager)
        {
            Renderer.ShaderManager = new CubismShaderManager(shaderManager);

            TextureShader = shaderManager.Load(VertexShaderDescriptor.TEXTURE_2, FragmentShaderDescriptor.TEXTURE);
            RoundedTextureShader = shaderManager.Load(VertexShaderDescriptor.TEXTURE_2, FragmentShaderDescriptor.TEXTURE_ROUNDED);

            if (Asset != null)
                initialize();
        }

        /// <summary>
        /// Starts a motion from a motion group as defined in the model file.
        /// </summary>
        /// <param name="force">Clears the queue and immediately plays the motion.</param>
        /// <param name="fadeOutTime">If force is true, smoothly fade out.</param>
        public bool StartMotion(string group, int index, CubismAsset.MotionType type = CubismAsset.MotionType.Base, bool force = false, bool loop = false, double fadeOutTime = 0)
        {
            if (!Asset.MotionGroups.ContainsKey(group)) return false;

            var motionGroup = Asset.MotionGroups[group];
            if (index < 0 || index > motionGroup.Length) return false;

            var motionQueue = getMotionQueue(type);
            if (force)
                motionQueue.Stop(fadeOutTime);

            motionQueue.Add(motionGroup[index], loop);
            return true;
        }

        /// <summary>
        /// Ends the current motion and plays the next in queue. This will do nothing if there's no other motions in queue.
        /// </summary>
        public void NextMotion(double fadeOutTime = 0, CubismAsset.MotionType type = CubismAsset.MotionType.Base) => getMotionQueue(type).Next(fadeOutTime);

        /// <summary>
        /// Pauses or unpauses the current motion.
        /// </summary>
        public void SuspendMotion(bool suspend = true, CubismAsset.MotionType type = CubismAsset.MotionType.Base) => getMotionQueue(type).Suspend(suspend);

        /// <summary>
        /// Ends the current motion and clears the queue.
        /// </summary>
        public void StopMotion(double fadeOutTime = 0, CubismAsset.MotionType type = CubismAsset.MotionType.Base) => getMotionQueue(type).Stop(fadeOutTime);

        /// <summary>
        /// Modify a model's parameter.
        /// </summary>
        public void SetParameter(string name, double value)
        {
            Asset.Model.RestoreSavedParameters();
            Asset.Model.GetParameter(name).Value = value;
            Asset.Model.SaveParameters();

            // Only invalidate when we're not moving to avoid the calls getting doubled
            if (!IsMoving)
                Invalidate(Invalidation.DrawNode);
        }

        /// <summary>
        /// Resets all model parameters to its defaults.
        /// </summary>
        public void ResetParameters()
        {
            Asset.Model.RestoreDefaultParameters();
            Asset.Model.SaveParameters();

            if (!IsMoving)
                Invalidate(Invalidation.DrawNode);
        }

        private void initialize()
        {
            var eyeBlinkController = new CubismEyeBlink(Asset.ParameterGroups["EyeBlink"]);
            eyeBlinkMotion = Asset.StartMotion(CubismAsset.MotionType.Effect, eyeBlinkController);
            eyeBlinkMotion.Suspend(!canEyeBlink);

            var breathController = new CubismBreath();
            breathController.SetParameter(new CubismBreathParameter(Asset.Model.GetParameter("ParamAngleX"), 0.0, 15.0, 6.5345, 0.5));
            breathController.SetParameter(new CubismBreathParameter(Asset.Model.GetParameter("ParamAngleY"), 0.0, 8.0, 3.5345, 0.5));
            breathController.SetParameter(new CubismBreathParameter(Asset.Model.GetParameter("ParamAngleZ"), 0.0, 10.0, 5.5345, 0.5));
            breathController.SetParameter(new CubismBreathParameter(Asset.Model.GetParameter("ParamBodyAngleX"), 0.0, 4.0, 15.5345, 0.5));
            breathController.SetParameter(new CubismBreathParameter(Asset.Model.GetParameter("ParamBreath"), 0.5, 0.5, 3.2345, 0.5));
            breatheMotion = Asset.StartMotion(CubismAsset.MotionType.Effect, breathController);
            breatheMotion.Suspend(!canBreathe);

            baseMotionQueue = new CubismMotionQueue(Asset, CubismAsset.MotionType.Base);
            expressionQueue = new CubismMotionQueue(Asset, CubismAsset.MotionType.Expression);
            effectQueue = new CubismMotionQueue(Asset, CubismAsset.MotionType.Effect);

            RenderingManager = new CubismRenderingManager(Renderer, Asset);
        }

        private CubismMotionQueue getMotionQueue(CubismAsset.MotionType type)
        {
            switch (type)
            {
                case CubismAsset.MotionType.Base:
                    return baseMotionQueue;
                case CubismAsset.MotionType.Expression:
                    return expressionQueue;
                case CubismAsset.MotionType.Effect:
                    return effectQueue;
                default:
                    throw new ArgumentException();
            }
        }

        protected override void Update()
        {
            base.Update();

            SetParameter("ParamMouthOpenY", Voice?.CurrentAmplitudes.Average ?? 0);

            Renderer.DrawSize = DrawSize;
            Asset.Update(Clock.ElapsedFrameTime / 1000);

            baseMotionQueue.Update();
            expressionQueue.Update();
            effectQueue.Update();

            // We just need to invalidate the draw node when we do want to animate
            if (IsMoving)
                Invalidate(Invalidation.DrawNode);
        }

        public Color4 BackgroundColour => new Color4(0, 0, 0, 0);
        public DrawColourInfo? FrameBufferDrawColour => base.DrawColourInfo;
        public Vector2 FrameBufferScale => Vector2.One;
        public IShader TextureShader { get; private set; }
        public IShader RoundedTextureShader { get; private set; }
        private readonly BufferedDrawNodeSharedData sharedData = new BufferedDrawNodeSharedData(new[] { RenderbufferInternalFormat.DepthComponent16 });
        protected override DrawNode CreateDrawNode() => new BufferedDrawNode(this, new CubismSpriteDrawNode(this), sharedData);
    }
}
using System;
using CubismFramework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Cubism.Renderer;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;

namespace osu.Framework.Graphics.Cubism
{
    public class CubismSprite : Drawable, IBufferedDrawable
    {
        #region Rendering

        internal CubismRenderer Renderer;
        internal CubismShaderManager ShaderManager;
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

        public Colour4 ModelColour
        {
            get => new Colour4(Asset.ModelColor[0], Asset.ModelColor[1], Asset.ModelColor[2], Asset.ModelColor[3]);
            set
            {
                if (Asset.ModelColor[0] == value.R &&
                    Asset.ModelColor[1] == value.G &&
                    Asset.ModelColor[2] == value.B &&
                    Asset.ModelColor[3] == value.A)
                    return;

                Asset.ModelColor = new float[] { value.R, value.G, value.B, value.A };
                Invalidate(Invalidation.DrawNode);
            }
        }

        private Vector2 modelOffset;
        public Vector2 ModelOffset
        {
            get => modelOffset;
            set
            {
                if (modelOffset == value) return;

                modelOffset = value;
                Invalidate(Invalidation.DrawNode);
            }
        }

        public float ModelOffsetX
        {
            get => modelOffset.X;
            set
            {
                if (modelOffset.X == value) return;

                modelOffset.X = value;
                Invalidate(Invalidation.DrawNode);
            }
        }

        public float ModelOffsetY
        {
            get => modelOffset.Y;
            set
            {
                if (modelOffset.Y == value) return;

                modelOffset.Y = value;
                Invalidate(Invalidation.DrawNode);
            }
        }

        private float modelScale;
        public float ModelScale
        {
            get => modelScale;
            set
            {
                if (modelScale == value) return;

                modelScale = value;
                Invalidate(Invalidation.DrawNode);
            }
        }

        #endregion

        # region Motion

        private CubismMotionQueueEntry eyeBlinkMotion;
        private CubismMotionQueueEntry breatheMotion;
        private CubismMotionQueueEntry lastQueuedMotion;

        private bool canBreathe;
        public bool CanBreathe
        {
            get => canBreathe;
            set
            {
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
                canEyeBlink = value;
                eyeBlinkMotion?.Suspend(!canEyeBlink);
            }
        }

        #endregion

        [BackgroundDependencyLoader]
        private void load(ShaderManager shaderManager)
        {
            this.ShaderManager = new CubismShaderManager(shaderManager);

            TextureShader = shaderManager.Load(VertexShaderDescriptor.TEXTURE_2, FragmentShaderDescriptor.TEXTURE);
            RoundedTextureShader = shaderManager.Load(VertexShaderDescriptor.TEXTURE_2, FragmentShaderDescriptor.TEXTURE_ROUNDED);

            if (Asset != null)
                initialize();
        }

        /// <summary>
        /// Starts a motion from a motion group as defined in the model file.
        /// </summary>
        /// <param name="force">Force the last motion to end.</param>
        public void StartMotion(string group, int index, CubismAsset.MotionType motionType = CubismAsset.MotionType.Base, bool force = false, bool loop = false)
        {
            if ((lastQueuedMotion != null) && (!lastQueuedMotion.Terminated))
                StopMotion();

            if (Asset.MotionGroups.TryGetValue(group, out var motionGroup))
            {
                if (index > motionGroup.Length)
                    throw new IndexOutOfRangeException();

                lastQueuedMotion = Asset.StartMotion(motionType, motionGroup[index], false);
            }
            else
                throw new ArgumentException($"Motion Group '{group}' was not found.");
        }

        /// <summary>
        /// Pauses or unpauses the last motion.
        /// </summary>
        public void SuspendMotion(bool suspend = true)
        {
            if (lastQueuedMotion.Terminated) return;

            if (suspend && lastQueuedMotion.Suspended) return;

            lastQueuedMotion.Suspend(suspend);
        }

        /// <summary>
        /// Ends the last motion.
        /// </summary>
        public void StopMotion(double fadeOutTime = 0)
        {
            if (lastQueuedMotion.Terminated) return;

            lastQueuedMotion.Terminate(fadeOutTime);
        }

        public void SetParameter(string key, double val)
        {
            // Checking within bounds is handled internally
            Asset.Model.GetParameter(key).Value = val;
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

            Renderer = new CubismRenderer(ShaderManager);
            RenderingManager = new CubismRenderingManager(Renderer, Asset);
        }

        protected override void Update()
        {
            base.Update();

            Asset.Update(Clock.ElapsedFrameTime / 1000);

            // We don't need to invalidate if the model is just a still
            if (CanEyeBlink || CanBreathe || lastQueuedMotion != null)
                Invalidate(Invalidation.DrawNode);
        }

        public Color4 BackgroundColour => new Color4(0, 0, 0, 0);
        public DrawColourInfo? FrameBufferDrawColour => base.DrawColourInfo;
        public Vector2 FrameBufferScale => Vector2.One;
        public IShader TextureShader { get; private set; }
        public IShader RoundedTextureShader { get; private set; }
        protected override DrawNode CreateDrawNode() =>
            new BufferedDrawNode(this, new CubismSpriteDrawNode(this), new BufferedDrawNodeSharedData());
    }
}
// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System;
using System.Collections.Generic;
using System.Linq;
using CubismFramework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Statistics;
using osuTK;
using osuTK.Graphics;
using osuTK.Graphics.ES30;
using osuTK.Input;

namespace osu.Framework.Graphics.Cubism
{
    /// <summary>
    /// A sprite that displays a Live2D model.
    /// </summary>
    public partial class CubismSprite : Drawable, IBufferedDrawable
    {
        public static readonly string[] PARAMS_EYE = new[] { "ParamEyeLOpen", "ParamEyeROpen" };
        public static readonly string[] PARAMS_LOOK = new[] { "ParamAngleX", "ParamAngleY", "ParamAngleZ", "ParamBodyAngleX" };
        public static readonly string[] PARAMS_BREATH = PARAMS_LOOK.Concat(new[] { "ParamBreath" }).ToArray();
        protected readonly CubismMotionQueue BaseMotionQueue;
        protected readonly CubismMotionQueue ExpressionQueue;
        protected readonly CubismMotionQueue EffectQueue;
        private static readonly GlobalStatistic<int> total_count = GlobalStatistics.Get<int>("Cubism", $"Total {nameof(CubismSprite)}s");
        private readonly BufferedDrawNodeSharedData sharedData = new BufferedDrawNodeSharedData(new[] { RenderbufferInternalFormat.DepthComponent16 });
        private readonly CubismAsset asset;
        private bool breathing;
        private bool blinking;
        private CubismLookType lookType;
        private CubismMotionQueueEntry breatheMotion;
        private CubismMotionQueueEntry eyeBlinkMotion;
        private CubismShaderManager cubismShaders;

        public CubismSprite(CubismAsset asset)
        {
            this.asset = asset;

            if (Breathing)
                initializeBreathing();

            if (Blinking)
                initializeEyeBlink();

            BaseMotionQueue = new CubismMotionQueue(this.asset, MotionType.Base);
            ExpressionQueue = new CubismMotionQueue(this.asset, MotionType.Expression);
            EffectQueue = new CubismMotionQueue(this.asset, MotionType.Effect);

            if (!CanLipSync)
                Logger.Log($"{nameof(CubismAsset)} does not have the required parameters for lip syncing.");

            total_count.Value++;
        }

        /// <summary>
        /// Gets a value indicating whether the model is performing any motions.
        /// </summary>
        public bool IsMoving
        {
            get
            {
                if (Voice?.CurrentAmplitudes.Average > 0)
                    return true;

                if (Breathing || Blinking)
                    return true;

                if ((BaseMotionQueue?.IsActive ?? false) || (ExpressionQueue?.IsActive ?? false) || (EffectQueue?.IsActive ?? false))
                    return true;

                return false;
            }
        }

        /// <summary>
        /// Gets or sets the audio to use for lip syncing.
        /// </summary>
        public IHasAmplitudes Voice { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow the model to perform breathing motions.
        /// </summary>
        public bool Breathing
        {
            get => breathing;
            set
            {
                if (breathing == value)
                    return;

                breathing = value;
                breatheMotion?.Terminate(0);
                ResetParameters(PARAMS_BREATH);

                if (breathing)
                    initializeBreathing();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to allow the model to perform eyeblinking motions.
        /// </summary>
        public bool Blinking
        {
            get => blinking;
            set
            {
                if (blinking == value)
                    return;

                blinking = value;
                eyeBlinkMotion?.Terminate(0);
                ResetParameters(PARAMS_EYE);

                if (blinking)
                    initializeEyeBlink();
            }
        }

        public bool CanLipSync => HasParameter("ParamMouthOpenY");

        /// <summary>
        /// Gets or sets a value indicating whether to allow the model to interact with the cursor. See <see cref="CubismLookType"/> for available ways.
        /// </summary>
        public CubismLookType LookType
        {
            get => lookType;
            set
            {
                if (lookType == value)
                    return;

                if ((value != CubismLookType.None) && PARAMS_LOOK.All(param => !HasParameter(param)))
                {
                    Logger.Log(@"CubismAsset does not have all the required parameters for look motion.");
                    lookType = CubismLookType.None;
                    return;
                }

                lookType = value;
            }
        }

        public Color4 BackgroundColour => new Color4(0, 0, 0, 0);

        public DrawColourInfo? FrameBufferDrawColour => base.DrawColourInfo;

        public Vector2 FrameBufferScale => Vector2.One;

        public IShader TextureShader { get; private set; }

        public IShader RoundedTextureShader { get; private set; }

        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => true;

        /// <summary>
        /// Starts a motion from a motion group as defined in the model file.
        /// </summary>
        /// <param name="group">The name of the motion group.</param>
        /// <param name="index">The index of the motion to play.</param>
        /// <param name="type">The type of motion to treat this as.</param>
        /// <param name="force">Clears the queue and immediately plays the motion.</param>
        /// <param name="loop">Should the motion loop.</param>
        /// <param name="fadeOutTime">If force is true, smoothly fade out.</param>
        /// <returns>If the motion was successfully enqueued.</returns>
        public bool StartMotion(string group, int index, MotionType type = MotionType.Base, bool force = false, bool loop = false, double fadeOutTime = 0)
        {
            if (!asset.MotionGroups.ContainsKey(group))
                return false;

            var motionGroup = asset.MotionGroups[group];
            if ((index < 0) || (index > motionGroup.Length - 1))
                return false;

            var motionQueue = getMotionQueue(type);
            if (force)
                motionQueue.Stop(fadeOutTime);

            motionQueue.Add(motionGroup[index], loop);
            return true;
        }

        /// <summary>
        /// Ends the current motion and plays the next in queue. This will do nothing if there's no other motions in queue.
        /// <param name="fadeOutTime">The time to smoothly adjust the motion to.</param>
        /// <param name="type">The motion queue type.</param>
        /// </summary>
        public void NextMotion(double fadeOutTime = 0, MotionType type = MotionType.Base) => getMotionQueue(type).Next(fadeOutTime);

        /// <summary>
        /// Pauses or unpauses the current motion.
        /// <param name="suspend">If true, the motion will be paused. It will be unpaused otherwise.</param>
        /// <param name="type">The motion queue type.</param>
        /// </summary>
        public void SuspendMotion(bool suspend = true, MotionType type = MotionType.Base) => getMotionQueue(type).Suspend(suspend);

        /// <summary>
        /// Ends the current motion and clears the queue.
        /// <param name="fadeOutTime">The time to smoothly adjust the motion to.</param>
        /// <param name="type">The motion queue type.</param>
        /// </summary>
        public void StopMotion(double fadeOutTime = 0, MotionType type = MotionType.Base) => getMotionQueue(type).Stop(fadeOutTime);

        /// <summary>
        /// Check whether the model has a parameter.
        /// <param name="name">The parameter name.</param>
        /// <returns>If the model has that parameter.</returns>
        /// </summary>
        public bool HasParameter(string name) => asset.Model.GetParameter(name) != null;

        /// <summary>
        /// Get the current value as percentage of a model's parameter.
        /// <param name="name">The parameter name.</param>
        /// <returns>The value of the parameter in percentage. Will throw if the parameter does not exist.</returns>
        /// </summary>
        public double GetParameterValue(string name)
        {
            var parameter = asset.Model.GetParameter(name);
            if (parameter == null)
                throw new ArgumentException($"Model does not have a parameter named '{name}'.");

            return parameter.Value / parameter.Maximum;
        }

        /// <summary>
        /// Modify a model's parameter.
        /// <param name="name">The parameter name.</param>
        /// <param name="value">The value to set to the parameter.</param>
        /// </summary>
        public void SetParameterValue(string name, double value) => modifyParameterValue(name, (current) => value );

        /// <summary>
        /// Add value to a model's parameter.
        /// <param name="name">The parameter name.</param>
        /// <param name="value">The value to add to the parameter.</param>
        /// </summary>
        public void AddParameterValue(string name, double value) => modifyParameterValue(name, (current) => current += value);

        /// <summary>
        /// Reduce value to a model's parameter.
        /// <param name="name">The parameter name.</param>
        /// <param name="value">The value to subtract to the parameter.</param>
        /// </summary>
        public void SubtractParameterValue(string name, double value) => modifyParameterValue(name, (current) => current -= value);

        /// <summary>
        /// Resets all model parameters to its defaults.
        /// </summary>
        public void ResetParameters()
        {
            asset.Model.RestoreDefaultParameters();
            asset.Model.SaveParameters();

            if (!IsMoving)
                Invalidate(Invalidation.DrawNode);
        }

        /// <summary>
        /// Resets a model's parameter to its default.
        /// <param name="parameters">A list of parameters to reset.</param>
        /// </summary>
        public void ResetParameters(IEnumerable<string> parameters)
        {
            asset.Model.RestoreSavedParameters();
            foreach (string name in parameters)
            {
                var param = asset.Model.GetParameter(name);
                if (param == null)
                    continue;

                param.Value = param.Default;
            }

            asset.Model.SaveParameters();
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            asset.Dispose();
            total_count.Value--;
        }

        protected override void Update()
        {
            base.Update();

            if (CanLipSync)
                SetParameterValue("ParamMouthOpenY", Voice?.CurrentAmplitudes.Average ?? 0);

            asset.Update(Clock.ElapsedFrameTime / 1000);

            BaseMotionQueue.Update();
            ExpressionQueue.Update();
            EffectQueue.Update();

            // We just need to invalidate the draw node when we do want to animate
            if (IsMoving)
                Invalidate(Invalidation.DrawNode);
        }

        protected override DrawNode CreateDrawNode() => new BufferedDrawNode(this, new CubismSpriteDrawNode(this), sharedData);

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            if ((LookType == CubismLookType.Drag && e.IsPressed(MouseButton.Left)) || (LookType == CubismLookType.Hover))
                lookAtPoint(e.MousePosition);
            return base.OnMouseMove(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            base.OnHoverLost(e);

            if (LookType != CubismLookType.None)
                ResetParameters(PARAMS_LOOK);
        }

        protected override void OnMouseUp(MouseUpEvent e)
        {
            base.OnMouseUp(e);

            if (LookType == CubismLookType.Drag && !e.IsPressed(MouseButton.Left))
                ResetParameters(PARAMS_LOOK);
        }

        [BackgroundDependencyLoader]
        private void load(ShaderManager shaders)
        {
            cubismShaders = new CubismShaderManager(shaders);
            TextureShader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, FragmentShaderDescriptor.TEXTURE);
            RoundedTextureShader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, FragmentShaderDescriptor.TEXTURE_ROUNDED);
        }

        private void initializeBreathing()
        {
            if (PARAMS_BREATH.All(param => !HasParameter(param)))
            {
                Logger.Log($"{nameof(CubismAsset)} does not have the required parameters for breathing.");
                Breathing = false;
                return;
            }

            var breathController = new CubismBreath();
            breathController.SetParameter(new CubismBreathParameter(asset.Model.GetParameter("ParamAngleX"), 0.0, 15.0, 6.5345, 0.5));
            breathController.SetParameter(new CubismBreathParameter(asset.Model.GetParameter("ParamAngleY"), 0.0, 8.0, 3.5345, 0.5));
            breathController.SetParameter(new CubismBreathParameter(asset.Model.GetParameter("ParamAngleZ"), 0.0, 10.0, 5.5345, 0.5));
            breathController.SetParameter(new CubismBreathParameter(asset.Model.GetParameter("ParamBodyAngleX"), 0.0, 4.0, 15.5345, 0.5));
            breathController.SetParameter(new CubismBreathParameter(asset.Model.GetParameter("ParamBreath"), 0.5, 0.5, 3.2345, 0.5));
            breatheMotion = asset.StartMotion(MotionType.Effect, breathController);
        }

        private void initializeEyeBlink()
        {
            if (PARAMS_EYE.All(param => !HasParameter(param)))
            {
                Logger.Log($"{nameof(CubismAsset)} does not have all the required parameters for eyeblinking.");
                Blinking = false;
                return;
            }

            var eyeBlinkController = new CubismEyeBlink(asset.ParameterGroups["EyeBlink"]);
            eyeBlinkMotion = asset.StartMotion(MotionType.Effect, eyeBlinkController);
        }

        private CubismMotionQueue getMotionQueue(MotionType type)
        {
            return type switch
            {
                MotionType.Base => BaseMotionQueue,
                MotionType.Expression => ExpressionQueue,
                MotionType.Effect => EffectQueue,
                _ => throw new ArgumentException($"{nameof(type)} is not a valid MotionType."),
            };
        }

        private void modifyParameterValue(string name, Func<double, double> mod)
        {
            var parameter = asset.Model.GetParameter(name);
            if (parameter == null)
                throw new ArgumentException($"Model does not have a parameter named '{name}'.");

            asset.Model.RestoreSavedParameters();
            parameter.Value = parameter.Maximum * mod.Invoke(parameter.Value / parameter.Maximum);
            asset.Model.SaveParameters();

            // Only invalidate when we're not moving to avoid the calls getting doubled
            if (!IsMoving)
                Invalidate(Invalidation.DrawNode);
        }

        private void lookAtPoint(Vector2 point)
        {
            double dragX = ((point.X - (DrawWidth / 2)) / DrawWidth) * 2;
            double dragY = ((point.Y - (DrawHeight / 2)) / DrawHeight) * 2;
            AddParameterValue("ParamAngleX", dragX * 30);
            AddParameterValue("ParamAngleY", dragY * -30);
            AddParameterValue("ParamAngleZ", dragX * dragY * -3);
            AddParameterValue("ParamBodyAngleX", dragX * 10);

            if (!IsMoving)
                Invalidate(Invalidation.DrawNode);
        }
    }
}

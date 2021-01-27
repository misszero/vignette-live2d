// Copyright (c) Vignette Project.
// Licensed under MIT. Please see LICENSE for more details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Textures;
using osuTK;
using osuTK.Graphics;
using osuTK.Graphics.ES30;
using Vignette.Application.Live2D.Model;

namespace Vignette.Application.Live2D.Graphics
{
    public class CubismSprite : Drawable, IBufferedDrawable
    {
        public bool UsePremultipliedAlpha { get; set; }

        public float ScaleAdjust { get; set; } = 1.0f;

        public float PositionXAdjust
        {
            get => positionAdjust.X;
            set => positionAdjust.X = value;
        }

        public float PositionYAdjust
        {
            get => positionAdjust.Y;
            set => positionAdjust.Y = value;
        }

        public Vector2 PositionAdjust
        {
            get => positionAdjust;
            set => positionAdjust = value;
        }

        private Vector2 positionAdjust;

        private readonly CubismModel model;

        private readonly List<Texture> textures;

        private readonly bool disposeModel;

        public CubismSprite(CubismModel model, IEnumerable<Texture> textures, bool disposeModel = false)
        {
            this.model = model;
            this.textures = textures.ToList();
            this.disposeModel = disposeModel;
        }

        [BackgroundDependencyLoader]
        private void load(ShaderManager shaders)
        {
            Renderer = new CubismRendererOpenGL(shaders, model, textures);
            TextureShader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, FragmentShaderDescriptor.TEXTURE);
            RoundedTextureShader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, FragmentShaderDescriptor.TEXTURE_ROUNDED);
        }

        protected override void Update()
        {
            base.Update();
            model.Update();

            Invalidate(Invalidation.DrawNode);
        }

        #region Draw Node

        public CubismRenderer Renderer { get; private set; }

        public Color4 BackgroundColour => Color4.Transparent;

        public DrawColourInfo? FrameBufferDrawColour => base.DrawColourInfo;

        public Vector2 FrameBufferScale => Vector2.One;

        public IShader TextureShader { get; private set; }

        public IShader RoundedTextureShader { get; private set; }

        private readonly BufferedDrawNodeSharedData sharedData = new BufferedDrawNodeSharedData(new[] { RenderbufferInternalFormat.DepthComponent16 });

        protected override DrawNode CreateDrawNode() => new BufferedDrawNode(this, new CubismSpriteDrawNode(this), sharedData);

        #endregion

        #region Disposal

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            if (disposeModel)
                model.Dispose();
        }

        #endregion
    }
}

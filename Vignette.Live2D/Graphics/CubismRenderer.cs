// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shaders;
using osuTK;
using osuTK.Graphics;
using osuTK.Graphics.ES30;

namespace Vignette.Live2D.Graphics
{
    internal partial class CubismRenderer : Drawable, IBufferedDrawable
    {
        private readonly CubismModel model;
        private CubismShaders shaders;

        public CubismRenderer(CubismModel model)
        {
            this.model = model;
        }

        public void Redraw() => Invalidate(Invalidation.DrawNode);

        [BackgroundDependencyLoader]
        private void load(ShaderManager shaders)
        {
            RelativeSizeAxes = Axes.Both;

            RoundedTextureShader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, FragmentShaderDescriptor.TEXTURE_ROUNDED);
            TextureShader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, FragmentShaderDescriptor.TEXTURE);

            this.shaders = new CubismShaders(shaders);
        }

        private readonly BufferedDrawNodeSharedData sharedData = new BufferedDrawNodeSharedData(new[] { RenderbufferInternalFormat.DepthComponent16 });
        public Color4 BackgroundColour => new Colour4(0, 0, 0, 0);
        public DrawColourInfo? FrameBufferDrawColour => new DrawColourInfo(Colour4.White, base.DrawColourInfo.Blending);
        public Vector2 FrameBufferScale { get; } = Vector2.One;
        public IShader TextureShader { get; private set; }
        public IShader RoundedTextureShader { get; private set; }
        protected override DrawNode CreateDrawNode() => new BufferedDrawNode(this, new CubismRendererDrawNode(this), sharedData);
    }
}

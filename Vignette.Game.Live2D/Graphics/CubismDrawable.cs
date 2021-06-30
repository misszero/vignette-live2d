// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Caching;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Textures;
using osuTK;
using Vignette.Game.Live2D.Graphics.Shaders;

namespace Vignette.Game.Live2D.Graphics
{
    /// <summary>
    /// A drawable that draws a single Live2D drawable.
    /// </summary>
    public class CubismDrawable : Drawable, ITexturedShaderDrawable, ICubismId
    {
        public CubismShaders Shaders { get; internal set; }

        public Action<CubismDrawable, int> RenderOrderChanged;

        public CubismDrawable()
        {
            RelativeSizeAxes = Axes.Both;
        }

        [BackgroundDependencyLoader]
        private void load(ShaderManager shaderManager)
        {
            RoundedTextureShader = shaderManager.Load(VertexShaderDescriptor.TEXTURE_2, FragmentShaderDescriptor.TEXTURE_ROUNDED);
            TextureShader = shaderManager.Load(VertexShaderDescriptor.TEXTURE_2, FragmentShaderDescriptor.TEXTURE);
            Shaders = new CubismShaders(shaderManager);
        }

        #region Properties

        private readonly List<Vector2> vertexPositions = new List<Vector2>();

        public IEnumerable<Vector2> VertexPositions
        {
            get => vertexPositions;
            internal set
            {
                vertexPositions.Clear();
                vertexPositions.AddRange(value);

                vertexBoundsCache.Invalidate();

                Invalidate(Invalidation.DrawNode);
            }
        }

        private readonly List<Vector2> texturePositions = new List<Vector2>();

        public IEnumerable<Vector2> TexturePositions
        {
            get => texturePositions;
            internal set
            {
                if (texturePositions.Count > 0)
                    throw new InvalidOperationException($"{nameof(TexturePositions)} cannot be modified after being initialized.");

                texturePositions.AddRange(value);
            }
        }

        private readonly List<short> indices = new List<short>();

        public IEnumerable<short> Indices
        {
            get => indices;
            internal set
            {
                if (indices.Count > 0)
                    throw new InvalidOperationException($"{nameof(Indices)} cannot be modified after being initialized.");

                indices.AddRange(value);
            }
        }

        private readonly List<int> masks = new List<int>();

        public IEnumerable<int> Masks
        {
            get => masks;
            internal set
            {
                if (masks.Count > 0)
                    throw new InvalidOperationException($"{nameof(Masks)} cannot be modified after being initialized.");

                masks.AddRange(value);
            }
        }

        private int renderOrder;

        public int RenderOrder
        {
            get => renderOrder;
            internal set
            {
                if (renderOrder == value)
                    return;

                renderOrder = value;
                RenderOrderChanged?.Invoke(this, renderOrder);
            }
        }

        private Texture texture;

        public Texture Texture
        {
            get => texture;
            internal set
            {
                if (texture != null)
                    throw new InvalidOperationException($"{nameof(Texture)} cannot be modified after being initialized.");

                texture = value;
            }
        }

        private readonly Cached<RectangleF> vertexBoundsCache = new Cached<RectangleF>();

        public RectangleF VertexBounds
        {
            get
            {
                if (vertexBoundsCache.IsValid)
                    return vertexBoundsCache.Value;

                if (vertexPositions.Count > 0)
                {
                    float minX = 0;
                    float minY = 0;
                    float maxX = 0;
                    float maxY = 0;

                    foreach (var v in vertexPositions)
                    {
                        minX = Math.Min(minX, v.X);
                        minY = Math.Min(minY, v.Y);
                        maxX = Math.Max(maxX, v.X);
                        maxY = Math.Max(maxY, v.Y);
                    }

                    return vertexBoundsCache.Value = new RectangleF(minX, minY, maxX - minX, maxY - minY);
                }

                return vertexBoundsCache.Value = RectangleF.Empty;
            }
        }

        public bool IsDoubleSided { get; set; }

        public bool IsInvertedMask { get; set; }

        #endregion

        #region ICubismId

        string ICubismId.Name => Name;

        public int ID { get; set; }

        #endregion

        #region Rendering

        public IShader TextureShader { get; private set; }

        public IShader RoundedTextureShader { get; private set; }

        protected override DrawNode CreateDrawNode() => new CubismDrawableDrawNode(this);

        #endregion

        protected override void Dispose(bool isDisposing)
        {
            RenderOrderChanged = null;
            base.Dispose(isDisposing);
        }
    }
}

// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics;
using osu.Framework.Graphics.OpenGL;
using osu.Framework.Graphics.OpenGL.Vertices;
using osu.Framework.Graphics.Textures;
using osuTK;
using Vignette.Game.Live2D.Graphics.OpenGL;
using Vignette.Game.Live2D.Graphics.Shaders;

namespace Vignette.Game.Live2D.Graphics
{
    public class CubismDrawableDrawNode : TexturedShaderDrawNode
    {
        protected new CubismDrawable Source => (CubismDrawable)base.Source;

        private CubismShaders shaders;
        private Texture texture;
        private Vector2[]positions;
        private Vector2[] texturePositions;

        private readonly MeshVertexBatch<TexturedMeshVertex2D> vertexBatch;

        public CubismDrawableDrawNode(CubismDrawable source)
            : base(source)
        {
            vertexBatch = new MeshVertexBatch<TexturedMeshVertex2D>(200, 10, source.Indices.ToArray());
        }

        public override void ApplyState()
        {
            base.ApplyState();

            texture = Source.Texture;
            shaders = Source.Shaders;
            positions = Source.VertexPositions.ToArray();
            texturePositions = Source.TexturePositions.ToArray();
        }

        public override void Draw(Action<TexturedVertex2D> vertexAction)
        {
            base.Draw(vertexAction);

            var shader = shaders.GetShaderFor(false, false, false);

            shader.Bind();

            // texture?.TextureGL.Bind();
            Texture.WhitePixel.TextureGL.Bind();

            GLWrapper.SetBlend(DrawColourInfo.Blending);

            shader.GetUniform<int>("s_texture0").Value = 0;
            shader.GetUniform<Matrix4>("u_matrix").Value = Matrix4.Identity;

            Colour4 colour = DrawColourInfo.Colour;
            shader.GetUniform<Vector4>("u_baseColor").Value = new Vector4(colour.R, colour.G, colour.B, colour.A);

            for (int i = 0; i < positions.Count(); i++)
            {
                vertexBatch.Add(new TexturedMeshVertex2D
                {
                    Position = positions[i],
                    TexturePosition = texturePositions[i],
                });
            }

            GLWrapper.SetBlend(BlendingParameters.None);

            shader.Unbind();
        }
    }
}

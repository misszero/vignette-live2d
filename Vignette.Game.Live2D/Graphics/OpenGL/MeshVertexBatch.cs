// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using System;
using osu.Framework.Graphics.Batches;
using osu.Framework.Graphics.OpenGL.Buffers;
using osu.Framework.Graphics.OpenGL.Vertices;
using osuTK.Graphics.ES30;

namespace Vignette.Game.Live2D.Graphics.OpenGL
{
    public class MeshVertexBatch<T> : VertexBatch<T>
        where T : struct, IEquatable<T>, IVertex
    {
        private readonly short[] indices;

        public MeshVertexBatch(int bufferSize, int maxBuffers, short[] indices)
            : base(bufferSize, maxBuffers)
        {
            this.indices = indices;
        }

        protected override VertexBuffer<T> CreateVertexBuffer() => new MeshVertexBuffer<T>(Size, indices, BufferUsageHint.DynamicDraw);
    }
}

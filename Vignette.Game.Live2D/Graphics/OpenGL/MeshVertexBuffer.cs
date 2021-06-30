// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using System;
using osu.Framework.Graphics.OpenGL;
using osu.Framework.Graphics.OpenGL.Buffers;
using osu.Framework.Graphics.OpenGL.Vertices;
using osuTK.Graphics.ES30;

namespace Vignette.Game.Live2D.Graphics.OpenGL
{
    public class MeshVertexBuffer<T> : VertexBuffer<T>
        where T : struct, IEquatable<T>, IVertex
    {
        private readonly short[] indices;

        private readonly int eboId;

        private readonly int amountVertices;

        protected override PrimitiveType Type => PrimitiveType.Triangles;

        public MeshVertexBuffer(int amountVertices, short[] indices, BufferUsageHint usage)
            : base(amountVertices, usage)
        {
            this.indices = indices;
            this.amountVertices = amountVertices;

            GL.GenBuffers(1, out eboId);
        }

        protected override void Initialise()
        {
            base.Initialise();

            GLWrapper.BindBuffer(BufferTarget.ElementArrayBuffer, eboId);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(amountVertices * sizeof(short)), indices, BufferUsageHint.StaticDraw);
        }

        public override void Bind(bool forRendering)
        {
            base.Bind(forRendering);

            if (forRendering)
                GLWrapper.BindBuffer(BufferTarget.ElementArrayBuffer, eboId);
        }

        protected override void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;

            GL.DeleteBuffers(1, new[] { eboId });

            base.Dispose(disposing);
        }
    }
}

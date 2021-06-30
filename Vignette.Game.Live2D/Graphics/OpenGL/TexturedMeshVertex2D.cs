// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using osu.Framework.Graphics.OpenGL.Vertices;
using osuTK;
using osuTK.Graphics.ES30;
using System;
using System.Runtime.InteropServices;

namespace Vignette.Game.Live2D.Graphics.OpenGL
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TexturedMeshVertex2D : IVertex, IEquatable<TexturedMeshVertex2D>
    {
        [VertexMember(2, VertexAttribPointerType.Float)]
        public Vector2 Position;

        [VertexMember(2, VertexAttribPointerType.Float)]
        public Vector2 TexturePosition;

        public bool Equals(TexturedMeshVertex2D other) => Position.Equals(other.Position) && TexturePosition.Equals(other.TexturePosition);
    }
}

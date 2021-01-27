// Copyright (c) Vignette Project.
// Licensed under MIT. Please see LICENSE for more details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using System;
using System.Runtime.InteropServices;
using Vignette.Application.Live2D.Model;
using Vignette.Application.Live2D.Utils;

namespace Vignette.Application.Live2D
{
    internal static class CubismCore
    {
        public delegate void CubismLogFunction(string message);

        public const int ALIGN_OF_MODEL = 16;

        public const int ALIGN_OF_MOC = 64;

        public static CubismVersion Version => new CubismVersion(csmGetVersion());

        public static CubismMocVersion LatestMocVersion => (CubismMocVersion)csmGetLatestMocVersion();

        private const string library = "Live2DCubismCore";

        [DllImport(library, ExactSpelling = true)]
        private static extern uint csmGetVersion();

        [DllImport(library, ExactSpelling = true)]
        private static extern uint csmGetLatestMocVersion();

#pragma warning disable IDE1006

        [DllImport(library, ExactSpelling = true)]
        public static extern CubismMocVersion csmGetMocVersion(IntPtr address);

        [DllImport(library, ExactSpelling = true)]
        public static extern CubismLogFunction csmGetLogFunction();

        [DllImport(library, ExactSpelling = true)]
        public static extern void csmSetLogFunction(CubismLogFunction function);

        [DllImport(library, ExactSpelling = true)]
        public static extern IntPtr csmReviveMocInPlace(IntPtr address, int size);

        [DllImport(library, ExactSpelling = true)]
        public static extern int csmGetSizeofModel(IntPtr moc);

        [DllImport(library, ExactSpelling = true)]
        public static extern IntPtr csmInitializeModelInPlace(IntPtr moc, IntPtr address, int size);

        [DllImport(library, ExactSpelling = true)]
        public static extern void csmUpdateModel(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        public static extern void csmReadCanvasInfo(
            IntPtr model,
            [In, Out] float[] outSizeInPixels,
            [In, Out] float[] outOriginInPixels,
            out float outPixelsPerUnit);

        [DllImport(library, ExactSpelling = true)]
        public static extern int csmGetParameterCount(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        public static extern IntPtr csmGetParameterIds(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        public static extern IntPtr csmGetParameterMinimumValues(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        public static extern IntPtr csmGetParameterMaximumValues(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        public static extern IntPtr csmGetParameterDefaultValues(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        public static extern IntPtr csmGetParameterValues(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        public static extern int csmGetPartCount(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        public static extern IntPtr csmGetPartIds(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        public static extern IntPtr csmGetPartOpacities(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        public static extern IntPtr csmGetPartParentPartIndices(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        public static extern int csmGetDrawableCount(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        public static extern IntPtr csmGetDrawableIds(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        public static extern IntPtr csmGetDrawableConstantFlags(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        public static extern IntPtr csmGetDrawableDynamicFlags(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        public static extern IntPtr csmGetDrawableTextureIndices(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        public static extern IntPtr csmGetDrawableDrawOrders(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        public static extern IntPtr csmGetDrawableRenderOrders(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        public static extern IntPtr csmGetDrawableOpacities(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        public static extern IntPtr csmGetDrawableMaskCounts(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        public static extern IntPtr csmGetDrawableMasks(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        public static extern IntPtr csmGetDrawableVertexCounts(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        public static extern IntPtr csmGetDrawableVertexPositions(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        public static extern IntPtr csmGetDrawableVertexUvs(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        public static extern IntPtr csmGetDrawableIndexCounts(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        public static extern IntPtr csmGetDrawableIndices(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        public static extern void csmResetDrawableDynamicFlags(IntPtr model);

#pragma warning restore IDE1006
    }
}

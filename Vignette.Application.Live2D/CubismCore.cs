// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System;
using System.Runtime.InteropServices;
using Vignette.Application.Live2D.Model;

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
        public static extern uint csmGetMocVersion(IntPtr address);

        [DllImport(library, ExactSpelling = true)]
        public static extern CubismLogFunction csmGetLogFunction();

        [DllImport(library, ExactSpelling = true)]
        public static extern void csmSetLogFunction(CubismLogFunction function);

        [DllImport(library, ExactSpelling = true)]
        public static extern IntPtr csmReviveMocInPlace(IntPtr address, uint size);

        [DllImport(library, ExactSpelling = true)]
        public static extern uint csmGetSizeofModel(IntPtr moc);

        [DllImport(library, ExactSpelling = true)]
        public static extern IntPtr csmInitializeModelInPlace(IntPtr moc, IntPtr address, uint size);

        [DllImport(library, ExactSpelling = true)]
        public static extern void csmUpdateModel(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        public static extern void csmReadCanvasInfo(
            IntPtr model,
            [In, Out] CubismVector2 outSizeInPixels,
            [In, Out] CubismVector2 outOriginInPixels,
            out float outPixelsPerUnit);

        [DllImport(library, ExactSpelling = true)]
        public static extern int csmGetParameterCount(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)]
        public static extern string[] csmGetParameterIds(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.LPArray)]
        public static extern float[] csmGetParameterMinimumValues(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.LPArray)]
        public static extern float[] csmGetParameterMaximumValues(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.LPArray)]
        public static extern float[] csmGetParameterDefaultValues(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        public static extern IntPtr csmGetParameterValues(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        public static extern int csmGetPartCount(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)]
        public static extern string[] csmGetPartIds(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        public static extern IntPtr csmGetPartOpacities(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.LPArray)]
        public static extern int[] csmGetPartParentPartIndices(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        public static extern int csmGetDrawableCount(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)]
        public static extern string[] csmGetDrawableIds(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.LPArray)]
        public static extern ConstantDrawableFlags[] csmGetDrawableConstantFlags(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.LPArray)]
        public static extern DynamicDrawableFlags[] csmGetDrawableDynamicFlags(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.LPArray)]
        public static extern int[] csmGetDrawableTextureIndices(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.LPArray)]
        public static extern int[] csmGetDrawableDrawOrders(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.LPArray)]
        public static extern int[] csmGetDrawableRenderOrders(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.LPArray)]
        public static extern float[] csmGetDrawableOpacities(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.LPArray)]
        public static extern int[] csmGetDrawableMaskCounts(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPArray)]
        public static extern int[][] csmGetDrawableMasks(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.LPArray)]
        public static extern int[] csmGetDrawableVertexCounts(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPArray)]
        public static extern CubismVector2[][] csmGetDrawableVertexPositions(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPArray)]
        public static extern CubismVector2[][] csmGetDrawableVertexUvs(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.LPArray)]
        public static extern int[] csmGetDrawableIndexCounts(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.LPArray)]
        public static extern short[][] csmGetDrawableIndices(IntPtr model);

        [DllImport(library, ExactSpelling = true)]
        public static extern void csmResetDrawableDynamicFlags(IntPtr model);

#pragma warning restore IDE1006
    }
}

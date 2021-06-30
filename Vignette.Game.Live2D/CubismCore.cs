// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using osuTK;
using System;
using System.Runtime.InteropServices;
using Vignette.Game.Live2D.Model;

namespace Vignette.Game.Live2D
{
    public unsafe static class CubismCore
    {
        internal const int ALIGN_OF_MODEL = 16;

        internal const int ALIGN_OF_MOC = 64;

        /// <summary>
        /// Invoked when the Live2D Cubism Core invokes a log message.
        /// </summary>
        /// <param name="message"></param>
        public delegate void CubismLogFunction(string message);

        /// <summary>
        /// Returns the version of the Live2D Cubism Core.
        /// </summary>
        public static CubismVersion Version => new CubismVersion(csmGetVersion());

        /// <summary>
        /// Returns the latest moc version the Live2D Cubism Core can handle.
        /// </summary>
        public static CubismMocVersion LatestMocVersion => (CubismMocVersion)csmGetLatestMocVersion();

        /// <summary>
        /// Sets the logging function for the Live2D Cubism Core.
        /// </summary>
        public static void SetLogFunction(CubismLogFunction action) => csmSetLogFunction(action);

        /// <summary>
        /// Gets the logging function of the Live2D Cubism Core.
        /// </summary>
        /// <returns>The logging function.</returns>
        public static CubismLogFunction GetLogFunction() => csmGetLogFunction();

        private const string library = @"Live2DCubismCore";

#pragma warning disable IDE1006

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern uint csmGetVersion();

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern uint csmGetLatestMocVersion();

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern CubismMocVersion csmGetMocVersion(IntPtr address);

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern CubismLogFunction csmGetLogFunction();

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void csmSetLogFunction(CubismLogFunction function);

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr csmReviveMocInPlace(IntPtr address, int size);

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int csmGetSizeofModel(IntPtr moc);

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr csmInitializeModelInPlace(IntPtr moc, IntPtr address, int size);

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void csmUpdateModel(IntPtr model);

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void csmReadCanvasInfo(IntPtr model, ref Vector2 outSizeInPixels, ref Vector2 outOriginInPixels, out float outPixelsPerUnit);

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int csmGetParameterCount(IntPtr model);

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern char** csmGetParameterIds(IntPtr model);

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float* csmGetParameterMinimumValues(IntPtr model);

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float* csmGetParameterMaximumValues(IntPtr model);

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float* csmGetParameterDefaultValues(IntPtr model);

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float* csmGetParameterValues(IntPtr model);

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int csmGetPartCount(IntPtr model);

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern char** csmGetPartIds(IntPtr model);

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float* csmGetPartOpacities(IntPtr model);

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int* csmGetPartParentPartIndices(IntPtr model);

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int csmGetDrawableCount(IntPtr model);

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern char** csmGetDrawableIds(IntPtr model);

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern CubismConstantFlags* csmGetDrawableConstantFlags(IntPtr model);

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern CubismDynamicFlags* csmGetDrawableDynamicFlags(IntPtr model);

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int* csmGetDrawableTextureIndices(IntPtr model);

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int* csmGetDrawableDrawOrders(IntPtr model);

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int* csmGetDrawableRenderOrders(IntPtr model);

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float* csmGetDrawableOpacities(IntPtr model);

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int* csmGetDrawableMaskCounts(IntPtr model);

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int** csmGetDrawableMasks(IntPtr model);

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int* csmGetDrawableVertexCounts(IntPtr model);

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern Vector2** csmGetDrawableVertexPositions(IntPtr model);

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern Vector2** csmGetDrawableVertexUvs(IntPtr model);

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int* csmGetDrawableIndexCounts(IntPtr model);

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern short** csmGetDrawableIndices(IntPtr model);

        [DllImport(library, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void csmResetDrawableDynamicFlags(IntPtr model);

#pragma warning restore
    }
}

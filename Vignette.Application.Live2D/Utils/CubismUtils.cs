// Copyright (c) Vignette Project.
// Licensed under MIT. Please see LICENSE for more details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using System;
using System.Runtime.InteropServices;

namespace Vignette.Application.Live2D.Utils
{
    public static class CubismUtils
    {
        public static IntPtr Align(IntPtr ptr, int offset)
        {
            IntPtr aligned;
            int totalOffset = 0;

            do
            {
                aligned = IntPtr.Add(ptr, totalOffset);
                totalOffset++;
            } while ((ulong)aligned % (ulong)offset != 0);

            return aligned;
        }

        public static string[] PointerToStringArray(IntPtr ptr, int count)
        {
            string[] ret = new string[count];

            for (int i = 0; i < count; i++)
            {
                var element = Marshal.PtrToStructure<IntPtr>(ptr);
                ptr = IntPtr.Add(ptr, IntPtr.Size);
                ret[i] = Marshal.PtrToStringAnsi(element);
            }

            return ret;
        }

        public static float[] PointerToFloatArray(IntPtr ptr, int count)
        {
            float[] ret = new float[count];
            Marshal.Copy(ptr, ret, 0, count);
            return ret;
        }

        public static short[] PointerToShortArray(IntPtr ptr, int count)
        {
            short[] ret = new short[count];
            Marshal.Copy(ptr, ret, 0, count);
            return ret;
        }

        public static int[] PointerToIntArray(IntPtr ptr, int count)
        {
            int[] ret = new int[count];
            Marshal.Copy(ptr, ret, 0, count);
            return ret;
        }

        public static byte[] PointerToByteArray(IntPtr ptr, int count)
        {
            byte[] ret = new byte[count];
            Marshal.Copy(ptr, ret, 0, count);
            return ret;
        }

        public static IntPtr[] PointerToPointerArray(IntPtr ptr, int count)
        {
            IntPtr[] ret = new IntPtr[count];
            Marshal.Copy(ptr, ret, 0, count);
            return ret;
        }
    }
}

// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System;
using Vignette.Application.Live2D.Id;
using Vignette.Application.Live2D.Utils;

namespace Vignette.Application.Live2D.Model
{
    public class CubismDrawableManager : CubismIdManager<CubismDrawable>
    {
        private int[] indexCounts;

        private int[] vertexCounts;

        private IntPtr[] indexPointers;

        private IntPtr[] texCoordPointers;

        public CubismDrawableManager(IntPtr model)
            : base(model)
        {
            int count = CubismCore.csmGetDrawableCount(model);

            vertexCounts = CubismUtils.PointerToIntArray(CubismCore.csmGetDrawableVertexCounts(model), count);
            texCoordPointers = CubismUtils.PointerToPointerArray(CubismCore.csmGetDrawableVertexUvs(model), count);

            indexCounts = CubismUtils.PointerToIntArray(CubismCore.csmGetDrawableIndexCounts(model), count);
            indexPointers = CubismUtils.PointerToPointerArray(CubismCore.csmGetDrawableIndices(model), count);

            int[] textureIds = CubismUtils.PointerToIntArray(CubismCore.csmGetDrawableTextureIndices(model), count);
            byte[] constFlags = CubismUtils.PointerToByteArray(CubismCore.csmGetDrawableConstantFlags(model), count);
            string[] names = CubismUtils.PointerToStringArray(CubismCore.csmGetDrawableIds(model), count);

            int[] maskCounts = CubismUtils.PointerToIntArray(CubismCore.csmGetDrawableMaskCounts(model), count);
            IntPtr[] maskPointers = CubismUtils.PointerToPointerArray(CubismCore.csmGetDrawableMasks(model), count);

            for (int i = 0; i < count; i++)
            {
                float[] texCoords = CubismUtils.PointerToFloatArray(texCoordPointers[i], vertexCounts[i] * 2);
                short[] indices = CubismUtils.PointerToShortArray(indexPointers[i], indexCounts[i]);
                int[] masks = CubismUtils.PointerToIntArray(maskPointers[i], maskCounts[i]);
                Add(new CubismDrawable(i, names[i], textureIds[i], masks, texCoords, indices, (ConstantDrawableFlags)constFlags[i]));
            }
        }

        public override void PostModelUpdate()
        {
            int[] renderOrder = CubismUtils.PointerToIntArray(CubismCore.csmGetDrawableRenderOrders(Model), Count);
            byte[] dynamicFlags = CubismUtils.PointerToByteArray(CubismCore.csmGetDrawableDynamicFlags(Model), Count);
            float[] opacities = CubismUtils.PointerToFloatArray(CubismCore.csmGetDrawableOpacities(Model), Count);
            IntPtr[] vertexPointers = CubismUtils.PointerToPointerArray(CubismCore.csmGetDrawableVertexPositions(Model), Count);

            foreach (var drawable in this)
            {
                int id = drawable.Id;
                float[] vertices = CubismUtils.PointerToFloatArray(vertexPointers[id], vertexCounts[id] * 2);
                drawable.Update(opacities[id], renderOrder[id], vertices, (DynamicDrawableFlags)dynamicFlags[id]);
            }
        }
    }
}

// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System;
using Vignette.Application.Live2D.Id;

namespace Vignette.Application.Live2D.Model
{
    public class CubismDrawableManager : CubismIdManager<CubismDrawable>
    {
        public CubismDrawableManager(IntPtr model)
            : base(model)
        {
            int count = CubismCore.csmGetDrawableCount(model);
            int[] textureIds = CubismCore.csmGetDrawableTextureIndices(model);
            int[][] masks = CubismCore.csmGetDrawableMasks(model);
            string[] names = CubismCore.csmGetDrawableIds(model);

            ConstantDrawableFlags[] constFlags = CubismCore.csmGetDrawableConstantFlags(model);

            for (int i = 0; i < count; i++)
                Add(new CubismDrawable(i, names[i], textureIds[i], masks[i], constFlags[i]));
        }

        public override void PostModelUpdate()
        {
            foreach (var drawable in this)
            {
                var vertices = CubismCore.csmGetDrawableVertexPositions(Model);
                var texCoords = CubismCore.csmGetDrawableVertexUvs(Model);
                short[][] indices = CubismCore.csmGetDrawableIndices(Model);
                float[] opacities = CubismCore.csmGetDrawableOpacities(Model);
                DynamicDrawableFlags[] dynamicFlags = CubismCore.csmGetDrawableDynamicFlags(Model);

                int id = drawable.Index;
                drawable.Update(opacities[id], vertices[id], texCoords[id], indices[id], dynamicFlags[id]);
            }
        }
    }
}

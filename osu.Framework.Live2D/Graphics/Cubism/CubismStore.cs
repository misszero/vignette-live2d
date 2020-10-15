// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System.IO;
using CubismFramework;
using osu.Framework.IO.Stores;

namespace osu.Framework.Graphics.Cubism
{
    public abstract class CubismStore : ResourceStore<byte[]>
    {
        public CubismStore(IResourceStore<byte[]> store = null)
            : base(store)
        {
        }

        /// <summary>
        /// Loads a <see cref="CubismAsset"/> used for <see cref="CubismSprite"/>.
        /// </summary>
        /// <param name="name">The path to the model json file.</param>
        /// <returns>The loaded CubismAsset.</returns>
        public new CubismAsset Get(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            return new CubismAsset(name, GetResource);
        }

        protected abstract Stream GetResource(string path);
    }
}

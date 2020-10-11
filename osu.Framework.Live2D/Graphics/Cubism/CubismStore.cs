// Copyright (c) Nitrous <n20gaming2000@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

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

        protected abstract Stream GetResource(string path);

        /// <summary>
        /// Loads a <see cref="CubismAsset"/> used for <see cref="CubismSprite"/>.
        /// </summary>
        /// <param name="name">The path to the model json file.</param>
        /// <returns>The loaded CubismAsset</returns>
        public new CubismAsset Get(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;
            
            return new CubismAsset(name, GetResource);
        }
    }
}
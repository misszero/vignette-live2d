using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CubismFramework;
using osu.Framework.IO.Stores;

namespace osu.Framework.Graphics.Cubism
{
    public class CubismAssetStore : IResourceStore<CubismAsset>
    {
        private IResourceStore<byte[]> store { get; }
        private readonly Dictionary<string, CubismAsset> cache = new Dictionary<string, CubismAsset>();

        public CubismAssetStore(IResourceStore<byte[]> store)
        {
            this.store = store;
        }

        /// <summary>
        /// Loads a model used for <see cref="CubismSprite"/>.
        /// </summary>
        /// <param name="name">The path to the model json file.</param>
        /// <returns>The loaded CubismAsset</returns>
        public CubismAsset Get(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;

            lock (cache)
            {
                if (!cache.TryGetValue(name, out var asset))
                {
                    try
                    {
                        var baseDir = name.Split('.', 2)[0];
                        var mdlPath = name.Substring(baseDir.Length + 1, (name.Length - baseDir.Length) - 1);
                        asset = new CubismAsset($"{baseDir}/{mdlPath}", (string path) =>
                        {
                            // CubismFileLoader internally uses System.IO.Path
                            path = path.Replace(Path.DirectorySeparatorChar, '.');
                            path = path.Replace(Path.AltDirectorySeparatorChar, '.');

                            // osu!framework prepends an underscore to numbers as file names
                            var matches = new Regex(@"\.(\d+)").Matches(path);
                            foreach (Match match in matches)
                            {
                                GroupCollection groups = match.Groups;
                                var start = groups[1].Index;
                                var end = start + groups[1].Length;
                                path = path.Substring(0, start) + $"_{groups[1].Value}" + path.Substring(end, path.Length - end);
                            }

                            return GetStream(path);
                        });
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }

                return asset;
            }
        }

        public Task<CubismAsset> GetAsync(string name) => Task.Run(() => Get(name));

        public IEnumerable<string> GetAvailableResources() => store.GetAvailableResources().Where(s => s.EndsWith(".model3.json"));

        public Stream GetStream(string name) => store.GetStream(name);

        private bool isDisposed;

        ~CubismAssetStore()
        {
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (!isDisposed)
            {
                isDisposed = true;
                store.Dispose();
            }
        }
    }
}
using System.Collections.Generic;
using CubismFramework;

namespace osu.Framework.Graphics.Cubism
{
    public class CubismMotionQueue
    {
        public CubismMotionQueueEntry Current { get; private set; }
        public bool IsActive => Current?.Playing ?? false;
        public int Queued => queue.Count;
        private readonly CubismAsset asset;
        private readonly CubismAsset.MotionType type;
        private List<(ICubismMotion, bool)> queue = new List<(ICubismMotion, bool)>();

        public CubismMotionQueue(CubismAsset asset, CubismAsset.MotionType type)
        {
            this.asset = asset;
            this.type = type;
        }

        public void Add(ICubismMotion motion, bool loop = false)
        {
            lock (queue) queue.Add((motion, loop));
        }

        public void Next(double fadeOutTime = 0)
        {
            if (queue.Count - 1 > 0)
                Current?.Terminate(fadeOutTime);
        }

        public void Suspend(bool enable = true) => Current?.Suspend(enable);

        public void Stop(double fadeOutTime = 0)
        {
            lock (queue) queue.Clear();
            Current?.Terminate(fadeOutTime);

            asset.Model.RestoreDefaultParameters();
            asset.Model.SaveParameters();
        }

        public void Update()
        {
            if (((Current?.Finished ?? true) || (Current?.Terminated ?? true)) && (queue.Count > 0))
            {
                var (motion, loop) = queue[0];
                Current = asset.StartMotion(type, motion, loop);

                lock (queue)  queue.RemoveAll(entry => entry.Item1 == motion);
            }
        }
    }
}
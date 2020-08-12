using CubismFramework;
using osu.Framework.Allocation;

namespace osu.Framework.Live2D.Tests.Visual.Motions
{
    public class TestSceneMotionBase : TestSceneMotions
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            foreach (var (key, val) in Sprite.Asset.MotionGroups)
            {
                for (int i = 0; i < val.Length; i++)
                {
                    var id = i;
                    AddStep($"play motion {i}", () => Sprite.StartMotion(key, id, CubismAsset.MotionType.Base, true));
                }
            }
        }
    }
}
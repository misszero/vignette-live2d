using System.Reflection;
using CubismFramework;
using osu.Framework.Allocation;

namespace osu.Framework.Live2D.Tests.Visual.Motions
{
    [System.ComponentModel.Description("Customize each parameter")]
    public class TestSceneModelParameters : TestSceneMotions
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            var parameters = typeof(CubismDefaultParameterId)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            
            foreach (var param in parameters)
            {
                var name = param.GetValue(null) as string;
                var csmParam = Sprite.Asset.Model.GetParameter(name);

                if (csmParam != null)
                    AddSliderStep<double>(name, csmParam.Minimum, csmParam.Maximum, csmParam.Default,
                        (double val) => Sprite.SetParameter(name, val));
            }
        }
    }
}
// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

namespace Vignette.Application.Live2D.Tests.Visual
{
    public class TestSceneParameterSandbox : TestSceneCubismSprite
    {
        public TestSceneParameterSandbox()
        {
            foreach (var param in Model.Parameters)
                AddSliderStep(param.Name, param.Minimum, param.Maximum, param.Value, (v) => param.Value = v);
        }
    }
}

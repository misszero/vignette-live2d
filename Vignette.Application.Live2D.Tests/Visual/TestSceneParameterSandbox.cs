// Copyright (c) Vignette Project.
// Licensed under MIT. Please see LICENSE for more details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

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

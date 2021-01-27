// Copyright (c) Vignette Project.
// Licensed under MIT. Please see LICENSE for more details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using System.Linq;
using NUnit.Framework;
using Vignette.Application.Live2D.Model;
using Vignette.Application.Live2D.Tests.Resources;
using Vignette.Application.Live2D.Utils;

namespace Vignette.Application.Live2D.Tests
{
    [TestFixture]
    public class CubismModelTest
    {
        private CubismModel model;

        [SetUp]
        public void SetUp()
        {
            var moc = new CubismMoc(TestResources.GetModelResource("Hiyori.moc3"));
            model = new CubismModel(moc);
        }

        [Test]
        public void TestParameterUpdate()
        {
            var paramHandle = CubismCore.csmGetParameterValues(model.Handle);

            float[] prev = CubismUtils.PointerToFloatArray(paramHandle, model.Parameters.Count);

            var param = model.Parameters.Get("ParamEyeLOpen");
            param.Value = param.Minimum;

            Assert.IsTrue(!prev.SequenceEqual(model.Parameters.Select(p => p.Value)));

            model.Update();
            float[] now = CubismUtils.PointerToFloatArray(paramHandle, model.Parameters.Count);

            Assert.IsTrue(!prev.SequenceEqual(now));
        }
    }
}

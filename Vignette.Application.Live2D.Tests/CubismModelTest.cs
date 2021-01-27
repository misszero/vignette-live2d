// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

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

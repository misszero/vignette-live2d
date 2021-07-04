// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using NUnit.Framework;
using Vignette.Game.Live2D.Model;
using Vignette.Game.Live2D.Tests.Resources;

namespace Vignette.Game.Live2D.Tests.Model
{
    public class CubismMocTest
    {
        [Test]
        public void CreateMocTest()
        {
            using var mocStream = TestResources.GetModelResourceStore().GetStream("Hiyori.moc3");
            using var moc = new CubismMoc(mocStream);

            Assert.That(moc.Version, Is.Not.EqualTo(CubismMocVersion.csmMocVersion_Unknown));
        }
    }
}

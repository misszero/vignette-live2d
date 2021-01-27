// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using System.IO;
using System.Linq;
using System.Text.Json;
using NUnit.Framework;
using Vignette.Application.Live2D.Json;
using Vignette.Application.Live2D.Model;
using Vignette.Application.Live2D.Motion;
using Vignette.Application.Live2D.Physics;
using Vignette.Application.Live2D.Tests.Resources;

namespace Vignette.Application.Live2D.Tests
{
    [TestFixture]
    public class FileParsingTest
    {
        private CubismMoc moc;

        private CubismModel model;

        private CubismModelSetting setting;

        [SetUp]
        public void SetUp()
        {
            moc = new CubismMoc(TestResources.GetModelResource(@"Hiyori.moc3"));
            model = new CubismModel(moc);
            setting = loadJsonFile<CubismModelSetting>(@"Hiyori.model3.json");
        }

        [Test]
        public void TestMocFileParsing()
        {
            Assert.IsTrue(moc.Version != CubismMocVersion.csmMocVersion_Unknown);
        }

        [Test]
        public void TestModelLoading()
        {
            Assert.IsTrue(model.Parameters.Count > 0);
            Assert.IsTrue(model.Drawables.Count > 0);
            Assert.IsTrue(model.Parts.Count > 0);

            Assert.DoesNotThrow(() => model.Update());

            Assert.IsTrue(model.Drawables.FirstOrDefault().TextureCoordinates.Length > 0);
            Assert.IsTrue(model.Drawables.FirstOrDefault().Vertices.Length > 0);
            Assert.IsTrue(model.Drawables.FirstOrDefault().Indices.Length > 0);
        }

        [Test]
        public void TestMotionFileParsing()
        {
            string motionFile = @"motions/Hiyori_m01.motion3.json";
            var motion = new CubismMotion(model, loadJsonFile<CubismMotionSetting>(motionFile), setting.FileReferences.Motions["Idle"][0]);

            Assert.IsTrue(motion.GlobalFadeInSeconds > 0.0f);
            Assert.IsTrue(motion.GlobalFadeOutSeconds > 0.0f);
            Assert.DoesNotThrow(() => motion.Update(1));
        }

        //[Test]
        //public void TestPoseFileParsing()
        //{
        //    var pose = new CubismPose(loadJsonFile<CubismPoseSetting>(@"Hiyori.pose3.json"), model);

        //    Assert.IsTrue(pose.PartGroups.Count > 0);
        //    Assert.DoesNotThrow(() => pose.Update(1));
        //}

        [Test]
        public void TestPhysicsFileParsing()
        {
            var physics = new CubismPhysics(model, loadJsonFile<CubismPhysicsSetting>(@"Hiyori.physics3.json"));

            Assert.DoesNotThrow(() => physics.Update(1));
        }

        // [Test]
        // public void TestExpressionFileParsing()
        // {
        // }

        [TearDown]
        public void TearDown()
        {
            model.Dispose();
            moc.Dispose();
        }

        private T loadJsonFile<T>(string file)
        {
            using var reader = new StreamReader(TestResources.GetModelResource(file));
            return JsonSerializer.Deserialize<T>(reader.ReadToEnd());
        }
    }
}

// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.
// This software implements Live2D. Copyright (c) Live2D Inc. All Rights Reserved.
// License for Live2D can be found here: http://live2d.com/eula/live2d-open-software-license-agreement_en.html

using System;
using osu.Framework;
using osu.Framework.IO.Stores;
using osu.Framework.Logging;
using osu.Framework.Testing;
using Vignette.Game.Live2D.Resources;
using CubismLogFunction = Vignette.Game.Live2D.CubismCore.CubismLogFunction;

namespace Vignette.Game.Live2D.Tests
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            using (var host = Host.GetSuitableHost("vignette"))
                host.Run(new VisualTestGame());
        }

        private class VisualTestGame : osu.Framework.Game
        {
            public VisualTestGame()
            {
                CubismCore.SetLogFunction(new CubismLogFunction(logCubismCoreMessage));
            }

            private static readonly Logger logger = Logger.GetLogger("performance-cubism");

            private static void logCubismCoreMessage(string message) => logger.Add(message);

            protected override void LoadComplete()
            {
                base.LoadComplete();

                Resources.AddStore(new NamespacedResourceStore<byte[]>(new DllResourceStore(CubismResources.ResourceAssembly), "Resources"));
                Add(new TestBrowser("Vignette"));
            }
        }
    }
}

using System;
using osu.Framework.Platform;

namespace osu.Framework.Live2D.Tests
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            using (GameHost host = Host.GetSuitableHost(@"osu-framework-live2d"))
            {
                host.Run(new VisualTestGame());
            }
        }
    }
}

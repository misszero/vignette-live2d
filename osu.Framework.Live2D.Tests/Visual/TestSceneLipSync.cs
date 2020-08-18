using osu.Framework.Allocation;
using osu.Framework.Audio;

namespace osu.Framework.Live2D.Tests.Visual
{
    [System.ComponentModel.Description("Sync mouth movement with samples")]
    public class TestSceneLipSync : TestSceneBase
    {
        [BackgroundDependencyLoader]
        private void load(AudioManager audio)
        {
            Sprite.CanBreathe = true;
            Sprite.CanEyeBlink = true;
            Sprite.Voice = audio.Samples.Get("tone.wav");
            AddStep("play sample", () => Sprite.Voice.Play());

            Add(new ParameterMonitor(Sprite, new[] { "ParamMouthOpenY" }));
        }
    }
}
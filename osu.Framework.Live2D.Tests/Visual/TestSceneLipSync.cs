// Copyright (c) Nitrous <n20gaming2000@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;

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
            AddStep("play sample", () => ((SampleChannel)Sprite.Voice).Play());

            Add(new ParameterMonitor(Sprite, new[] { "ParamMouthOpenY" }));
        }
    }
}
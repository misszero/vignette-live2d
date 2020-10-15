// Copyright 2020 - 2021 Vignette Project
// Licensed under MIT. See LICENSE for details.

using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;

namespace osu.Framework.Live2D.Tests.Visual
{
    [System.ComponentModel.Description("Sync mouth movement with samples")]
    public class TestSceneLipSync : CubismTestScene
    {
        [BackgroundDependencyLoader]
        private void load(AudioManager audio)
        {
            Sprite.Voice = audio.Samples.Get("tone.wav");
            AddParameterTracker("ParamMouthOpenY");

            var voice = Sprite.Voice as SampleChannel;
            AddStep("play sample", () => voice.Play());
        }
    }
}

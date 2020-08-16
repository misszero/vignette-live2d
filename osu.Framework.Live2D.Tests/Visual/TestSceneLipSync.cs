using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace osu.Framework.Live2D.Tests.Visual
{
    [System.ComponentModel.Description("Sync mouth movement with samples")]
    public class TestSceneLipSync : TestSceneBase
    {
        private Box box;

        [BackgroundDependencyLoader]
        private void load(AudioManager audio)
        {
            Sprite.CanBreathe = true;
            Sprite.CanEyeBlink = true;
            Sprite.Voice = audio.Samples.Get("tone.wav");
            AddStep("play sample", () => Sprite.Voice.Play());

            Add(new FillFlowContainer
            {
                Size = new Vector2(400, 100),
                Margin = new MarginPadding(10),
                Direction = FillDirection.Vertical,
                Children = new Drawable[]
                {
                    new SpriteText { Text = "ParamMouthOpenY" },
                    box = new Box
                    {
                        RelativeSizeAxes = Axes.X,
                        Colour = Colour4.White,
                        Height = 10
                    },
                }
            });
        }

        protected override void Update()
        {
            base.Update();

            box.Width = ((float)Sprite.Asset.Model.GetParameter("ParamMouthOpenY").Value);
        }
    }
}
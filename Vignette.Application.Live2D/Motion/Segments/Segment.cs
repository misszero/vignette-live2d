namespace Vignette.Application.Live2D.Motion.Segments
{
    public abstract class Segment : ISegment
    {
        public ControlPoint[] Points { get; protected set; }

        public Segment(int points)
        {
            Points = new ControlPoint[points];
        }

        public abstract float Evaluate(float time);
    }
}

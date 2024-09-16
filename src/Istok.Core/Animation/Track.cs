namespace Istok.Core.Animation;

public class Track<T>(IAnimationBinding<T> binding, Curve<T> curve) : ITrack where T : struct
{
    public IAnimationBinding<T> Binding { get; } = binding;
    public Curve<T> Curve { get; } = curve;

    public float Duration => Curve.Duration;

    public void Evaluate(float time)
    {
        Binding.Setter?.Invoke(Curve.Evaluate(time));
    }

    public void Bind(Node sceneNode)
    {
        Binding.Bind(sceneNode);
    }
}

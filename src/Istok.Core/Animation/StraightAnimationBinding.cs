namespace Istok.Core.Animation;

public class StraightAnimationBinding<T>(Action<T> setter) : IAnimationBinding<T>
{
    public Action<T> Setter { get; } = setter;
    public void Bind(Node sceneNode) { }
}

namespace Istok.Core.Animation;

public interface IAnimationBinding<in T>
{
    Action<T>? Setter { get; }
    void Bind(Node sceneNode);
}

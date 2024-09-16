namespace Istok.Core.Animation;

public abstract class AnimationBinding<T>(string path, string property) : IAnimationBinding<T>
{
    public string Path { get; } = path;
    public string Property { get; } = property;

    public Action<T>? Setter { get; internal set; }
    public abstract void Bind(Node sceneNode);
}

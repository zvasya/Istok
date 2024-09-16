namespace Istok.Core.Animation;

public interface ITrack
{
    public float Duration { get; }
    public void Evaluate(float time);
    void Bind(Node sceneNode);
}

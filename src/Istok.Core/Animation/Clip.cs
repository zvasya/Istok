namespace Istok.Core.Animation;

public class Clip
{
    public required List<ITrack> Tracks { get; init; }

    public void Evaluate(float time)
    {
        foreach (ITrack track in Tracks)
            track.Evaluate(time);
    }
}

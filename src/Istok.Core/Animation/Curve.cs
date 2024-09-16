namespace Istok.Core.Animation;

public class Curve<T>(Sampler<T> sampler) where T : struct
{
    readonly List<Keyframe<T>> _keyframes = [];

    public void AddKeyframe(Keyframe<T> keyframe)
    {
        _keyframes.Add(keyframe);
    }

    public float Duration => _keyframes[^1].Time;

    public T Evaluate(float time)
    {
        int i = 0;
        if (time <= _keyframes[i].Time)
            return _keyframes[i].Value;
        i++;

        while (i < _keyframes.Count - 1 && _keyframes[i].Time < time)
        {
            i++;
        }

        Keyframe<T> keyframeFrom = _keyframes[i-1];
        Keyframe<T> keyframeTo = _keyframes[i];
        float dt = (time - keyframeFrom.Time) / (keyframeTo.Time - keyframeFrom.Time);

        return sampler(keyframeFrom, keyframeTo, dt);
    }
}

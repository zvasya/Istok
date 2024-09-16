namespace Istok.Core.Animation;

public readonly struct Keyframe<T>(float time, T value, T inTangent = default, T outTangent = default) where T : struct
{
    public readonly float Time = time;
    public readonly T Value = value;
    public readonly T InTangent = inTangent;
    public readonly T OutTangent = outTangent;
}

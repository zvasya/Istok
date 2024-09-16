using System.Numerics;

namespace Istok.Core.Animation;

public delegate T Sampler<T>(Keyframe<T> from, Keyframe<T> to, float t) where T : struct;

public static class Samplers
{
    public static Vector3 Step(Keyframe<Vector3> from, Keyframe<Vector3> to, float t) => from.Value;

    public static Vector3 Linear(Keyframe<Vector3> from, Keyframe<Vector3> to, float t) => Vector3.Lerp(from.Value, to.Value, t);

    public static Vector3 Cubic(Keyframe<Vector3> from, Keyframe<Vector3> to, float t)
    {
        (float startValue, float endValue, float startTangent, float endTangent) = MathExt.GetHermiteCoefficients(t);

        return from.Value * startValue + to.Value * endValue + from.OutTangent * startTangent + to.InTangent * endTangent;
    }

    public static Quaternion Step(Keyframe<Quaternion> from, Keyframe<Quaternion> to, float t) => from.Value;

    public static Quaternion Linear(Keyframe<Quaternion> from, Keyframe<Quaternion> to, float t) => Quaternion.Lerp(from.Value, to.Value, t);


    public static Quaternion Cubic(Keyframe<Quaternion> from, Keyframe<Quaternion> to, float t)
    {
        (float startValue, float endValue, float startTangent, float endTangent) = MathExt.GetHermiteCoefficients(t);

        return Quaternion.Normalize(from.Value * startValue + to.Value * endValue + from.OutTangent * startTangent + to.InTangent * endTangent);
    }
}

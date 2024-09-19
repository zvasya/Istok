using System.Numerics;
using System.Runtime.CompilerServices;

namespace Istok.Core;

public static class MathExt
{
    public const double DegToRad = Math.PI / 180.0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Radians(float deg)
    {
        return (float)(deg * DegToRad);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Radians(double deg)
    {
        return deg * DegToRad;
    }
    public static (float startValue, float endValue, float startTangent, float endTangent) GetHermiteCoefficients(double amount)
    {
        if (amount <= 0.0)
            return (1, 0, 0, 0);

        if (amount >= 1.0)
            return (0,1,0,0);

        double squared = amount * amount;
        double cubed = squared * amount;

        // https://en.wikipedia.org/wiki/Cubic_Hermite_spline#Unit_interval_[0,_1]
        double endTangent = cubed -  squared;                  // t^3 - t^2
        double startTangent = endTangent - squared + amount;   // t^3 - 2*t^2 + t
        double endValue = squared - 2.0 * endTangent;          // - 2*t^3 + 3*t^2
        double startValue = 1.0 - endValue;                    // 2*t^3 - 3*t^2 + 1

        return ((float)startValue, (float)endValue, (float)startTangent, (float)endTangent);
    }
}

using System.Numerics;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Istok.Mathematics;

public struct BoundingBox(Vector3 center, Vector3 extent)
{
    public static readonly BoundingBox Empty = new BoundingBox(Vector3.Zero, Vector3.Zero);

    public Vector3 Center = center;

    public Vector3 Extent = extent;

    public static BoundingBox FromMinMax (Vector3 minimum, Vector3 maximum)
    {
        return new BoundingBox
        {
            Center = (minimum + maximum) / 2,
            Extent = (maximum - minimum) / 2
        };
    }

    public Vector3 Minimum => Center - Extent;

    public Vector3 Maximum => Center + Extent;

    public static BoundingBox Transform(BoundingBox boundingBox, Matrix4x4 world)
    {
        var center4 = Vector4.Transform(boundingBox.Center, world);
        var center = new Vector3(center4.X / center4.W, center4.Y / center4.W, center4.Z / center4.W);

        // Update world matrix into absolute form
        ref float matrixData = ref Unsafe.AsRef(in world.M11);
        for (int j = 0; j < 16; ++j)
        {
            matrixData = MathF.Abs(matrixData);
            matrixData = ref Unsafe.Add(ref matrixData, 1);
        }

        var extent = Vector3.TransformNormal(boundingBox.Extent, world);
        
        return new BoundingBox(center, extent);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundingBox Merge(ref readonly BoundingBox value1, ref readonly BoundingBox value2)
    {
        var maximum = Vector3.Max(value1.Maximum, value2.Maximum);
        var minimum = Vector3.Min(value1.Minimum, value2.Minimum);

        return new BoundingBox((minimum + maximum) / 2, (maximum - minimum) / 2);
    }

    public bool Equals(BoundingBox other)
    {
        return Center.Equals(other.Center) && Extent.Equals(other.Extent);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        return obj is BoundingBox && Equals((BoundingBox)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (Center.GetHashCode() * 397) ^ Extent.GetHashCode();
        }
    }

    public static bool operator ==(BoundingBox left, BoundingBox right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(BoundingBox left, BoundingBox right)
    {
        return !left.Equals(right);
    }
}
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Istok.Mathematics;

public class Frustum(ref readonly Matrix4x4 matrix)
{
    public Plane LeftPlane = Plane.Normalize(new Plane(
        matrix.M14 + matrix.M11,
        matrix.M24 + matrix.M21,
        matrix.M34 + matrix.M31,
        matrix.M44 + matrix.M41));

    public Plane RightPlane = Plane.Normalize(new Plane(
        matrix.M14 - matrix.M11,
        matrix.M24 - matrix.M21,
        matrix.M34 - matrix.M31,
        matrix.M44 - matrix.M41));

    public Plane TopPlane = Plane.Normalize(new Plane(
        matrix.M14 - matrix.M12,
        matrix.M24 - matrix.M22,
        matrix.M34 - matrix.M32,
        matrix.M44 - matrix.M42));

    public Plane BottomPlane = Plane.Normalize(new Plane(
        matrix.M14 + matrix.M12,
        matrix.M24 + matrix.M22,
        matrix.M34 + matrix.M32,
        matrix.M44 + matrix.M42));

    public Plane NearPlane = Plane.Normalize(new Plane(
        matrix.M13,
        matrix.M23,
        matrix.M33,
        matrix.M43
    ));

    public Plane FarPlane = Plane.Normalize(new Plane(
        matrix.M14 - matrix.M13,
        matrix.M24 - matrix.M23,
        matrix.M34 - matrix.M33,
        matrix.M44 - matrix.M43
    ));

    public bool Contains(ref readonly BoundingBox boundingBoxExt)
    {
        ref Plane plane = ref Unsafe.AsRef(in LeftPlane);
        for (int i = 0; i < 6; ++i)
        {
            if (Vector3.Dot(boundingBoxExt.Center, plane.Normal)
                + boundingBoxExt.Extent.X * MathF.Abs(plane.Normal.X)
                + boundingBoxExt.Extent.Y * MathF.Abs(plane.Normal.Y)
                + boundingBoxExt.Extent.Z * MathF.Abs(plane.Normal.Z)
                <= -plane.D)
                return false;
            plane = ref Unsafe.Add(ref plane, 1);
        }

        return true;
    }
}
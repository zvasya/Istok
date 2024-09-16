namespace Istok.Rendering;

public static class ArrayExtensions
{
    internal static void EnsureArrayMinimumSize<T>(ref T[] array, uint size)
    {
        if (array == null)
            array = new T[size];
        else if (array.Length < size)
            Array.Resize(ref array, (int)size);
    }

    internal static T[] ClearOrCreateNewOfMinimumSize<T>(T[] array, uint size)
    {
        if (array == null || array.Length < size)
            return new T[size];

        Array.Clear(array);
        return array;
    }

    internal static bool ArrayEquals<T>(T[] left, T[] right) where T : class
    {
        if (left == null || right == null)
            return left == right;

        if (left.Length != right.Length)
            return false;

        for (int i = 0; i < left.Length; i++)
        {
            if (!ReferenceEquals(left[i], right[i]))
            {
                return false;
            }
        }

        return true;
    }

    internal static bool ArrayEqualsEquatable<T>(T[] left, T[] right) where T : struct, IEquatable<T>
    {
        if (left == null || right == null)
            return left == right;

        if (left.Length != right.Length)
            return false;

        for (int i = 0; i < left.Length; i++)
        {
            if (!left[i].Equals(right[i]))
            {
                return false;
            }
        }

        return true;
    }
}

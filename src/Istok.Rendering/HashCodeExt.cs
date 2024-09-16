namespace Istok;

internal static class HashCodeExt
{
    public static int Combine<T>(IEnumerable<T> items)
    {
        HashCode hash = new HashCode();
        foreach (T item in items)
            hash.Add(item);
        return hash.ToHashCode();
    }
}

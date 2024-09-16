namespace Istok.Rendering;

internal readonly struct BoundResourceSetInfo(ResourceSet set, in Span<uint> offsets) : IEquatable<BoundResourceSetInfo>
{
    public readonly ResourceSet Set = set;
    public readonly uint[] Offsets = offsets.ToArray();

    public bool Equals(ResourceSet set, in Span<uint> offsets)
    {
        return Set == set && offsets.SequenceEqual(Offsets);
    }

    public bool Equals(BoundResourceSetInfo other)
    {
        return Set == other.Set && Offsets.AsSpan().SequenceEqual(other.Offsets);
    }
}

namespace Istok.Rendering;

public readonly struct DescriptorSetLayoutBindings(params DescriptorSetLayoutBindingEntry[] elements) : IEquatable<DescriptorSetLayoutBindings>
{
    public readonly DescriptorSetLayoutBindingEntry[] Elements = elements;

    public bool Equals(DescriptorSetLayoutBindings other)
    {
        return Elements.AsSpan().SequenceEqual(other.Elements);
    }

    public override int GetHashCode()
    {
        return HashCodeExt.Combine(Elements);
    }
}

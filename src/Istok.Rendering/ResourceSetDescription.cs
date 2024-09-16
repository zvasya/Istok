namespace Istok.Rendering;

public readonly struct ResourceSetDescription(DescriptorSetLayout layout, params ResourcesSetBinding[] boundResources) : IEquatable<ResourceSetDescription>
{
    public readonly DescriptorSetLayout Layout = layout;
    public readonly ResourcesSetBinding[] BoundResources = boundResources;

    public bool Equals(ResourceSetDescription other)
    {
        return Layout.Equals(other.Layout) && ArrayExtensions.ArrayEquals(BoundResources, other.BoundResources);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Layout, HashCodeExt.Combine(BoundResources));
    }
}

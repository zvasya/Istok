using System.Collections.Frozen;
using Silk.NET.Vulkan;

namespace Istok.Rendering;

public class DescriptorSet(DescriptorPool descriptorPool, Silk.NET.Vulkan.DescriptorSet descriptorSet, Dictionary<DescriptorType, uint> resource)
{
    public Silk.NET.Vulkan.DescriptorSet Set { get; } = descriptorSet;
    FrozenDictionary<DescriptorType, uint> Resource { get; } = resource.ToFrozenDictionary();

    public void Dispose()
    {
        descriptorPool.Free(Set, Resource);
    }
}

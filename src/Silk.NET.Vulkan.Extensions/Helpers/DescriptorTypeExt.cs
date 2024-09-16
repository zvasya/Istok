using Silk.NET.Vulkan;

namespace Silk.NET.Vulkan.Extensions.Helpers;

public static class DescriptorTypeExt
{
    public static bool IsDynamic(this DescriptorType type)
    {
        return type is DescriptorType.UniformBufferDynamic or DescriptorType.StorageBufferDynamic;
    }
}

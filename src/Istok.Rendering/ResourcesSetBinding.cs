using Silk.NET.Vulkan;

namespace Istok.Rendering;

public abstract class ResourcesSetBinding
{
    public abstract unsafe void Bind(
        int i,
        WriteDescriptorSet* descriptorWrites,
        DescriptorBufferInfo* bufferInfos,
        DescriptorImageInfo* imageInfos,
        List<Image> sampledTextures,
        List<Image> storageTextures
    );
}

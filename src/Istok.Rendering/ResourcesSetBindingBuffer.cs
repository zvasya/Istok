using Silk.NET.Vulkan;

namespace Istok.Rendering;

public class ResourcesSetBindingBuffer(DescriptorBufferInfo bufferInfo) : ResourcesSetBinding
{
    public override unsafe void Bind(
        int i,
        WriteDescriptorSet* descriptorWrites,
        DescriptorBufferInfo* bufferInfos,
        DescriptorImageInfo* imageInfos,
        List<Image> sampledTextures,
        List<Image> storageTextures
    )
    {
        bufferInfos[i] = bufferInfo;
        descriptorWrites[i].PBufferInfo = &bufferInfos[i];
    }
}

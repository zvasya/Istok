using Silk.NET.Vulkan;

namespace Istok.Rendering;

public class ResourcesSetBindingSampler(Sampler sampler) : ResourcesSetBinding
{
    public override unsafe void Bind(int i, WriteDescriptorSet* descriptorWrites, DescriptorBufferInfo* bufferInfos, DescriptorImageInfo* imageInfos, List<Image> sampledTextures, List<Image> storageTextures)
    {
        imageInfos[i].Sampler = sampler.DeviceSampler;

        descriptorWrites[i].PImageInfo = &imageInfos[i];
    }
}

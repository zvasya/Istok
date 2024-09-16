using Silk.NET.Vulkan;

namespace Istok.Rendering;

public class ResourcesSetBindingCombinedImageSampler(ImageView imageView, Sampler sampler) : ResourcesSetBinding
{
    public override unsafe void Bind(int i, WriteDescriptorSet* descriptorWrites, DescriptorBufferInfo* bufferInfos, DescriptorImageInfo* imageInfos, List<Image> sampledTextures, List<Image> storageTextures)
    {
        imageInfos[i].ImageView = imageView.DeviceImageView;
        imageInfos[i].ImageLayout = ImageLayout.ShaderReadOnlyOptimal;
        imageInfos[i].Sampler = sampler.DeviceSampler;

        descriptorWrites[i].PImageInfo = &imageInfos[i];
        sampledTextures.Add(imageView.Target);
    }
}

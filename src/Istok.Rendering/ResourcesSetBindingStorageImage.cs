using Silk.NET.Vulkan;

namespace Istok.Rendering;

public class ResourcesSetBindingStorageImage(ImageView imageView) : ResourcesSetBinding
{
    public override unsafe void Bind(int i, WriteDescriptorSet* descriptorWrites, DescriptorBufferInfo* bufferInfos, DescriptorImageInfo* imageInfos, List<Image> sampledTextures, List<Image> storageTextures)
    {
        imageInfos[i].ImageView = imageView.DeviceImageView;
        imageInfos[i].ImageLayout = ImageLayout.General;

        descriptorWrites[i].PImageInfo = &imageInfos[i];
        storageTextures.Add(imageView.Target);
    }
}

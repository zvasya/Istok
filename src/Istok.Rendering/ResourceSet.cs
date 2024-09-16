using Silk.NET.Vulkan;

namespace Istok.Rendering;

public unsafe class ResourceSet : IDisposable
{
    readonly DescriptorSet _descriptorSet;
    readonly LogicalDevice _logicalDevice;
    string _name;

    public ResourceSet(LogicalDevice logicalDevice, DescriptorPoolManager descriptorPoolManager, in ResourceSetDescription description)
    {
        _logicalDevice = logicalDevice;

        Dictionary<DescriptorType, uint> descriptorCounts = description.Layout.DescriptorResourceCounts;
        _descriptorSet = descriptorPoolManager.Get(descriptorCounts, description.Layout);

        ResourcesSetBinding[] boundResources = description.BoundResources;
        uint descriptorWriteCount = (uint)boundResources.Length;
        WriteDescriptorSet* descriptorWrites = stackalloc WriteDescriptorSet[(int)descriptorWriteCount];
        DescriptorBufferInfo* bufferInfos = stackalloc DescriptorBufferInfo[(int)descriptorWriteCount];
        DescriptorImageInfo* imageInfos = stackalloc DescriptorImageInfo[(int)descriptorWriteCount];

        for (int i = 0; i < descriptorWriteCount; i++)
        {
            DescriptorType type = description.Layout.DescriptorTypes[i];

            descriptorWrites[i].SType = StructureType.WriteDescriptorSet;
            descriptorWrites[i].DescriptorCount = 1;
            descriptorWrites[i].DescriptorType = type;
            descriptorWrites[i].DstBinding = (uint)i;
            descriptorWrites[i].DstSet = DescriptorSet;
            //TODO rework this
            boundResources[i].Bind(i, descriptorWrites, bufferInfos, imageInfos, SampledTextures, StorageTextures);
        }

        _logicalDevice.UpdateDescriptorSets(descriptorWriteCount, descriptorWrites, 0, null);
    }

    public Silk.NET.Vulkan.DescriptorSet DescriptorSet => _descriptorSet.Set;

    public List<Image> SampledTextures { get; } = [];

    public List<Image> StorageTextures { get; } = [];

    public  string Name
    {
        get => _name;
        set
        {
            _name = value;
            _logicalDevice.SetObjectName(ObjectType.DescriptorSet, DescriptorSet.Handle, value);
        }
    }

    public bool IsDisposed { get; private set; }

    public void Dispose()
    {
        if (!IsDisposed)
        {
            IsDisposed = true;
            _descriptorSet.Dispose();
        }
    }
}

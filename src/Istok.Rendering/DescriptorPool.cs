using Silk.NET.Vulkan;

namespace Istok.Rendering;

public class DescriptorPool : IDisposable
{
    readonly object _lockObject = new object();
    readonly Dictionary<DescriptorType, uint> _descriptors;
    readonly LogicalDevice _logicalDevice;
    readonly Silk.NET.Vulkan.DescriptorPool _pool;

    uint _remainingSets;

    DescriptorPool(LogicalDevice logicalDevice, Silk.NET.Vulkan.DescriptorPool pool, uint totalSets, DescriptorPoolSize[] descriptorCount)
    {
        _logicalDevice = logicalDevice;
        _pool = pool;
        _remainingSets = totalSets;
        _descriptors = descriptorCount.ToDictionary(size => size.Type, size => size.DescriptorCount);
    }

    public void Dispose()
    {
        unsafe
        {
            _logicalDevice.DestroyDescriptorPool(_pool, null);
        }
    }

    public static DescriptorPool Create(LogicalDevice logicalDevice, uint totalSets, DescriptorPoolSize[] sizes)
    {
        unsafe
        {
            fixed (DescriptorPoolSize* sizesPtr = &sizes[0])
            {
                DescriptorPoolCreateInfo poolCI = new DescriptorPoolCreateInfo
                {
                    SType = StructureType.DescriptorPoolCreateInfo,
                    Flags = DescriptorPoolCreateFlags.FreeDescriptorSetBit,
                    MaxSets = totalSets,
                    PPoolSizes = sizesPtr,
                    PoolSizeCount = (uint)sizes.Length,
                };

                Result result = logicalDevice.CreateDescriptorPool(in poolCI, null, out Silk.NET.Vulkan.DescriptorPool descriptorPool);
                Helpers.CheckErrors(result);

                return new DescriptorPool(logicalDevice, descriptorPool, totalSets, sizes);
            }

        }
    }

    public bool Get(Dictionary<DescriptorType, uint> counts, DescriptorSetLayout setLayout, out DescriptorSet descriptorSet)
    {
        lock (_lockObject)
        {
            if (_remainingSets > 0 && counts.All(v => _descriptors.TryGetValue(v.Key, out uint availableCount) && availableCount > v.Value))
            {
                _remainingSets -= 1;
                foreach ((DescriptorType descriptorType, uint count) in counts)
                {
                    _descriptors[descriptorType] -= count;
                }

                unsafe
                {
                    Silk.NET.Vulkan.DescriptorSetLayout descriptorSetLayout = setLayout.DeviceDescriptorSetLayout;
                    DescriptorSetAllocateInfo descriptorSetAllocateInfo = new DescriptorSetAllocateInfo
                    {
                        SType = StructureType.DescriptorSetAllocateInfo,
                        DescriptorSetCount = 1,
                        PSetLayouts = &descriptorSetLayout,
                        DescriptorPool = _pool,
                    };
                    Result result = _logicalDevice.AllocateDescriptorSets(in descriptorSetAllocateInfo, out Silk.NET.Vulkan.DescriptorSet set);
                    descriptorSet = new DescriptorSet(this, set, counts);
                    Helpers.CheckErrors(result);

                    return true;
                }
            }
            else
            {
                descriptorSet = null;
                return false;
            }
        }
    }

    internal void Free(Silk.NET.Vulkan.DescriptorSet set, IDictionary<DescriptorType, uint> counts)
    {
        _logicalDevice.FreeDescriptorSets(_pool, 1, in set);
        lock (_lockObject)
        {
            _remainingSets += 1;

            foreach ((DescriptorType descriptorType, uint count) in counts)
            {
                _descriptors[descriptorType] += count;
            }
        }
    }
}


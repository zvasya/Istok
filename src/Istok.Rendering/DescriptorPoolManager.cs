using System.Diagnostics;
using Silk.NET.Vulkan;

namespace Istok.Rendering;

public class DescriptorPoolManager
{
    readonly object _lock = new object();

    readonly LogicalDevice _logicalDevice;
    readonly List<DescriptorPool> _pools = [];

    readonly uint _totalSets;
    readonly DescriptorPoolSize[] _sizes;

    public DescriptorPoolManager(LogicalDevice logicalDevice, uint totalSets, DescriptorPoolSize[] sizes)
    {
        _totalSets = totalSets;
        _sizes = sizes;
        _logicalDevice = logicalDevice;
        _pools.Add(DescriptorPool.Create(_logicalDevice, _totalSets, _sizes));
    }

    public DescriptorSet Get(Dictionary<DescriptorType, uint> counts, DescriptorSetLayout setLayout)
    {
        lock (_lock)
        {
            DescriptorSet set;
            foreach (DescriptorPool poolInfo in _pools)
            {
                if (poolInfo.Get(counts, setLayout, out set))
                {
                    return set;
                }
            }

            DescriptorPool newPool = DescriptorPool.Create(_logicalDevice, _totalSets, _sizes);
            _pools.Add(newPool);
            bool result = newPool.Get(counts, setLayout, out set);
            Debug.Assert(result);
            return set;
        }
    }

    public void Return(DescriptorSet set)
    {
        set.Dispose();
    }

    public void DestroyAll()
    {
        foreach (DescriptorPool poolInfo in _pools)
        {
            poolInfo.Dispose();
        }
    }
}

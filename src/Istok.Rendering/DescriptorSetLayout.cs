using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.Helpers;


namespace Istok.Rendering;

/// <summary>
/// Defined by an array of zero or more descriptor bindings
/// </summary>
public unsafe class DescriptorSetLayout : IDisposable
{
    readonly LogicalDevice _logicalDevice;
    string _name;

    DescriptorSetLayout(LogicalDevice logicalDevice, DescriptorType[] descriptorTypes, int dynamicBufferCount, Dictionary<DescriptorType, uint> descriptorResourceCounts, Silk.NET.Vulkan.DescriptorSetLayout descriptorSetLayout)
    {
        DynamicBufferCount = dynamicBufferCount;
        DescriptorResourceCounts = descriptorResourceCounts;
        DeviceDescriptorSetLayout = descriptorSetLayout;
        DescriptorTypes = descriptorTypes;
        _logicalDevice = logicalDevice;
    }

    public Silk.NET.Vulkan.DescriptorSetLayout DeviceDescriptorSetLayout { get; }

    public DescriptorType[] DescriptorTypes { get; }

    public Dictionary<DescriptorType, uint> DescriptorResourceCounts { get; }

    public int DynamicBufferCount { get; }

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            _logicalDevice.SetObjectName(
                ObjectType.DescriptorSetLayout,
                DeviceDescriptorSetLayout.Handle,
                value);
        }
    }


    public bool IsDisposed { get; private set; }

    public void Dispose()
    {
        if (!IsDisposed)
        {
            IsDisposed = true;
            _logicalDevice.DestroyDescriptorSetLayout(DeviceDescriptorSetLayout, null);
        }
    }

    public static DescriptorSetLayout Create(LogicalDevice logicalDevice, in DescriptorSetLayoutBindings description)
    {
        DescriptorSetLayoutBindingEntry[] elements = description.Elements;
        DescriptorType[] descriptorTypes = new DescriptorType[elements.Length];
        DescriptorSetLayoutBinding* bindings = stackalloc DescriptorSetLayoutBinding[elements.Length];

        Dictionary<DescriptorType, uint> descriptorResourceCounts = new Dictionary<DescriptorType, uint>();

        int dynamicBufferCount = 0;
        for (uint i = 0; i < elements.Length; i++)
        {
            DescriptorType descriptorType = elements[i].Kind;
            bindings[i].Binding = i;
            bindings[i].DescriptorCount = 1;
            bindings[i].DescriptorType = descriptorType;
            bindings[i].StageFlags = elements[i].Stages;
            if (elements[i].Kind.IsDynamic())
            {
                dynamicBufferCount += 1;
            }

            descriptorTypes[i] = descriptorType;

            if (!descriptorResourceCounts.TryAdd(descriptorType, 1))
                descriptorResourceCounts[descriptorType] += 1;
        }

        DescriptorSetLayoutCreateInfo descriptorSetLayoutCreateInfo = new DescriptorSetLayoutCreateInfo
        {
            SType = StructureType.DescriptorSetLayoutCreateInfo,
            BindingCount = (uint)elements.Length,
            PBindings = bindings,
        };

        Result result = logicalDevice.CreateDescriptorSetLayout(in descriptorSetLayoutCreateInfo, null, out Silk.NET.Vulkan.DescriptorSetLayout descriptorSetLayout);
        Helpers.CheckErrors(result);

        return new DescriptorSetLayout(logicalDevice, descriptorTypes, dynamicBufferCount, descriptorResourceCounts, descriptorSetLayout);
    }
}

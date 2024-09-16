using Silk.NET.Vulkan;

namespace Istok.Rendering;

/// <summary>
/// Specifying a single entry in descriptor set layout
/// </summary>
public readonly record struct DescriptorSetLayoutBindingEntry
{
    /// <summary>
    /// The name of the entry
    /// </summary>
    public readonly string Name;
    /// <summary>
    /// The kind of resource.
    /// </summary>
    public readonly DescriptorType Kind = DescriptorType.UniformBuffer;
    /// <summary>
    /// Bitmask specifying which pipeline shader stages can access a resource
    /// </summary>
    public readonly ShaderStageFlags Stages;

    public DescriptorSetLayoutBindingEntry(string name, DescriptorType kind, ShaderStageFlags stages)
    {
        Name = name;
        Kind = kind;
        Stages = stages;
    }
}

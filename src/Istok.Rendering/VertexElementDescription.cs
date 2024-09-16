using Silk.NET.Vulkan;

namespace Istok.Rendering;

/// <summary>
/// Describes a single element of a vertex for constructing VertexInputAttributeDescription
/// </summary>
public readonly record struct VertexElementDescription
{
    public readonly string Name;

    public readonly Format Format = Format.R32Sfloat;

    public VertexElementDescription(
        string name,
        Format format)
    {
        Name = name;
        Format = format;
    }
}

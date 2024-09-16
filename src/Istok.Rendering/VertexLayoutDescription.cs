using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.Helpers;

namespace Istok.Rendering;

/// <summary>
/// Describes the layout of vertex data in a <see cref="Buffer"/>
/// </summary>
public readonly struct VertexLayoutDescription : IEquatable<VertexLayoutDescription>
{
    /// <summary>
    /// The byte stride between consecutive elements within the <see cref="Buffer"/>.
    /// </summary>
    public readonly uint Stride;

    /// <summary>
    /// An array of <see cref="VertexElementDescription"/>, describing a single element of vertex data.
    /// </summary>
    public readonly VertexElementDescription[] Elements;

    /// <summary>
    /// Specifying whether vertex attribute addressing is a function of the vertex index or of the instance index
    /// </summary>
    public readonly VertexInputRate InputRate;

    public VertexLayoutDescription(uint stride, VertexInputRate inputRate = VertexInputRate.Vertex, params VertexElementDescription[] elements)
    {
        Stride = stride;
        Elements = elements;
        InputRate = inputRate;
    }

    /// <summary>
    /// Constructs a new VertexLayoutDescription. The stride is assumed to be the sum of the size of all elements.
    /// </summary>
    /// <param name="elements">An array of <see cref="VertexElementDescription"/> objects, each describing a single element
    /// of vertex data.</param>
    public VertexLayoutDescription(params VertexElementDescription[] elements)
    {
        Elements = elements;
        uint computedStride = 0;
        for (int i = 0; i < elements.Length; i++)
        {
            uint elementSize = elements[i].Format.ElementSize();
            computedStride += elementSize;
        }

        Stride = computedStride;
        InputRate = VertexInputRate.Vertex;
    }

    public VertexInputBindingDescription ToVertexInputBindingDescription(uint binding)
    {
        return new VertexInputBindingDescription
        {
            Binding = binding,
            InputRate = InputRate,
            Stride = Stride,
        };
    }

    public bool Equals(VertexLayoutDescription other)
    {
        return Stride.Equals(other.Stride)
               && ArrayExtensions.ArrayEqualsEquatable(Elements, other.Elements)
               && InputRate.Equals(other.InputRate);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Stride, HashCodeExt.Combine(Elements), InputRate);
    }
}

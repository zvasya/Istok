namespace Istok.Rendering;

public readonly struct ShaderSetDescription(VertexLayoutDescription[] vertexLayouts, ShaderModule[] shaders, SpecializationConstant[] specializations = null) : IEquatable<ShaderSetDescription>
{
    /// <summary>
    /// An array of <see cref="VertexLayoutDescription"/> describing the input layout of a <see cref="Istok.Rendering.Buffer"/>
    /// </summary>
    public readonly VertexLayoutDescription[] VertexLayouts = vertexLayouts;

    /// <summary>
    /// An array of <see cref="ShaderModule"/>, one for each shader stage in the <see cref="Pipeline"/>
    /// </summary>
    public readonly ShaderModule[] Shaders = shaders;

    /// <summary>
    /// Array of <see cref="SpecializationConstant"/> specifying data for a specialization map at the time the <see cref="Pipeline"/> is created
    /// </summary>
    public readonly SpecializationConstant[] Specializations = specializations;

    public bool Equals(ShaderSetDescription other)
    {
        return ArrayExtensions.ArrayEqualsEquatable(VertexLayouts, other.VertexLayouts)
               && ArrayExtensions.ArrayEquals(Shaders, other.Shaders)
               && ArrayExtensions.ArrayEqualsEquatable(Specializations, other.Specializations);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(HashCodeExt.Combine(VertexLayouts), HashCodeExt.Combine(Shaders), HashCodeExt.Combine(Specializations));
    }
}

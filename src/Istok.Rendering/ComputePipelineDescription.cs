namespace Istok.Rendering;

public readonly struct ComputePipelineDescription(
    Shader shaderStage,
    SpecializationConstant[] specializations,
    params DescriptorSetLayout[] descriptorSetLayout) : IEquatable<ComputePipelineDescription>
{
    public readonly Shader ComputeShader = shaderStage;
    public readonly DescriptorSetLayout[] DescriptorSetLayouts = descriptorSetLayout;
    public readonly SpecializationConstant[] Specializations = specializations;

    public ComputePipelineDescription(
        Shader computeShader,
        params DescriptorSetLayout[] descriptorSetLayouts) : this(computeShader, null, descriptorSetLayouts)
    {
    }

    public bool Equals(ComputePipelineDescription other)
    {
        return ComputeShader.Equals(other.ComputeShader)
               && ArrayExtensions.ArrayEquals(DescriptorSetLayouts, other.DescriptorSetLayouts)
               && Specializations.AsSpan().SequenceEqual(other.Specializations);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ComputeShader, HashCodeExt.Combine(DescriptorSetLayouts), HashCodeExt.Combine(Specializations));
    }
}

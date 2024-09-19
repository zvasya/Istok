namespace Istok.Rendering;

public readonly struct ComputePipelineDescription(
    ShaderModule shaderModuleStage,
    SpecializationConstant[] specializations,
    params DescriptorSetLayout[] descriptorSetLayout) : IEquatable<ComputePipelineDescription>
{
    public readonly ShaderModule ComputeShaderModule = shaderModuleStage;
    public readonly DescriptorSetLayout[] DescriptorSetLayouts = descriptorSetLayout;
    public readonly SpecializationConstant[] Specializations = specializations;

    public ComputePipelineDescription(
        ShaderModule computeShaderModule,
        params DescriptorSetLayout[] descriptorSetLayouts) : this(computeShaderModule, null, descriptorSetLayouts)
    {
    }

    public bool Equals(ComputePipelineDescription other)
    {
        return ComputeShaderModule.Equals(other.ComputeShaderModule)
               && ArrayExtensions.ArrayEquals(DescriptorSetLayouts, other.DescriptorSetLayouts)
               && Specializations.AsSpan().SequenceEqual(other.Specializations);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ComputeShaderModule, HashCodeExt.Combine(DescriptorSetLayouts), HashCodeExt.Combine(Specializations));
    }
}

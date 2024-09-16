using Silk.NET.Vulkan;

namespace Istok.Rendering;

public struct GraphicsPipelineDescription : IEquatable<GraphicsPipelineDescription>
{
    /// <summary>
    /// A description of the blend params
    /// </summary>
    public readonly BlendStateDescription BlendState;
    /// <summary>
    /// Controlling the depth bounds tests, stencil test, and depth test
    /// </summary>
    public readonly PipelineDepthStencilStateCreateInfo DepthStencilState;
    /// <summary>
    /// A description of the rasterization state
    /// </summary>
    public PipelineRasterizationStateCreateInfo RasterizerState;
    /// <summary>
    /// Determines how consecutive vertices are organized into primitives, and determines the type of primitive that is used at the beginning of the graphics pipeline
    /// </summary>
    public readonly PrimitiveTopology PrimitiveTopology;
    /// <summary>
    /// A description of the shaders to be used
    /// </summary>
    public readonly ShaderSetDescription ShaderSet;
    /// <summary>
    /// An array of <see cref="DescriptorSetLayout"/>, controls the layout of shader resources in the <see cref="Pipeline"/>
    /// </summary>
    public readonly DescriptorSetLayout[] Layouts;
    /// <summary>
    /// A collection of attachments, subpasses, and dependencies between the subpasses, and describes how the attachments are used over the course of the subpasses
    /// </summary>
    public RenderPass Outputs;

    public GraphicsPipelineDescription(
        BlendStateDescription blendState,
        PipelineDepthStencilStateCreateInfo depthStencilStateDescription,
        PipelineRasterizationStateCreateInfo rasterizerState,
        PrimitiveTopology primitiveTopology,
        ShaderSetDescription shaderSet,
        RenderPass outputs,
        params DescriptorSetLayout[] resourceLayouts
    )
    {
        BlendState = blendState;
        DepthStencilState = depthStencilStateDescription;
        RasterizerState = rasterizerState;
        PrimitiveTopology = primitiveTopology;
        ShaderSet = shaderSet;
        Layouts = resourceLayouts;
        Outputs = outputs;
    }

    public bool Equals(GraphicsPipelineDescription other)
    {
        return BlendState.Equals(other.BlendState)
               && DepthStencilState.Equals(other.DepthStencilState)
               && RasterizerState.Equals(other.RasterizerState)
               && PrimitiveTopology == other.PrimitiveTopology
               && ShaderSet.Equals(other.ShaderSet)
               && ArrayExtensions.ArrayEquals(Layouts, other.Layouts)
               && Outputs.Equals(other.Outputs);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            BlendState,
            DepthStencilState,
            RasterizerState,
            PrimitiveTopology,
            ShaderSet,
            HashCodeExt.Combine(Layouts),
            Outputs);
    }
}

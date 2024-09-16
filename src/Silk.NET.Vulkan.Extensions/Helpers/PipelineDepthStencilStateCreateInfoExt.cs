using Silk.NET.Vulkan;

namespace Silk.NET.Vulkan.Extensions.Helpers;
/// <summary>
/// Set of predefined structures specifying parameters of a newly created <see cref="Pipeline"/> depth stencil state
/// </summary>
public static class PipelineDepthStencilStateCreateInfoExt
{
    /// <summary>
    /// Describes a depth-only depth stencil with read, write and <see cref="CompareOp.LessOrEqual"/> comparison
    /// The stencil test is disabled.
    /// </summary>
    public static readonly PipelineDepthStencilStateCreateInfo DepthOnlyLessEqual = new PipelineDepthStencilStateCreateInfo
    {
        SType = StructureType.PipelineDepthStencilStateCreateInfo,
        DepthTestEnable = true,
        DepthWriteEnable = true,
        DepthCompareOp = CompareOp.LessOrEqual,
    };

    /// <summary>
    /// Describes a depth-only depth stencil with <see cref="CompareOp.LessOrEqual"/> comparison
    /// The stencil test is disabled.
    /// </summary>
    public static readonly PipelineDepthStencilStateCreateInfo DepthOnlyLessEqualRead = new PipelineDepthStencilStateCreateInfo
    {
        SType = StructureType.PipelineDepthStencilStateCreateInfo,
        DepthTestEnable = true,
        DepthWriteEnable = false,
        DepthCompareOp = CompareOp.LessOrEqual,
    };

    /// <summary>
    /// Describes a depth-only depth stencil with read, write and <see cref="CompareOp.GreaterOrEqual"/> comparison
    /// The stencil test is disabled.
    /// </summary>
    public static readonly PipelineDepthStencilStateCreateInfo DepthOnlyGreaterEqual = new PipelineDepthStencilStateCreateInfo
    {
        SType = StructureType.PipelineDepthStencilStateCreateInfo,
        DepthTestEnable = true,
        DepthWriteEnable = true,
        DepthCompareOp = CompareOp.GreaterOrEqual,
    };

    /// <summary>
    /// Describes a depth-only depth stencil with <see cref="CompareOp.GreaterOrEqual"/> comparison
    /// The stencil test is disabled.
    /// </summary>
    public static readonly PipelineDepthStencilStateCreateInfo DepthOnlyGreaterEqualRead = new PipelineDepthStencilStateCreateInfo
    {
        SType = StructureType.PipelineDepthStencilStateCreateInfo,
        DepthTestEnable = true,
        DepthWriteEnable = false,
        DepthCompareOp = CompareOp.GreaterOrEqual,
    };

    /// <summary>
    /// Describes a depth stencil
    /// Depth and stencil test is disabled.
    /// </summary>
    public static readonly PipelineDepthStencilStateCreateInfo Disabled = new PipelineDepthStencilStateCreateInfo
    {
        SType = StructureType.PipelineDepthStencilStateCreateInfo,
        DepthTestEnable = false,
        DepthWriteEnable = false,
        DepthCompareOp = CompareOp.LessOrEqual,
    };
}

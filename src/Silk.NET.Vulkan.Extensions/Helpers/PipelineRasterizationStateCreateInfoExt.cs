using Silk.NET.Vulkan;

namespace Silk.NET.Vulkan.Extensions.Helpers;

/// <summary>
/// Set of predefined structures specifying parameters of a newly created pipeline rasterization state
/// </summary>
public static class PipelineRasterizationStateCreateInfoExt
{
    /// <summary>
    /// Constructs a structure specifying parameters of a newly created pipeline rasterization state
    /// </summary>
    /// <param name="cullMode">Triangle facing direction used for primitive culling</param>
    /// <param name="polygonMode">Triangle rendering mode</param>
    /// <param name="frontFace">Specifying the front-facing triangle orientation to be used for culling</param>
    /// <param name="depthClampEnable">Whether to clamp the fragment’s depth values as described in Depth Test</param>
    /// <returns>new PipelineRasterizationStateCreateInfo</returns>
    public static PipelineRasterizationStateCreateInfo Create(
        CullModeFlags cullMode,
        PolygonMode polygonMode,
        FrontFace frontFace,
        bool depthClampEnable)
    {
        return new PipelineRasterizationStateCreateInfo
        {
            SType = StructureType.PipelineRasterizationStateCreateInfo,
            CullMode = cullMode,
            PolygonMode = polygonMode,
            FrontFace = frontFace,
            DepthClampEnable = depthClampEnable,
            LineWidth = 1f,
        };
    }

    /// <summary>
    /// Describes the default rasterizer state, with counter clockwise backface culling, solid polygon filling, and both depth
    /// clipping and scissor tests enabled.
    /// Settings:
    ///     CullMode = <see cref="CullModeFlags.BackBit"/>
    ///     FillMode = <see cref="PolygonMode.Fill"/>
    ///     FrontFace = <see cref="FrontFace.Clockwise"/>
    ///     DepthClampEnable = false
    /// </summary>
    public static readonly PipelineRasterizationStateCreateInfo Default = new PipelineRasterizationStateCreateInfo
    {
        SType = StructureType.PipelineRasterizationStateCreateInfo,
        CullMode = CullModeFlags.BackBit,
        PolygonMode = PolygonMode.Fill,
        FrontFace = FrontFace.CounterClockwise,
        DepthClampEnable = false,
        LineWidth = 1,
    };

    /// <summary>
    /// Describes the default rasterizer state, with clockwise backface culling, solid polygon filling, and both depth
    /// clipping and scissor tests enabled.
    /// Settings:
    ///     CullMode = <see cref="CullModeFlags.BackBit"/>
    ///     FillMode = <see cref="PolygonMode.Fill"/>
    ///     FrontFace = <see cref="FrontFace.Clockwise"/>
    ///     DepthClampEnable = false
    /// </summary>
    public static readonly PipelineRasterizationStateCreateInfo DefaultClockwise = new PipelineRasterizationStateCreateInfo
    {
        SType = StructureType.PipelineRasterizationStateCreateInfo,
        CullMode = CullModeFlags.BackBit,
        PolygonMode = PolygonMode.Fill,
        FrontFace = FrontFace.Clockwise,
        DepthClampEnable = false,
        LineWidth = 1,
    };

    /// <summary>
    /// Describes a rasterizer state with no culling, solid polygon filling, and both depth
    /// clipping and scissor tests enabled.
    /// Settings:
    ///     CullMode = <see cref="CullModeFlags.None"/>
    ///     FillMode = <see cref="PolygonMode.Fill"/>
    ///     FrontFace = <see cref="FrontFace.Clockwise"/>
    ///     DepthClampEnable = false
    /// </summary>
    public static readonly PipelineRasterizationStateCreateInfo CullNone = new PipelineRasterizationStateCreateInfo
    {
        SType = StructureType.PipelineRasterizationStateCreateInfo,
        CullMode = CullModeFlags.None,
        PolygonMode = PolygonMode.Fill,
        FrontFace = FrontFace.CounterClockwise,
        DepthClampEnable = false,
        LineWidth = 1,
    };
}

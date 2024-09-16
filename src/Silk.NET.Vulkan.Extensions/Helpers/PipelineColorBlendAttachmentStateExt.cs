using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.Helpers;

namespace Silk.NET.Vulkan.Extensions.Helpers;

/// <summary>
/// Structure specifying a <see cref="Pipeline"/> color blend attachment state
/// </summary>
public static class PipelineColorBlendAttachmentStateExt
{
    /// <summary>
    /// Constructs a new <see cref="PipelineColorBlendAttachmentState"/>.
    /// </summary>
    /// <param name="blendEnabled">Is blending is enabled for the corresponding color attachment</param>
    /// <param name="sourceColorFactor">Blend factor is used to determine the source factors</param>
    /// <param name="destinationColorFactor">Blend factor is used to determine the destination factors</param>
    /// <param name="colorFunction">Blend operation is used to calculate the RGB values to write to the color attachment</param>
    /// <param name="sourceAlphaFactor">Blend factor is used to determine the source factor</param>
    /// <param name="destinationAlphaFactor">Blend factor is used to determine the destination factor</param>
    /// <param name="alphaFunction">Blend operation is used to calculate the alpha values to write to the color attachment</param>
    /// <returns>new PipelineColorBlendAttachmentState</returns>
    public static PipelineColorBlendAttachmentState Create(
        bool blendEnabled,
        BlendFactor sourceColorFactor,
        BlendFactor destinationColorFactor,
        BlendOp colorFunction,
        BlendFactor sourceAlphaFactor,
        BlendFactor destinationAlphaFactor,
        BlendOp alphaFunction)
    {
        return new PipelineColorBlendAttachmentState
        {
            BlendEnable = blendEnabled,
            SrcColorBlendFactor = sourceColorFactor,
            DstColorBlendFactor = destinationColorFactor,
            ColorBlendOp = colorFunction,
            SrcAlphaBlendFactor = sourceAlphaFactor,
            DstAlphaBlendFactor = destinationAlphaFactor,
            AlphaBlendOp = alphaFunction,
            ColorWriteMask = ColorComponentFlagsExt.All,
        };
    }

    /// <summary>
    /// Constructs a new <see cref="PipelineColorBlendAttachmentState"/>.
    /// </summary>
    /// <param name="blendEnabled">Is blending is enabled for the corresponding color attachment</param>
    /// <param name="sourceColorFactor">Blend factor is used to determine the source factors</param>
    /// <param name="destinationColorFactor">Blend factor is used to determine the destination factors</param>
    /// <param name="colorFunction">Blend operation is used to calculate the RGB values to write to the color attachment</param>
    /// <param name="sourceAlphaFactor">Blend factor is used to determine the source factor</param>
    /// <param name="destinationAlphaFactor">Blend factor is used to determine the destination factor</param>
    /// <param name="alphaFunction">Blend operation is used to calculate the alpha values to write to the color attachment</param>
    /// <param name="colorWriteMask">Bitmask of VkColorComponentFlagBits specifying which of the R, G, B, and/or A components are enabled for writing, as described for the Color Write Mask</param>
    /// <returns>new PipelineColorBlendAttachmentState</returns>
    public static PipelineColorBlendAttachmentState Create(
        bool blendEnabled,
        BlendFactor sourceColorFactor,
        BlendFactor destinationColorFactor,
        BlendOp colorFunction,
        BlendFactor sourceAlphaFactor,
        BlendFactor destinationAlphaFactor,
        BlendOp alphaFunction,
        ColorComponentFlags colorWriteMask)
    {
        return new PipelineColorBlendAttachmentState
        {
            BlendEnable = blendEnabled,
            SrcColorBlendFactor = sourceColorFactor,
            DstColorBlendFactor = destinationColorFactor,
            ColorBlendOp = colorFunction,
            SrcAlphaBlendFactor = sourceAlphaFactor,
            DstAlphaBlendFactor = destinationAlphaFactor,
            AlphaBlendOp = alphaFunction,
            ColorWriteMask = colorWriteMask,
        };
    }

    /// <summary>
    /// Default Opaque blending (completely overrides the destination)
    /// </summary>
    public static readonly PipelineColorBlendAttachmentState OverrideBlend = new PipelineColorBlendAttachmentState
    {
        BlendEnable = true,
        SrcColorBlendFactor = BlendFactor.One,
        DstColorBlendFactor = BlendFactor.Zero,
        ColorBlendOp = BlendOp.Add,
        SrcAlphaBlendFactor = BlendFactor.One,
        DstAlphaBlendFactor = BlendFactor.Zero,
        AlphaBlendOp = BlendOp.Add,
        ColorWriteMask = ColorComponentFlagsExt.All,
    };

    /// <summary>
    /// Traditional transparency blending
    /// </summary>
    public static readonly PipelineColorBlendAttachmentState AlphaBlend = new PipelineColorBlendAttachmentState
    {
        BlendEnable = true,
        SrcColorBlendFactor = BlendFactor.SrcAlpha,
        DstColorBlendFactor = BlendFactor.OneMinusSrcAlpha,
        ColorBlendOp = BlendOp.Add,
        SrcAlphaBlendFactor = BlendFactor.SrcAlpha,
        DstAlphaBlendFactor = BlendFactor.OneMinusSrcAlpha,
        AlphaBlendOp = BlendOp.Add,
        ColorWriteMask = ColorComponentFlagsExt.All,
    };

    /// <summary>
    /// Additive blending
    /// </summary>
    public static readonly PipelineColorBlendAttachmentState AdditiveBlend = new PipelineColorBlendAttachmentState
    {
        BlendEnable = true,
        SrcColorBlendFactor = BlendFactor.SrcAlpha,
        DstColorBlendFactor = BlendFactor.One,
        ColorBlendOp = BlendOp.Add,
        SrcAlphaBlendFactor = BlendFactor.SrcAlpha,
        DstAlphaBlendFactor = BlendFactor.One,
        AlphaBlendOp = BlendOp.Add,
        ColorWriteMask = ColorComponentFlagsExt.All,
    };

    /// <summary>
    /// No blending
    /// </summary>
    public static readonly PipelineColorBlendAttachmentState Disabled = new PipelineColorBlendAttachmentState
    {
        BlendEnable = false,
        SrcColorBlendFactor = BlendFactor.One,
        DstColorBlendFactor = BlendFactor.Zero,
        ColorBlendOp = BlendOp.Add,
        SrcAlphaBlendFactor = BlendFactor.One,
        DstAlphaBlendFactor = BlendFactor.Zero,
        AlphaBlendOp = BlendOp.Add,
        ColorWriteMask = ColorComponentFlagsExt.All,
    };
}

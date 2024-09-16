using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.Helpers;

namespace Istok.Rendering;

/// <summary>
/// A structure describing different params of blending for <see cref="Pipeline"/>
/// </summary>
public struct BlendStateDescription : IEquatable<BlendStateDescription>
{
    /// <summary>
    /// The blend constant that are used in blending, depending on the blend factor
    /// </summary>
    public Color BlendFactor;
    /// <summary>
    /// Defining blend state for each color attachment
    /// </summary>
    public PipelineColorBlendAttachmentState[] AttachmentStates;
    /// <summary>
    /// Controls whether a temporary coverage value is generated based on the alpha component of the fragment’s first color output as specified in the Multisample Coverage section.
    /// </summary>
    public bool AlphaToCoverageEnabled;

    /// <summary>
    /// Constructs a new <see cref="BlendStateDescription"/>
    /// </summary>
    /// <param name="blendFactor">The blend constant</param>
    /// <param name="attachmentStates">The blend state for each color attachment</param>
    public BlendStateDescription(Color blendFactor, params PipelineColorBlendAttachmentState[] attachmentStates)
    {
        BlendFactor = blendFactor;
        AttachmentStates = attachmentStates;
        AlphaToCoverageEnabled = false;
    }

    /// <summary>
    /// Constructs a new <see cref="BlendStateDescription"/>
    /// </summary>
    /// <param name="blendFactor">The blend constant</param>
    /// <param name="alphaToCoverageEnabled">Enables alpha-to-coverage</param>
    /// <param name="attachmentStates">The blend state for each color attachment</param>
    public BlendStateDescription(
        Color blendFactor,
        bool alphaToCoverageEnabled,
        params PipelineColorBlendAttachmentState[] attachmentStates)
    {
        BlendFactor = blendFactor;
        AttachmentStates = attachmentStates;
        AlphaToCoverageEnabled = alphaToCoverageEnabled;
    }

    public static readonly BlendStateDescription SingleOverrideBlend = new BlendStateDescription
    {
        AttachmentStates = [PipelineColorBlendAttachmentStateExt.OverrideBlend],
    };

    public static readonly BlendStateDescription SingleAlphaBlend = new BlendStateDescription
    {
        AttachmentStates = [PipelineColorBlendAttachmentStateExt.AlphaBlend],
    };

    public static readonly BlendStateDescription SingleAdditiveBlend = new BlendStateDescription
    {
        AttachmentStates = [PipelineColorBlendAttachmentStateExt.AdditiveBlend],
    };

    public static readonly BlendStateDescription SingleDisabled = new BlendStateDescription
    {
        AttachmentStates = [PipelineColorBlendAttachmentStateExt.Disabled],
    };

    public static readonly BlendStateDescription Empty = new BlendStateDescription
    {
        AttachmentStates = [],
    };

    public bool Equals(BlendStateDescription other)
    {
        return BlendFactor.Equals(other.BlendFactor)
               && AlphaToCoverageEnabled.Equals(other.AlphaToCoverageEnabled)
               && AttachmentStates.AsSpan().SequenceEqual(other.AttachmentStates);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(BlendFactor, AlphaToCoverageEnabled, HashCodeExt.Combine(AttachmentStates));
    }
}

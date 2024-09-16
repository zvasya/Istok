using Silk.NET.Vulkan;

namespace Istok.Rendering;
#nullable enable

public unsafe class RenderPass : IDisposable
{
    public AttachmentDescription? DepthTarget { get; }
    public IReadOnlyList<AttachmentDescription> ColorTargets { get; }

    readonly LogicalDevice _logicalDevice;
    readonly Silk.NET.Vulkan.RenderPass _renderPass;
    public Silk.NET.Vulkan.RenderPass DeviceRenderPass => _renderPass;

    public readonly SampleCountFlags SampleCount;

    public uint AttachmentCount { get; }

    RenderPass(LogicalDevice logicalDevice, Silk.NET.Vulkan.RenderPass renderPass, IReadOnlyList<AttachmentDescription> colorTargets, AttachmentDescription? depthTarget)
    {
        _logicalDevice = logicalDevice;
        _renderPass = renderPass;
        ColorTargets = colorTargets;
        DepthTarget = depthTarget;
        AttachmentCount = (uint)colorTargets.Count + (depthTarget != null ? 1u : 0u);

        SampleCountFlags maxSampleCount = depthTarget?.Samples ?? SampleCountFlags.Count1Bit;
        foreach (AttachmentDescription attachmentDescription in ColorTargets)
        {
            if (attachmentDescription.Samples > maxSampleCount)
                maxSampleCount = attachmentDescription.Samples;
        }

        SampleCount = maxSampleCount;
    }

    public static RenderPass Create(LogicalDevice logicalDevice, IReadOnlyList<AttachmentDescription> colorTargets, AttachmentDescription? depthTarget)
    {
        int colorAttachmentCount = colorTargets.Count;
        int totalAttachmentsCount = colorAttachmentCount + (depthTarget != null ? 1 : 0);


        AttachmentDescription* attachments = stackalloc AttachmentDescription[totalAttachmentsCount];
        AttachmentReference* colorAttachmentRefs = stackalloc AttachmentReference[colorAttachmentCount];

        for (int i = 0; i < colorAttachmentCount; i++)
        {
            attachments[i] = colorTargets[i];
            colorAttachmentRefs[i]= new AttachmentReference((uint)i, ImageLayout.ColorAttachmentOptimal);
        }

        AttachmentReference depthAttachmentRef = new AttachmentReference();
        if (depthTarget != null)
        {
            attachments[colorAttachmentCount] = depthTarget.Value;
            depthAttachmentRef = new AttachmentReference((uint)colorTargets.Count, ImageLayout.DepthStencilAttachmentOptimal);
        }

        SubpassDescription subpass = new SubpassDescription
        {
            PipelineBindPoint = PipelineBindPoint.Graphics,
            ColorAttachmentCount = (uint)colorAttachmentCount,
            PColorAttachments = colorAttachmentRefs,
            PDepthStencilAttachment = depthTarget != null ? &depthAttachmentRef : default,
        };

        SubpassDependency subpassDependency = new SubpassDependency
        {
            SrcSubpass = Vk.SubpassExternal,
            SrcStageMask = PipelineStageFlags.ColorAttachmentOutputBit,
            DstStageMask = PipelineStageFlags.ColorAttachmentOutputBit,
            DstAccessMask = AccessFlags.ColorAttachmentReadBit | AccessFlags.ColorAttachmentWriteBit,
        };

        RenderPassCreateInfo renderPassCI = new RenderPassCreateInfo
        {
            SType = StructureType.RenderPassCreateInfo,
            AttachmentCount = (uint)totalAttachmentsCount,
            PAttachments = attachments,
            SubpassCount = 1,
            PSubpasses = &subpass,
            DependencyCount = 1,
            PDependencies = &subpassDependency,
        };

        Result creationResult = logicalDevice.CreateRenderPass(in renderPassCI, null, out Silk.NET.Vulkan.RenderPass renderPass);
        Helpers.CheckErrors(creationResult);
        return new RenderPass(logicalDevice, renderPass, colorTargets, depthTarget);
    }

    public bool IsDisposed { get; private set; }

    public void Dispose()
    {
        if (!IsDisposed)
        {
            _logicalDevice.DestroyRenderPass(_renderPass, null);
            IsDisposed = true;
        }
    }
}

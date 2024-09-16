using System.Diagnostics;
using Silk.NET.Vulkan;
using VkImageView = Silk.NET.Vulkan.ImageView;

namespace Istok.Rendering;

public sealed unsafe class Framebuffer : IDisposable
{
    readonly List<ImageView> _attachmentViews = [];
    readonly Silk.NET.Vulkan.Framebuffer _deviceFramebuffer;
    readonly LogicalDevice _logicalDevice;
    public RenderPass RenderPass { get; }

    string _name;

    public Framebuffer(LogicalDevice logicalDevice, in FramebufferDescription description, RenderPass renderPass)
    {
        DepthTarget = description.DepthTarget;
        ImageView[] colorTargets = description.ColorTargets.ToArray();

        ColorTargets = colorTargets;
        RenderPass = renderPass;
        ImageView imageView = ColorTargets.FirstOrDefault() ?? DepthTarget;
        Debug.Assert(imageView != null);

        imageView.Target.GetMipDimensions(imageView.BaseMipLevel, out uint mipWidth, out uint mipHeight, out _);
        Width = mipWidth;
        Height = mipHeight;

        _logicalDevice = logicalDevice;

        uint attachmentsCount = (uint)ColorTargets.Count + (description.DepthTarget != null ? 1u : 0u);
        AttachmentCount = attachmentsCount;
        VkImageView* attachments = stackalloc VkImageView[(int)attachmentsCount];
        for (int i = 0; i < ColorTargets.Count; i++)
        {
            ImageView dest = ColorTargets[i];
            attachments[i] = dest.DeviceImageView;
            _attachmentViews.Add(dest);
        }

        if (description.DepthTarget != null)
        {
            ImageView dest = description.DepthTarget;
            attachments[attachmentsCount - 1] = dest.DeviceImageView;
            _attachmentViews.Add(dest);
        }

        FramebufferCreateInfo framebufferCreateInfo = new FramebufferCreateInfo
        {
            SType = StructureType.FramebufferCreateInfo,
            Width = mipWidth,
            Height = mipHeight,
            AttachmentCount = attachmentsCount,
            PAttachments = attachments,
            Layers = 1,
            RenderPass = renderPass.DeviceRenderPass,
        };

        Result creationResult = _logicalDevice.CreateFramebuffer(in framebufferCreateInfo, null, out _deviceFramebuffer);
        Helpers.CheckErrors(creationResult);
    }

    public ImageView? DepthTarget { get; }

    public IReadOnlyList<ImageView> ColorTargets { get; }

    public uint Width { get; }

    public uint Height { get; }

    public Silk.NET.Vulkan.Framebuffer CurrentFramebuffer => _deviceFramebuffer;
    public uint AttachmentCount { get; }

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            _logicalDevice.SetObjectName(ObjectType.Framebuffer, CurrentFramebuffer.Handle, value);
        }
    }

    public bool IsDisposed { get; private set; }


    public void Dispose()
    {
        if (!IsDisposed)
        {
            _logicalDevice.DestroyFramebuffer(_deviceFramebuffer, null);
            foreach (ImageView view in _attachmentViews)
            {
                view.Dispose();
            }
            DepthTarget?.Dispose();
            foreach (ImageView colorTarget in ColorTargets)
            {
                colorTarget.Dispose();
            }

            IsDisposed = true;
        }
    }

    public void TransitionToIntermediateLayout()
    {
        foreach (ImageView imageView in ColorTargets)
        {
            imageView.Target.SetImageLayout(imageView.BaseMipLevel, imageView.BaseArrayLayer, ImageLayout.ColorAttachmentOptimal);
        }

        DepthTarget?.Target.SetImageLayout(
            DepthTarget.BaseMipLevel,
            DepthTarget.BaseArrayLayer,
            ImageLayout.DepthStencilAttachmentOptimal);
    }

    public void TransitionToFinalLayout(CommandBuffer commandBuffer)
    {
        foreach (ImageView imageView in ColorTargets)
        {
            Image image = imageView.Target;
            if (image.Usage.HasFlag(ImageUsageFlags.SampledBit) || image.IsSwapchainImage)
            {
                image.TransitionImageLayout(
                    commandBuffer,
                    imageView.BaseMipLevel, 1,
                    imageView.BaseArrayLayer, 1,
                    image.IsSwapchainImage ? ImageLayout.PresentSrcKhr : ImageLayout.ShaderReadOnlyOptimal);
            }
        }

        if (DepthTarget != null)
        {
            Image image = DepthTarget.Target;
            if (image.Usage.HasFlag(ImageUsageFlags.SampledBit))
            {
                image.TransitionImageLayout(
                    commandBuffer,
                    DepthTarget.BaseMipLevel, 1,
                    DepthTarget.BaseArrayLayer, 1,
                    ImageLayout.ShaderReadOnlyOptimal);
            }
        }
    }
}

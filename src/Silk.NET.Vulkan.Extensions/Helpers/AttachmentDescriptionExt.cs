namespace Silk.NET.Vulkan.Extensions.Helpers;

public static class AttachmentDescriptionExt
{
    public static AttachmentDescription CreateColor(
        Format format,
        SampleCountFlags samples = SampleCountFlags.Count1Bit,
        AttachmentLoadOp loadOp = AttachmentLoadOp.Clear,
        AttachmentStoreOp storeOp = AttachmentStoreOp.Store,
        AttachmentLoadOp stencilLoadOp = AttachmentLoadOp.DontCare,
        AttachmentStoreOp stencilStoreOp = AttachmentStoreOp.DontCare,
        ImageLayout initialLayout = ImageLayout.Undefined,
        ImageLayout finalLayout = ImageLayout.ColorAttachmentOptimal
    )
    {
        return new AttachmentDescription
        {
            Format = format,
            Samples = samples,
            LoadOp = loadOp,
            StoreOp = storeOp,
            StencilLoadOp = stencilLoadOp,
            StencilStoreOp = stencilStoreOp,
            InitialLayout = initialLayout,
            FinalLayout = finalLayout,
        };
    }

    public static AttachmentDescription CreateColorSwapchain(
        Format format,
        SampleCountFlags samples = SampleCountFlags.Count1Bit,
        AttachmentLoadOp loadOp = AttachmentLoadOp.Clear,
        AttachmentStoreOp storeOp = AttachmentStoreOp.Store,
        AttachmentLoadOp stencilLoadOp = AttachmentLoadOp.DontCare,
        AttachmentStoreOp stencilStoreOp = AttachmentStoreOp.DontCare,
        ImageLayout finalLayout = ImageLayout.ColorAttachmentOptimal
    ) => CreateColor(format, samples, loadOp, storeOp, stencilLoadOp, stencilStoreOp, ImageLayout.PresentSrcKhr, finalLayout);

    public static AttachmentDescription CreateColorSampled(
        Format format,
        SampleCountFlags samples = SampleCountFlags.Count1Bit,
        AttachmentLoadOp loadOp = AttachmentLoadOp.Clear,
        AttachmentStoreOp storeOp = AttachmentStoreOp.Store,
        AttachmentLoadOp stencilLoadOp = AttachmentLoadOp.DontCare,
        AttachmentStoreOp stencilStoreOp = AttachmentStoreOp.DontCare,
        ImageLayout finalLayout = ImageLayout.ColorAttachmentOptimal
    ) => CreateColor(format, samples, loadOp, storeOp, stencilLoadOp, stencilStoreOp, ImageLayout.ShaderReadOnlyOptimal, finalLayout);

    public static AttachmentDescription CreateColorOptimal(
        Format format,
        SampleCountFlags samples = SampleCountFlags.Count1Bit,
        AttachmentLoadOp loadOp = AttachmentLoadOp.Clear,
        AttachmentStoreOp storeOp = AttachmentStoreOp.Store,
        AttachmentLoadOp stencilLoadOp = AttachmentLoadOp.DontCare,
        AttachmentStoreOp stencilStoreOp = AttachmentStoreOp.DontCare,
        ImageLayout finalLayout = ImageLayout.ColorAttachmentOptimal
    ) => CreateColor(format, samples, loadOp, storeOp, stencilLoadOp, stencilStoreOp, ImageLayout.ColorAttachmentOptimal, finalLayout);


    public static AttachmentDescription CreateDepth(
        Format format,
        SampleCountFlags samples = SampleCountFlags.Count1Bit,
        AttachmentLoadOp loadOp = AttachmentLoadOp.Clear,
        AttachmentStoreOp storeOp = AttachmentStoreOp.Store,
        AttachmentLoadOp stencilLoadOp = AttachmentLoadOp.Clear,
        AttachmentStoreOp? stencilStoreOp = null,
        ImageLayout initialLayout = ImageLayout.Undefined,
        ImageLayout finalLayout = ImageLayout.DepthStencilAttachmentOptimal
    )
    {
        return new AttachmentDescription
        {
            Format = format,
            Samples = samples,
            LoadOp = loadOp,
            StoreOp = storeOp,
            StencilLoadOp = stencilLoadOp,
            StencilStoreOp = stencilStoreOp
                             ?? (format.HasStencil()
                                 ? AttachmentStoreOp.Store
                                 : AttachmentStoreOp.DontCare),
            InitialLayout = initialLayout,
            FinalLayout = finalLayout,
        };
    }
}

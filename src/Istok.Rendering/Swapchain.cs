using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.Helpers;
using Silk.NET.Windowing;
using Semaphore = Silk.NET.Vulkan.Semaphore;

namespace Istok.Rendering;

public unsafe class Swapchain : IDisposable
{
    readonly bool _colorSrgb;
    readonly SwapchainFramebuffer _framebuffer;
    readonly Queue _graphicsQueue;
    readonly LogicalDevice _logicalDevice;
    readonly Queue _presentQueue;
    readonly SurfaceKHR _surface;
    readonly RenderPass _renderPass;
    uint _currentImageIndex;
    SwapchainKHR _deviceSwapchain;
    string _name;
    bool? _newSyncToVBlank;
    bool _syncToVBlank;

    public Swapchain(LogicalDevice logicalDevice, StagingBuffersPool stagingBuffersPool, ThreadLocal<CommandPool> commandPoolsStorage, Queue graphicsQueue, Queue presentQueue, ref Description description, SurfaceKHR existingSurface)
    {
        _logicalDevice = logicalDevice;
        _syncToVBlank = description.SyncToVerticalBlank;
        _colorSrgb = description.ColorSrgb;
        _surface = existingSurface;

        _presentQueue = presentQueue;
        _graphicsQueue = graphicsQueue;

        AttachmentDescription colorAttachmentDescription = AttachmentDescriptionExt.CreateColorSwapchain(SelectSurfaceFormatKhr().Format, loadOp: AttachmentLoadOp.Load);

        AttachmentDescription? depthAttachmentDescription = !description.DepthFormat.HasValue
            ? null
            : AttachmentDescriptionExt.CreateDepth(description.DepthFormat.Value, loadOp: AttachmentLoadOp.Load, stencilLoadOp: AttachmentLoadOp.DontCare, initialLayout: ImageLayout.DepthStencilAttachmentOptimal);

        _renderPass = RenderPass.Create(_logicalDevice, [colorAttachmentDescription], depthAttachmentDescription);
        _framebuffer = new SwapchainFramebuffer(_logicalDevice, stagingBuffersPool, commandPoolsStorage, this, _surface, _renderPass);
        ImageAvailableFence = Fence.Create(_logicalDevice, false);

        CreateSwapchain();

        AcquireNextImage(new Semaphore(), ImageAvailableFence);
        ImageAvailableFence.Wait();
        ImageAvailableFence.Reset();
    }

    public SwapchainKHR DeviceSwapchain => _deviceSwapchain;
    public uint ImageIndex => _currentImageIndex;
    public Fence ImageAvailableFence { get; }
    public Queue PresentQueue => _presentQueue;

    public SwapchainFramebuffer Framebuffer => _framebuffer;

    public bool SyncToVerticalBlank
    {
        get => _newSyncToVBlank ?? _syncToVBlank;
        set
        {
            if (_syncToVBlank != value)
            {
                _newSyncToVBlank = value;
            }
        }
    }

    public string Name
    {
        get =>
            _name;
        set
        {
            _name = value;
            _logicalDevice.SetObjectName(ObjectType.SwapchainKhr, DeviceSwapchain.Handle, value);
        }
    }

    public bool IsDisposed { get; private set; }

    public void Dispose()
    {
        if (!IsDisposed)
        {
            ImageAvailableFence.Dispose();
            _framebuffer.Dispose();
            _renderPass.Dispose();
            _logicalDevice.DestroySwapchain(_deviceSwapchain, null);
            _logicalDevice.Engine.DestroySurface(_surface, null);
        }

        IsDisposed = true;
    }

    public bool AcquireNextImage(Semaphore semaphore, Fence fence)
    {
        if (_newSyncToVBlank != null)
        {
            _syncToVBlank = _newSyncToVBlank.Value;
            _newSyncToVBlank = null;
            RecreateAndReacquire();
            return false;
        }

        Result result = _logicalDevice.AcquireNextImage(
            _deviceSwapchain,
            ulong.MaxValue,
            semaphore,
            fence.DeviceFence,
            ref _currentImageIndex);
        _framebuffer.SetImageIndex(_currentImageIndex);
        if (result is Result.ErrorOutOfDateKhr or Result.SuboptimalKhr)
        {
            fence.Wait();
            fence.Reset();
            RecreateAndReacquire();

            return false;
        }
        else if (result != Result.Success)
        {
            throw new Exception("Could not acquire next image from the Vulkan swapchain.");
        }

        return true;
    }

    public void RecreateAndReacquire()
    {
        if (CreateSwapchain())
        {
            if (AcquireNextImage(new Semaphore(), ImageAvailableFence))
            {
                ImageAvailableFence.Wait();
                ImageAvailableFence.Reset();
            }
        }
    }

    bool CreateSwapchain()
    {
        SurfaceCapabilitiesKHR surfaceCapabilities = _logicalDevice.PhysicalDevice.GetSurfaceCapabilities(_surface);

        _currentImageIndex = 0;

        SurfaceFormatKHR surfaceFormat = SelectSurfaceFormatKhr();
        PresentModeKHR presentMode = SelectPresentModeKhr();

        uint maxImageCount = surfaceCapabilities.MaxImageCount == 0 ? uint.MaxValue : surfaceCapabilities.MaxImageCount;
        uint imageCount = Math.Min(maxImageCount, surfaceCapabilities.MinImageCount + 1);

        uint width = surfaceCapabilities.CurrentExtent.Width;
        uint height = surfaceCapabilities.CurrentExtent.Height;

        SwapchainKHR oldSwapchain = _deviceSwapchain;

        SwapchainCreateInfoKHR swapchainCI = new SwapchainCreateInfoKHR
        {
            SType = StructureType.SwapchainCreateInfoKhr,
            Surface = _surface,
            PresentMode = presentMode,
            ImageFormat = surfaceFormat.Format,
            ImageColorSpace = surfaceFormat.ColorSpace,
            ImageExtent = new Extent2D { Width = width, Height = height },
            MinImageCount = imageCount,
            ImageArrayLayers = 1,
            ImageUsage = ImageUsageFlags.ColorAttachmentBit | ImageUsageFlags.TransferDstBit,
            PreTransform = SurfaceTransformFlagsKHR.IdentityBitKhr,
            CompositeAlpha = CompositeAlphaFlagsKHR.OpaqueBitKhr,
            Clipped = true,
            OldSwapchain = oldSwapchain,
        };

        uint* queueFamilyIndices = stackalloc uint[] { _graphicsQueue.FamilyIndex, _presentQueue.FamilyIndex };
        if (_graphicsQueue.FamilyIndex != _presentQueue.FamilyIndex)
        {
            swapchainCI.ImageSharingMode = SharingMode.Concurrent;
            swapchainCI.QueueFamilyIndexCount = 2;
            swapchainCI.PQueueFamilyIndices = queueFamilyIndices;
        }
        else
        {
            swapchainCI.ImageSharingMode = SharingMode.Exclusive;
            swapchainCI.QueueFamilyIndexCount = 0; //Optional
            swapchainCI.PQueueFamilyIndices = null; //Optional
        }

        Result result = _logicalDevice.CreateSwapchain(in swapchainCI, null, out _deviceSwapchain);
        Helpers.CheckErrors(result);
        if (oldSwapchain.Handle != default)
            _logicalDevice.DestroySwapchain(oldSwapchain, null);

        _framebuffer.SetNewSwapchain(_deviceSwapchain, width, height, surfaceFormat, swapchainCI.ImageExtent);
        return true;
    }

    PresentModeKHR SelectPresentModeKhr()
    {
        PresentModeKHR[] presentModes = _logicalDevice.PhysicalDevice.GetSurfacePresentModes(_surface);

        PresentModeKHR presentMode = PresentModeKHR.FifoKhr;

        if (_syncToVBlank)
        {
            if (presentModes.Contains(PresentModeKHR.FifoRelaxedKhr))
                presentMode = PresentModeKHR.FifoRelaxedKhr;
        }
        else
        {
            if (presentModes.Contains(PresentModeKHR.MailboxKhr))
                presentMode = PresentModeKHR.MailboxKhr;
            else if (presentModes.Contains(PresentModeKHR.ImmediateKhr))
                presentMode = PresentModeKHR.ImmediateKhr;
        }

        return presentMode;
    }

    SurfaceFormatKHR SelectSurfaceFormatKhr()
    {
        SurfaceFormatKHR[] formats = _logicalDevice.PhysicalDevice.GetSurfaceFormats(_surface);

        Format desiredFormat = _colorSrgb
            ? Format.B8G8R8A8Srgb
            : Format.B8G8R8A8Unorm;

        SurfaceFormatKHR surfaceFormat = formats.FirstOrDefault(format => format.ColorSpace == ColorSpaceKHR.SpaceSrgbNonlinearKhr && format.Format == desiredFormat);
        if (surfaceFormat.Format == Format.Undefined)
        {
            surfaceFormat = formats.Length == 1 && formats[0].Format == Format.Undefined
                ? new SurfaceFormatKHR { ColorSpace = ColorSpaceKHR.SpaceSrgbNonlinearKhr, Format = desiredFormat }
                : formats[0];
        }

        return surfaceFormat;
    }

    public readonly record struct Description
    (
        IView Source,
        Format? DepthFormat,
        bool SyncToVerticalBlank,
        bool ColorSrgb = false
    );
}

using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.Helpers;

namespace Istok.Rendering;

public class SwapchainFramebuffer : IDisposable
{
    readonly ThreadLocal<CommandPool> _commandPoolsStorage;
    readonly LogicalDevice _logicalDevice;
    readonly RenderPass _renderPass;
    readonly StagingBuffersPool _stagingBuffersPool;
    readonly SurfaceKHR _surface;
    uint _currentImageIndex;
    Image? _depthTarget;

    Image[][] _scColorTextures;
    Extent2D _scExtent;
    Framebuffer[] _scFramebuffers;
    Format _scImageFormat;
    Silk.NET.Vulkan.Image[] _scImages = [];

    public SwapchainFramebuffer(
        LogicalDevice logicalDevice,
        StagingBuffersPool stagingBuffersPool,
        ThreadLocal<CommandPool> commandPoolsStorage,
        Swapchain swapchain,
        SurfaceKHR surface,
        RenderPass renderPass)
    {
        if (!(renderPass.AttachmentCount == 1 || (renderPass.AttachmentCount == 2 && renderPass.DepthTarget.HasValue)))
        {
            throw new Exception("For now supports only 1 Color or 1 Color + 1 Depth ");
        }

        _logicalDevice = logicalDevice;
        _stagingBuffersPool = stagingBuffersPool;
        _commandPoolsStorage = commandPoolsStorage;
        Swapchain = swapchain;
        _surface = surface;
        _renderPass = renderPass;

        AttachmentCount = _renderPass.AttachmentCount;
    }

    public RenderPass RenderPass => _renderPass;

    public Framebuffer CurrentFramebuffer => _scFramebuffers[(int)_currentImageIndex];

    public uint Width { get; private set; }

    public uint Height { get; private set; }

    public uint ImageIndex => _currentImageIndex;


    public uint AttachmentCount { get; }

    public Swapchain Swapchain { get; }

    public bool IsDisposed { get; private set; }

    public void Dispose()
    {
        if (!IsDisposed)
        {
            IsDisposed = true;
            _depthTarget?.Dispose();
            foreach (Image[] scColorTexture in _scColorTextures)
            foreach (Image colorTexture in scColorTexture)
                colorTexture.Dispose();
            DestroySwapchainFramebuffers();
        }
    }

    internal void SetImageIndex(uint index)
    {
        _currentImageIndex = index;
    }

    internal void SetNewSwapchain(
        SwapchainKHR deviceSwapchain,
        uint width,
        uint height,
        SurfaceFormatKHR surfaceFormat,
        Extent2D swapchainExtent)
    {
        Width = width;
        Height = height;

        _logicalDevice.GetSwapchainImages(deviceSwapchain, ref _scImages);

        _scImageFormat = surfaceFormat.Format;
        _scExtent = swapchainExtent;

        CreateDepthTexture();
        CreateFramebuffers();
    }

    void DestroySwapchainFramebuffers()
    {
        if (_scFramebuffers != null)
        {
            for (int i = 0; i < _scFramebuffers.Length; i++)
            {
                _scFramebuffers[i]?.Dispose();
            }
            Array.Clear(_scFramebuffers);
        }
    }

    void CreateDepthTexture()
    {
        if (_renderPass.DepthTarget.HasValue)
        {
            _depthTarget?.Dispose();
            ImageCreateInfo imageCreateInfo = ImageCreateInfoExt.Texture2D(
                Math.Max(1, _scExtent.Width),
                Math.Max(1, _scExtent.Height),
                1,
                1,
                _renderPass.DepthTarget.Value.Format,
                ImageUsageFlags.DepthStencilAttachmentBit | ImageUsageFlags.TransferDstBit | ImageUsageFlags.TransferSrcBit);
            Image depthImage = Image.Create(_logicalDevice, _stagingBuffersPool, _commandPoolsStorage, ref imageCreateInfo);

            _depthTarget = depthImage;
        }
    }

    void CreateFramebuffers()
    {
        DestroySwapchainFramebuffers();

        ArrayExtensions.EnsureArrayMinimumSize(ref _scFramebuffers, (uint)_scImages.Length);
        ArrayExtensions.EnsureArrayMinimumSize(ref _scColorTextures, (uint)_scImages.Length);
        for (uint i = 0; i < _scImages.Length; i++)
        {
            Image colorTex = Image.CreateSwapchain(_logicalDevice,
                _stagingBuffersPool,
                _commandPoolsStorage,
                new Extent3D(
                    Math.Max(1, _scExtent.Width),
                    Math.Max(1, _scExtent.Height),
                    1),
                1,
                1,
                _scImageFormat,
                ImageUsageFlags.ColorAttachmentBit | ImageUsageFlags.TransferDstBit | ImageUsageFlags.TransferSrcBit,
                ImageCreateFlags.CreateMutableFormatBit,
                SampleCountFlags.Count1Bit,
                _scImages[i]);
            FramebufferDescription desc = new FramebufferDescription(_depthTarget?.GetFullImageView(), colorTex?.GetFullImageView());
            Framebuffer fb = new Framebuffer(_logicalDevice, in desc, _renderPass);
            _scFramebuffers[i] = fb;
            _scColorTextures[i] = [ colorTex ];
        }
    }
}

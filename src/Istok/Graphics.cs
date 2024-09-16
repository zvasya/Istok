using System.Drawing;
using Istok.Rendering;
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.Helpers;
using CommandPool = Istok.Rendering.CommandPool;
using Fence = Istok.Rendering.Fence;
using PhysicalDevice = Istok.Rendering.PhysicalDevice;
using Queue = Istok.Rendering.Queue;
using Sampler = Istok.Rendering.Sampler;
using Semaphore = Silk.NET.Vulkan.Semaphore;

namespace Istok;

public unsafe partial class Graphics : IDisposable
{
    readonly Sampler? _samplerAnisotropy4x;

    public Graphics(IPlatform platform, Swapchain.Description? description)
    {
        Engine = Engine.Create(platform, "AppName");

        SurfaceKHR surface = default;
        if (description != null)
            surface = Engine.CreateSurface(description.Value.Source);

        PhysicalDevice? physicalDevice = Engine.GetPhysicalDevices().FirstOrDefault(physicalDevice2 => physicalDevice2.IsPhysicalDeviceSuitable(surface));

        if (physicalDevice == null)
        {
            Engine.Dispose();
            throw new Exception("Suitable PhysicalDevice not found");
        }

        PhysicalDevice = physicalDevice;

        GraphicsQueueIndex = PhysicalDevice.GraphicsQueueIndex;
        PresentQueueIndex = PhysicalDevice.GetPresentQueueIndex(surface);

        LogicalDevice = PhysicalDevice.CreateLogicalDevice(GraphicsQueueIndex, PresentQueueIndex, platform);

        GraphicsQueue = LogicalDevice.GetDeviceQueue(GraphicsQueueIndex, 0);
        PresentQueue = GraphicsQueueIndex == PresentQueueIndex
            ? GraphicsQueue
            : LogicalDevice.GetDeviceQueue(PresentQueueIndex, 0);

        FenceManager = new FenceManager(LogicalDevice);
        CommandPoolsStorage = new ThreadLocal<CommandPool>(() => new CommandPool(LogicalDevice, CommandPoolCreateFlags.TransientBit | CommandPoolCreateFlags.ResetCommandBufferBit, FenceManager, GraphicsQueue), true);
        StagingBuffersPool = new StagingBuffersPool(LogicalDevice, CommandPoolsStorage);

        if (description != null)
        {
            Swapchain.Description desc = description.Value;
            MainSwapchain = new Swapchain(LogicalDevice, StagingBuffersPool, CommandPoolsStorage, GraphicsQueue, PresentQueue, ref desc, surface);
        }

        DescriptorPoolManager = CreateDescriptorPool(LogicalDevice);

        PointSampler = new Sampler(LogicalDevice, SamplerCreateInfoExt.Point);
        LinearSampler = new Sampler(LogicalDevice,SamplerCreateInfoExt.Linear);
        _samplerAnisotropy4x = null;
        if (PhysicalDevice.Features.SamplerAnisotropy)
        {
            _samplerAnisotropy4x = new Sampler(LogicalDevice,SamplerCreateInfoExt.Aniso4X);
        }
    }

    public Engine Engine { get; }
    public FenceManager FenceManager { get; }
    public StagingBuffersPool StagingBuffersPool { get; }
    public ThreadLocal<CommandPool> CommandPoolsStorage { get; }
    public LogicalDevice LogicalDevice { get; }
    public PhysicalDevice PhysicalDevice { get; }
    public Queue GraphicsQueue { get; }
    public Queue PresentQueue { get; }
    public uint GraphicsQueueIndex { get; }
    public uint PresentQueueIndex { get; }
    public DescriptorPoolManager DescriptorPoolManager { get; }
    public Swapchain? MainSwapchain { get; }

    /// <summary>
    /// Gets a simple point-filtered <see cref="Sampler"/> object owned by this instance.
    /// This object is created with <see cref="Point"/>.
    /// </summary>
    public Sampler PointSampler { get; }

    /// <summary>
    /// Gets a simple linear-filtered <see cref="Sampler"/> object owned by this instance.
    /// This object is created with <see cref="SamplerCreateInfoExt.Linear"/>.
    /// </summary>
    public Sampler LinearSampler { get; }

    /// <summary>
    /// Gets a simple 4x anisotropic-filtered <see cref="Sampler"/> object owned by this instance.
    /// This object is created with <see cref="SamplerCreateInfoExt.Aniso4X"/>.
    /// This property can only be used when <see cref="Silk.NET.Vulkan.PhysicalDeviceFeatures.SamplerAnisotropy"/> is supported.
    /// </summary>
    public Sampler? SamplerAnisotropy4x
    {
        get
        {
            if (!PhysicalDevice.Features.SamplerAnisotropy)
                throw new Exception("GraphicsDevice.SamplerAnisotropy4x cannot be used unless PhysicalDeviceFeatures.SamplerAnisotropy is supported.");

            return _samplerAnisotropy4x;
        }
    }

    /// <summary>
    /// Gets or sets whether the main Swapchain's <see cref="SwapBuffers()"/> should be synchronized to the window system's
    /// vertical refresh rate.
    /// This is equivalent to <see cref="MainSwapchain"/>.<see cref="Swapchain.SyncToVerticalBlank"/>.
    /// This property cannot be set if this GraphicsDevice was created without a main Swapchain.
    /// </summary>
    public bool SyncToVerticalBlank
    {
        get => MainSwapchain?.SyncToVerticalBlank ?? false;
        set
        {
            if (MainSwapchain == null)
            {
                throw new Exception("This GraphicsDevice was created without a main Swapchain. This property cannot be set.");
            }

            MainSwapchain.SyncToVerticalBlank = value;
        }
    }

    public void Dispose()
    {
        WaitForIdle();
        PointSampler.Dispose();
        LinearSampler.Dispose();
        _samplerAnisotropy4x?.Dispose();
        FenceManager.Dispose();
        MainSwapchain?.Dispose();
        DescriptorPoolManager.DestroyAll();
        StagingBuffersPool.Dispose();

        foreach (CommandPool commandPool in CommandPoolsStorage.Values)
            commandPool.Dispose();

        CommandPoolsStorage.Dispose();

        Result result = LogicalDevice.DeviceWaitIdle();
        Helpers.CheckErrors(result);
        LogicalDevice.DestroyDevice(null);
        Engine.Dispose();
    }

    static DescriptorPoolManager CreateDescriptorPool(LogicalDevice logicalDevice)
    {
        const uint totalSets = 1000;
        const uint descriptorCount = 100;
        DescriptorPoolSize[] sizes =
        [
            new DescriptorPoolSize { Type = DescriptorType.UniformBuffer, DescriptorCount = descriptorCount },
            new DescriptorPoolSize { Type = DescriptorType.SampledImage, DescriptorCount = descriptorCount },
            new DescriptorPoolSize { Type = DescriptorType.Sampler, DescriptorCount = descriptorCount },
            new DescriptorPoolSize { Type = DescriptorType.StorageBuffer, DescriptorCount = descriptorCount },
            new DescriptorPoolSize { Type = DescriptorType.StorageImage, DescriptorCount = descriptorCount },
            new DescriptorPoolSize { Type = DescriptorType.UniformBufferDynamic, DescriptorCount = descriptorCount },
            new DescriptorPoolSize { Type = DescriptorType.StorageBufferDynamic, DescriptorCount = descriptorCount },
            new DescriptorPoolSize { Type = DescriptorType.CombinedImageSampler, DescriptorCount = descriptorCount },
        ];
        return new DescriptorPoolManager(logicalDevice, totalSets, sizes);
    }

    /// <summary>
    /// Swaps the buffers of the main swapchain and presents the rendered image to the screen.
    /// This is equivalent to passing <see cref="MainSwapchain"/> to <see cref="SwapBuffers(Swapchain)"/>.
    /// This method can only be called if this GraphicsDevice was created with a main Swapchain.
    /// </summary>
    public void SwapBuffers()
    {
        if (MainSwapchain == null)
        {
            throw new Exception("This GraphicsDevice was created without a main Swapchain, so the requested operation cannot be performed.");
        }

        SwapBuffers(MainSwapchain);
    }

    /// <summary>
    /// Swaps the buffers of the given swapchain.
    /// </summary>
    /// <param name="swapchain">The <see cref="Swapchain"/> to swap and present.</param>
    public void SwapBuffers(Swapchain swapchain)
    {
        uint imageIndex = swapchain.ImageIndex;
        SwapchainKHR deviceSwapchain = swapchain.DeviceSwapchain;
        PresentInfoKHR presentInfo = new PresentInfoKHR
        {
            SType = StructureType.PresentInfoKhr, SwapchainCount = 1, PSwapchains = &deviceSwapchain, PImageIndices = &imageIndex,
        };

        swapchain.PresentQueue.Present(in presentInfo);
        if (swapchain.AcquireNextImage(new Semaphore(), swapchain.ImageAvailableFence))
        {
            Fence fence = swapchain.ImageAvailableFence;
            fence.Wait();
            fence.Reset();
        }
    }

    /// <summary>
    /// Notifies this instance that the main window has been resized. This causes the <see cref="SwapchainFramebuffer"/> to
    /// be appropriately resized and recreated.
    /// This is equivalent to calling <see cref="MainSwapchain"/>.<see cref="Swapchain.RecreateAndReacquire()"/>.
    /// This method can only be called if this GraphicsDevice was created with a main Swapchain.
    /// </summary>
    public void ResizeMainWindow()
    {
        if (MainSwapchain == null)
        {
            throw new Exception("This GraphicsDevice was created without a main Swapchain, so the requested operation cannot be performed.");
        }

        MainSwapchain.RecreateAndReacquire();
    }

    /// <summary>
    /// A blocking method that returns when all submitted <see cref="CommandList"/> objects have fully completed.
    /// </summary>
    public void WaitForIdle()
    {
        GraphicsQueue.WaitIdle();
        FenceManager.CheckAllSubmittedFence();
    }
}

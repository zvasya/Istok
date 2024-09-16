using Silk.NET.Vulkan;
using System.Diagnostics;
using Silk.NET.Vulkan.Extensions.Helpers;

namespace Istok.Rendering;

/// <summary>
/// Represent multidimensional - up to 3 - arrays of data which can be used for various purposes (e.g. attachments, textures), by binding them to a graphics or compute pipeline via descriptor sets, or by directly specifying them as parameters to certain commands.
/// </summary>
public unsafe class Image : IDisposable
{
    readonly ThreadLocal<CommandPool> _commandPoolStorage;
    readonly ImageCreateInfo _description;

    readonly object _fullImageViewLock = new object();

    readonly Silk.NET.Vulkan.Image _image;

    readonly ImageLayout[] _imageLayouts;
    readonly LogicalDevice _logicalDevice;
    readonly DeviceMemory _memoryBlock;
    readonly StagingBuffersPool _stagingBuffersPool;
    bool _destroyed;
    ImageView _fullImageView;
    string _name;


    Image(Silk.NET.Vulkan.Image image, LogicalDevice logicalDevice,
        StagingBuffersPool stagingBuffersPool,
        ThreadLocal<CommandPool> commandPoolsStorage,
        in ImageCreateInfo description,
        ImageLayout[] imageLayout,
        bool isSwapchainImage,
        DeviceMemory memory)
    {
        _image = image;
        _logicalDevice = logicalDevice;
        _stagingBuffersPool = stagingBuffersPool;
        _commandPoolStorage = commandPoolsStorage;
        _description = description;
        _imageLayouts = imageLayout;
        IsSwapchainImage = isSwapchainImage;
        _memoryBlock = memory;
    }

    public Silk.NET.Vulkan.Image DeviceImage => _image;

    public bool IsSwapchainImage { get; }

    /// <summary>
    /// The number of samples per texel
    /// </summary>
    public uint ArrayLayers => _description.ArrayLayers;

    /// <summary>
    /// The format and type of the texel blocks that will be contained in the image
    /// </summary>
    public Format Format => _description.Format;

    /// <summary>
    /// The number of data elements in all dimension of the base level
    /// </summary>
    public Extent3D Extent => _description.Extent;

    /// <summary>
    /// The number of data elements in X dimension of the base level
    /// </summary>
    public uint Width => Extent.Width;

    /// <summary>
    /// The number of data elements in Y dimension of the base level
    /// </summary>
    public uint Height => Extent.Height;

    /// <summary>
    /// The number of data elements in Z dimension of the base level
    /// </summary>
    public uint Depth => Extent.Depth;

    /// <summary>
    /// The number of levels of detail available for minified sampling of the image
    /// </summary>
    public uint MipLevels => _description.MipLevels;

    /// <summary>
    /// The intended usage of the image
    /// </summary>
    public ImageUsageFlags Usage => _description.Usage;

    /// <summary>
    /// Additional parameters of the image
    /// </summary>
    public ImageCreateFlags CreateFlags => _description.Flags;

    /// <summary>
    /// Is <see cref="CreateFlags"/> has <see cref="ImageCreateFlags.CreateCubeCompatibleBit"/> flag
    /// </summary>
    public bool IsCubemap => CreateFlags.HasFlag(ImageCreateFlags.CreateCubeCompatibleBit);

    /// <summary>
    /// The <see cref="ImageType"/> of image
    /// </summary>
    public ImageType Type => _description.ImageType;

    /// <summary>
    /// The number of samples per texel
    /// </summary>
    public SampleCountFlags SampleCount => _description.Samples;

    public bool IsDisposed => _destroyed;

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            _logicalDevice.SetObjectName(ObjectType.Image, DeviceImage.Handle, value);
        }
    }

    public void Dispose()
    {
        lock (_fullImageViewLock)
        {
            _fullImageView?.Dispose();
        }

        if (!_destroyed)
        {
            _destroyed = true;

            if (!IsSwapchainImage)
                _logicalDevice.DestroyImage(_image, null);

            if (_memoryBlock.Handle != 0)
            {
                _logicalDevice.FreeMemory(_memoryBlock, null);
            }
        }
    }

    /// <summary>
    /// Construct Image with given ImageCreateInfo
    /// </summary>
    /// <param name="logicalDevice"></param>
    /// <param name="stagingBuffersPool"></param>
    /// <param name="commandPoolsStorage"></param>
    /// <param name="description"></param>
    /// <returns>new Image</returns>
    public static Image Create(LogicalDevice logicalDevice, StagingBuffersPool stagingBuffersPool, ThreadLocal<CommandPool> commandPoolsStorage, in ImageCreateInfo description)
    {
        uint subresourceCount = description.MipLevels * description.ArrayLayers * description.Extent.Depth;
        Result result = logicalDevice.CreateImage(in description, null, out Silk.NET.Vulkan.Image vkImage);
        Helpers.CheckErrors(result);

        logicalDevice.GetImageMemoryRequirements(vkImage, out MemoryRequirements memRequirements);

        MemoryAllocateInfo allocInfo = new MemoryAllocateInfo
        {
            SType = StructureType.MemoryAllocateInfo,
            AllocationSize = memRequirements.Size,
            MemoryTypeIndex = logicalDevice.PhysicalDevice.FindMemoryType(memRequirements.MemoryTypeBits, MemoryPropertyFlags.DeviceLocalBit),
        };

        Helpers.CheckErrors(logicalDevice.AllocateMemory(&allocInfo, null, out DeviceMemory memoryBlock));

        result = logicalDevice.BindImageMemory(vkImage, memoryBlock, 0);
        Helpers.CheckErrors(result);


        ImageLayout[] imageLayouts = Enumerable.Repeat(ImageLayout.Preinitialized, (int)subresourceCount).ToArray();

        Image image =  new Image(vkImage,
            logicalDevice,
            stagingBuffersPool,
            commandPoolsStorage,
            in description,
            imageLayouts,
            false,
            memoryBlock);

        image.ClearIfRenderTarget();
        image.TransitionIfSampled();

        return image;
    }

    /// <summary>
    /// Construct Image from existing VKImage with params
    /// </summary>
    /// <param name="logicalDevice"></param>
    /// <param name="stagingBuffersPool"></param>
    /// <param name="commandPoolsStorage"></param>
    /// <param name="extent"></param>
    /// <param name="mipLevels"></param>
    /// <param name="arrayLayers"></param>
    /// <param name="vkFormat"></param>
    /// <param name="usage"></param>
    /// <param name="createFlags"></param>
    /// <param name="sampleCount"></param>
    /// <param name="existingImage"></param>
    /// <returns>new Image</returns>
    internal static Image CreateSwapchain(LogicalDevice logicalDevice, StagingBuffersPool stagingBuffersPool, ThreadLocal<CommandPool> commandPoolsStorage, Extent3D extent, uint mipLevels, uint arrayLayers, Format vkFormat, ImageUsageFlags usage, ImageCreateFlags createFlags, SampleCountFlags sampleCount, Silk.NET.Vulkan.Image existingImage)
    {
        Debug.Assert(extent.Width > 0 && extent.Height > 0);

        ImageCreateInfo description = new ImageCreateInfo
        {
            SType = StructureType.ImageCreateInfo,
            Extent = extent,
            MipLevels = mipLevels,
            Flags = createFlags,
            ArrayLayers = arrayLayers,
            Format = vkFormat,
            Usage = usage,
            ImageType = ImageType.Type2D,
            Samples = sampleCount,
        };
        ImageLayout[] imageLayouts = [ImageLayout.Undefined];

        Image image = new Image(existingImage, logicalDevice, stagingBuffersPool, commandPoolsStorage, ref description, imageLayouts, true, default);

        image.TransitionImageLayout(ImageLayout.PresentSrcKhr);
        //TODO: ClearIfRenderTarget cause freezes when recreate swapchain, replaced with TransitionImageLayout for now
        // image.ClearIfRenderTarget();
        return image;
    }

    void ClearIfRenderTarget()
    {
        // If the image is going to be used as a render target, we need to clear the data before its first use.
        if (Usage.HasFlag(ImageUsageFlags.ColorAttachmentBit))
        {
            ClearColorImage(new ClearColorValue(0, 0, 0, 0));
        }
        else if (Usage.HasFlag(ImageUsageFlags.DepthStencilAttachmentBit))
        {
            ClearDepthImage(new ClearDepthStencilValue(0, 0));
        }
    }

    void ClearColorImage(ClearColorValue color)
    {
        uint effectiveLayers = ArrayLayers;
        ImageSubresourceRange range = new ImageSubresourceRange(
            ImageAspectFlags.ColorBit,
            0,
            MipLevels,
            0,
            effectiveLayers);
        CommandPool commandPool = _commandPoolStorage.Value;
        CommandBuffer cb = commandPool.AllocateCommandBuffer();
        cb.Begin();
        TransitionImageLayout(cb, 0, MipLevels, 0, effectiveLayers, ImageLayout.TransferDstOptimal);
        cb.CmdClearColorImage(DeviceImage, ImageLayout.TransferDstOptimal, &color, 1, &range);
        ImageLayout colorLayout = IsSwapchainImage ? ImageLayout.PresentSrcKhr : ImageLayout.ColorAttachmentOptimal;
        TransitionImageLayout(cb, 0, MipLevels, 0, effectiveLayers, colorLayout);
        cb.End();
        cb.SubmitCommandBuffer(0, null, 0, null).OnReset += () =>
        {
            commandPool.Return(cb);
        };
    }

    void ClearDepthImage(ClearDepthStencilValue clearValue)
    {
        uint effectiveLayers = ArrayLayers;
        ImageAspectFlags aspect = Format.HasStencil()
            ? ImageAspectFlags.DepthBit | ImageAspectFlags.StencilBit
            : ImageAspectFlags.DepthBit;
        ImageSubresourceRange range = new ImageSubresourceRange(
            aspect,
            0,
            MipLevels,
            0,
            effectiveLayers);
        CommandPool commandPool = _commandPoolStorage.Value;
        CommandBuffer cb = commandPool.AllocateCommandBuffer();
        cb.Begin();
        TransitionImageLayout(cb, 0, MipLevels, 0, effectiveLayers, ImageLayout.TransferDstOptimal);
        cb.CmdClearDepthStencilImage(
            DeviceImage,
            ImageLayout.TransferDstOptimal,
            &clearValue,
            1,
            &range);
        TransitionImageLayout(cb, 0, MipLevels, 0, effectiveLayers, ImageLayout.DepthStencilAttachmentOptimal);
        cb.End();
        cb.SubmitCommandBuffer(0, null, 0, null).OnReset += () =>
        {
            commandPool.Return(cb);
        };
    }

    void TransitionIfSampled()
    {
        if (Usage.HasFlag(ImageUsageFlags.SampledBit))
        {
            TransitionImageLayout(ImageLayout.ShaderReadOnlyOptimal);
        }
    }

    void TransitionImageLayout(ImageLayout layout)
    {
        CommandPool commandPool = _commandPoolStorage.Value;
        CommandBuffer cb = commandPool.AllocateCommandBuffer();
        cb.Begin();
        TransitionImageLayout(cb, 0, MipLevels, 0, ArrayLayers, layout);
        cb.End();
        cb.SubmitCommandBuffer(0, null, 0, null).OnReset += () =>
        {
            commandPool.Return(cb);
        };
    }

    void GetMipLevelAndArrayLayer(uint subresource, out uint mipLevel, out uint arrayLayer)
    {
        arrayLayer = subresource / MipLevels;
        mipLevel = subresource - arrayLayer * MipLevels;
    }

    internal SubresourceLayout GetSubresourceLayout(uint subresource)
    {
        GetMipLevelAndArrayLayer( subresource, out uint mipLevel, out uint arrayLayer);
        ImageAspectFlags aspect = Usage.HasFlag(ImageUsageFlags.DepthStencilAttachmentBit)
            ? ImageAspectFlags.DepthBit | ImageAspectFlags.StencilBit
            : ImageAspectFlags.ColorBit;
        ImageSubresource imageSubresource = new ImageSubresource
        {
            ArrayLayer = arrayLayer,
            MipLevel = mipLevel,
            AspectMask = aspect,
        };

        _logicalDevice.GetImageSubresourceLayout(_image, in imageSubresource, out SubresourceLayout layout);
        return layout;
    }

    internal void TransitionImageLayout(
        CommandBuffer cb,
        uint baseMipLevel,
        uint levelCount,
        uint baseArrayLayer,
        uint layerCount,
        ImageLayout newLayout)
    {
        ImageLayout oldLayout = _imageLayouts[CalculateSubresource(baseMipLevel, baseArrayLayer)];
#if DEBUG
        for (uint level = 0; level < levelCount; level++)
        {
            for (uint layer = 0; layer < layerCount; layer++)
            {
                if (_imageLayouts[CalculateSubresource(baseMipLevel + level, baseArrayLayer + layer)] != oldLayout)
                {
                    throw new Exception("Unexpected image layout.");
                }
            }
        }
#endif
        if (oldLayout != newLayout)
        {
            ImageAspectFlags aspectMask;
            if (Usage.HasFlag(ImageUsageFlags.DepthStencilAttachmentBit))
            {
                aspectMask = Format.HasStencil()
                    ? ImageAspectFlags.DepthBit | ImageAspectFlags.StencilBit
                    : ImageAspectFlags.DepthBit;
            }
            else
            {
                aspectMask = ImageAspectFlags.ColorBit;
            }

            TransitionImageLayout(
                cb,
                baseMipLevel,
                levelCount,
                baseArrayLayer,
                layerCount,
                aspectMask,
                _imageLayouts[CalculateSubresource(baseMipLevel, baseArrayLayer)],
                newLayout);

            for (uint level = 0; level < levelCount; level++)
            {
                for (uint layer = 0; layer < layerCount; layer++)
                {
                    _imageLayouts[CalculateSubresource(baseMipLevel + level, baseArrayLayer + layer)] = newLayout;
                }
            }
        }
    }

    internal void TransitionImageLayoutNonmatching(
        CommandBuffer cb,
        uint baseMipLevel,
        uint levelCount,
        uint baseArrayLayer,
        uint layerCount,
        ImageLayout newLayout)
    {
        for (uint level = baseMipLevel; level < baseMipLevel + levelCount; level++)
        {
            for (uint layer = baseArrayLayer; layer < baseArrayLayer + layerCount; layer++)
            {
                uint subresource = CalculateSubresource(level, layer);
                ImageLayout oldLayout = _imageLayouts[subresource];

                if (oldLayout != newLayout)
                {
                    ImageAspectFlags aspectMask;
                    if (Usage.HasFlag(ImageUsageFlags.DepthStencilAttachmentBit))
                    {
                        aspectMask = Format.HasStencil()
                            ? ImageAspectFlags.DepthBit | ImageAspectFlags.StencilBit
                            : ImageAspectFlags.DepthBit;
                    }
                    else
                    {
                        aspectMask = ImageAspectFlags.ColorBit;
                    }

                    TransitionImageLayout(
                        cb,
                        level,
                        1,
                        layer,
                        1,
                        aspectMask,
                        oldLayout,
                        newLayout);

                    _imageLayouts[subresource] = newLayout;
                }
            }
        }
    }

    internal ImageLayout GetImageLayout(uint mipLevel, uint arrayLayer)
    {
        return _imageLayouts[CalculateSubresource(mipLevel, arrayLayer)];
    }

    internal void SetImageLayout(uint mipLevel, uint arrayLayer, ImageLayout layout)
    {
        _imageLayouts[CalculateSubresource(mipLevel, arrayLayer)] = layout;
    }

    /// <summary>
    /// Calculates the subresource index, given a mipmap level and array layer
    /// </summary>
    /// <param name="mipLevel">The number of level of detail, should be less than <see cref="MipLevels"/></param>
    /// <param name="arrayLayer">The number of sample, should be less than <see cref="ArrayLayers"/></param>
    /// <returns>The subresource index.</returns>
    public uint CalculateSubresource(uint mipLevel, uint arrayLayer)
    {
        return arrayLayer * MipLevels + mipLevel;
    }

    public ImageView GetFullImageView()
    {
        lock (_fullImageViewLock)
        {
            return _fullImageView ??= ImageView.Create(_logicalDevice, this);
        }
    }

    public ImageViewCreateInfo GetImageViewCreateInfo()
    {
        return ImageViewCreateInfoExt.Create(_image, _description);
    }

    public ImageViewCreateInfo GetImageViewCreateInfo(uint baseMipLevel, uint mipLevels, uint baseArrayLayer, uint arrayLayers)
    {
        return ImageViewCreateInfoExt.Create(_image, _description, baseMipLevel, mipLevels, baseArrayLayer, arrayLayers);
    }

    public ImageViewCreateInfo GetImageViewCreateInfo(Format format)
    {
        return ImageViewCreateInfoExt.Create(_image, _description, format);
    }

    public ImageViewCreateInfo GetImageViewCreateInfo(Format format, uint baseMipLevel, uint mipLevels, uint baseArrayLayer, uint arrayLayers)
    {
        return ImageViewCreateInfoExt.Create(_image, _description, format, baseMipLevel, mipLevels, baseArrayLayer, arrayLayers);
    }


    /// <summary>
    /// Fill part of a <see cref="Rendering.Image"/>
    /// </summary>
    /// <param name="source">A pointer to the new data</param>
    /// <param name="x">X value of the region.</param>
    /// <param name="y">Y value of the region.</param>
    /// <param name="z">Z value of the updated region.</param>
    /// <param name="width">Width of the region</param>
    /// <param name="height">Height of the region</param>
    /// <param name="depth">Depth of the region</param>
    /// <param name="mipLevel">The number of level of detail to update, should be less than <see cref="MipLevels"/></param>
    /// <param name="arrayLayer">The number of sample to update, should be less than <see cref="ArrayLayers"/></param>
    public void Fill(
        IntPtr source,
        uint x, uint y, uint z,
        uint width, uint height, uint depth,
        uint mipLevel, uint arrayLayer)
    {
        uint totalSize = Format.GetRegionSize(width, height, depth);
        ReadOnlySpan<byte> span = new ReadOnlySpan<byte>(source.ToPointer(), (int)totalSize);
        Fill(span, x,y,z,width,height,depth,mipLevel, arrayLayer);
    }

    /// <summary>
    /// Fill <see cref="Rendering.Image"/>
    /// </summary>
    /// <param name="source">New data</param>
    public void Fill<T>(
        ReadOnlySpan<T> source
    ) where T : unmanaged
    {
        Fill(source, 0, 0, 0, Width, Height, Depth, 0, 0);
    }

    /// <summary>
    /// Fill part of a <see cref="Rendering.Image"/>
    /// </summary>
    /// <param name="source">New data</param>
    /// <param name="x">X value of the region.</param>
    /// <param name="y">Y value of the region.</param>
    /// <param name="z">Z value of the updated region.</param>
    /// <param name="width">Width of the region</param>
    /// <param name="height">Height of the region</param>
    /// <param name="depth">Depth of the region</param>
    /// <param name="mipLevel">The number of level of detail to update, should be less than <see cref="MipLevels"/></param>
    /// <param name="arrayLayer">The number of sample to update, should be less than <see cref="ArrayLayers"/></param>
    public void Fill<T>(
        ReadOnlySpan<T> source,
        uint x, uint y, uint z,
        uint width, uint height, uint depth,
        uint mipLevel, uint arrayLayer) where T : unmanaged
    {
        Image image = this;
        uint totalSize = image.Format.GetRegionSize(width, height, depth);
        Buffer stagingTex = _stagingBuffersPool.Get(totalSize);
        stagingTex.Fill(0, source);

        CommandPool commandPool = _commandPoolStorage.Value;
        CommandBuffer cb = commandPool.AllocateCommandBuffer();
        cb.Begin();
        cb.CopyBufferToImage(
            stagingTex,
            image, x, y, z, mipLevel, arrayLayer,
            width, height, depth, 1);

        cb.End();
        cb.SubmitCommandBuffer(0, null, 0, null).OnReset += () =>
        {
            commandPool.Return(cb);
            _stagingBuffersPool.Return(stagingTex);
        };
    }

    public void TransitionImageLayout(
        CommandBuffer cb,
        uint baseMipLevel,
        uint levelCount,
        uint baseArrayLayer,
        uint layerCount,
        ImageAspectFlags aspectMask,
        ImageLayout oldLayout,
        ImageLayout newLayout)
    {
        Debug.Assert(oldLayout != newLayout);
        ImageMemoryBarrier barrier = new ImageMemoryBarrier
        {
            SType = StructureType.ImageMemoryBarrier,
            OldLayout = oldLayout,
            NewLayout = newLayout,
            SrcQueueFamilyIndex = Vk.QueueFamilyIgnored,
            DstQueueFamilyIndex = Vk.QueueFamilyIgnored,
            Image = _image,
            SubresourceRange = new ImageSubresourceRange
            {
                AspectMask = aspectMask,
                BaseMipLevel = baseMipLevel,
                LevelCount = levelCount,
                BaseArrayLayer = baseArrayLayer,
                LayerCount = layerCount,
            },
        };

        PipelineStageFlags srcStageFlags = PipelineStageFlags.None;
        PipelineStageFlags dstStageFlags = PipelineStageFlags.None;

        switch (oldLayout, newLayout)
        {
            case (ImageLayout.Undefined, ImageLayout.TransferDstOptimal):
            case (ImageLayout.Undefined, ImageLayout.PresentSrcKhr):
            case (ImageLayout.Preinitialized, ImageLayout.TransferDstOptimal):
                barrier.SrcAccessMask = AccessFlags.None;
                barrier.DstAccessMask = AccessFlags.TransferWriteBit;
                srcStageFlags = PipelineStageFlags.TopOfPipeBit;
                dstStageFlags = PipelineStageFlags.TransferBit;
                break;
            case (ImageLayout.ShaderReadOnlyOptimal, ImageLayout.TransferSrcOptimal):
                barrier.SrcAccessMask = AccessFlags.ShaderReadBit;
                barrier.DstAccessMask = AccessFlags.TransferReadBit;
                srcStageFlags = PipelineStageFlags.FragmentShaderBit;
                dstStageFlags = PipelineStageFlags.TransferBit;
                break;
            case (ImageLayout.ShaderReadOnlyOptimal, ImageLayout.TransferDstOptimal):
                barrier.SrcAccessMask = AccessFlags.ShaderReadBit;
                barrier.DstAccessMask = AccessFlags.TransferWriteBit;
                srcStageFlags = PipelineStageFlags.FragmentShaderBit;
                dstStageFlags = PipelineStageFlags.TransferBit;
                break;
            case (ImageLayout.Preinitialized, ImageLayout.TransferSrcOptimal):
                barrier.SrcAccessMask = AccessFlags.None;
                barrier.DstAccessMask = AccessFlags.TransferReadBit;
                srcStageFlags = PipelineStageFlags.TopOfPipeBit;
                dstStageFlags = PipelineStageFlags.TransferBit;
                break;
            case (ImageLayout.Preinitialized, ImageLayout.General):
                barrier.SrcAccessMask = AccessFlags.None;
                barrier.DstAccessMask = AccessFlags.ShaderReadBit;
                srcStageFlags = PipelineStageFlags.TopOfPipeBit;
                dstStageFlags = PipelineStageFlags.ComputeShaderBit;
                break;
            case (ImageLayout.Preinitialized, ImageLayout.ShaderReadOnlyOptimal):
                barrier.SrcAccessMask = AccessFlags.None;
                barrier.DstAccessMask = AccessFlags.ShaderReadBit;
                srcStageFlags = PipelineStageFlags.TopOfPipeBit;
                dstStageFlags = PipelineStageFlags.FragmentShaderBit;
                break;
            case (ImageLayout.General, ImageLayout.ShaderReadOnlyOptimal):
            case (ImageLayout.TransferSrcOptimal, ImageLayout.ShaderReadOnlyOptimal):
                barrier.SrcAccessMask = AccessFlags.TransferReadBit;
                barrier.DstAccessMask = AccessFlags.ShaderReadBit;
                srcStageFlags = PipelineStageFlags.TransferBit;
                dstStageFlags = PipelineStageFlags.FragmentShaderBit;
                break;
            case (ImageLayout.ShaderReadOnlyOptimal, ImageLayout.General):
                barrier.SrcAccessMask = AccessFlags.ShaderReadBit;
                barrier.DstAccessMask = AccessFlags.ShaderReadBit;
                srcStageFlags = PipelineStageFlags.FragmentShaderBit;
                dstStageFlags = PipelineStageFlags.ComputeShaderBit;
                break;
            case (ImageLayout.TransferDstOptimal, ImageLayout.ShaderReadOnlyOptimal):
                barrier.SrcAccessMask = AccessFlags.TransferWriteBit;
                barrier.DstAccessMask = AccessFlags.ShaderReadBit;
                srcStageFlags = PipelineStageFlags.TransferBit;
                dstStageFlags = PipelineStageFlags.FragmentShaderBit;
                break;
            case (ImageLayout.TransferSrcOptimal, ImageLayout.TransferDstOptimal):
                barrier.SrcAccessMask = AccessFlags.TransferReadBit;
                barrier.DstAccessMask = AccessFlags.TransferWriteBit;
                srcStageFlags = PipelineStageFlags.TransferBit;
                dstStageFlags = PipelineStageFlags.TransferBit;
                break;
            case (ImageLayout.TransferDstOptimal, ImageLayout.TransferSrcOptimal):
                barrier.SrcAccessMask = AccessFlags.TransferWriteBit;
                barrier.DstAccessMask = AccessFlags.TransferReadBit;
                srcStageFlags = PipelineStageFlags.TransferBit;
                dstStageFlags = PipelineStageFlags.TransferBit;
                break;
            case (ImageLayout.ColorAttachmentOptimal, ImageLayout.TransferSrcOptimal):
                barrier.SrcAccessMask = AccessFlags.ColorAttachmentWriteBit;
                barrier.DstAccessMask = AccessFlags.TransferReadBit;
                srcStageFlags = PipelineStageFlags.ColorAttachmentOutputBit;
                dstStageFlags = PipelineStageFlags.TransferBit;
                break;
            case (ImageLayout.ColorAttachmentOptimal, ImageLayout.TransferDstOptimal):
                barrier.SrcAccessMask = AccessFlags.ColorAttachmentWriteBit;
                barrier.DstAccessMask = AccessFlags.TransferWriteBit;
                srcStageFlags = PipelineStageFlags.ColorAttachmentOutputBit;
                dstStageFlags = PipelineStageFlags.TransferBit;
                break;
            case (ImageLayout.ColorAttachmentOptimal, ImageLayout.ShaderReadOnlyOptimal):
                barrier.SrcAccessMask = AccessFlags.ColorAttachmentWriteBit;
                barrier.DstAccessMask = AccessFlags.ShaderReadBit;
                srcStageFlags = PipelineStageFlags.ColorAttachmentOutputBit;
                dstStageFlags = PipelineStageFlags.FragmentShaderBit;
                break;
            case (ImageLayout.DepthStencilAttachmentOptimal, ImageLayout.ShaderReadOnlyOptimal):
                barrier.SrcAccessMask = AccessFlags.DepthStencilAttachmentWriteBit;
                barrier.DstAccessMask = AccessFlags.ShaderReadBit;
                srcStageFlags = PipelineStageFlags.LateFragmentTestsBit;
                dstStageFlags = PipelineStageFlags.FragmentShaderBit;
                break;
            case (ImageLayout.ColorAttachmentOptimal, ImageLayout.PresentSrcKhr):
                barrier.SrcAccessMask = AccessFlags.ColorAttachmentWriteBit;
                barrier.DstAccessMask = AccessFlags.MemoryReadBit;
                srcStageFlags = PipelineStageFlags.ColorAttachmentOutputBit;
                dstStageFlags = PipelineStageFlags.BottomOfPipeBit;
                break;
            case (ImageLayout.TransferDstOptimal, ImageLayout.PresentSrcKhr):
                barrier.SrcAccessMask = AccessFlags.TransferWriteBit;
                barrier.DstAccessMask = AccessFlags.MemoryReadBit;
                srcStageFlags = PipelineStageFlags.TransferBit;
                dstStageFlags = PipelineStageFlags.BottomOfPipeBit;
                break;
            case (ImageLayout.TransferDstOptimal, ImageLayout.ColorAttachmentOptimal):
                barrier.SrcAccessMask = AccessFlags.TransferWriteBit;
                barrier.DstAccessMask = AccessFlags.ColorAttachmentWriteBit;
                srcStageFlags = PipelineStageFlags.TransferBit;
                dstStageFlags = PipelineStageFlags.ColorAttachmentOutputBit;
                break;
            case (ImageLayout.TransferDstOptimal, ImageLayout.DepthStencilAttachmentOptimal):
                barrier.SrcAccessMask = AccessFlags.TransferWriteBit;
                barrier.DstAccessMask = AccessFlags.DepthStencilAttachmentWriteBit;
                srcStageFlags = PipelineStageFlags.TransferBit;
                dstStageFlags = PipelineStageFlags.LateFragmentTestsBit;
                break;
            case (ImageLayout.General, ImageLayout.TransferSrcOptimal):
                barrier.SrcAccessMask = AccessFlags.ShaderWriteBit;
                barrier.DstAccessMask = AccessFlags.TransferReadBit;
                srcStageFlags = PipelineStageFlags.ComputeShaderBit;
                dstStageFlags = PipelineStageFlags.TransferBit;
                break;
            case (ImageLayout.General, ImageLayout.TransferDstOptimal):
                barrier.SrcAccessMask = AccessFlags.ShaderWriteBit;
                barrier.DstAccessMask = AccessFlags.TransferWriteBit;
                srcStageFlags = PipelineStageFlags.ComputeShaderBit;
                dstStageFlags = PipelineStageFlags.TransferBit;
                break;
            case (ImageLayout.PresentSrcKhr, ImageLayout.TransferSrcOptimal):
                barrier.SrcAccessMask = AccessFlags.MemoryReadBit;
                barrier.DstAccessMask = AccessFlags.TransferReadBit;
                srcStageFlags = PipelineStageFlags.BottomOfPipeBit;
                dstStageFlags = PipelineStageFlags.TransferBit;
                break;
            default:
                Debug.Fail("Invalid image layout transition.");
                break;
        }

        cb.CmdPipelineBarrier(
            srcStageFlags,
            dstStageFlags,
            DependencyFlags.None,
            0, null,
            0, null,
            1, &barrier);
    }

    public void GetMipDimensions(uint mipLevel, out uint width, out uint height, out uint depth)
    {
        width = Math.Max(1, Width >> (int)mipLevel);
        height = Math.Max(1, Height >> (int)mipLevel);
        depth = Math.Max(1, Depth >> (int)mipLevel);
    }
}

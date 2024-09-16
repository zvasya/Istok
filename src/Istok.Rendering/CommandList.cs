using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.Helpers;
using Semaphore = Silk.NET.Vulkan.Semaphore;

namespace Istok.Rendering;

public unsafe class CommandList
{
    readonly ConcurrentStack<StagingResourceInfo> _availableStagingInfos = new ConcurrentStack<StagingResourceInfo>();

    readonly ThreadLocal<CommandPool> _commandPool;
    readonly DebugUtils _debugUtils;

    readonly LogicalDevice _logicalDevice;
    readonly StagingBuffersPool _stagingBuffersPool;
    readonly List<Image> _preDrawSampledImages = [];

    RenderPass _activeRenderPass;
    CommandPool _currentCommandPool;
    StagingResourceInfo _currentStagingInfo;
    CommandBuffer _commandBuffer;
    string _name;

    ClearValue[] _clearValues = [];
    ClearValue _depthClearValue;

    // Compute State
    Pipeline _computePipeline;
    BoundResourceSetInfo[] _computeResourceSets = [];
    bool[] _computeResourceSetsChanged;


    // Graphics State
    Framebuffer _framebuffer;
    Pipeline _graphicsPipeline;
    BoundResourceSetInfo[] _graphicsResourceSets = [];
    bool[] _graphicsResourceSetsChanged;

    public CommandList(LogicalDevice logicalDevice, StagingBuffersPool stagingBuffersPool, ThreadLocal<CommandPool> commandPoolsStorage)
    {
        _logicalDevice = logicalDevice;
        _debugUtils = logicalDevice.Engine.Debug;
        _stagingBuffersPool = stagingBuffersPool;
        _commandPool = commandPoolsStorage;
    }

    public bool IsDisposed { get; private set; }

    public  string Name
    {
        get => _name;
        set
        {
            _name = value;
            if (_commandBuffer != null)
                _commandBuffer.Name = $"{value}_CommandBuffer";
        }
    }

    public void Dispose()
    {
        if (!IsDisposed)
        {
            IsDisposed = true;

            if (_currentStagingInfo != null)
                RecycleStagingInfo(_currentStagingInfo);
        }
    }

    StagingResourceInfo GetStagingResourceInfo()
    {
        return _availableStagingInfos.TryPop(out StagingResourceInfo ret) ? ret : new StagingResourceInfo();
    }

    void RecycleStagingInfo(StagingResourceInfo info)
    {
        foreach (Buffer buffer in info.BuffersUsed)
        {
            buffer.Name = $"returned (CommandList {_name})";
            _stagingBuffersPool.Return(buffer);
        }

        info.Clear();
        _availableStagingInfos.Push(info);
    }

    Buffer GetStagingBuffer(uint size)
    {
        Buffer stagingBuffer = _stagingBuffersPool.Get(size);
        stagingBuffer.Name = $"Staging Buffer (CommandList {_name})";
        _currentStagingInfo.BuffersUsed.Add(stagingBuffer);
        return stagingBuffer;
    }

    /// <summary>
    /// Open a command buffer marker region
    /// </summary>
    /// <param name="name">The name of the region</param>
    public DebugGroupScope PushDebugGroup(string name)
    {
        _debugUtils?.BeginLabel(_commandBuffer, name);
        return new DebugGroupScope(this);
    }


    /// <summary>
    /// Close a command buffer marker region
    /// </summary>
    public void PopDebugGroup()
    {
        _debugUtils?.EndLabel(_commandBuffer);
    }

    /// <summary>
    /// Allows you to add labels, that work as markers inside the Vulkan Event chain
    /// </summary>
    /// <param name="name">The name of the marker</param>
    public void InsertDebugMarker(string name)
    {
        _debugUtils?.InsertLabel(_commandBuffer, name);
    }

    public Fence SubmitCommandList(
        uint waitSemaphoreCount,
        Semaphore* waitSemaphoresPtr,
        uint signalSemaphoreCount,
        Semaphore* signalSemaphoresPtr)
    {
        CommandBuffer commandBuffer = _commandBuffer;
        _commandBuffer = null;
        StagingResourceInfo currentStagingInfo = _currentStagingInfo;
        _currentStagingInfo = null;

        Fence fence = commandBuffer.SubmitCommandBuffer(waitSemaphoreCount, waitSemaphoresPtr, signalSemaphoreCount, signalSemaphoresPtr);
        fence.OnReset += () =>
        {
            RecycleStagingInfo(currentStagingInfo);
            commandBuffer.Reset();
            _currentCommandPool.Return(commandBuffer);
        };
        return fence;
    }

    void TransitionImages(IReadOnlyList<Image> sampledTextures, ImageLayout layout)
    {
        foreach (Image tex in sampledTextures)
        {
            tex.TransitionImageLayout(_commandBuffer, 0, tex.MipLevels, 0, tex.ArrayLayers, layout);
        }
    }

    public void SetFullScissorRect()
    {
        SetScissorRect(new Rect2D(extent: new Extent2D(_framebuffer.Width, _framebuffer.Height)));
    }

    public void SetScissorRect(int x, int y, uint width, uint height)
    {
        SetScissorRect(new Rect2D(new Offset2D(x, y), new Extent2D(width, height)));
    }

    public void SetScissorRect(in Rect2D scissor)
    {
        _commandBuffer.CmdSetScissor(0, 1, in scissor);
    }


    public void SetFullViewport()
    {
        SetViewport(new Viewport(0, _framebuffer.Height, _framebuffer.Width, -_framebuffer.Height, 0, 1));
    }

    public void SetViewport(in Viewport viewport)
    {
        _commandBuffer.CmdSetViewport(0, 1, in viewport);
    }


    /// <summary>
    /// Bind vertex buffers to a command buffer
    /// </summary>
    /// <param name="firstBinding">the index of the vertex input binding whose state is updated by the command</param>
    /// <param name="buffer">The buffer being bound</param>
    /// <param name="offset">The starting offset in bytes within buffer used in vertex buffer address calculations
    /// </param>
    public void SetVertexBuffer(uint firstBinding, Buffer buffer, ulong offset = 0)
    {
        Silk.NET.Vulkan.Buffer deviceBuffer = buffer.DeviceBuffer;
        _commandBuffer.CmdBindVertexBuffers(firstBinding, 1, in deviceBuffer, in offset);
    }

    /// <summary>
    /// Bind an index buffer to a command buffer
    /// </summary>
    /// <param name="buffer">The buffer being bound</param>
    /// <param name="format">The format specifying the size of the indices</param>
    /// <param name="offset">The starting offset in bytes within buffer used in index buffer address calculations
    /// </param>
    public void SetIndexBuffer(Buffer buffer, IndexType format, uint offset = 0)
    {
        _commandBuffer.CmdBindIndexBuffer(buffer.DeviceBuffer, offset, format);
    }

    /// <summary>
    /// Copy data between buffer
    /// </summary>
    /// <param name="source">The source buffer</param>
    /// <param name="sourceOffset">The starting offset in bytes from the start of source</param>
    /// <param name="destination">The destination buffer</param>
    /// <param name="destinationOffset">The starting offset in bytes from the start of destination</param>
    /// <param name="sizeInBytes">The number of bytes to copy</param>
    public void CopyBuffer(Buffer source, uint sourceOffset, Buffer destination, uint destinationOffset, uint sizeInBytes)
    {
        if (sizeInBytes == 0)
        {
            return;
        }

        BufferCopy region = new BufferCopy { SrcOffset = sourceOffset, DstOffset = destinationOffset, Size = sizeInBytes };

        _commandBuffer.CmdCopyBuffer(source.DeviceBuffer, destination.DeviceBuffer, 1, in region);

        MemoryBarrier barrier = new MemoryBarrier
        {
            SType = StructureType.MemoryBarrier,
            SrcAccessMask = AccessFlags.TransferWriteBit,
            DstAccessMask = AccessFlags.VertexAttributeReadBit,
            PNext = null,
        };
        _commandBuffer.CmdPipelineBarrier(
            PipelineStageFlags.TransferBit, PipelineStageFlags.VertexInputBit,
            DependencyFlags.None,
            1, in barrier,
            0, null,
            0, null);
    }

    public void GenerateMipmaps(Image image)
    {
        if (image.Usage.HasFlag(ImageUsageFlags.DepthStencilAttachmentBit))
        {
            throw new Exception(
                $"Texture with {nameof(ImageUsageFlags)}.{nameof(ImageUsageFlags.DepthStencilAttachmentBit)} cannot cannot have Mipmaps");
        }

        if (image.MipLevels > 1)
        {
            uint layerCount = image.ArrayLayers;

            uint width = image.Width;
            uint height = image.Height;
            uint depth = image.Depth;
            for (uint level = 1; level < image.MipLevels; level++)
            {
                image.TransitionImageLayoutNonmatching(_commandBuffer, level - 1, 1, 0, layerCount, ImageLayout.TransferSrcOptimal);
                image.TransitionImageLayoutNonmatching(_commandBuffer, level, 1, 0, layerCount, ImageLayout.TransferDstOptimal);

                Silk.NET.Vulkan.Image deviceImage = image.DeviceImage;
                uint mipWidth = Math.Max(width >> 1, 1);
                uint mipHeight = Math.Max(height >> 1, 1);
                uint mipDepth = Math.Max(depth >> 1, 1);

                ImageBlit region = new ImageBlit
                {
                    SrcSubresource = new ImageSubresourceLayers { AspectMask = ImageAspectFlags.ColorBit, BaseArrayLayer = 0, LayerCount = layerCount, MipLevel = level - 1 },
                    SrcOffsets = new ImageBlit.SrcOffsetsBuffer { Element0 = new Offset3D(), Element1 = new Offset3D { X = (int)width, Y = (int)height, Z = (int)depth } },
                    DstSubresource = new ImageSubresourceLayers { AspectMask = ImageAspectFlags.ColorBit, BaseArrayLayer = 0, LayerCount = layerCount, MipLevel = level },
                    DstOffsets = new ImageBlit.DstOffsetsBuffer { Element0 = new Offset3D(), Element1 = new Offset3D { X = (int)mipWidth, Y = (int)mipHeight, Z = (int)mipDepth } },
                };

                _commandBuffer.CmdBlitImage(
                    deviceImage, ImageLayout.TransferSrcOptimal,
                    deviceImage, ImageLayout.TransferDstOptimal,
                    1, &region,
                    _logicalDevice.GetFormatFilter(image.Format));

                width = mipWidth;
                height = mipHeight;
                depth = mipDepth;
            }

            if (image.Usage.HasFlag(ImageUsageFlags.SampledBit))
            {
                image.TransitionImageLayoutNonmatching(_commandBuffer, 0, image.MipLevels, 0, layerCount, ImageLayout.ShaderReadOnlyOptimal);
            }
        }
    }

    /// <summary>
    /// Bind a pipeline object to a command buffer
    /// </summary>
    /// <param name="pipeline">The pipeline to be bound</param>
    public void SetPipeline(Pipeline pipeline)
    {
        if (!pipeline.IsComputePipeline && _graphicsPipeline != pipeline)
        {
            _graphicsResourceSets = ArrayExtensions.ClearOrCreateNewOfMinimumSize(_graphicsResourceSets, pipeline.ResourceSetCount);
            _graphicsResourceSetsChanged = ArrayExtensions.ClearOrCreateNewOfMinimumSize(_graphicsResourceSetsChanged, pipeline.ResourceSetCount);

            _commandBuffer.CmdBindPipeline(PipelineBindPoint.Graphics, pipeline.DevicePipeline);
            _graphicsPipeline = pipeline;
        }
        else if (pipeline.IsComputePipeline && _computePipeline != pipeline)
        {
            _computeResourceSets = ArrayExtensions.ClearOrCreateNewOfMinimumSize(_computeResourceSets, pipeline.ResourceSetCount);
            _computeResourceSetsChanged = ArrayExtensions.ClearOrCreateNewOfMinimumSize(_computeResourceSetsChanged, pipeline.ResourceSetCount);

            _commandBuffer.CmdBindPipeline(PipelineBindPoint.Compute, pipeline.DevicePipeline);
            _computePipeline = pipeline;
        }
    }

    public CommandBufferScope Begin()
    {
        if (_commandBuffer == null)
        {
            _commandBuffer = GetNextCommandBuffer();
            if (_currentStagingInfo != null)
            {
                RecycleStagingInfo(_currentStagingInfo);
            }
        }

        _currentStagingInfo = GetStagingResourceInfo();
        _framebuffer = null;
        _graphicsPipeline = null;
        _computePipeline = null;

        _commandBuffer.Begin();
        return new CommandBufferScope(this);
    }

    public void End()
    {
        _commandBuffer.EndCommandBuffer();
    }

    CommandBuffer GetNextCommandBuffer()
    {
        _currentCommandPool = _commandPool.Value;
        return _currentCommandPool.AllocateCommandBuffer();
    }

    public void ClearDepthStencil(float depth, byte stencil = 0)
    {
        ClearValue clearValue = new ClearValue { DepthStencil = new ClearDepthStencilValue(depth, stencil) };

        if (_activeRenderPass != null)
        {
            ImageAspectFlags aspect = _framebuffer.DepthTarget!.Target.Format.HasStencil()
                ? ImageAspectFlags.DepthBit | ImageAspectFlags.StencilBit
                : ImageAspectFlags.DepthBit;
            ClearAttachment clearAttachment = new ClearAttachment { AspectMask = aspect, ClearValue = clearValue };

            uint width = _framebuffer.Width;
            uint height = _framebuffer.Height;
            if (width > 0 && height > 0)
            {
                ClearRect clearRect = new ClearRect { BaseArrayLayer = 0, LayerCount = 1, Rect = new Rect2D(new Offset2D(0, 0), new Extent2D(width, height)) };

                _commandBuffer.CmdClearAttachments(1, in clearAttachment, 1, in clearRect);
            }
        }
        else
        {
            // Queue up the clear value for the next RenderPass.
            _depthClearValue = clearValue;
        }
    }

    /// <summary>
    /// Clear region within bound framebuffer attachment at the given index
    /// </summary>
    /// <param name="index">An target index into the currently bound color attachments</param>
    /// <param name="clearColor">The color value to clear the attachment to.</param>
    public void ClearColorTarget(uint index, Color clearColor)
    {
        ClearValue clearValue = new ClearValue { Color = clearColor.ToClearColorValue() };

        if (_activeRenderPass != null)
        {
            ClearAttachment clearAttachment = new ClearAttachment { ColorAttachment = index, AspectMask = ImageAspectFlags.ColorBit, ClearValue = clearValue };

            Image colorTex = _framebuffer.ColorTargets[(int)index].Target;
            ClearRect clearRect = new ClearRect { BaseArrayLayer = 0, LayerCount = 1, Rect = new Rect2D(extent: new Extent2D(colorTex.Width, colorTex.Height)) };

            _commandBuffer.CmdClearAttachments(1, in clearAttachment, 1, in clearRect);
        }
        else
        {
            // Queue up the clear value for the next RenderPass.
            _clearValues[index] = clearValue;
        }
    }


    /// <summary>
    /// Draw primitives with indexed vertices
    /// </summary>
    /// <param name="indexCount">The number of vertices to draw</param>
    /// <param name="instanceCount">The number of instances to draw</param>
    /// <param name="indexStart">The base index within the index buffer</param>
    /// <param name="vertexOffset">The value added to the vertex index before indexing into the vertex buffer</param>
    /// <param name="instanceStart">The instance ID of the first instance to draw</param>
    public void DrawIndexed(uint indexCount, uint instanceCount = 1, uint indexStart = 0, int vertexOffset = 0, uint instanceStart = 0)
    {
        PreDrawCommand();
        _commandBuffer.CmdDrawIndexed(indexCount, instanceCount, indexStart, vertexOffset, instanceStart);
    }


    /// <summary>
    /// Draw primitives (non-indexed draw)
    /// </summary>
    /// <param name="vertexCount">The number of vertices to draw</param>
    /// <param name="instanceCount">The number of instances to draw</param>
    /// <param name="vertexStart">The index of the first vertex to draw</param>
    /// <param name="instanceStart">The instance ID of the first instance to draw</param>
    public void Draw(uint vertexCount, uint instanceCount = 1, uint vertexStart = 0, uint instanceStart = 0)
    {
        PreDrawCommand();
        _commandBuffer.CmdDraw(vertexCount, instanceCount, vertexStart, instanceStart);
    }

    /// <summary>
    /// Draw primitives with indirect parameters
    /// </summary>
    /// <param name="indirectBuffer">The buffer containing draw parameters</param>
    /// <param name="offset">The byte offset into buffer where parameters begin</param>
    /// <param name="drawCount">The number of draws to execute, and can be zero</param>
    /// <param name="stride">The byte stride between successive sets of draw parameters</param>
    public void DrawIndirect(Buffer indirectBuffer, uint offset, uint drawCount, uint stride)
    {
        PreDrawCommand();
        _commandBuffer.CmdDrawIndirect(indirectBuffer.DeviceBuffer, offset, drawCount, stride);
    }


    /// <summary>
    /// Draw primitives with indirect parameters and indexed vertices
    /// </summary>
    /// <param name="indirectBuffer">The buffer containing draw parameters</param>
    /// <param name="offset">The byte offset into buffer where parameters begin</param>
    /// <param name="drawCount">The number of draws to execute, and can be zero</param>
    /// <param name="stride">The byte stride between successive sets of draw parameters</param>
    public void DrawIndexedIndirect(Buffer indirectBuffer, uint offset, uint drawCount, uint stride)
    {
        PreDrawCommand();
        _commandBuffer.CmdDrawIndexedIndirect(indirectBuffer.DeviceBuffer, offset, drawCount, stride);
    }

    /// <summary>
    /// Dispatch compute work items
    /// </summary>
    /// <param name="groupCountX">The number of local workgroups to dispatch in the X dimension</param>
    /// <param name="groupCountY">The number of local workgroups to dispatch in the Y dimension</param>
    /// <param name="groupCountZ">The number of local workgroups to dispatch in the Z dimension</param>
    public void Dispatch(uint groupCountX, uint groupCountY, uint groupCountZ)
    {
        PreDispatchCommand();
        _commandBuffer.CmdDispatch(groupCountX, groupCountY, groupCountZ);
    }

    /// <summary>
    /// Dispatch compute work items with indirect parameters
    /// </summary>
    /// <param name="indirectBuffer">The buffer containing dispatch parameters</param>
    /// <param name="offset">The byte offset into buffer where parameters begin</param>
    public void DispatchIndirect(Buffer indirectBuffer, uint offset)
    {
        PreDispatchCommand();
        _commandBuffer.CmdDispatchIndirect(indirectBuffer.DeviceBuffer, offset);
    }


    /// <summary>
    /// Resolve a multisample color image to a non-multisample color image
    /// </summary>
    /// <param name="source">The source image</param>
    /// <param name="destination">The destination image</param>
    public void ResolveTexture(Image source, Image destination)
    {
        ImageAspectFlags aspectFlags = source.Usage.HasFlag(ImageUsageFlags.DepthStencilAttachmentBit)
            ? ImageAspectFlags.DepthBit | ImageAspectFlags.StencilBit
            : ImageAspectFlags.ColorBit;
        ImageResolve region = new ImageResolve
        {
            Extent = new Extent3D { Width = source.Width, Height = source.Height, Depth = source.Depth },
            SrcSubresource = new ImageSubresourceLayers { LayerCount = 1, AspectMask = aspectFlags },
            DstSubresource = new ImageSubresourceLayers { LayerCount = 1, AspectMask = aspectFlags }
        };

        source.TransitionImageLayout(_commandBuffer, 0, 1, 0, 1, ImageLayout.TransferSrcOptimal);
        destination.TransitionImageLayout(_commandBuffer, 0, 1, 0, 1, ImageLayout.TransferDstOptimal);

        _commandBuffer.CmdResolveImage(
            source.DeviceImage,
            ImageLayout.TransferSrcOptimal,
            destination.DeviceImage,
            ImageLayout.TransferDstOptimal,
            1,
            in region);

        if (destination.Usage.HasFlag(ImageUsageFlags.SampledBit))
        {
            destination.TransitionImageLayout(_commandBuffer, 0, 1, 0, 1, ImageLayout.ShaderReadOnlyOptimal);
        }
    }

    public RenderPassScope BeginRenderPass()
    {
        Debug.Assert(_activeRenderPass == null);
        Debug.Assert(_framebuffer != null);

        uint attachmentCount = _framebuffer.AttachmentCount;

        if (_framebuffer.DepthTarget != null)
        {
            _clearValues[_framebuffer.ColorTargets.Count] = _depthClearValue;
        }

        fixed (ClearValue* clearValuesPtr = &_clearValues[0])
        {
            RenderPassBeginInfo renderPassBeginInfo = new RenderPassBeginInfo
            {
                SType = StructureType.RenderPassBeginInfo,
                RenderArea = new Rect2D(new Offset2D(0, 0), new Extent2D(_framebuffer.Width, _framebuffer.Height)),
                Framebuffer = _framebuffer.CurrentFramebuffer,
                RenderPass = _framebuffer.RenderPass.DeviceRenderPass,
                ClearValueCount = attachmentCount,
                PClearValues = clearValuesPtr,
            };

            _commandBuffer.CmdBeginRenderPass(in renderPassBeginInfo, SubpassContents.Inline);
        }
        _activeRenderPass = _framebuffer.RenderPass;
        return new RenderPassScope(this);
    }

    public void EndRenderPass()
    {
        Debug.Assert(_activeRenderPass != null);
        _commandBuffer.CmdEndRenderPass();
        _framebuffer.TransitionToIntermediateLayout();
        _activeRenderPass = null;

        _commandBuffer.CmdPipelineBarrier(
            PipelineStageFlags.BottomOfPipeBit,
            PipelineStageFlags.TopOfPipeBit,
            DependencyFlags.None,
            0,
            null,
            0,
            null,
            0,
            null);
    }

    public FramebufferScope SetFramebuffer(Framebuffer fb)
    {
        if (_framebuffer != fb)
        {
            _framebuffer = fb;
            _clearValues = ArrayExtensions.ClearOrCreateNewOfMinimumSize(_clearValues, (uint)fb.ColorTargets.Count + 1); // Leave an extra space for the depth value (tracked separately).
            _depthClearValue = default;

            SetFullViewport();
            SetFullScissorRect();
        }

        return new FramebufferScope(this);
    }

    public void EndFramebuffer()
    {
        _framebuffer?.TransitionToFinalLayout(_commandBuffer);
        _framebuffer = null;
    }

    public FramebufferScope SetSwapchainFramebuffer(SwapchainFramebuffer swapchainFramebuffer)
    {
        return SetFramebuffer(swapchainFramebuffer.CurrentFramebuffer);
    }

    void FlushNewResourceSets(
        BoundResourceSetInfo[] resourceSets,
        bool[] resourceSetsChanged,
        uint resourceSetCount,
        PipelineBindPoint bindPoint,
        PipelineLayout pipelineLayout
    )
    {
        Pipeline pipeline = bindPoint == PipelineBindPoint.Graphics ? _graphicsPipeline : _computePipeline;

        Silk.NET.Vulkan.DescriptorSet* descriptorSets = stackalloc Silk.NET.Vulkan.DescriptorSet[(int)resourceSetCount];
        uint* dynamicOffsets = stackalloc uint[pipeline.DynamicOffsetsCount];
        uint currentBatchCount = 0;
        uint currentBatchFirstSet = 0;
        int currentBatchDynamicOffsetCount = 0;

        for (uint currentSlot = 0; currentSlot < resourceSetCount; currentSlot++)
        {
            bool batchEnded = !resourceSetsChanged[currentSlot] || currentSlot == resourceSetCount - 1;

            if (resourceSetsChanged[currentSlot])
            {
                resourceSetsChanged[currentSlot] = false;
                descriptorSets[currentBatchCount] = resourceSets[currentSlot].Set.DescriptorSet;
                currentBatchCount += 1;

                int length = resourceSets[currentSlot].Offsets.Length;
                resourceSets[currentSlot].Offsets.CopyTo(new Span<uint>(dynamicOffsets + currentBatchDynamicOffsetCount,length));
                currentBatchDynamicOffsetCount += length;
            }

            if (batchEnded)
            {
                if (currentBatchCount != 0)
                {
                    // Flush current batch.
                    _commandBuffer.CmdBindDescriptorSets(
                        bindPoint,
                        pipelineLayout,
                        currentBatchFirstSet,
                        currentBatchCount,
                        descriptorSets,
                        (uint)currentBatchDynamicOffsetCount,
                        dynamicOffsets);
                }

                currentBatchCount = 0;
                currentBatchFirstSet = currentSlot + 1;
            }
        }
    }

    void PreDispatchCommand()
    {
        for (uint currentSlot = 0; currentSlot < _computePipeline.ResourceSetCount; currentSlot++)
        {
            ResourceSet set = _computeResourceSets[currentSlot].Set;

            TransitionImages(set.SampledTextures, ImageLayout.ShaderReadOnlyOptimal);
            TransitionImages(set.StorageTextures, ImageLayout.General);
            foreach (Image storageTex in set.StorageTextures)
            {
                if (storageTex.Usage.HasFlag(ImageUsageFlags.SampledBit))
                {
                    _preDrawSampledImages.Add(storageTex);
                }
            }
        }

        FlushNewResourceSets(
            _computeResourceSets,
            _computeResourceSetsChanged,
            _computePipeline.ResourceSetCount,
            PipelineBindPoint.Compute,
            _computePipeline.PipelineLayout);
    }


    void PreDrawCommand()
    {
        TransitionImages(_preDrawSampledImages, ImageLayout.ShaderReadOnlyOptimal);
        _preDrawSampledImages.Clear();

        FlushNewResourceSets(
            _graphicsResourceSets,
            _graphicsResourceSetsChanged,
            _graphicsPipeline.ResourceSetCount,
            PipelineBindPoint.Graphics,
            _graphicsPipeline.PipelineLayout);
    }

    public void UpdateBuffer<T>(
        Buffer buffer,
        uint bufferOffsetInBytes,
        T source
    ) where T : unmanaged
    {
        UpdateBuffer(buffer, bufferOffsetInBytes, new ReadOnlySpan<T>(ref source));
    }

    public void UpdateBuffer<T>(
        Buffer buffer,
        uint bufferOffsetInBytes,
        ref T source
    ) where T : unmanaged
    {
        UpdateBuffer(buffer, bufferOffsetInBytes,new ReadOnlySpan<T>(ref source));
    }

    public void UpdateBuffer<T>(
        Buffer buffer,
        uint bufferOffsetInBytes,
        in T[] source
    ) where T : unmanaged
    {
        UpdateBuffer(buffer, bufferOffsetInBytes, (ReadOnlySpan<T>)source);
    }

    public void UpdateBuffer<T>(
        Buffer buffer,
        uint bufferOffsetInBytes,
        in ReadOnlySpan<T> source
    ) where T : unmanaged
    {
        UpdateBuffer(buffer, bufferOffsetInBytes, MemoryMarshal.Cast<T,byte>( source));
    }

    public void UpdateBuffer(
        Buffer buffer,
        uint bufferOffsetInBytes,
        IntPtr source,
        uint sizeInBytes
    )
    {
        UpdateBuffer(buffer, bufferOffsetInBytes, new ReadOnlySpan<byte>(source.ToPointer(), (int)sizeInBytes));
    }

    public void UpdateBuffer(
        Buffer buffer,
        uint bufferOffsetInBytes,
        in ReadOnlySpan<byte> data
    )
    {
        int sizeInBytes = data.Length;
        if (bufferOffsetInBytes + sizeInBytes > buffer.SizeInBytes)
        {
            throw new Exception(
                $"The DeviceBuffer's capacity ({buffer.SizeInBytes}) is not large enough to store the amount of " +
                $"data specified ({sizeInBytes}) at the given offset ({bufferOffsetInBytes}).");
        }

        if (sizeInBytes == 0)
        {
            return;
        }

        Buffer stagingBuffer = GetStagingBuffer((uint)sizeInBytes);
        stagingBuffer.Fill(0, data);
        CopyBuffer(stagingBuffer, 0, buffer, bufferOffsetInBytes, (uint)sizeInBytes);
    }

    public void SetGraphicsResourceSet(uint slot, ResourceSet rs, Span<uint> dynamicOffsets)
    {
        if (!_graphicsResourceSets[slot].Equals(rs, dynamicOffsets))
        {
            _graphicsResourceSets[slot] = new BoundResourceSetInfo(rs, dynamicOffsets);
            _graphicsResourceSetsChanged[slot] = true;
        }
    }

    public void SetComputeResourceSet(uint slot, ResourceSet rs, Span<uint> dynamicOffsets)
    {
        if (!_computeResourceSets[slot].Equals(rs, dynamicOffsets))
        {
            _computeResourceSets[slot] = new BoundResourceSetInfo(rs, dynamicOffsets);
            _computeResourceSetsChanged[slot] = true;
        }
    }


    class StagingResourceInfo
    {
        public List<Buffer> BuffersUsed { get; } = [];

        public void Clear()
        {
            BuffersUsed.Clear();
        }
    }

    public readonly ref struct FramebufferScope(CommandList commandList)
    {
        public void Dispose()
        {
            commandList.EndFramebuffer();
        }
    }

    public readonly ref struct RenderPassScope(CommandList commandList)
    {
        public void Dispose()
        {
            commandList.EndRenderPass();
        }
    }

    public readonly ref struct DebugGroupScope(CommandList commandList)
    {
        public void Dispose()
        {
            commandList.PopDebugGroup();
        }
    }

    public readonly ref struct CommandBufferScope(CommandList commandList)
    {
        public void Dispose()
        {
            commandList.End();
        }
    }
}

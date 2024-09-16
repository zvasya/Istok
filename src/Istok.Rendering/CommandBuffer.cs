using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.Helpers;
using Semaphore = Silk.NET.Vulkan.Semaphore;

namespace Istok.Rendering;

public partial class CommandBuffer
{
    readonly Silk.NET.Vulkan.CommandBuffer _commandBuffer;
    readonly LogicalDevice _logicalDevice;
    readonly FenceManager _fenceManager;
    readonly Queue _graphicsQueue;
    string _name;
    public Silk.NET.Vulkan.CommandBuffer Buffer => _commandBuffer;

    public CommandBuffer(Silk.NET.Vulkan.CommandBuffer commandBuffer, LogicalDevice logicalDevice, FenceManager fenceManager, Queue graphicsQueue)
    {
        _commandBuffer = commandBuffer;
        _logicalDevice = logicalDevice;
        _fenceManager = fenceManager;
        _graphicsQueue = graphicsQueue;
    }

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            _logicalDevice.SetObjectName(ObjectType.CommandBuffer, (ulong)_commandBuffer.Handle, value);
        }
    }

    public void Begin(CommandBufferUsageFlags flags = CommandBufferUsageFlags.OneTimeSubmitBit)
    {
        CommandBufferBeginInfo beginInfo = new CommandBufferBeginInfo
        {
            SType = StructureType.CommandBufferBeginInfo,
            Flags = flags,
        };

        Result result = Engine.VK.BeginCommandBuffer(_commandBuffer, in beginInfo);
        Helpers.CheckErrors(result);
    }



    public void Reset(CommandBufferResetFlags flags = CommandBufferResetFlags.None)
    {
        Result resetResult = ResetCommandBuffer(flags);
        Helpers.CheckErrors(resetResult);
    }

    public void End()
    {
        Result result = EndCommandBuffer();
        Helpers.CheckErrors(result);
    }

    internal unsafe Fence SubmitCommandBuffer(
        uint waitSemaphoreCount,
        Semaphore* waitSemaphoresPtr,
        uint signalSemaphoreCount,
        Semaphore* signalSemaphoresPtr)
    {
        _fenceManager.CheckAllSubmittedFence();
        fixed (Silk.NET.Vulkan.CommandBuffer* pCommandBuffer = &_commandBuffer)
        {
            PipelineStageFlags waitDstStageMask = PipelineStageFlags.ColorAttachmentOutputBit;
            SubmitInfo si = new SubmitInfo
            {
                SType = StructureType.SubmitInfo,
                CommandBufferCount = 1,
                PCommandBuffers = pCommandBuffer,
                PWaitDstStageMask = &waitDstStageMask,
                PWaitSemaphores = waitSemaphoresPtr,
                WaitSemaphoreCount = waitSemaphoreCount,
                PSignalSemaphores = signalSemaphoresPtr,
                SignalSemaphoreCount = signalSemaphoreCount,
            };

            Fence submissionFence = _fenceManager.Get();

            _graphicsQueue.Submit(1, in si, submissionFence);

            submissionFence.OnReset += () => _fenceManager.Return(submissionFence);
            return submissionFence;
        }
    }

    internal void CopyBufferToImage(
        Buffer source,
        Image destination,
        uint dstX, uint dstY, uint dstZ,
        uint dstMipLevel,
        uint dstBaseArrayLayer,
        uint width, uint height, uint depth,
        uint layerCount)
    {
        destination.TransitionImageLayout(
            this,
            dstMipLevel,
            1,
            dstBaseArrayLayer,
            layerCount,
            ImageLayout.TransferDstOptimal);

        ImageSubresourceLayers dstSubresource = new ImageSubresourceLayers { AspectMask = ImageAspectFlags.ColorBit, LayerCount = layerCount, MipLevel = dstMipLevel, BaseArrayLayer = dstBaseArrayLayer };

        uint blockSize = destination.Format.IsCompressed() ? 4u : 1u;
        uint bufferRowLength = Math.Max(width, blockSize);
        uint bufferImageHeight = Math.Max(height, blockSize);

        BufferImageCopy regions = new BufferImageCopy
        {
            BufferOffset = 0,
            BufferRowLength = bufferRowLength,
            BufferImageHeight = bufferImageHeight,
            ImageExtent = new Extent3D { Width = width, Height = height, Depth = depth },
            ImageOffset = new Offset3D { X = (int)dstX, Y = (int)dstY, Z = (int)dstZ },
            ImageSubresource = dstSubresource
        };

        CmdCopyBufferToImage(source.DeviceBuffer, destination.DeviceImage, ImageLayout.TransferDstOptimal, 1, in regions);

        if (destination.Usage.HasFlag(ImageUsageFlags.SampledBit))
        {
            destination.TransitionImageLayout(
                this,
                dstMipLevel,
                1,
                dstBaseArrayLayer,
                layerCount,
                ImageLayout.ShaderReadOnlyOptimal);
        }
    }
}

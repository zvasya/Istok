using Silk.NET.Vulkan;

namespace Istok.Rendering;

public class CommandPool : IDisposable
{
    readonly LogicalDevice _logicalDevice;
    readonly Silk.NET.Vulkan.CommandPool _pool;
    readonly FenceManager _fenceManager;
    readonly Queue _graphicsQueue;

    string _name;

    public CommandPool(LogicalDevice logicalDevice, CommandPoolCreateFlags flags, FenceManager fenceManager, Queue graphicsQueue)
    {
        _logicalDevice = logicalDevice;
        _fenceManager = fenceManager;
        _graphicsQueue = graphicsQueue;

        CommandPoolCreateInfo commandPoolCreateInfo = new CommandPoolCreateInfo
        {
            SType = StructureType.CommandPoolCreateInfo,
            Flags = flags,
            QueueFamilyIndex = logicalDevice.PhysicalDevice.GraphicsQueueIndex,
        };
        unsafe
        {
            Result result = logicalDevice.CreateCommandPool(in commandPoolCreateInfo, null, out _pool);
            Helpers.CheckErrors(result);
        }
    }

    public  string Name
    {
        get => _name;
        set
        {
            _name = value;
            _logicalDevice.SetObjectName(ObjectType.CommandPool, _pool.Handle, value);
        }
    }

    public void Dispose()
    {
        unsafe
        {
            _logicalDevice.DestroyCommandPool(_pool, null);
        }
    }

    public CommandBuffer AllocateCommandBuffer()
    {
        CommandBufferAllocateInfo commandBufferAllocateInfo = new CommandBufferAllocateInfo
        {
            SType = StructureType.CommandBufferAllocateInfo,
            CommandPool = _pool,
            CommandBufferCount = 1,
            Level = CommandBufferLevel.Primary
        };
        Result result = _logicalDevice.AllocateCommandBuffers(in commandBufferAllocateInfo, out Silk.NET.Vulkan.CommandBuffer cb);

        Helpers.CheckErrors(result);
        return new CommandBuffer(cb, _logicalDevice, _fenceManager, _graphicsQueue);
    }

    public void Return(CommandBuffer commandBuffer)
    {
        Silk.NET.Vulkan.CommandBuffer buffer = commandBuffer.Buffer;
        _logicalDevice.FreeCommandBuffers(_pool, 1, in buffer);
    }
}

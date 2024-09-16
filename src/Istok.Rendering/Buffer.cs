using System.Runtime.InteropServices;
using Silk.NET.Vulkan;

namespace Istok.Rendering;

/// <summary>
/// represent linear arrays of data which are used for various purposes by binding them to a graphics or compute pipeline via descriptor sets or certain commands, or by directly specifying them as parameters to certain commands
/// </summary>
public unsafe class Buffer : IDisposable
{
    readonly StagingBuffersPool _stagingBuffersPool;
    readonly ThreadLocal<CommandPool> _commandPoolsStorage;
    readonly Silk.NET.Vulkan.Buffer _buffer;
    readonly DeviceMemory _memory;

    bool _destroyed;
    string _name;

    public const MemoryPropertyFlags DefaultDynamic = MemoryPropertyFlags.HostVisibleBit | MemoryPropertyFlags.HostCoherentBit;
    public const MemoryPropertyFlags DefaultStaging = MemoryPropertyFlags.HostCachedBit | DefaultDynamic;

    LogicalDevice LogicalDevice { get; }

    public bool IsDisposed => _destroyed;

    public bool IsDynamic => (Property & DefaultDynamic) == DefaultDynamic;

    public uint SizeInBytes { get; }

    /// <summary>
    /// Bitmask specifying allowed usage of a buffer
    /// </summary>
    public BufferUsageFlags Usage { get; }

    /// <summary>
    /// Bitmask specifying properties for a memory type
    /// </summary>
    public MemoryPropertyFlags Property { get; }

    public Silk.NET.Vulkan.Buffer DeviceBuffer => _buffer;

    public Buffer(LogicalDevice logicalDevice, StagingBuffersPool stagingBuffersPool, ThreadLocal<CommandPool> commandPoolsStorage, uint sizeInBytes, BufferUsageFlags usage, MemoryPropertyFlags property)
    {
        MemoryRequirements bufferMemoryRequirements;
        LogicalDevice = logicalDevice;
        _stagingBuffersPool = stagingBuffersPool;
        _commandPoolsStorage = commandPoolsStorage;
        SizeInBytes = sizeInBytes;
        Usage = usage;
        Property = property;

        BufferCreateInfo bufferCI = new BufferCreateInfo
        {
            SType = StructureType.BufferCreateInfo,
            Size = sizeInBytes,
            Usage = usage,
        };

        Result result = LogicalDevice.CreateBuffer(in bufferCI, null, out _buffer);
        Helpers.CheckErrors(result);

        LogicalDevice.GetBufferMemoryRequirements(_buffer, out bufferMemoryRequirements);

        MemoryAllocateInfo allocateInfo = new MemoryAllocateInfo
        {
            SType = StructureType.MemoryAllocateInfo, AllocationSize = bufferMemoryRequirements.Size, MemoryTypeIndex = LogicalDevice.PhysicalDevice.FindMemoryType(bufferMemoryRequirements.MemoryTypeBits, Property),
        };

        if (LogicalDevice.AllocateMemory(allocateInfo, null, out _memory) != Result.Success)
        {
            throw new Exception("failed to allocate vertex buffer memory!");
        }

        result = LogicalDevice.BindBufferMemory(_buffer, _memory, 0);
        Helpers.CheckErrors(result);
    }

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            LogicalDevice.SetObjectName(ObjectType.Buffer, _buffer.Handle, value);
        }
    }

    void FillInternal(uint bufferOffsetInBytes, ReadOnlySpan<byte> data)
    {
        void* dataPointer;
        int dataLength = data.Length;
        LogicalDevice.MapMemory(_memory, bufferOffsetInBytes, (ulong)dataLength, 0, &dataPointer);
        data.CopyTo(new Span<byte>(dataPointer, dataLength));
        LogicalDevice.UnmapMemory(_memory);
    }

    public void Dispose()
    {
        if (!_destroyed)
        {
            _destroyed = true;
            LogicalDevice.DestroyBuffer(_buffer, null);
            LogicalDevice.FreeMemory(_memory, null);
        }
    }

    public void Fill<T>(
        uint bufferOffsetInBytes,
        T source) where T : unmanaged
    {
        ReadOnlySpan<T> span = new ReadOnlySpan<T>(ref source);
        Fill(bufferOffsetInBytes, span);
    }

    public void Fill<T>(
        uint bufferOffsetInBytes,
        ref T source) where T : unmanaged
    {
        ReadOnlySpan<T> span = new ReadOnlySpan<T>(ref source);
        Fill(bufferOffsetInBytes, span);
    }

    public void Fill<T>(
        uint offset,
        ReadOnlySpan<T> source) where T : unmanaged
    {
        Fill(offset, MemoryMarshal.Cast<T, byte>(source));
    }

    public void Fill(
        uint offset,
        ReadOnlySpan<byte> source)
    {
        int sizeInBytes = source.Length;
        if (offset + sizeInBytes > SizeInBytes)
        {
            throw new ArgumentOutOfRangeException($"Failed to write {sizeInBytes} bytes with {offset} bytes offset in buffer {SizeInBytes} bytes size");
        }

        bool isDynamic = IsDynamic;
        if (isDynamic)
        {
            FillInternal(offset, source);
        }
        else
        {
            Buffer copySrcBuffer = _stagingBuffersPool.Get((uint)sizeInBytes);
            copySrcBuffer.FillInternal(0, source);

            CommandPool commandPool = _commandPoolsStorage.Value;
            CommandBuffer cb = commandPool.AllocateCommandBuffer();
            cb.Begin();

            BufferCopy copyRegion = new BufferCopy { DstOffset = offset, Size = (ulong)sizeInBytes };
            cb.CmdCopyBuffer(copySrcBuffer._buffer, _buffer, 1, in copyRegion);

            cb.End();
            cb.SubmitCommandBuffer(0, null, 0, null).OnReset += () =>
            {
                commandPool.Return(cb);
                _stagingBuffersPool.Return(copySrcBuffer);
            };
        }
    }

    public ResourcesSetBindingBuffer GetResourcesSetBound()
    {
        return new ResourcesSetBindingBuffer(GetDescriptor());
    }

    public ResourcesSetBindingBuffer GetResourcesSetBound(ulong offset, ulong range)
    {
        return new ResourcesSetBindingBuffer(GetDescriptor(offset, range));
    }

    public DescriptorBufferInfo GetDescriptor() => GetDescriptor(0, SizeInBytes);

    public DescriptorBufferInfo GetDescriptor(ulong offset, ulong range)
    {
        return new DescriptorBufferInfo { Buffer = _buffer, Offset = offset, Range = range };
    }
}

using System.Collections.Concurrent;
using Silk.NET.Vulkan;

namespace Istok.Rendering;

public class StagingBuffersPool : IDisposable
{
    const int MinSize = 64;

    readonly ConcurrentDictionary<uint, ConcurrentBag<Buffer>> _buffers = new ConcurrentDictionary<uint, ConcurrentBag<Buffer>>();
    readonly LogicalDevice _logicalDevice;
    readonly ThreadLocal<CommandPool> _commandPoolsStorage;

    public StagingBuffersPool(LogicalDevice logicalDevice, ThreadLocal<CommandPool> commandPoolsStorage)
    {
        _logicalDevice = logicalDevice;
        _commandPoolsStorage = commandPoolsStorage;
    }

    public Buffer Get(uint size)
    {
        size = Math.Max(MinSize, size);
        size = (uint)1 << (int)MathF.Ceiling(MathF.Log2(size));

        if (!_buffers.TryGetValue(size, out ConcurrentBag<Buffer> bag) || !bag.TryTake(out Buffer buffer))
        {
            buffer = new Buffer(_logicalDevice, this, _commandPoolsStorage, size, BufferUsageFlags.TransferSrcBit | BufferUsageFlags.TransferDstBit, Buffer.DefaultStaging);
        }

        return buffer;
    }

    public void Return(Buffer buffer)
    {
        uint size = buffer.SizeInBytes;
        uint checkSize = (uint)1 << (int)MathF.Ceiling(MathF.Log2(size));
        if (checkSize == size)
        {
            ConcurrentBag<Buffer> bag = _buffers.GetOrAdd(size, _ => []);
            bag.Add(buffer);
        }
        else
        {
            buffer.Dispose();
        }
    }

    public void Dispose()
    {
        foreach ((_, ConcurrentBag<Buffer> bag) in _buffers)
        {
            foreach (Buffer buffer in bag)
            {
                buffer.Dispose();
            }
            bag.Clear();
        }
        _buffers.Clear();
    }
}

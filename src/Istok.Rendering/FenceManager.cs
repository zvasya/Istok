using System.Collections.Concurrent;
using System.Collections.Frozen;

namespace Istok.Rendering;

public class FenceManager : IDisposable
{
    readonly ConcurrentQueue<Fence> _available = new ConcurrentQueue<Fence>();
    readonly ConcurrentDictionary<ulong, Fence> _submitted = new ConcurrentDictionary<ulong, Fence>();
    readonly LogicalDevice _logicalDevice;

    public FenceManager(LogicalDevice logicalDevice)
    {
        _logicalDevice = logicalDevice;
    }

    public void Return(Fence fence)
    {
        if (_submitted.TryRemove(fence.DeviceFence.Handle, out _))
            _available.Enqueue(fence);
    }

    public Fence Get()
    {
        if (!_available.TryDequeue(out Fence fence))
            fence = Fence.Create(_logicalDevice, false);

        _submitted.TryAdd(fence.DeviceFence.Handle, fence);

        return fence;
    }

    public void CheckAllSubmittedFence()
    {
        FrozenDictionary<ulong, Fence> d = _submitted.ToFrozenDictionary();
        foreach ((ulong _, Fence fence) in d)
        {
            if (fence.Signaled)
            {
                fence.Reset();
                Return(fence);
            }
        }
    }

    public void WaitAllSubmitted()
    {
        FrozenDictionary<ulong, Fence> d = _submitted.ToFrozenDictionary();

        int fenceCount = d.Count;
        unsafe
        {
            Silk.NET.Vulkan.Fence* fences = stackalloc Silk.NET.Vulkan.Fence[fenceCount];
            int i = 0;
            foreach ((ulong _, Fence fence) in d)
            {
                fences[i] = fence.DeviceFence;
                i++;
            }

            _logicalDevice.WaitForFences((uint)fenceCount, fences, true, ulong.MaxValue);
        }
        foreach ((ulong _, Fence fence) in d)
        {
            fence.Reset();
            Return(fence);
        }
    }

    public void Dispose()
    {
        foreach (Fence fence in _available)
        {
            fence.Dispose();
        }
    }
}

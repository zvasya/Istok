using Silk.NET.Vulkan;

namespace Istok.Rendering;

/// <summary>
/// A synchronization primitive that can be used to insert a dependency from a queue to the host
/// </summary>
public unsafe class Fence : IDisposable
{
    readonly Silk.NET.Vulkan.Fence _fence;
    readonly LogicalDevice _logicalDevice;
    public event Action OnReset;
    string _name;

    Fence(LogicalDevice logicalDevice, Silk.NET.Vulkan.Fence fence)
    {
        _logicalDevice = logicalDevice;
        _fence = fence;
    }

    public Silk.NET.Vulkan.Fence DeviceFence => _fence;

    /// <summary>
    /// Status of a fence
    /// </summary>
    public bool Signaled => _logicalDevice.GetFenceStatus(_fence) == Result.Success;

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            _logicalDevice.SetObjectName(ObjectType.Fence, DeviceFence.Handle, value);
        }
    }

    public bool IsDisposed { get; private set; }

    public void Dispose()
    {
        if (!IsDisposed)
        {
            _logicalDevice.DestroyFence(_fence, null);
            IsDisposed = true;
        }
    }

    public static Fence Create(LogicalDevice logicalDevice, bool signaled)
    {
        FenceCreateInfo fenceCI = new FenceCreateInfo
        {
            SType = StructureType.FenceCreateInfo,
            Flags = signaled ? FenceCreateFlags.SignaledBit : FenceCreateFlags.None,
        };
        Result result = logicalDevice.CreateFence(in fenceCI, null, out Silk.NET.Vulkan.Fence fence);
        Helpers.CheckErrors(result);
        return new Fence(logicalDevice, fence);
    }

    /// <summary>
    /// Resets fence (set unsignaled status)
    /// </summary>
    public void Reset()
    {
        Result result = _logicalDevice.ResetFences(1, in _fence);
        Helpers.CheckErrors(result);
        OnReset?.Invoke();
        OnReset = null;
    }

    /// <summary>
    /// Wait for fence to enter the signaled state on the host
    /// </summary>
    /// <param name="timeout">timeout period in units of nanoseconds</param>
    /// <returns>return True if was signaled or returns False after the timeout has expired</returns>
    public bool Wait(ulong timeout = ulong.MaxValue)
    {
        Result result = _logicalDevice.WaitForFences(1, in _fence, true, timeout);
        if (result == Result.Timeout)
            return false;
        Helpers.CheckErrors(result);
        return true;
    }
}

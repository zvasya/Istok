using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.KHR;

namespace Istok.Rendering;

public partial class Queue
{
    readonly KhrSwapchain _khrSwapchain;
    readonly Silk.NET.Vulkan.Queue _queue;
    readonly object _lock = new object();

    public uint FamilyIndex { get; }
    public uint Index { get; }
    Queue(Silk.NET.Vulkan.Queue queue, KhrSwapchain khrSwapchain, uint familyIndex, uint index)
    {
        _queue = queue;
        _khrSwapchain = khrSwapchain;
        FamilyIndex = familyIndex;
        Index = index;
    }

    public static Queue Create(Silk.NET.Vulkan.Queue queue, KhrSwapchain khrSwapchain, uint familyIndex, uint index)
    {
        return new Queue(queue, khrSwapchain, familyIndex, index);
    }

    public void Submit(uint submitCount, in SubmitInfo submits, Fence submissionFence)
    {
        lock (_lock)
        {
            Result result = QueueSubmit(submitCount, in submits, submissionFence.DeviceFence);
            Helpers.CheckErrors(result);
        }
    }

    public void WaitIdle()
    {
        lock (_lock)
        {
            QueueWaitIdle();
        }
    }

    public void Present(in PresentInfoKHR presentInfo)
    {
        lock (_lock)
        {
            QueuePresent(in presentInfo);
        }
    }
}

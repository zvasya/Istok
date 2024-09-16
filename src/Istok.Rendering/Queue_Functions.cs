using Silk.NET.Vulkan;
using static Istok.Rendering.Engine;

namespace Istok.Rendering;

public partial class Queue
{
    public unsafe Result QueueSubmit2(
        uint submitCount,
        SubmitInfo2* pSubmits,
        Silk.NET.Vulkan.Fence fence)
    {
        return VK.QueueSubmit2(
            _queue,
            submitCount,
            pSubmits,
            fence);
    }

    public Result QueueSubmit2(
        uint submitCount,
        in SubmitInfo2 pSubmits,
        Silk.NET.Vulkan.Fence fence)
    {
        return VK.QueueSubmit2(
            _queue,
            submitCount,
            in pSubmits,
            fence);
    }


    public unsafe Result QueueBindSparse(
        uint bindInfoCount,
        BindSparseInfo* pBindInfo,
        Silk.NET.Vulkan.Fence fence)
    {
        return VK.QueueBindSparse(
            _queue,
            bindInfoCount,
            pBindInfo,
            fence);
    }

    public Result QueueBindSparse(
        uint bindInfoCount,
        in BindSparseInfo pBindInfo,
        Silk.NET.Vulkan.Fence fence)
    {
        return VK.QueueBindSparse(
            _queue,
            bindInfoCount,
            in pBindInfo,
            fence);
    }

    public unsafe Result QueueSubmit(
        uint submitCount,
        SubmitInfo* pSubmits,
        Silk.NET.Vulkan.Fence fence)
    {
        return VK.QueueSubmit(
            _queue,
            submitCount,
            pSubmits,
            fence);
    }

    public Result QueueSubmit(
        uint submitCount,
        in SubmitInfo pSubmits,
        Silk.NET.Vulkan.Fence fence)
    {
        return VK.QueueSubmit(
            _queue,
            submitCount,
            in pSubmits,
            fence);
    }


    public Result QueueWaitIdle()
    {
        return VK.QueueWaitIdle(
            _queue);
    }
}

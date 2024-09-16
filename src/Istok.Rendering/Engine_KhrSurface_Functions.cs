using Silk.NET.Vulkan;

// ReSharper disable UnusedMember.Global

namespace Istok.Rendering;

public partial class Engine
{
    public unsafe void DestroySurface(SurfaceKHR surface, AllocationCallbacks* pAllocator)
    {
        KhrSurfaceExtension.DestroySurface(_instance, surface, pAllocator);
    }

    public void DestroySurface(SurfaceKHR surface, in AllocationCallbacks pAllocator)
    {
        KhrSurfaceExtension.DestroySurface(_instance, surface, in pAllocator);
    }
}

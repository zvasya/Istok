using Silk.NET.Core;
using Silk.NET.Core.Native;
using Silk.NET.Vulkan;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Istok.Rendering;

public partial class Engine
{
    public bool TryGetDeviceExtension<T>(Device device, out T ext, string? layer = null) where T : NativeExtension<Vk>
    {
        return VK.TryGetDeviceExtension(_instance, device, out ext, layer);
    }

    public bool TryGetInstanceExtension<T>(out T ext, string? layer = null) where T : NativeExtension<Vk>
    {
        return VK.TryGetInstanceExtension(_instance, out ext, layer);
    }

    public unsafe void DestroyInstance(AllocationCallbacks* pAllocator)
    {
        VK.DestroyInstance(_instance, pAllocator);
    }

    public void DestroyInstance(in AllocationCallbacks pAllocator)
    {
        VK.DestroyInstance(_instance, in pAllocator);
    }

    public unsafe Result EnumeratePhysicalDeviceGroups(uint* pPhysicalDeviceGroupCount, PhysicalDeviceGroupProperties* pPhysicalDeviceGroupProperties)
    {
        return VK.EnumeratePhysicalDeviceGroups(_instance, pPhysicalDeviceGroupCount, pPhysicalDeviceGroupProperties);
    }

    public unsafe Result EnumeratePhysicalDeviceGroups(uint* pPhysicalDeviceGroupCount, ref PhysicalDeviceGroupProperties pPhysicalDeviceGroupProperties)
    {
        return VK.EnumeratePhysicalDeviceGroups(_instance, pPhysicalDeviceGroupCount, ref pPhysicalDeviceGroupProperties);
    }

    public unsafe Result EnumeratePhysicalDeviceGroups(ref uint pPhysicalDeviceGroupCount, PhysicalDeviceGroupProperties* pPhysicalDeviceGroupProperties)
    {
        return VK.EnumeratePhysicalDeviceGroups(_instance, ref pPhysicalDeviceGroupCount, pPhysicalDeviceGroupProperties);
    }

    public Result EnumeratePhysicalDeviceGroups(ref uint pPhysicalDeviceGroupCount, ref PhysicalDeviceGroupProperties pPhysicalDeviceGroupProperties)
    {
        return VK.EnumeratePhysicalDeviceGroups(_instance, ref pPhysicalDeviceGroupCount, ref pPhysicalDeviceGroupProperties);
    }

    public unsafe Result EnumeratePhysicalDevices(uint* pPhysicalDeviceCount, Silk.NET.Vulkan.PhysicalDevice* pPhysicalDevices)
    {
        return VK.EnumeratePhysicalDevices(_instance, pPhysicalDeviceCount, pPhysicalDevices);
    }

    public unsafe Result EnumeratePhysicalDevices(uint* pPhysicalDeviceCount, ref Silk.NET.Vulkan.PhysicalDevice pPhysicalDevices)
    {
        return VK.EnumeratePhysicalDevices(_instance, pPhysicalDeviceCount, ref pPhysicalDevices);
    }

    public unsafe Result EnumeratePhysicalDevices(ref uint pPhysicalDeviceCount, Silk.NET.Vulkan.PhysicalDevice* pPhysicalDevices)
    {
        return VK.EnumeratePhysicalDevices(_instance, ref pPhysicalDeviceCount, pPhysicalDevices);
    }

    public Result EnumeratePhysicalDevices(ref uint pPhysicalDeviceCount, ref Silk.NET.Vulkan.PhysicalDevice pPhysicalDevices)
    {
        return VK.EnumeratePhysicalDevices(_instance, ref pPhysicalDeviceCount, ref pPhysicalDevices);
    }

    public unsafe PfnVoidFunction GetInstanceProcAddr(byte* pName)
    {
        return VK.GetInstanceProcAddr(_instance, pName);
    }

    public PfnVoidFunction GetInstanceProcAddr(in byte pName)
    {
        return VK.GetInstanceProcAddr(_instance, in pName);
    }

    public PfnVoidFunction GetInstanceProcAddr(string pName)
    {
        return VK.GetInstanceProcAddr(_instance, pName);
    }
}

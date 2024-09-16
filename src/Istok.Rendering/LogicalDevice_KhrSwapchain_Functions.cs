using Silk.NET.Vulkan;
using Semaphore = Silk.NET.Vulkan.Semaphore;

// ReSharper disable UnusedMember.Global

namespace Istok.Rendering;

public partial class LogicalDevice
{
    public unsafe Result AcquireNextImage(SwapchainKHR swapchain, ulong timeout, Semaphore semaphore, Silk.NET.Vulkan.Fence fence, uint* pImageIndex)
    {
        return KhrSwapchain.AcquireNextImage(_device, swapchain, timeout, semaphore, fence, pImageIndex);
    }

    public Result AcquireNextImage(SwapchainKHR swapchain, ulong timeout, Semaphore semaphore, Silk.NET.Vulkan.Fence fence, ref uint pImageIndex)
    {
        return KhrSwapchain.AcquireNextImage(_device, swapchain, timeout, semaphore, fence, ref pImageIndex);
    }

    public unsafe Result AcquireNextImage2(AcquireNextImageInfoKHR* pAcquireInfo, uint* pImageIndex)
    {
        return KhrSwapchain.AcquireNextImage2(_device, pAcquireInfo, pImageIndex);
    }

    public unsafe Result AcquireNextImage2(AcquireNextImageInfoKHR* pAcquireInfo, ref uint pImageIndex)
    {
        return KhrSwapchain.AcquireNextImage2(_device, pAcquireInfo, ref pImageIndex);
    }

    public unsafe Result AcquireNextImage2(in AcquireNextImageInfoKHR pAcquireInfo, uint* pImageIndex)
    {
        return KhrSwapchain.AcquireNextImage2(_device, in pAcquireInfo, pImageIndex);
    }

    public Result AcquireNextImage2(in AcquireNextImageInfoKHR pAcquireInfo, ref uint pImageIndex)
    {
        return KhrSwapchain.AcquireNextImage2(_device, in pAcquireInfo, ref pImageIndex);
    }

    public unsafe Result CreateSwapchain(SwapchainCreateInfoKHR* pCreateInfo, AllocationCallbacks* pAllocator, SwapchainKHR* pSwapchain)
    {
        return KhrSwapchain.CreateSwapchain(_device, pCreateInfo, pAllocator, pSwapchain);
    }

    public unsafe Result CreateSwapchain(SwapchainCreateInfoKHR* pCreateInfo, AllocationCallbacks* pAllocator, out SwapchainKHR pSwapchain)
    {
        return KhrSwapchain.CreateSwapchain(_device, pCreateInfo, pAllocator, out pSwapchain);
    }

    public unsafe Result CreateSwapchain(SwapchainCreateInfoKHR* pCreateInfo, in AllocationCallbacks pAllocator, SwapchainKHR* pSwapchain)
    {
        return KhrSwapchain.CreateSwapchain(_device, pCreateInfo, in pAllocator, pSwapchain);
    }

    public unsafe Result CreateSwapchain(SwapchainCreateInfoKHR* pCreateInfo, in AllocationCallbacks pAllocator, out SwapchainKHR pSwapchain)
    {
        return KhrSwapchain.CreateSwapchain(_device, pCreateInfo, in pAllocator, out pSwapchain);
    }

    public unsafe Result CreateSwapchain(in SwapchainCreateInfoKHR pCreateInfo, AllocationCallbacks* pAllocator, SwapchainKHR* pSwapchain)
    {
        return KhrSwapchain.CreateSwapchain(_device, in pCreateInfo, pAllocator, pSwapchain);
    }

    public unsafe Result CreateSwapchain(in SwapchainCreateInfoKHR pCreateInfo, AllocationCallbacks* pAllocator, out SwapchainKHR pSwapchain)
    {
        return KhrSwapchain.CreateSwapchain(_device, in pCreateInfo, pAllocator, out pSwapchain);
    }

    public unsafe Result CreateSwapchain(in SwapchainCreateInfoKHR pCreateInfo, in AllocationCallbacks pAllocator, SwapchainKHR* pSwapchain)
    {
        return KhrSwapchain.CreateSwapchain(_device, in pCreateInfo, in pAllocator, pSwapchain);
    }

    public Result CreateSwapchain(in SwapchainCreateInfoKHR pCreateInfo, in AllocationCallbacks pAllocator, out SwapchainKHR pSwapchain)
    {
        return KhrSwapchain.CreateSwapchain(_device, in pCreateInfo, in pAllocator, out pSwapchain);
    }

    public unsafe void DestroySwapchain(SwapchainKHR swapchain, AllocationCallbacks* pAllocator)
    {
        KhrSwapchain.DestroySwapchain(_device, swapchain, pAllocator);
    }

    public void DestroySwapchain(SwapchainKHR swapchain, in AllocationCallbacks pAllocator)
    {
        KhrSwapchain.DestroySwapchain(_device, swapchain, in pAllocator);
    }

    public unsafe Result GetDeviceGroupPresentCapabilities(DeviceGroupPresentCapabilitiesKHR* pDeviceGroupPresentCapabilities)
    {
        return KhrSwapchain.GetDeviceGroupPresentCapabilities(_device, pDeviceGroupPresentCapabilities);
    }

    public Result GetDeviceGroupPresentCapabilities(out DeviceGroupPresentCapabilitiesKHR pDeviceGroupPresentCapabilities)
    {
        return KhrSwapchain.GetDeviceGroupPresentCapabilities(_device, out pDeviceGroupPresentCapabilities);
    }

    public unsafe Result GetDeviceGroupSurfacePresentModes(SurfaceKHR surface, DeviceGroupPresentModeFlagsKHR* pModes)
    {
        return KhrSwapchain.GetDeviceGroupSurfacePresentModes(_device, surface, pModes);
    }

    public Result GetDeviceGroupSurfacePresentModes(SurfaceKHR surface, out DeviceGroupPresentModeFlagsKHR pModes)
    {
        return KhrSwapchain.GetDeviceGroupSurfacePresentModes(_device, surface, out pModes);
    }

    public unsafe Result GetSwapchainImages(SwapchainKHR swapchain, uint* pSwapchainImageCount, Silk.NET.Vulkan.Image* pSwapchainImages)
    {
        return KhrSwapchain.GetSwapchainImages(_device, swapchain, pSwapchainImageCount, pSwapchainImages);
    }

    public unsafe Result GetSwapchainImages(SwapchainKHR swapchain, uint* pSwapchainImageCount, out Silk.NET.Vulkan.Image pSwapchainImages)
    {
        return KhrSwapchain.GetSwapchainImages(_device, swapchain, pSwapchainImageCount, out pSwapchainImages);
    }

    public unsafe Result GetSwapchainImages(SwapchainKHR swapchain, ref uint pSwapchainImageCount, Silk.NET.Vulkan.Image* pSwapchainImages)
    {
        return KhrSwapchain.GetSwapchainImages(_device, swapchain, ref pSwapchainImageCount, pSwapchainImages);
    }

    public Result GetSwapchainImages(SwapchainKHR swapchain, ref uint pSwapchainImageCount, out Silk.NET.Vulkan.Image pSwapchainImages)
    {
        return KhrSwapchain.GetSwapchainImages(_device, swapchain, ref pSwapchainImageCount, out pSwapchainImages);
    }
}

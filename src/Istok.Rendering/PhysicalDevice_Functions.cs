using Silk.NET.Vulkan;
using static Istok.Rendering.Engine;

namespace Istok.Rendering;

public partial class PhysicalDevice
{
    public unsafe void GetPhysicalDeviceFeatures2(PhysicalDeviceFeatures2* pFeatures)
    {
        VK.GetPhysicalDeviceFeatures2(_physicalDevice, pFeatures);
    }

    public unsafe void GetPhysicalDeviceMemoryProperties2(PhysicalDeviceMemoryProperties2* pMemoryProperties)
    {
        VK.GetPhysicalDeviceMemoryProperties2(_physicalDevice, pMemoryProperties);
    }

    public unsafe void GetPhysicalDeviceProperties2(PhysicalDeviceProperties2* pProperties)
    {
        VK.GetPhysicalDeviceProperties2(_physicalDevice, pProperties);
    }

    public unsafe void GetPhysicalDeviceFeatures(PhysicalDeviceFeatures* pFeatures)
    {
        VK.GetPhysicalDeviceFeatures(_physicalDevice, pFeatures);
    }

    public unsafe void GetPhysicalDeviceMemoryProperties(PhysicalDeviceMemoryProperties* pMemoryProperties)
    {
        VK.GetPhysicalDeviceMemoryProperties(_physicalDevice, pMemoryProperties);
    }

    public unsafe void GetPhysicalDeviceProperties(PhysicalDeviceProperties* pProperties)
    {
        VK.GetPhysicalDeviceProperties(_physicalDevice, pProperties);
    }

    public unsafe void GetPhysicalDeviceExternalBufferProperties(PhysicalDeviceExternalBufferInfo* pExternalBufferInfo, ExternalBufferProperties* pExternalBufferProperties)
    {
        VK.GetPhysicalDeviceExternalBufferProperties(_physicalDevice, pExternalBufferInfo, pExternalBufferProperties);
    }

    public unsafe void GetPhysicalDeviceExternalFenceProperties(PhysicalDeviceExternalFenceInfo* pExternalFenceInfo, ExternalFenceProperties* pExternalFenceProperties)
    {
        VK.GetPhysicalDeviceExternalFenceProperties(_physicalDevice, pExternalFenceInfo, pExternalFenceProperties);
    }

    public unsafe void GetPhysicalDeviceExternalSemaphoreProperties(PhysicalDeviceExternalSemaphoreInfo* pExternalSemaphoreInfo, ExternalSemaphoreProperties* pExternalSemaphoreProperties)
    {
        VK.GetPhysicalDeviceExternalSemaphoreProperties(_physicalDevice, pExternalSemaphoreInfo, pExternalSemaphoreProperties);
    }

    public unsafe void GetPhysicalDeviceFormatProperties2(Format format, FormatProperties2* pFormatProperties)
    {
        VK.GetPhysicalDeviceFormatProperties2(_physicalDevice, format, pFormatProperties);
    }

    public unsafe void GetPhysicalDeviceQueueFamilyProperties2(uint* pQueueFamilyPropertyCount, QueueFamilyProperties2* pQueueFamilyProperties)
    {
        VK.GetPhysicalDeviceQueueFamilyProperties2(_physicalDevice, pQueueFamilyPropertyCount, pQueueFamilyProperties);
    }

    public unsafe void GetPhysicalDeviceFormatProperties(Format format, FormatProperties* pFormatProperties)
    {
        VK.GetPhysicalDeviceFormatProperties(_physicalDevice, format, pFormatProperties);
    }

    public unsafe void GetPhysicalDeviceQueueFamilyProperties(uint* pQueueFamilyPropertyCount, QueueFamilyProperties* pQueueFamilyProperties)
    {
        VK.GetPhysicalDeviceQueueFamilyProperties(_physicalDevice, pQueueFamilyPropertyCount, pQueueFamilyProperties);
    }

    public unsafe void GetPhysicalDeviceSparseImageFormatProperties2(PhysicalDeviceSparseImageFormatInfo2* pFormatInfo, uint* pPropertyCount, SparseImageFormatProperties2* pProperties)
    {
        VK.GetPhysicalDeviceSparseImageFormatProperties2(_physicalDevice, pFormatInfo, pPropertyCount, pProperties);
    }

    public unsafe void GetPhysicalDeviceSparseImageFormatProperties(Format format, ImageType type, SampleCountFlags samples, ImageUsageFlags usage, ImageTiling tiling, uint* pPropertyCount,
        SparseImageFormatProperties* pProperties)
    {
        VK.GetPhysicalDeviceSparseImageFormatProperties(_physicalDevice, format, type, samples, usage, tiling, pPropertyCount, pProperties);
    }

    public unsafe void GetPhysicalDeviceExternalBufferProperties(PhysicalDeviceExternalBufferInfo* pExternalBufferInfo, out ExternalBufferProperties pExternalBufferProperties)
    {
        VK.GetPhysicalDeviceExternalBufferProperties(_physicalDevice, pExternalBufferInfo, out pExternalBufferProperties);
    }

    public unsafe void GetPhysicalDeviceExternalBufferProperties(in PhysicalDeviceExternalBufferInfo pExternalBufferInfo, ExternalBufferProperties* pExternalBufferProperties)
    {
        VK.GetPhysicalDeviceExternalBufferProperties(_physicalDevice, in pExternalBufferInfo, pExternalBufferProperties);
    }

    public void GetPhysicalDeviceExternalBufferProperties(in PhysicalDeviceExternalBufferInfo pExternalBufferInfo, out ExternalBufferProperties pExternalBufferProperties)
    {
        VK.GetPhysicalDeviceExternalBufferProperties(_physicalDevice, in pExternalBufferInfo, out pExternalBufferProperties);
    }

    public unsafe void GetPhysicalDeviceExternalFenceProperties(PhysicalDeviceExternalFenceInfo* pExternalFenceInfo, out ExternalFenceProperties pExternalFenceProperties)
    {
        VK.GetPhysicalDeviceExternalFenceProperties(_physicalDevice, pExternalFenceInfo, out pExternalFenceProperties);
    }

    public unsafe void GetPhysicalDeviceExternalFenceProperties(in PhysicalDeviceExternalFenceInfo pExternalFenceInfo, ExternalFenceProperties* pExternalFenceProperties)
    {
        VK.GetPhysicalDeviceExternalFenceProperties(_physicalDevice, in pExternalFenceInfo, pExternalFenceProperties);
    }

    public void GetPhysicalDeviceExternalFenceProperties(in PhysicalDeviceExternalFenceInfo pExternalFenceInfo, out ExternalFenceProperties pExternalFenceProperties)
    {
        VK.GetPhysicalDeviceExternalFenceProperties(_physicalDevice, in pExternalFenceInfo, out pExternalFenceProperties);
    }

    public unsafe void GetPhysicalDeviceExternalSemaphoreProperties(PhysicalDeviceExternalSemaphoreInfo* pExternalSemaphoreInfo, out ExternalSemaphoreProperties pExternalSemaphoreProperties)
    {
        VK.GetPhysicalDeviceExternalSemaphoreProperties(_physicalDevice, pExternalSemaphoreInfo, out pExternalSemaphoreProperties);
    }

    public unsafe void GetPhysicalDeviceExternalSemaphoreProperties(in PhysicalDeviceExternalSemaphoreInfo pExternalSemaphoreInfo, ExternalSemaphoreProperties* pExternalSemaphoreProperties)
    {
        VK.GetPhysicalDeviceExternalSemaphoreProperties(_physicalDevice, in pExternalSemaphoreInfo, pExternalSemaphoreProperties);
    }

    public void GetPhysicalDeviceExternalSemaphoreProperties(in PhysicalDeviceExternalSemaphoreInfo pExternalSemaphoreInfo, out ExternalSemaphoreProperties pExternalSemaphoreProperties)
    {
        VK.GetPhysicalDeviceExternalSemaphoreProperties(_physicalDevice, in pExternalSemaphoreInfo, out pExternalSemaphoreProperties);
    }

    public void GetPhysicalDeviceFeatures2(out PhysicalDeviceFeatures2 pFeatures)
    {
        VK.GetPhysicalDeviceFeatures2(_physicalDevice, out pFeatures);
    }

    public void GetPhysicalDeviceFormatProperties2(Format format, out FormatProperties2 pFormatProperties)
    {
        VK.GetPhysicalDeviceFormatProperties2(_physicalDevice, format, out pFormatProperties);
    }

    public void GetPhysicalDeviceMemoryProperties2(out PhysicalDeviceMemoryProperties2 pMemoryProperties)
    {
        VK.GetPhysicalDeviceMemoryProperties2(_physicalDevice, out pMemoryProperties);
    }

    public void GetPhysicalDeviceProperties2(out PhysicalDeviceProperties2 pProperties)
    {
        VK.GetPhysicalDeviceProperties2(_physicalDevice, out pProperties);
    }

    public unsafe void GetPhysicalDeviceQueueFamilyProperties2(uint* pQueueFamilyPropertyCount, out QueueFamilyProperties2 pQueueFamilyProperties)
    {
        VK.GetPhysicalDeviceQueueFamilyProperties2(_physicalDevice, pQueueFamilyPropertyCount, out pQueueFamilyProperties);
    }

    public unsafe void GetPhysicalDeviceQueueFamilyProperties2(ref uint pQueueFamilyPropertyCount, QueueFamilyProperties2* pQueueFamilyProperties)
    {
        VK.GetPhysicalDeviceQueueFamilyProperties2(_physicalDevice, ref pQueueFamilyPropertyCount, pQueueFamilyProperties);
    }

    public void GetPhysicalDeviceQueueFamilyProperties2(ref uint pQueueFamilyPropertyCount, out QueueFamilyProperties2 pQueueFamilyProperties)
    {
        VK.GetPhysicalDeviceQueueFamilyProperties2(_physicalDevice, ref pQueueFamilyPropertyCount, out pQueueFamilyProperties);
    }

    public unsafe void GetPhysicalDeviceSparseImageFormatProperties2(PhysicalDeviceSparseImageFormatInfo2* pFormatInfo, uint* pPropertyCount, out SparseImageFormatProperties2 pProperties)
    {
        VK.GetPhysicalDeviceSparseImageFormatProperties2(_physicalDevice, pFormatInfo, pPropertyCount, out pProperties);
    }

    public unsafe void GetPhysicalDeviceSparseImageFormatProperties2(PhysicalDeviceSparseImageFormatInfo2* pFormatInfo, ref uint pPropertyCount, SparseImageFormatProperties2* pProperties)
    {
        VK.GetPhysicalDeviceSparseImageFormatProperties2(_physicalDevice, pFormatInfo, ref pPropertyCount, pProperties);
    }

    public unsafe void GetPhysicalDeviceSparseImageFormatProperties2(PhysicalDeviceSparseImageFormatInfo2* pFormatInfo, ref uint pPropertyCount, out SparseImageFormatProperties2 pProperties)
    {
        VK.GetPhysicalDeviceSparseImageFormatProperties2(_physicalDevice, pFormatInfo, ref pPropertyCount, out pProperties);
    }

    public unsafe void GetPhysicalDeviceSparseImageFormatProperties2(in PhysicalDeviceSparseImageFormatInfo2 pFormatInfo, uint* pPropertyCount, SparseImageFormatProperties2* pProperties)
    {
        VK.GetPhysicalDeviceSparseImageFormatProperties2(_physicalDevice, in pFormatInfo, pPropertyCount, pProperties);
    }

    public unsafe void GetPhysicalDeviceSparseImageFormatProperties2(in PhysicalDeviceSparseImageFormatInfo2 pFormatInfo, uint* pPropertyCount, out SparseImageFormatProperties2 pProperties)
    {
        VK.GetPhysicalDeviceSparseImageFormatProperties2(_physicalDevice, in pFormatInfo, pPropertyCount, out pProperties);
    }

    public unsafe void GetPhysicalDeviceSparseImageFormatProperties2(in PhysicalDeviceSparseImageFormatInfo2 pFormatInfo, ref uint pPropertyCount, SparseImageFormatProperties2* pProperties)
    {
        VK.GetPhysicalDeviceSparseImageFormatProperties2(_physicalDevice, in pFormatInfo, ref pPropertyCount, pProperties);
    }

    public void GetPhysicalDeviceSparseImageFormatProperties2(in PhysicalDeviceSparseImageFormatInfo2 pFormatInfo, ref uint pPropertyCount, out SparseImageFormatProperties2 pProperties)
    {
        VK.GetPhysicalDeviceSparseImageFormatProperties2(_physicalDevice, in pFormatInfo, ref pPropertyCount, out pProperties);
    }

    public void GetPhysicalDeviceFeatures(out PhysicalDeviceFeatures pFeatures)
    {
        VK.GetPhysicalDeviceFeatures(_physicalDevice, out pFeatures);
    }

    public void GetPhysicalDeviceFormatProperties(Format format, out FormatProperties pFormatProperties)
    {
        VK.GetPhysicalDeviceFormatProperties(_physicalDevice, format, out pFormatProperties);
    }

    public void GetPhysicalDeviceMemoryProperties(out PhysicalDeviceMemoryProperties pMemoryProperties)
    {
        VK.GetPhysicalDeviceMemoryProperties(_physicalDevice, out pMemoryProperties);
    }

    public void GetPhysicalDeviceProperties(out PhysicalDeviceProperties pProperties)
    {
        VK.GetPhysicalDeviceProperties(_physicalDevice, out pProperties);
    }

    public unsafe void GetPhysicalDeviceQueueFamilyProperties(uint* pQueueFamilyPropertyCount, out QueueFamilyProperties pQueueFamilyProperties)
    {
        VK.GetPhysicalDeviceQueueFamilyProperties(_physicalDevice, pQueueFamilyPropertyCount, out pQueueFamilyProperties);
    }

    public unsafe void GetPhysicalDeviceQueueFamilyProperties(ref uint pQueueFamilyPropertyCount, QueueFamilyProperties* pQueueFamilyProperties)
    {
        VK.GetPhysicalDeviceQueueFamilyProperties(_physicalDevice, ref pQueueFamilyPropertyCount, pQueueFamilyProperties);
    }

    public void GetPhysicalDeviceQueueFamilyProperties(ref uint pQueueFamilyPropertyCount, out QueueFamilyProperties pQueueFamilyProperties)
    {
        VK.GetPhysicalDeviceQueueFamilyProperties(_physicalDevice, ref pQueueFamilyPropertyCount, out pQueueFamilyProperties);
    }

    public unsafe void GetPhysicalDeviceSparseImageFormatProperties(Format format, ImageType type, SampleCountFlags samples, ImageUsageFlags usage, ImageTiling tiling, uint* pPropertyCount,
        out SparseImageFormatProperties pProperties)
    {
        VK.GetPhysicalDeviceSparseImageFormatProperties(_physicalDevice, format, type, samples, usage, tiling, pPropertyCount, out pProperties);
    }

    public unsafe void GetPhysicalDeviceSparseImageFormatProperties(Format format, ImageType type, SampleCountFlags samples, ImageUsageFlags usage, ImageTiling tiling, ref uint pPropertyCount,
        SparseImageFormatProperties* pProperties)
    {
        VK.GetPhysicalDeviceSparseImageFormatProperties(_physicalDevice, format, type, samples, usage, tiling, ref pPropertyCount, pProperties);
    }

    public void GetPhysicalDeviceSparseImageFormatProperties(Format format, ImageType type, SampleCountFlags samples, ImageUsageFlags usage, ImageTiling tiling, ref uint pPropertyCount,
        out SparseImageFormatProperties pProperties)
    {
        VK.GetPhysicalDeviceSparseImageFormatProperties(_physicalDevice, format, type, samples, usage, tiling, ref pPropertyCount, out pProperties);
    }

    public unsafe Result GetPhysicalDeviceToolProperties(uint* pToolCount, PhysicalDeviceToolProperties* pToolProperties)
    {
        return VK.GetPhysicalDeviceToolProperties(_physicalDevice, pToolCount, pToolProperties);
    }

    public unsafe Result GetPhysicalDeviceToolProperties(uint* pToolCount, out PhysicalDeviceToolProperties pToolProperties)
    {
        return VK.GetPhysicalDeviceToolProperties(_physicalDevice, pToolCount, out pToolProperties);
    }

    public unsafe Result GetPhysicalDeviceToolProperties(ref uint pToolCount, PhysicalDeviceToolProperties* pToolProperties)
    {
        return VK.GetPhysicalDeviceToolProperties(_physicalDevice, ref pToolCount, pToolProperties);
    }

    public Result GetPhysicalDeviceToolProperties(ref uint pToolCount, out PhysicalDeviceToolProperties pToolProperties)
    {
        return VK.GetPhysicalDeviceToolProperties(_physicalDevice, ref pToolCount, out pToolProperties);
    }

    public unsafe Result GetPhysicalDeviceImageFormatProperties2(PhysicalDeviceImageFormatInfo2* pImageFormatInfo, ImageFormatProperties2* pImageFormatProperties)
    {
        return VK.GetPhysicalDeviceImageFormatProperties2(_physicalDevice, pImageFormatInfo, pImageFormatProperties);
    }

    public unsafe Result GetPhysicalDeviceImageFormatProperties2(PhysicalDeviceImageFormatInfo2* pImageFormatInfo, out ImageFormatProperties2 pImageFormatProperties)
    {
        return VK.GetPhysicalDeviceImageFormatProperties2(_physicalDevice, pImageFormatInfo, out pImageFormatProperties);
    }

    public unsafe Result GetPhysicalDeviceImageFormatProperties2(in PhysicalDeviceImageFormatInfo2 pImageFormatInfo, ImageFormatProperties2* pImageFormatProperties)
    {
        return VK.GetPhysicalDeviceImageFormatProperties2(_physicalDevice, in pImageFormatInfo, pImageFormatProperties);
    }

    public Result GetPhysicalDeviceImageFormatProperties2(in PhysicalDeviceImageFormatInfo2 pImageFormatInfo, out ImageFormatProperties2 pImageFormatProperties)
    {
        return VK.GetPhysicalDeviceImageFormatProperties2(_physicalDevice, in pImageFormatInfo, out pImageFormatProperties);
    }

    public unsafe Result CreateDevice(DeviceCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, Device* pDevice)
    {
        return VK.CreateDevice(_physicalDevice, pCreateInfo, pAllocator, pDevice);
    }

    public unsafe Result CreateDevice(DeviceCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, out Device pDevice)
    {
        return VK.CreateDevice(_physicalDevice, pCreateInfo, pAllocator, out pDevice);
    }

    public unsafe Result CreateDevice(DeviceCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, Device* pDevice)
    {
        return VK.CreateDevice(_physicalDevice, pCreateInfo, in pAllocator, pDevice);
    }

    public unsafe Result CreateDevice(DeviceCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, out Device pDevice)
    {
        return VK.CreateDevice(_physicalDevice, pCreateInfo, in pAllocator, out pDevice);
    }

    public unsafe Result CreateDevice(in DeviceCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, Device* pDevice)
    {
        return VK.CreateDevice(_physicalDevice, in pCreateInfo, pAllocator, pDevice);
    }

    public unsafe Result CreateDevice(in DeviceCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, out Device pDevice)
    {
        return VK.CreateDevice(_physicalDevice, in pCreateInfo, pAllocator, out pDevice);
    }

    public unsafe Result CreateDevice(in DeviceCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, Device* pDevice)
    {
        return VK.CreateDevice(_physicalDevice, in pCreateInfo, in pAllocator, pDevice);
    }

    public Result CreateDevice(in DeviceCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, out Device pDevice)
    {
        return VK.CreateDevice(_physicalDevice, in pCreateInfo, in pAllocator, out pDevice);
    }

    public unsafe Result EnumerateDeviceExtensionProperties(byte* pLayerName, uint* pPropertyCount, ExtensionProperties* pProperties)
    {
        return VK.EnumerateDeviceExtensionProperties(_physicalDevice, pLayerName, pPropertyCount, pProperties);
    }

    public unsafe Result EnumerateDeviceExtensionProperties(byte* pLayerName, uint* pPropertyCount, ref ExtensionProperties pProperties)
    {
        return VK.EnumerateDeviceExtensionProperties(_physicalDevice, pLayerName, pPropertyCount, ref pProperties);
    }

    public unsafe Result EnumerateDeviceExtensionProperties(byte* pLayerName, ref uint pPropertyCount, ExtensionProperties* pProperties)
    {
        return VK.EnumerateDeviceExtensionProperties(_physicalDevice, pLayerName, ref pPropertyCount, pProperties);
    }

    public unsafe Result EnumerateDeviceExtensionProperties(byte* pLayerName, ref uint pPropertyCount, ref ExtensionProperties pProperties)
    {
        return VK.EnumerateDeviceExtensionProperties(_physicalDevice, pLayerName, ref pPropertyCount, ref pProperties);
    }

    public unsafe Result EnumerateDeviceExtensionProperties(in byte pLayerName, uint* pPropertyCount, ExtensionProperties* pProperties)
    {
        return VK.EnumerateDeviceExtensionProperties(_physicalDevice, in pLayerName, pPropertyCount, pProperties);
    }

    public unsafe Result EnumerateDeviceExtensionProperties(in byte pLayerName, uint* pPropertyCount, ref ExtensionProperties pProperties)
    {
        return VK.EnumerateDeviceExtensionProperties(_physicalDevice, in pLayerName, pPropertyCount, ref pProperties);
    }

    public unsafe Result EnumerateDeviceExtensionProperties(in byte pLayerName, ref uint pPropertyCount, ExtensionProperties* pProperties)
    {
        return VK.EnumerateDeviceExtensionProperties(_physicalDevice, in pLayerName, ref pPropertyCount, pProperties);
    }

    public Result EnumerateDeviceExtensionProperties(in byte pLayerName, ref uint pPropertyCount, ref ExtensionProperties pProperties)
    {
        return VK.EnumerateDeviceExtensionProperties(_physicalDevice, in pLayerName, ref pPropertyCount, ref pProperties);
    }

    public unsafe Result EnumerateDeviceExtensionProperties(string pLayerName, uint* pPropertyCount, ExtensionProperties* pProperties)
    {
        return VK.EnumerateDeviceExtensionProperties(_physicalDevice, pLayerName, pPropertyCount, pProperties);
    }

    public unsafe Result EnumerateDeviceExtensionProperties(string pLayerName, uint* pPropertyCount, ref ExtensionProperties pProperties)
    {
        return VK.EnumerateDeviceExtensionProperties(_physicalDevice, pLayerName, pPropertyCount, ref pProperties);
    }

    public unsafe Result EnumerateDeviceExtensionProperties(string pLayerName, ref uint pPropertyCount, ExtensionProperties* pProperties)
    {
        return VK.EnumerateDeviceExtensionProperties(_physicalDevice, pLayerName, ref pPropertyCount, pProperties);
    }

    public Result EnumerateDeviceExtensionProperties(string pLayerName, ref uint pPropertyCount, ref ExtensionProperties pProperties)
    {
        return VK.EnumerateDeviceExtensionProperties(_physicalDevice, pLayerName, ref pPropertyCount, ref pProperties);
    }

    public unsafe Result EnumerateDeviceLayerProperties(uint* pPropertyCount, LayerProperties* pProperties)
    {
        return VK.EnumerateDeviceLayerProperties(_physicalDevice, pPropertyCount, pProperties);
    }

    public unsafe Result EnumerateDeviceLayerProperties(uint* pPropertyCount, ref LayerProperties pProperties)
    {
        return VK.EnumerateDeviceLayerProperties(_physicalDevice, pPropertyCount, ref pProperties);
    }

    public unsafe Result EnumerateDeviceLayerProperties(ref uint pPropertyCount, LayerProperties* pProperties)
    {
        return VK.EnumerateDeviceLayerProperties(_physicalDevice, ref pPropertyCount, pProperties);
    }

    public Result EnumerateDeviceLayerProperties(ref uint pPropertyCount, ref LayerProperties pProperties)
    {
        return VK.EnumerateDeviceLayerProperties(_physicalDevice, ref pPropertyCount, ref pProperties);
    }

    public unsafe Result GetPhysicalDeviceImageFormatProperties(Format format, ImageType type, ImageTiling tiling, ImageUsageFlags usage, ImageCreateFlags flags,
        ImageFormatProperties* pImageFormatProperties)
    {
        return VK.GetPhysicalDeviceImageFormatProperties(_physicalDevice, format, type, tiling, usage, flags, pImageFormatProperties);
    }

    public Result GetPhysicalDeviceImageFormatProperties(Format format, ImageType type, ImageTiling tiling, ImageUsageFlags usage, ImageCreateFlags flags,
        out ImageFormatProperties pImageFormatProperties)
    {
        return VK.GetPhysicalDeviceImageFormatProperties(_physicalDevice, format, type, tiling, usage, flags, out pImageFormatProperties);
    }
}

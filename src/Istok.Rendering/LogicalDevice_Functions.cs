using Silk.NET.Core;
using Silk.NET.Core.Native;
using Silk.NET.Vulkan;
using static Istok.Rendering.Engine;
using Semaphore = Silk.NET.Vulkan.Semaphore;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Istok.Rendering;

public partial class LogicalDevice
{
    public bool TryGetDeviceExtension<T>(out T ext, string? layer = null) where T : NativeExtension<Vk>
    {
        return _engine.TryGetDeviceExtension(_device, out ext, layer);
    }

    public void UnmapMemory(DeviceMemory memory)
    {
        VK.UnmapMemory(_device, memory);
    }

    public unsafe void DestroyDevice(AllocationCallbacks* pAllocator)
    {
        VK.DestroyDevice(_device, pAllocator);
    }

    public void TrimCommandPool(Silk.NET.Vulkan.CommandPool commandPool, uint flags)
    {
        VK.TrimCommandPool(_device, commandPool, flags);
    }

    public void ResetQueryPool(QueryPool queryPool, uint firstQuery, uint queryCount)
    {
        VK.ResetQueryPool(_device, queryPool, firstQuery, queryCount);
    }

    public unsafe void DestroyPrivateDataSlot(PrivateDataSlot privateDataSlot, AllocationCallbacks* pAllocator)
    {
        VK.DestroyPrivateDataSlot(_device, privateDataSlot, pAllocator);
    }

    public unsafe void GetDeviceBufferMemoryRequirements(DeviceBufferMemoryRequirements* pInfo, MemoryRequirements2* pMemoryRequirements)
    {
        VK.GetDeviceBufferMemoryRequirements(_device, pInfo, pMemoryRequirements);
    }

    public unsafe void GetDeviceImageMemoryRequirements(DeviceImageMemoryRequirements* pInfo, MemoryRequirements2* pMemoryRequirements)
    {
        VK.GetDeviceImageMemoryRequirements(_device, pInfo, pMemoryRequirements);
    }

    public unsafe void DestroyDescriptorUpdateTemplate(DescriptorUpdateTemplate descriptorUpdateTemplate, AllocationCallbacks* pAllocator)
    {
        VK.DestroyDescriptorUpdateTemplate(_device, descriptorUpdateTemplate, pAllocator);
    }

    public unsafe void DestroySamplerYcbcrConversion(SamplerYcbcrConversion ycbcrConversion, AllocationCallbacks* pAllocator)
    {
        VK.DestroySamplerYcbcrConversion(_device, ycbcrConversion, pAllocator);
    }

    public unsafe void GetBufferMemoryRequirements2(BufferMemoryRequirementsInfo2* pInfo, MemoryRequirements2* pMemoryRequirements)
    {
        VK.GetBufferMemoryRequirements2(_device, pInfo, pMemoryRequirements);
    }

    public unsafe void GetDescriptorSetLayoutSupport(DescriptorSetLayoutCreateInfo* pCreateInfo, DescriptorSetLayoutSupport* pSupport)
    {
        VK.GetDescriptorSetLayoutSupport(_device, pCreateInfo, pSupport);
    }

    public unsafe void GetDeviceQueue2(DeviceQueueInfo2* pQueueInfo, Silk.NET.Vulkan.Queue* pQueue)
    {
        VK.GetDeviceQueue2(_device, pQueueInfo, pQueue);
    }

    public unsafe void GetImageMemoryRequirements2(ImageMemoryRequirementsInfo2* pInfo, MemoryRequirements2* pMemoryRequirements)
    {
        VK.GetImageMemoryRequirements2(_device, pInfo, pMemoryRequirements);
    }

    public unsafe void DestroyBuffer(Silk.NET.Vulkan.Buffer buffer, AllocationCallbacks* pAllocator)
    {
        VK.DestroyBuffer(_device, buffer, pAllocator);
    }

    public unsafe void DestroyBufferView(BufferView bufferView, AllocationCallbacks* pAllocator)
    {
        VK.DestroyBufferView(_device, bufferView, pAllocator);
    }

    public unsafe void DestroyCommandPool(Silk.NET.Vulkan.CommandPool commandPool, AllocationCallbacks* pAllocator)
    {
        VK.DestroyCommandPool(_device, commandPool, pAllocator);
    }

    public unsafe void DestroyDescriptorPool(Silk.NET.Vulkan.DescriptorPool descriptorPool, AllocationCallbacks* pAllocator)
    {
        VK.DestroyDescriptorPool(_device, descriptorPool, pAllocator);
    }

    public unsafe void DestroyDescriptorSetLayout(Silk.NET.Vulkan.DescriptorSetLayout descriptorSetLayout, AllocationCallbacks* pAllocator)
    {
        VK.DestroyDescriptorSetLayout(_device, descriptorSetLayout, pAllocator);
    }

    public unsafe void DestroyEvent(Event @event, AllocationCallbacks* pAllocator)
    {
        VK.DestroyEvent(_device, @event, pAllocator);
    }

    public unsafe void DestroyFence(Silk.NET.Vulkan.Fence fence, AllocationCallbacks* pAllocator)
    {
        VK.DestroyFence(_device, fence, pAllocator);
    }

    public unsafe void DestroyFramebuffer(Silk.NET.Vulkan.Framebuffer framebuffer, AllocationCallbacks* pAllocator)
    {
        VK.DestroyFramebuffer(_device, framebuffer, pAllocator);
    }

    public unsafe void DestroyImage(Silk.NET.Vulkan.Image image, AllocationCallbacks* pAllocator)
    {
        VK.DestroyImage(_device, image, pAllocator);
    }

    public unsafe void DestroyImageView(Silk.NET.Vulkan.ImageView imageView, AllocationCallbacks* pAllocator)
    {
        VK.DestroyImageView(_device, imageView, pAllocator);
    }

    public unsafe void DestroyPipeline(Silk.NET.Vulkan.Pipeline pipeline, AllocationCallbacks* pAllocator)
    {
        VK.DestroyPipeline(_device, pipeline, pAllocator);
    }

    public unsafe void DestroyPipelineCache(PipelineCache pipelineCache, AllocationCallbacks* pAllocator)
    {
        VK.DestroyPipelineCache(_device, pipelineCache, pAllocator);
    }

    public unsafe void DestroyPipelineLayout(PipelineLayout pipelineLayout, AllocationCallbacks* pAllocator)
    {
        VK.DestroyPipelineLayout(_device, pipelineLayout, pAllocator);
    }

    public unsafe void DestroyQueryPool(QueryPool queryPool, AllocationCallbacks* pAllocator)
    {
        VK.DestroyQueryPool(_device, queryPool, pAllocator);
    }

    public unsafe void DestroyRenderPass(Silk.NET.Vulkan.RenderPass renderPass, AllocationCallbacks* pAllocator)
    {
        VK.DestroyRenderPass(_device, renderPass, pAllocator);
    }

    public unsafe void DestroySampler(Silk.NET.Vulkan.Sampler sampler, AllocationCallbacks* pAllocator)
    {
        VK.DestroySampler(_device, sampler, pAllocator);
    }

    public unsafe void DestroySemaphore(Semaphore semaphore, AllocationCallbacks* pAllocator)
    {
        VK.DestroySemaphore(_device, semaphore, pAllocator);
    }

    public unsafe void DestroyShaderModule(ShaderModule shaderModule, AllocationCallbacks* pAllocator)
    {
        VK.DestroyShaderModule(_device, shaderModule, pAllocator);
    }

    public unsafe void FreeMemory(DeviceMemory memory, AllocationCallbacks* pAllocator)
    {
        VK.FreeMemory(_device, memory, pAllocator);
    }

    public unsafe void GetBufferMemoryRequirements(Silk.NET.Vulkan.Buffer buffer, MemoryRequirements* pMemoryRequirements)
    {
        VK.GetBufferMemoryRequirements(_device, buffer, pMemoryRequirements);
    }

    public unsafe void GetDeviceMemoryCommitment(DeviceMemory memory, ulong* pCommittedMemoryInBytes)
    {
        VK.GetDeviceMemoryCommitment(_device, memory, pCommittedMemoryInBytes);
    }

    public unsafe void GetImageMemoryRequirements(Silk.NET.Vulkan.Image image, MemoryRequirements* pMemoryRequirements)
    {
        VK.GetImageMemoryRequirements(_device, image, pMemoryRequirements);
    }

    public unsafe void GetRenderAreaGranularity(Silk.NET.Vulkan.RenderPass renderPass, Extent2D* pGranularity)
    {
        VK.GetRenderAreaGranularity(_device, renderPass, pGranularity);
    }

    public unsafe void GetDeviceImageSparseMemoryRequirements(DeviceImageMemoryRequirements* pInfo, uint* pSparseMemoryRequirementCount, SparseImageMemoryRequirements2* pSparseMemoryRequirements)
    {
        VK.GetDeviceImageSparseMemoryRequirements(_device, pInfo, pSparseMemoryRequirementCount, pSparseMemoryRequirements);
    }

    public unsafe void GetImageSparseMemoryRequirements2(ImageSparseMemoryRequirementsInfo2* pInfo, uint* pSparseMemoryRequirementCount, SparseImageMemoryRequirements2* pSparseMemoryRequirements)
    {
        VK.GetImageSparseMemoryRequirements2(_device, pInfo, pSparseMemoryRequirementCount, pSparseMemoryRequirements);
    }

    public unsafe void UpdateDescriptorSetWithTemplate(Silk.NET.Vulkan.DescriptorSet descriptorSet, DescriptorUpdateTemplate descriptorUpdateTemplate, void* pData)
    {
        VK.UpdateDescriptorSetWithTemplate(_device, descriptorSet, descriptorUpdateTemplate, pData);
    }

    public unsafe void FreeCommandBuffers(Silk.NET.Vulkan.CommandPool commandPool, uint commandBufferCount, Silk.NET.Vulkan.CommandBuffer* pCommandBuffers)
    {
        VK.FreeCommandBuffers(_device, commandPool, commandBufferCount, pCommandBuffers);
    }

    public unsafe void GetDeviceQueue(uint queueFamilyIndex, uint queueIndex, Silk.NET.Vulkan.Queue* pQueue)
    {
        VK.GetDeviceQueue(_device, queueFamilyIndex, queueIndex, pQueue);
    }

    public unsafe void GetImageSparseMemoryRequirements(Silk.NET.Vulkan.Image image, uint* pSparseMemoryRequirementCount, SparseImageMemoryRequirements* pSparseMemoryRequirements)
    {
        VK.GetImageSparseMemoryRequirements(_device, image, pSparseMemoryRequirementCount, pSparseMemoryRequirements);
    }

    public unsafe void GetImageSubresourceLayout(Silk.NET.Vulkan.Image image, ImageSubresource* pSubresource, SubresourceLayout* pLayout)
    {
        VK.GetImageSubresourceLayout(_device, image, pSubresource, pLayout);
    }

    public unsafe void GetPrivateData(ObjectType objectType, ulong objectHandle, PrivateDataSlot privateDataSlot, ulong* pData)
    {
        VK.GetPrivateData(_device, objectType, objectHandle, privateDataSlot, pData);
    }

    public unsafe void GetDeviceGroupPeerMemoryFeatures(uint heapIndex, uint localDeviceIndex, uint remoteDeviceIndex, PeerMemoryFeatureFlags* pPeerMemoryFeatures)
    {
        VK.GetDeviceGroupPeerMemoryFeatures(_device, heapIndex, localDeviceIndex, remoteDeviceIndex, pPeerMemoryFeatures);
    }

    public unsafe void UpdateDescriptorSets(uint descriptorWriteCount, WriteDescriptorSet* pDescriptorWrites, uint descriptorCopyCount, CopyDescriptorSet* pDescriptorCopies)
    {
        VK.UpdateDescriptorSets(_device, descriptorWriteCount, pDescriptorWrites, descriptorCopyCount, pDescriptorCopies);
    }

    public void DestroyPrivateDataSlot(PrivateDataSlot privateDataSlot, in AllocationCallbacks pAllocator)
    {
        VK.DestroyPrivateDataSlot(_device, privateDataSlot, in pAllocator);
    }

    public unsafe void GetDeviceBufferMemoryRequirements(DeviceBufferMemoryRequirements* pInfo, out MemoryRequirements2 pMemoryRequirements)
    {
        VK.GetDeviceBufferMemoryRequirements(_device, pInfo, out pMemoryRequirements);
    }

    public unsafe void GetDeviceBufferMemoryRequirements(in DeviceBufferMemoryRequirements pInfo, MemoryRequirements2* pMemoryRequirements)
    {
        VK.GetDeviceBufferMemoryRequirements(_device, in pInfo, pMemoryRequirements);
    }

    public void GetDeviceBufferMemoryRequirements(in DeviceBufferMemoryRequirements pInfo, out MemoryRequirements2 pMemoryRequirements)
    {
        VK.GetDeviceBufferMemoryRequirements(_device, in pInfo, out pMemoryRequirements);
    }

    public unsafe void GetDeviceImageMemoryRequirements(DeviceImageMemoryRequirements* pInfo, out MemoryRequirements2 pMemoryRequirements)
    {
        VK.GetDeviceImageMemoryRequirements(_device, pInfo, out pMemoryRequirements);
    }

    public unsafe void GetDeviceImageMemoryRequirements(in DeviceImageMemoryRequirements pInfo, MemoryRequirements2* pMemoryRequirements)
    {
        VK.GetDeviceImageMemoryRequirements(_device, in pInfo, pMemoryRequirements);
    }

    public void GetDeviceImageMemoryRequirements(in DeviceImageMemoryRequirements pInfo, out MemoryRequirements2 pMemoryRequirements)
    {
        VK.GetDeviceImageMemoryRequirements(_device, in pInfo, out pMemoryRequirements);
    }

    public unsafe void GetDeviceImageSparseMemoryRequirements(DeviceImageMemoryRequirements* pInfo, uint* pSparseMemoryRequirementCount, out SparseImageMemoryRequirements2 pSparseMemoryRequirements)
    {
        VK.GetDeviceImageSparseMemoryRequirements(_device, pInfo, pSparseMemoryRequirementCount, out pSparseMemoryRequirements);
    }

    public unsafe void GetDeviceImageSparseMemoryRequirements(DeviceImageMemoryRequirements* pInfo, ref uint pSparseMemoryRequirementCount, SparseImageMemoryRequirements2* pSparseMemoryRequirements)
    {
        VK.GetDeviceImageSparseMemoryRequirements(_device, pInfo, ref pSparseMemoryRequirementCount, pSparseMemoryRequirements);
    }

    public unsafe void GetDeviceImageSparseMemoryRequirements(DeviceImageMemoryRequirements* pInfo, ref uint pSparseMemoryRequirementCount,
        out SparseImageMemoryRequirements2 pSparseMemoryRequirements)
    {
        VK.GetDeviceImageSparseMemoryRequirements(_device, pInfo, ref pSparseMemoryRequirementCount, out pSparseMemoryRequirements);
    }

    public unsafe void GetDeviceImageSparseMemoryRequirements(in DeviceImageMemoryRequirements pInfo, uint* pSparseMemoryRequirementCount, SparseImageMemoryRequirements2* pSparseMemoryRequirements)
    {
        VK.GetDeviceImageSparseMemoryRequirements(_device, in pInfo, pSparseMemoryRequirementCount, pSparseMemoryRequirements);
    }

    public unsafe void GetDeviceImageSparseMemoryRequirements(in DeviceImageMemoryRequirements pInfo, uint* pSparseMemoryRequirementCount, out SparseImageMemoryRequirements2 pSparseMemoryRequirements)
    {
        VK.GetDeviceImageSparseMemoryRequirements(_device, in pInfo, pSparseMemoryRequirementCount, out pSparseMemoryRequirements);
    }

    public unsafe void GetDeviceImageSparseMemoryRequirements(in DeviceImageMemoryRequirements pInfo, ref uint pSparseMemoryRequirementCount, SparseImageMemoryRequirements2* pSparseMemoryRequirements)
    {
        VK.GetDeviceImageSparseMemoryRequirements(_device, in pInfo, ref pSparseMemoryRequirementCount, pSparseMemoryRequirements);
    }

    public void GetDeviceImageSparseMemoryRequirements(in DeviceImageMemoryRequirements pInfo, ref uint pSparseMemoryRequirementCount,
        out SparseImageMemoryRequirements2 pSparseMemoryRequirements)
    {
        VK.GetDeviceImageSparseMemoryRequirements(_device, in pInfo, ref pSparseMemoryRequirementCount, out pSparseMemoryRequirements);
    }

    public void GetPrivateData(ObjectType objectType, ulong objectHandle, PrivateDataSlot privateDataSlot, out ulong pData)
    {
        VK.GetPrivateData(_device, objectType, objectHandle, privateDataSlot, out pData);
    }

    public void DestroyDescriptorUpdateTemplate(DescriptorUpdateTemplate descriptorUpdateTemplate, in AllocationCallbacks pAllocator)
    {
        VK.DestroyDescriptorUpdateTemplate(_device, descriptorUpdateTemplate, in pAllocator);
    }

    public void DestroySamplerYcbcrConversion(SamplerYcbcrConversion ycbcrConversion, in AllocationCallbacks pAllocator)
    {
        VK.DestroySamplerYcbcrConversion(_device, ycbcrConversion, in pAllocator);
    }

    public unsafe void GetBufferMemoryRequirements2(BufferMemoryRequirementsInfo2* pInfo, out MemoryRequirements2 pMemoryRequirements)
    {
        VK.GetBufferMemoryRequirements2(_device, pInfo, out pMemoryRequirements);
    }

    public unsafe void GetBufferMemoryRequirements2(in BufferMemoryRequirementsInfo2 pInfo, MemoryRequirements2* pMemoryRequirements)
    {
        VK.GetBufferMemoryRequirements2(_device, in pInfo, pMemoryRequirements);
    }

    public void GetBufferMemoryRequirements2(in BufferMemoryRequirementsInfo2 pInfo, out MemoryRequirements2 pMemoryRequirements)
    {
        VK.GetBufferMemoryRequirements2(_device, in pInfo, out pMemoryRequirements);
    }

    public unsafe void GetDescriptorSetLayoutSupport(DescriptorSetLayoutCreateInfo* pCreateInfo, out DescriptorSetLayoutSupport pSupport)
    {
        VK.GetDescriptorSetLayoutSupport(_device, pCreateInfo, out pSupport);
    }

    public unsafe void GetDescriptorSetLayoutSupport(in DescriptorSetLayoutCreateInfo pCreateInfo, DescriptorSetLayoutSupport* pSupport)
    {
        VK.GetDescriptorSetLayoutSupport(_device, in pCreateInfo, pSupport);
    }

    public void GetDescriptorSetLayoutSupport(in DescriptorSetLayoutCreateInfo pCreateInfo, out DescriptorSetLayoutSupport pSupport)
    {
        VK.GetDescriptorSetLayoutSupport(_device, in pCreateInfo, out pSupport);
    }

    public void GetDeviceGroupPeerMemoryFeatures(uint heapIndex, uint localDeviceIndex, uint remoteDeviceIndex, out PeerMemoryFeatureFlags pPeerMemoryFeatures)
    {
        VK.GetDeviceGroupPeerMemoryFeatures(_device, heapIndex, localDeviceIndex, remoteDeviceIndex, out pPeerMemoryFeatures);
    }

    public unsafe void GetDeviceQueue2(DeviceQueueInfo2* pQueueInfo, out Silk.NET.Vulkan.Queue pQueue)
    {
        VK.GetDeviceQueue2(_device, pQueueInfo, out pQueue);
    }

    public unsafe void GetDeviceQueue2(in DeviceQueueInfo2 pQueueInfo, Silk.NET.Vulkan.Queue* pQueue)
    {
        VK.GetDeviceQueue2(_device, in pQueueInfo, pQueue);
    }

    public void GetDeviceQueue2(in DeviceQueueInfo2 pQueueInfo, out Silk.NET.Vulkan.Queue pQueue)
    {
        VK.GetDeviceQueue2(_device, in pQueueInfo, out pQueue);
    }

    public unsafe void GetImageMemoryRequirements2(ImageMemoryRequirementsInfo2* pInfo, out MemoryRequirements2 pMemoryRequirements)
    {
        VK.GetImageMemoryRequirements2(_device, pInfo, out pMemoryRequirements);
    }

    public unsafe void GetImageMemoryRequirements2(in ImageMemoryRequirementsInfo2 pInfo, MemoryRequirements2* pMemoryRequirements)
    {
        VK.GetImageMemoryRequirements2(_device, in pInfo, pMemoryRequirements);
    }

    public void GetImageMemoryRequirements2(in ImageMemoryRequirementsInfo2 pInfo, out MemoryRequirements2 pMemoryRequirements)
    {
        VK.GetImageMemoryRequirements2(_device, in pInfo, out pMemoryRequirements);
    }

    public unsafe void GetImageSparseMemoryRequirements2(ImageSparseMemoryRequirementsInfo2* pInfo, uint* pSparseMemoryRequirementCount, out SparseImageMemoryRequirements2 pSparseMemoryRequirements)
    {
        VK.GetImageSparseMemoryRequirements2(_device, pInfo, pSparseMemoryRequirementCount, out pSparseMemoryRequirements);
    }

    public unsafe void GetImageSparseMemoryRequirements2(ImageSparseMemoryRequirementsInfo2* pInfo, ref uint pSparseMemoryRequirementCount, SparseImageMemoryRequirements2* pSparseMemoryRequirements)
    {
        VK.GetImageSparseMemoryRequirements2(_device, pInfo, ref pSparseMemoryRequirementCount, pSparseMemoryRequirements);
    }

    public unsafe void GetImageSparseMemoryRequirements2(ImageSparseMemoryRequirementsInfo2* pInfo, ref uint pSparseMemoryRequirementCount,
        out SparseImageMemoryRequirements2 pSparseMemoryRequirements)
    {
        VK.GetImageSparseMemoryRequirements2(_device, pInfo, ref pSparseMemoryRequirementCount, out pSparseMemoryRequirements);
    }

    public unsafe void GetImageSparseMemoryRequirements2(in ImageSparseMemoryRequirementsInfo2 pInfo, uint* pSparseMemoryRequirementCount, SparseImageMemoryRequirements2* pSparseMemoryRequirements)
    {
        VK.GetImageSparseMemoryRequirements2(_device, in pInfo, pSparseMemoryRequirementCount, pSparseMemoryRequirements);
    }

    public unsafe void GetImageSparseMemoryRequirements2(in ImageSparseMemoryRequirementsInfo2 pInfo, uint* pSparseMemoryRequirementCount, out SparseImageMemoryRequirements2 pSparseMemoryRequirements)
    {
        VK.GetImageSparseMemoryRequirements2(_device, in pInfo, pSparseMemoryRequirementCount, out pSparseMemoryRequirements);
    }

    public unsafe void GetImageSparseMemoryRequirements2(in ImageSparseMemoryRequirementsInfo2 pInfo, ref uint pSparseMemoryRequirementCount, SparseImageMemoryRequirements2* pSparseMemoryRequirements)
    {
        VK.GetImageSparseMemoryRequirements2(_device, in pInfo, ref pSparseMemoryRequirementCount, pSparseMemoryRequirements);
    }

    public void GetImageSparseMemoryRequirements2(in ImageSparseMemoryRequirementsInfo2 pInfo, ref uint pSparseMemoryRequirementCount,
        out SparseImageMemoryRequirements2 pSparseMemoryRequirements)
    {
        VK.GetImageSparseMemoryRequirements2(_device, in pInfo, ref pSparseMemoryRequirementCount, out pSparseMemoryRequirements);
    }

    public void UpdateDescriptorSetWithTemplate<T0>(Silk.NET.Vulkan.DescriptorSet descriptorSet, DescriptorUpdateTemplate descriptorUpdateTemplate, ref T0 pData) where T0 : unmanaged
    {
        VK.UpdateDescriptorSetWithTemplate(_device, descriptorSet, descriptorUpdateTemplate, ref pData);
    }

    public void DestroyBuffer(Silk.NET.Vulkan.Buffer buffer, in AllocationCallbacks pAllocator)
    {
        VK.DestroyBuffer(_device, buffer, in pAllocator);
    }

    public void DestroyBufferView(BufferView bufferView, in AllocationCallbacks pAllocator)
    {
        VK.DestroyBufferView(_device, bufferView, in pAllocator);
    }

    public void DestroyCommandPool(Silk.NET.Vulkan.CommandPool commandPool, in AllocationCallbacks pAllocator)
    {
        VK.DestroyCommandPool(_device, commandPool, in pAllocator);
    }

    public void DestroyDescriptorPool(Silk.NET.Vulkan.DescriptorPool descriptorPool, in AllocationCallbacks pAllocator)
    {
        VK.DestroyDescriptorPool(_device, descriptorPool, in pAllocator);
    }

    public void DestroyDescriptorSetLayout(Silk.NET.Vulkan.DescriptorSetLayout descriptorSetLayout, in AllocationCallbacks pAllocator)
    {
        VK.DestroyDescriptorSetLayout(_device, descriptorSetLayout, in pAllocator);
    }

    public void DestroyDevice(in AllocationCallbacks pAllocator)
    {
        VK.DestroyDevice(_device, in pAllocator);
    }

    public void DestroyEvent(Event @event, in AllocationCallbacks pAllocator)
    {
        VK.DestroyEvent(_device, @event, in pAllocator);
    }

    public void DestroyFence(Silk.NET.Vulkan.Fence fence, in AllocationCallbacks pAllocator)
    {
        VK.DestroyFence(_device, fence, in pAllocator);
    }

    public void DestroyFramebuffer(Silk.NET.Vulkan.Framebuffer framebuffer, in AllocationCallbacks pAllocator)
    {
        VK.DestroyFramebuffer(_device, framebuffer, in pAllocator);
    }

    public void DestroyImage(Silk.NET.Vulkan.Image image, in AllocationCallbacks pAllocator)
    {
        VK.DestroyImage(_device, image, in pAllocator);
    }

    public void DestroyImageView(Silk.NET.Vulkan.ImageView imageView, in AllocationCallbacks pAllocator)
    {
        VK.DestroyImageView(_device, imageView, in pAllocator);
    }

    public void DestroyPipeline(Silk.NET.Vulkan.Pipeline pipeline, in AllocationCallbacks pAllocator)
    {
        VK.DestroyPipeline(_device, pipeline, in pAllocator);
    }

    public void DestroyPipelineCache(PipelineCache pipelineCache, in AllocationCallbacks pAllocator)
    {
        VK.DestroyPipelineCache(_device, pipelineCache, in pAllocator);
    }

    public void DestroyPipelineLayout(PipelineLayout pipelineLayout, in AllocationCallbacks pAllocator)
    {
        VK.DestroyPipelineLayout(_device, pipelineLayout, in pAllocator);
    }

    public void DestroyQueryPool(QueryPool queryPool, in AllocationCallbacks pAllocator)
    {
        VK.DestroyQueryPool(_device, queryPool, in pAllocator);
    }

    public void DestroyRenderPass(Silk.NET.Vulkan.RenderPass renderPass, in AllocationCallbacks pAllocator)
    {
        VK.DestroyRenderPass(_device, renderPass, in pAllocator);
    }

    public void DestroySampler(Silk.NET.Vulkan.Sampler sampler, in AllocationCallbacks pAllocator)
    {
        VK.DestroySampler(_device, sampler, in pAllocator);
    }

    public void DestroySemaphore(Semaphore semaphore, in AllocationCallbacks pAllocator)
    {
        VK.DestroySemaphore(_device, semaphore, in pAllocator);
    }

    public void DestroyShaderModule(ShaderModule shaderModule, in AllocationCallbacks pAllocator)
    {
        VK.DestroyShaderModule(_device, shaderModule, in pAllocator);
    }

    public void FreeCommandBuffers(Silk.NET.Vulkan.CommandPool commandPool, uint commandBufferCount, in Silk.NET.Vulkan.CommandBuffer pCommandBuffers)
    {
        VK.FreeCommandBuffers(_device, commandPool, commandBufferCount, in pCommandBuffers);
    }

    public void FreeMemory(DeviceMemory memory, in AllocationCallbacks pAllocator)
    {
        VK.FreeMemory(_device, memory, in pAllocator);
    }

    public void GetBufferMemoryRequirements(Silk.NET.Vulkan.Buffer buffer, out MemoryRequirements pMemoryRequirements)
    {
        VK.GetBufferMemoryRequirements(_device, buffer, out pMemoryRequirements);
    }

    public void GetDeviceMemoryCommitment(DeviceMemory memory, out ulong pCommittedMemoryInBytes)
    {
        VK.GetDeviceMemoryCommitment(_device, memory, out pCommittedMemoryInBytes);
    }

    public void GetDeviceQueue(uint queueFamilyIndex, uint queueIndex, out Silk.NET.Vulkan.Queue pQueue)
    {
        VK.GetDeviceQueue(_device, queueFamilyIndex, queueIndex, out pQueue);
    }

    public void GetImageMemoryRequirements(Silk.NET.Vulkan.Image image, out MemoryRequirements pMemoryRequirements)
    {
        VK.GetImageMemoryRequirements(_device, image, out pMemoryRequirements);
    }

    public unsafe void GetImageSparseMemoryRequirements(Silk.NET.Vulkan.Image image, uint* pSparseMemoryRequirementCount, out SparseImageMemoryRequirements pSparseMemoryRequirements)
    {
        VK.GetImageSparseMemoryRequirements(_device, image, pSparseMemoryRequirementCount, out pSparseMemoryRequirements);
    }

    public unsafe void GetImageSparseMemoryRequirements(Silk.NET.Vulkan.Image image, ref uint pSparseMemoryRequirementCount, SparseImageMemoryRequirements* pSparseMemoryRequirements)
    {
        VK.GetImageSparseMemoryRequirements(_device, image, ref pSparseMemoryRequirementCount, pSparseMemoryRequirements);
    }

    public void GetImageSparseMemoryRequirements(Silk.NET.Vulkan.Image image, ref uint pSparseMemoryRequirementCount, out SparseImageMemoryRequirements pSparseMemoryRequirements)
    {
        VK.GetImageSparseMemoryRequirements(_device, image, ref pSparseMemoryRequirementCount, out pSparseMemoryRequirements);
    }

    public unsafe void GetImageSubresourceLayout(Silk.NET.Vulkan.Image image, ImageSubresource* pSubresource, out SubresourceLayout pLayout)
    {
        VK.GetImageSubresourceLayout(_device, image, pSubresource, out pLayout);
    }

    public unsafe void GetImageSubresourceLayout(Silk.NET.Vulkan.Image image, in ImageSubresource pSubresource, SubresourceLayout* pLayout)
    {
        VK.GetImageSubresourceLayout(_device, image, in pSubresource, pLayout);
    }

    public void GetImageSubresourceLayout(Silk.NET.Vulkan.Image image, in ImageSubresource pSubresource, out SubresourceLayout pLayout)
    {
        VK.GetImageSubresourceLayout(_device, image, in pSubresource, out pLayout);
    }

    public void GetRenderAreaGranularity(Silk.NET.Vulkan.RenderPass renderPass, out Extent2D pGranularity)
    {
        VK.GetRenderAreaGranularity(_device, renderPass, out pGranularity);
    }

    public unsafe void UpdateDescriptorSets(uint descriptorWriteCount, WriteDescriptorSet* pDescriptorWrites, uint descriptorCopyCount, in CopyDescriptorSet pDescriptorCopies)
    {
        VK.UpdateDescriptorSets(_device, descriptorWriteCount, pDescriptorWrites, descriptorCopyCount, in pDescriptorCopies);
    }

    public unsafe void UpdateDescriptorSets(uint descriptorWriteCount, in WriteDescriptorSet pDescriptorWrites, uint descriptorCopyCount, CopyDescriptorSet* pDescriptorCopies)
    {
        VK.UpdateDescriptorSets(_device, descriptorWriteCount, in pDescriptorWrites, descriptorCopyCount, pDescriptorCopies);
    }

    public void UpdateDescriptorSets(uint descriptorWriteCount, in WriteDescriptorSet pDescriptorWrites, uint descriptorCopyCount, in CopyDescriptorSet pDescriptorCopies)
    {
        VK.UpdateDescriptorSets(_device, descriptorWriteCount, in pDescriptorWrites, descriptorCopyCount, in pDescriptorCopies);
    }

    public unsafe Result CreateRenderPass2(RenderPassCreateInfo2* pCreateInfo, AllocationCallbacks* pAllocator, Silk.NET.Vulkan.RenderPass* pRenderPass)
    {
        return VK.CreateRenderPass2(_device, pCreateInfo, pAllocator, pRenderPass);
    }

    public unsafe Result CreateRenderPass2(RenderPassCreateInfo2* pCreateInfo, AllocationCallbacks* pAllocator, out Silk.NET.Vulkan.RenderPass pRenderPass)
    {
        return VK.CreateRenderPass2(_device, pCreateInfo, pAllocator, out pRenderPass);
    }

    public unsafe Result CreateRenderPass2(RenderPassCreateInfo2* pCreateInfo, in AllocationCallbacks pAllocator, Silk.NET.Vulkan.RenderPass* pRenderPass)
    {
        return VK.CreateRenderPass2(_device, pCreateInfo, in pAllocator, pRenderPass);
    }

    public unsafe Result CreateRenderPass2(RenderPassCreateInfo2* pCreateInfo, in AllocationCallbacks pAllocator, out Silk.NET.Vulkan.RenderPass pRenderPass)
    {
        return VK.CreateRenderPass2(_device, pCreateInfo, in pAllocator, out pRenderPass);
    }

    public unsafe Result CreateRenderPass2(in RenderPassCreateInfo2 pCreateInfo, AllocationCallbacks* pAllocator, Silk.NET.Vulkan.RenderPass* pRenderPass)
    {
        return VK.CreateRenderPass2(_device, in pCreateInfo, pAllocator, pRenderPass);
    }

    public unsafe Result CreateRenderPass2(in RenderPassCreateInfo2 pCreateInfo, AllocationCallbacks* pAllocator, out Silk.NET.Vulkan.RenderPass pRenderPass)
    {
        return VK.CreateRenderPass2(_device, in pCreateInfo, pAllocator, out pRenderPass);
    }

    public unsafe Result CreateRenderPass2(in RenderPassCreateInfo2 pCreateInfo, in AllocationCallbacks pAllocator, Silk.NET.Vulkan.RenderPass* pRenderPass)
    {
        return VK.CreateRenderPass2(_device, in pCreateInfo, in pAllocator, pRenderPass);
    }

    public Result CreateRenderPass2(in RenderPassCreateInfo2 pCreateInfo, in AllocationCallbacks pAllocator, out Silk.NET.Vulkan.RenderPass pRenderPass)
    {
        return VK.CreateRenderPass2(_device, in pCreateInfo, in pAllocator, out pRenderPass);
    }

    public unsafe Result GetSemaphoreCounterValue(Semaphore semaphore, ulong* pValue)
    {
        return VK.GetSemaphoreCounterValue(_device, semaphore, pValue);
    }

    public Result GetSemaphoreCounterValue(Semaphore semaphore, out ulong pValue)
    {
        return VK.GetSemaphoreCounterValue(_device, semaphore, out pValue);
    }

    public unsafe Result SignalSemaphore(SemaphoreSignalInfo* pSignalInfo)
    {
        return VK.SignalSemaphore(_device, pSignalInfo);
    }

    public Result SignalSemaphore(in SemaphoreSignalInfo pSignalInfo)
    {
        return VK.SignalSemaphore(_device, in pSignalInfo);
    }

    public unsafe Result WaitSemaphores(SemaphoreWaitInfo* pWaitInfo, ulong timeout)
    {
        return VK.WaitSemaphores(_device, pWaitInfo, timeout);
    }

    public Result WaitSemaphores(in SemaphoreWaitInfo pWaitInfo, ulong timeout)
    {
        return VK.WaitSemaphores(_device, in pWaitInfo, timeout);
    }

    public unsafe Result CreatePrivateDataSlot(PrivateDataSlotCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, PrivateDataSlot* pPrivateDataSlot)
    {
        return VK.CreatePrivateDataSlot(_device, pCreateInfo, pAllocator, pPrivateDataSlot);
    }

    public unsafe Result CreatePrivateDataSlot(PrivateDataSlotCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, out PrivateDataSlot pPrivateDataSlot)
    {
        return VK.CreatePrivateDataSlot(_device, pCreateInfo, pAllocator, out pPrivateDataSlot);
    }

    public unsafe Result CreatePrivateDataSlot(PrivateDataSlotCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, PrivateDataSlot* pPrivateDataSlot)
    {
        return VK.CreatePrivateDataSlot(_device, pCreateInfo, in pAllocator, pPrivateDataSlot);
    }

    public unsafe Result CreatePrivateDataSlot(PrivateDataSlotCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, out PrivateDataSlot pPrivateDataSlot)
    {
        return VK.CreatePrivateDataSlot(_device, pCreateInfo, in pAllocator, out pPrivateDataSlot);
    }

    public unsafe Result CreatePrivateDataSlot(in PrivateDataSlotCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, PrivateDataSlot* pPrivateDataSlot)
    {
        return VK.CreatePrivateDataSlot(_device, in pCreateInfo, pAllocator, pPrivateDataSlot);
    }

    public unsafe Result CreatePrivateDataSlot(in PrivateDataSlotCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, out PrivateDataSlot pPrivateDataSlot)
    {
        return VK.CreatePrivateDataSlot(_device, in pCreateInfo, pAllocator, out pPrivateDataSlot);
    }

    public unsafe Result CreatePrivateDataSlot(in PrivateDataSlotCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, PrivateDataSlot* pPrivateDataSlot)
    {
        return VK.CreatePrivateDataSlot(_device, in pCreateInfo, in pAllocator, pPrivateDataSlot);
    }

    public Result CreatePrivateDataSlot(in PrivateDataSlotCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, out PrivateDataSlot pPrivateDataSlot)
    {
        return VK.CreatePrivateDataSlot(_device, in pCreateInfo, in pAllocator, out pPrivateDataSlot);
    }

    public unsafe Result BindBufferMemory2(uint bindInfoCount, BindBufferMemoryInfo* pBindInfos)
    {
        return VK.BindBufferMemory2(_device, bindInfoCount, pBindInfos);
    }

    public Result BindBufferMemory2(uint bindInfoCount, in BindBufferMemoryInfo pBindInfos)
    {
        return VK.BindBufferMemory2(_device, bindInfoCount, in pBindInfos);
    }

    public unsafe Result BindImageMemory2(uint bindInfoCount, BindImageMemoryInfo* pBindInfos)
    {
        return VK.BindImageMemory2(_device, bindInfoCount, pBindInfos);
    }

    public Result BindImageMemory2(uint bindInfoCount, in BindImageMemoryInfo pBindInfos)
    {
        return VK.BindImageMemory2(_device, bindInfoCount, in pBindInfos);
    }

    public unsafe Result CreateDescriptorUpdateTemplate(DescriptorUpdateTemplateCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, DescriptorUpdateTemplate* pDescriptorUpdateTemplate)
    {
        return VK.CreateDescriptorUpdateTemplate(_device, pCreateInfo, pAllocator, pDescriptorUpdateTemplate);
    }

    public unsafe Result CreateDescriptorUpdateTemplate(DescriptorUpdateTemplateCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, out DescriptorUpdateTemplate pDescriptorUpdateTemplate)
    {
        return VK.CreateDescriptorUpdateTemplate(_device, pCreateInfo, pAllocator, out pDescriptorUpdateTemplate);
    }

    public unsafe Result CreateDescriptorUpdateTemplate(DescriptorUpdateTemplateCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, DescriptorUpdateTemplate* pDescriptorUpdateTemplate)
    {
        return VK.CreateDescriptorUpdateTemplate(_device, pCreateInfo, in pAllocator, pDescriptorUpdateTemplate);
    }

    public unsafe Result CreateDescriptorUpdateTemplate(DescriptorUpdateTemplateCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, out DescriptorUpdateTemplate pDescriptorUpdateTemplate)
    {
        return VK.CreateDescriptorUpdateTemplate(_device, pCreateInfo, in pAllocator, out pDescriptorUpdateTemplate);
    }

    public unsafe Result CreateDescriptorUpdateTemplate(in DescriptorUpdateTemplateCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, DescriptorUpdateTemplate* pDescriptorUpdateTemplate)
    {
        return VK.CreateDescriptorUpdateTemplate(_device, in pCreateInfo, pAllocator, pDescriptorUpdateTemplate);
    }

    public unsafe Result CreateDescriptorUpdateTemplate(in DescriptorUpdateTemplateCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, out DescriptorUpdateTemplate pDescriptorUpdateTemplate)
    {
        return VK.CreateDescriptorUpdateTemplate(_device, in pCreateInfo, pAllocator, out pDescriptorUpdateTemplate);
    }

    public unsafe Result CreateDescriptorUpdateTemplate(in DescriptorUpdateTemplateCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, DescriptorUpdateTemplate* pDescriptorUpdateTemplate)
    {
        return VK.CreateDescriptorUpdateTemplate(_device, in pCreateInfo, in pAllocator, pDescriptorUpdateTemplate);
    }

    public Result CreateDescriptorUpdateTemplate(in DescriptorUpdateTemplateCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, out DescriptorUpdateTemplate pDescriptorUpdateTemplate)
    {
        return VK.CreateDescriptorUpdateTemplate(_device, in pCreateInfo, in pAllocator, out pDescriptorUpdateTemplate);
    }

    public unsafe Result CreateSamplerYcbcrConversion(SamplerYcbcrConversionCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, SamplerYcbcrConversion* pYcbcrConversion)
    {
        return VK.CreateSamplerYcbcrConversion(_device, pCreateInfo, pAllocator, pYcbcrConversion);
    }

    public unsafe Result CreateSamplerYcbcrConversion(SamplerYcbcrConversionCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, out SamplerYcbcrConversion pYcbcrConversion)
    {
        return VK.CreateSamplerYcbcrConversion(_device, pCreateInfo, pAllocator, out pYcbcrConversion);
    }

    public unsafe Result CreateSamplerYcbcrConversion(SamplerYcbcrConversionCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, SamplerYcbcrConversion* pYcbcrConversion)
    {
        return VK.CreateSamplerYcbcrConversion(_device, pCreateInfo, in pAllocator, pYcbcrConversion);
    }

    public unsafe Result CreateSamplerYcbcrConversion(SamplerYcbcrConversionCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, out SamplerYcbcrConversion pYcbcrConversion)
    {
        return VK.CreateSamplerYcbcrConversion(_device, pCreateInfo, in pAllocator, out pYcbcrConversion);
    }

    public unsafe Result CreateSamplerYcbcrConversion(in SamplerYcbcrConversionCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, SamplerYcbcrConversion* pYcbcrConversion)
    {
        return VK.CreateSamplerYcbcrConversion(_device, in pCreateInfo, pAllocator, pYcbcrConversion);
    }

    public unsafe Result CreateSamplerYcbcrConversion(in SamplerYcbcrConversionCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, out SamplerYcbcrConversion pYcbcrConversion)
    {
        return VK.CreateSamplerYcbcrConversion(_device, in pCreateInfo, pAllocator, out pYcbcrConversion);
    }

    public unsafe Result CreateSamplerYcbcrConversion(in SamplerYcbcrConversionCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, SamplerYcbcrConversion* pYcbcrConversion)
    {
        return VK.CreateSamplerYcbcrConversion(_device, in pCreateInfo, in pAllocator, pYcbcrConversion);
    }

    public Result CreateSamplerYcbcrConversion(in SamplerYcbcrConversionCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, out SamplerYcbcrConversion pYcbcrConversion)
    {
        return VK.CreateSamplerYcbcrConversion(_device, in pCreateInfo, in pAllocator, out pYcbcrConversion);
    }

    public unsafe Result AllocateCommandBuffers(CommandBufferAllocateInfo* pAllocateInfo, Silk.NET.Vulkan.CommandBuffer* pCommandBuffers)
    {
        return VK.AllocateCommandBuffers(_device, pAllocateInfo, pCommandBuffers);
    }

    public unsafe Result AllocateCommandBuffers(CommandBufferAllocateInfo* pAllocateInfo, out Silk.NET.Vulkan.CommandBuffer pCommandBuffers)
    {
        return VK.AllocateCommandBuffers(_device, pAllocateInfo, out pCommandBuffers);
    }

    public unsafe Result AllocateCommandBuffers(in CommandBufferAllocateInfo pAllocateInfo, Silk.NET.Vulkan.CommandBuffer* pCommandBuffers)
    {
        return VK.AllocateCommandBuffers(_device, in pAllocateInfo, pCommandBuffers);
    }

    public Result AllocateCommandBuffers(in CommandBufferAllocateInfo pAllocateInfo, out Silk.NET.Vulkan.CommandBuffer pCommandBuffers)
    {
        return VK.AllocateCommandBuffers(_device, in pAllocateInfo, out pCommandBuffers);
    }

    public unsafe Result AllocateDescriptorSets(DescriptorSetAllocateInfo* pAllocateInfo, Silk.NET.Vulkan.DescriptorSet* pDescriptorSets)
    {
        return VK.AllocateDescriptorSets(_device, pAllocateInfo, pDescriptorSets);
    }

    public unsafe Result AllocateDescriptorSets(DescriptorSetAllocateInfo* pAllocateInfo, out Silk.NET.Vulkan.DescriptorSet pDescriptorSets)
    {
        return VK.AllocateDescriptorSets(_device, pAllocateInfo, out pDescriptorSets);
    }

    public unsafe Result AllocateDescriptorSets(in DescriptorSetAllocateInfo pAllocateInfo, Silk.NET.Vulkan.DescriptorSet* pDescriptorSets)
    {
        return VK.AllocateDescriptorSets(_device, in pAllocateInfo, pDescriptorSets);
    }

    public Result AllocateDescriptorSets(in DescriptorSetAllocateInfo pAllocateInfo, out Silk.NET.Vulkan.DescriptorSet pDescriptorSets)
    {
        return VK.AllocateDescriptorSets(_device, in pAllocateInfo, out pDescriptorSets);
    }

    public unsafe Result AllocateMemory(MemoryAllocateInfo* pAllocateInfo, AllocationCallbacks* pAllocator, DeviceMemory* pMemory)
    {
        return VK.AllocateMemory(_device, pAllocateInfo, pAllocator, pMemory);
    }

    public unsafe Result AllocateMemory(MemoryAllocateInfo* pAllocateInfo, AllocationCallbacks* pAllocator, out DeviceMemory pMemory)
    {
        return VK.AllocateMemory(_device, pAllocateInfo, pAllocator, out pMemory);
    }

    public unsafe Result AllocateMemory(MemoryAllocateInfo* pAllocateInfo, in AllocationCallbacks pAllocator, DeviceMemory* pMemory)
    {
        return VK.AllocateMemory(_device, pAllocateInfo, in pAllocator, pMemory);
    }

    public unsafe Result AllocateMemory(MemoryAllocateInfo* pAllocateInfo, in AllocationCallbacks pAllocator, out DeviceMemory pMemory)
    {
        return VK.AllocateMemory(_device, pAllocateInfo, in pAllocator, out pMemory);
    }

    public unsafe Result AllocateMemory(in MemoryAllocateInfo pAllocateInfo, AllocationCallbacks* pAllocator, DeviceMemory* pMemory)
    {
        return VK.AllocateMemory(_device, in pAllocateInfo, pAllocator, pMemory);
    }

    public unsafe Result AllocateMemory(in MemoryAllocateInfo pAllocateInfo, AllocationCallbacks* pAllocator, out DeviceMemory pMemory)
    {
        return VK.AllocateMemory(_device, in pAllocateInfo, pAllocator, out pMemory);
    }

    public unsafe Result AllocateMemory(in MemoryAllocateInfo pAllocateInfo, in AllocationCallbacks pAllocator, DeviceMemory* pMemory)
    {
        return VK.AllocateMemory(_device, in pAllocateInfo, in pAllocator, pMemory);
    }

    public Result AllocateMemory(in MemoryAllocateInfo pAllocateInfo, in AllocationCallbacks pAllocator, out DeviceMemory pMemory)
    {
        return VK.AllocateMemory(_device, in pAllocateInfo, in pAllocator, out pMemory);
    }

    public unsafe Result CreateBuffer(BufferCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, Silk.NET.Vulkan.Buffer* pBuffer)
    {
        return VK.CreateBuffer(_device, pCreateInfo, pAllocator, pBuffer);
    }

    public unsafe Result CreateBuffer(BufferCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, out Silk.NET.Vulkan.Buffer pBuffer)
    {
        return VK.CreateBuffer(_device, pCreateInfo, pAllocator, out pBuffer);
    }

    public unsafe Result CreateBuffer(BufferCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, Silk.NET.Vulkan.Buffer* pBuffer)
    {
        return VK.CreateBuffer(_device, pCreateInfo, in pAllocator, pBuffer);
    }

    public unsafe Result CreateBuffer(BufferCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, out Silk.NET.Vulkan.Buffer pBuffer)
    {
        return VK.CreateBuffer(_device, pCreateInfo, in pAllocator, out pBuffer);
    }

    public unsafe Result CreateBuffer(in BufferCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, Silk.NET.Vulkan.Buffer* pBuffer)
    {
        return VK.CreateBuffer(_device, in pCreateInfo, pAllocator, pBuffer);
    }

    public unsafe Result CreateBuffer(in BufferCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, out Silk.NET.Vulkan.Buffer pBuffer)
    {
        return VK.CreateBuffer(_device, in pCreateInfo, pAllocator, out pBuffer);
    }

    public unsafe Result CreateBuffer(in BufferCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, Silk.NET.Vulkan.Buffer* pBuffer)
    {
        return VK.CreateBuffer(_device, in pCreateInfo, in pAllocator, pBuffer);
    }

    public Result CreateBuffer(in BufferCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, out Silk.NET.Vulkan.Buffer pBuffer)
    {
        return VK.CreateBuffer(_device, in pCreateInfo, in pAllocator, out pBuffer);
    }

    public unsafe Result CreateBufferView(BufferViewCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, BufferView* pView)
    {
        return VK.CreateBufferView(_device, pCreateInfo, pAllocator, pView);
    }

    public unsafe Result CreateBufferView(BufferViewCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, out BufferView pView)
    {
        return VK.CreateBufferView(_device, pCreateInfo, pAllocator, out pView);
    }

    public unsafe Result CreateBufferView(BufferViewCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, BufferView* pView)
    {
        return VK.CreateBufferView(_device, pCreateInfo, in pAllocator, pView);
    }

    public unsafe Result CreateBufferView(BufferViewCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, out BufferView pView)
    {
        return VK.CreateBufferView(_device, pCreateInfo, in pAllocator, out pView);
    }

    public unsafe Result CreateBufferView(in BufferViewCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, BufferView* pView)
    {
        return VK.CreateBufferView(_device, in pCreateInfo, pAllocator, pView);
    }

    public unsafe Result CreateBufferView(in BufferViewCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, out BufferView pView)
    {
        return VK.CreateBufferView(_device, in pCreateInfo, pAllocator, out pView);
    }

    public unsafe Result CreateBufferView(in BufferViewCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, BufferView* pView)
    {
        return VK.CreateBufferView(_device, in pCreateInfo, in pAllocator, pView);
    }

    public Result CreateBufferView(in BufferViewCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, out BufferView pView)
    {
        return VK.CreateBufferView(_device, in pCreateInfo, in pAllocator, out pView);
    }

    public unsafe Result CreateCommandPool(CommandPoolCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, Silk.NET.Vulkan.CommandPool* pCommandPool)
    {
        return VK.CreateCommandPool(_device, pCreateInfo, pAllocator, pCommandPool);
    }

    public unsafe Result CreateCommandPool(CommandPoolCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, out Silk.NET.Vulkan.CommandPool pCommandPool)
    {
        return VK.CreateCommandPool(_device, pCreateInfo, pAllocator, out pCommandPool);
    }

    public unsafe Result CreateCommandPool(CommandPoolCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, Silk.NET.Vulkan.CommandPool* pCommandPool)
    {
        return VK.CreateCommandPool(_device, pCreateInfo, in pAllocator, pCommandPool);
    }

    public unsafe Result CreateCommandPool(CommandPoolCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, out Silk.NET.Vulkan.CommandPool pCommandPool)
    {
        return VK.CreateCommandPool(_device, pCreateInfo, in pAllocator, out pCommandPool);
    }

    public unsafe Result CreateCommandPool(in CommandPoolCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, Silk.NET.Vulkan.CommandPool* pCommandPool)
    {
        return VK.CreateCommandPool(_device, in pCreateInfo, pAllocator, pCommandPool);
    }

    public unsafe Result CreateCommandPool(in CommandPoolCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, out Silk.NET.Vulkan.CommandPool pCommandPool)
    {
        return VK.CreateCommandPool(_device, in pCreateInfo, pAllocator, out pCommandPool);
    }

    public unsafe Result CreateCommandPool(in CommandPoolCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, Silk.NET.Vulkan.CommandPool* pCommandPool)
    {
        return VK.CreateCommandPool(_device, in pCreateInfo, in pAllocator, pCommandPool);
    }

    public Result CreateCommandPool(in CommandPoolCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, out Silk.NET.Vulkan.CommandPool pCommandPool)
    {
        return VK.CreateCommandPool(_device, in pCreateInfo, in pAllocator, out pCommandPool);
    }

    public unsafe Result CreateComputePipelines(PipelineCache pipelineCache, uint createInfoCount, ComputePipelineCreateInfo* pCreateInfos, AllocationCallbacks* pAllocator, Silk.NET.Vulkan.Pipeline* pPipelines)
    {
        return VK.CreateComputePipelines(_device, pipelineCache, createInfoCount, pCreateInfos, pAllocator, pPipelines);
    }

    public unsafe Result CreateComputePipelines(PipelineCache pipelineCache, uint createInfoCount, ComputePipelineCreateInfo* pCreateInfos, AllocationCallbacks* pAllocator, out Silk.NET.Vulkan.Pipeline pPipelines)
    {
        return VK.CreateComputePipelines(_device, pipelineCache, createInfoCount, pCreateInfos, pAllocator, out pPipelines);
    }

    public unsafe Result CreateComputePipelines(PipelineCache pipelineCache, uint createInfoCount, ComputePipelineCreateInfo* pCreateInfos, in AllocationCallbacks pAllocator, Silk.NET.Vulkan.Pipeline* pPipelines)
    {
        return VK.CreateComputePipelines(_device, pipelineCache, createInfoCount, pCreateInfos, in pAllocator, pPipelines);
    }

    public unsafe Result CreateComputePipelines(PipelineCache pipelineCache, uint createInfoCount, ComputePipelineCreateInfo* pCreateInfos, in AllocationCallbacks pAllocator, out Silk.NET.Vulkan.Pipeline pPipelines)
    {
        return VK.CreateComputePipelines(_device, pipelineCache, createInfoCount, pCreateInfos, in pAllocator, out pPipelines);
    }

    public unsafe Result CreateComputePipelines(PipelineCache pipelineCache, uint createInfoCount, in ComputePipelineCreateInfo pCreateInfos, AllocationCallbacks* pAllocator, Silk.NET.Vulkan.Pipeline* pPipelines)
    {
        return VK.CreateComputePipelines(_device, pipelineCache, createInfoCount, in pCreateInfos, pAllocator, pPipelines);
    }

    public unsafe Result CreateComputePipelines(PipelineCache pipelineCache, uint createInfoCount, in ComputePipelineCreateInfo pCreateInfos, AllocationCallbacks* pAllocator, out Silk.NET.Vulkan.Pipeline pPipelines)
    {
        return VK.CreateComputePipelines(_device, pipelineCache, createInfoCount, in pCreateInfos, pAllocator, out pPipelines);
    }

    public unsafe Result CreateComputePipelines(PipelineCache pipelineCache, uint createInfoCount, in ComputePipelineCreateInfo pCreateInfos, in AllocationCallbacks pAllocator, Silk.NET.Vulkan.Pipeline* pPipelines)
    {
        return VK.CreateComputePipelines(_device, pipelineCache, createInfoCount, in pCreateInfos, in pAllocator, pPipelines);
    }

    public Result CreateComputePipelines(PipelineCache pipelineCache, uint createInfoCount, in ComputePipelineCreateInfo pCreateInfos, in AllocationCallbacks pAllocator,
        out Silk.NET.Vulkan.Pipeline pPipelines)
    {
        return VK.CreateComputePipelines(_device, pipelineCache, createInfoCount, in pCreateInfos, in pAllocator, out pPipelines);
    }

    public unsafe Result CreateDescriptorPool(DescriptorPoolCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, Silk.NET.Vulkan.DescriptorPool* pDescriptorPool)
    {
        return VK.CreateDescriptorPool(_device, pCreateInfo, pAllocator, pDescriptorPool);
    }

    public unsafe Result CreateDescriptorPool(DescriptorPoolCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, out Silk.NET.Vulkan.DescriptorPool pDescriptorPool)
    {
        return VK.CreateDescriptorPool(_device, pCreateInfo, pAllocator, out pDescriptorPool);
    }

    public unsafe Result CreateDescriptorPool(DescriptorPoolCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, Silk.NET.Vulkan.DescriptorPool* pDescriptorPool)
    {
        return VK.CreateDescriptorPool(_device, pCreateInfo, in pAllocator, pDescriptorPool);
    }

    public unsafe Result CreateDescriptorPool(DescriptorPoolCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, out Silk.NET.Vulkan.DescriptorPool pDescriptorPool)
    {
        return VK.CreateDescriptorPool(_device, pCreateInfo, in pAllocator, out pDescriptorPool);
    }

    public unsafe Result CreateDescriptorPool(in DescriptorPoolCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, Silk.NET.Vulkan.DescriptorPool* pDescriptorPool)
    {
        return VK.CreateDescriptorPool(_device, in pCreateInfo, pAllocator, pDescriptorPool);
    }

    public unsafe Result CreateDescriptorPool(in DescriptorPoolCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, out Silk.NET.Vulkan.DescriptorPool pDescriptorPool)
    {
        return VK.CreateDescriptorPool(_device, in pCreateInfo, pAllocator, out pDescriptorPool);
    }

    public unsafe Result CreateDescriptorPool(in DescriptorPoolCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, Silk.NET.Vulkan.DescriptorPool* pDescriptorPool)
    {
        return VK.CreateDescriptorPool(_device, in pCreateInfo, in pAllocator, pDescriptorPool);
    }

    public Result CreateDescriptorPool(in DescriptorPoolCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, out Silk.NET.Vulkan.DescriptorPool pDescriptorPool)
    {
        return VK.CreateDescriptorPool(_device, in pCreateInfo, in pAllocator, out pDescriptorPool);
    }

    public unsafe Result CreateDescriptorSetLayout(DescriptorSetLayoutCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, Silk.NET.Vulkan.DescriptorSetLayout* pSetLayout)
    {
        return VK.CreateDescriptorSetLayout(_device, pCreateInfo, pAllocator, pSetLayout);
    }

    public unsafe Result CreateDescriptorSetLayout(DescriptorSetLayoutCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, out Silk.NET.Vulkan.DescriptorSetLayout pSetLayout)
    {
        return VK.CreateDescriptorSetLayout(_device, pCreateInfo, pAllocator, out pSetLayout);
    }

    public unsafe Result CreateDescriptorSetLayout(DescriptorSetLayoutCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, Silk.NET.Vulkan.DescriptorSetLayout* pSetLayout)
    {
        return VK.CreateDescriptorSetLayout(_device, pCreateInfo, in pAllocator, pSetLayout);
    }

    public unsafe Result CreateDescriptorSetLayout(DescriptorSetLayoutCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, out Silk.NET.Vulkan.DescriptorSetLayout pSetLayout)
    {
        return VK.CreateDescriptorSetLayout(_device, pCreateInfo, in pAllocator, out pSetLayout);
    }

    public unsafe Result CreateDescriptorSetLayout(in DescriptorSetLayoutCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, Silk.NET.Vulkan.DescriptorSetLayout* pSetLayout)
    {
        return VK.CreateDescriptorSetLayout(_device, in pCreateInfo, pAllocator, pSetLayout);
    }

    public unsafe Result CreateDescriptorSetLayout(in DescriptorSetLayoutCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, out Silk.NET.Vulkan.DescriptorSetLayout pSetLayout)
    {
        return VK.CreateDescriptorSetLayout(_device, in pCreateInfo, pAllocator, out pSetLayout);
    }

    public unsafe Result CreateDescriptorSetLayout(in DescriptorSetLayoutCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, Silk.NET.Vulkan.DescriptorSetLayout* pSetLayout)
    {
        return VK.CreateDescriptorSetLayout(_device, in pCreateInfo, in pAllocator, pSetLayout);
    }

    public Result CreateDescriptorSetLayout(in DescriptorSetLayoutCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, out Silk.NET.Vulkan.DescriptorSetLayout pSetLayout)
    {
        return VK.CreateDescriptorSetLayout(_device, in pCreateInfo, in pAllocator, out pSetLayout);
    }

    public unsafe Result CreateEvent(EventCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, Event* pEvent)
    {
        return VK.CreateEvent(_device, pCreateInfo, pAllocator, pEvent);
    }

    public unsafe Result CreateEvent(EventCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, out Event pEvent)
    {
        return VK.CreateEvent(_device, pCreateInfo, pAllocator, out pEvent);
    }

    public unsafe Result CreateEvent(EventCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, Event* pEvent)
    {
        return VK.CreateEvent(_device, pCreateInfo, in pAllocator, pEvent);
    }

    public unsafe Result CreateEvent(EventCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, out Event pEvent)
    {
        return VK.CreateEvent(_device, pCreateInfo, in pAllocator, out pEvent);
    }

    public unsafe Result CreateEvent(in EventCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, Event* pEvent)
    {
        return VK.CreateEvent(_device, in pCreateInfo, pAllocator, pEvent);
    }

    public unsafe Result CreateEvent(in EventCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, out Event pEvent)
    {
        return VK.CreateEvent(_device, in pCreateInfo, pAllocator, out pEvent);
    }

    public unsafe Result CreateEvent(in EventCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, Event* pEvent)
    {
        return VK.CreateEvent(_device, in pCreateInfo, in pAllocator, pEvent);
    }

    public Result CreateEvent(in EventCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, out Event pEvent)
    {
        return VK.CreateEvent(_device, in pCreateInfo, in pAllocator, out pEvent);
    }

    public unsafe Result CreateFence(FenceCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, Silk.NET.Vulkan.Fence* pFence)
    {
        return VK.CreateFence(_device, pCreateInfo, pAllocator, pFence);
    }

    public unsafe Result CreateFence(FenceCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, out Silk.NET.Vulkan.Fence pFence)
    {
        return VK.CreateFence(_device, pCreateInfo, pAllocator, out pFence);
    }

    public unsafe Result CreateFence(FenceCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, Silk.NET.Vulkan.Fence* pFence)
    {
        return VK.CreateFence(_device, pCreateInfo, in pAllocator, pFence);
    }

    public unsafe Result CreateFence(FenceCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, out Silk.NET.Vulkan.Fence pFence)
    {
        return VK.CreateFence(_device, pCreateInfo, in pAllocator, out pFence);
    }

    public unsafe Result CreateFence(in FenceCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, Silk.NET.Vulkan.Fence* pFence)
    {
        return VK.CreateFence(_device, in pCreateInfo, pAllocator, pFence);
    }

    public unsafe Result CreateFence(in FenceCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, out Silk.NET.Vulkan.Fence pFence)
    {
        return VK.CreateFence(_device, in pCreateInfo, pAllocator, out pFence);
    }

    public unsafe Result CreateFence(in FenceCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, Silk.NET.Vulkan.Fence* pFence)
    {
        return VK.CreateFence(_device, in pCreateInfo, in pAllocator, pFence);
    }

    public Result CreateFence(in FenceCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, out Silk.NET.Vulkan.Fence pFence)
    {
        return VK.CreateFence(_device, in pCreateInfo, in pAllocator, out pFence);
    }

    public unsafe Result CreateFramebuffer(FramebufferCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, Silk.NET.Vulkan.Framebuffer* pFramebuffer)
    {
        return VK.CreateFramebuffer(_device, pCreateInfo, pAllocator, pFramebuffer);
    }

    public unsafe void CreateFramebuffer(FramebufferCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, out Silk.NET.Vulkan.Framebuffer pFramebuffer)
    {
        Helpers.CheckErrors(VK.CreateFramebuffer(_device, pCreateInfo, pAllocator, out pFramebuffer));
    }

    public unsafe Result CreateFramebuffer(FramebufferCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, Silk.NET.Vulkan.Framebuffer* pFramebuffer)
    {
        return VK.CreateFramebuffer(_device, pCreateInfo, in pAllocator, pFramebuffer);
    }

    public unsafe Result CreateFramebuffer(FramebufferCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, out Silk.NET.Vulkan.Framebuffer pFramebuffer)
    {
        return VK.CreateFramebuffer(_device, pCreateInfo, in pAllocator, out pFramebuffer);
    }

    public unsafe Result CreateFramebuffer(in FramebufferCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, Silk.NET.Vulkan.Framebuffer* pFramebuffer)
    {
        return VK.CreateFramebuffer(_device, in pCreateInfo, pAllocator, pFramebuffer);
    }

    public unsafe Result CreateFramebuffer(in FramebufferCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, out Silk.NET.Vulkan.Framebuffer pFramebuffer)
    {
        return VK.CreateFramebuffer(_device, in pCreateInfo, pAllocator, out pFramebuffer);
    }

    public unsafe Result CreateFramebuffer(in FramebufferCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, Silk.NET.Vulkan.Framebuffer* pFramebuffer)
    {
        return VK.CreateFramebuffer(_device, in pCreateInfo, in pAllocator, pFramebuffer);
    }

    public Result CreateFramebuffer(in FramebufferCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, out Silk.NET.Vulkan.Framebuffer pFramebuffer)
    {
        return VK.CreateFramebuffer(_device, in pCreateInfo, in pAllocator, out pFramebuffer);
    }

    public unsafe Result CreateGraphicsPipelines(PipelineCache pipelineCache, uint createInfoCount, GraphicsPipelineCreateInfo* pCreateInfos, AllocationCallbacks* pAllocator, Silk.NET.Vulkan.Pipeline* pPipelines)
    {
        return VK.CreateGraphicsPipelines(_device, pipelineCache, createInfoCount, pCreateInfos, pAllocator, pPipelines);
    }

    public unsafe Result CreateGraphicsPipelines(PipelineCache pipelineCache, uint createInfoCount, GraphicsPipelineCreateInfo* pCreateInfos, AllocationCallbacks* pAllocator, out Silk.NET.Vulkan.Pipeline pPipelines)
    {
        return VK.CreateGraphicsPipelines(_device, pipelineCache, createInfoCount, pCreateInfos, pAllocator, out pPipelines);
    }

    public unsafe Result CreateGraphicsPipelines(PipelineCache pipelineCache, uint createInfoCount, GraphicsPipelineCreateInfo* pCreateInfos, in AllocationCallbacks pAllocator, Silk.NET.Vulkan.Pipeline* pPipelines)
    {
        return VK.CreateGraphicsPipelines(_device, pipelineCache, createInfoCount, pCreateInfos, in pAllocator, pPipelines);
    }

    public unsafe Result CreateGraphicsPipelines(PipelineCache pipelineCache, uint createInfoCount, GraphicsPipelineCreateInfo* pCreateInfos, in AllocationCallbacks pAllocator,
        out Silk.NET.Vulkan.Pipeline pPipelines)
    {
        return VK.CreateGraphicsPipelines(_device, pipelineCache, createInfoCount, pCreateInfos, in pAllocator, out pPipelines);
    }

    public unsafe Result CreateGraphicsPipelines(PipelineCache pipelineCache, uint createInfoCount, in GraphicsPipelineCreateInfo pCreateInfos, AllocationCallbacks* pAllocator, Silk.NET.Vulkan.Pipeline* pPipelines)
    {
        return VK.CreateGraphicsPipelines(_device, pipelineCache, createInfoCount, in pCreateInfos, pAllocator, pPipelines);
    }

    public unsafe Result CreateGraphicsPipelines(PipelineCache pipelineCache, uint createInfoCount, in GraphicsPipelineCreateInfo pCreateInfos, AllocationCallbacks* pAllocator,
        out Silk.NET.Vulkan.Pipeline pPipelines)
    {
        return VK.CreateGraphicsPipelines(_device, pipelineCache, createInfoCount, in pCreateInfos, pAllocator, out pPipelines);
    }

    public unsafe Result CreateGraphicsPipelines(PipelineCache pipelineCache, uint createInfoCount, in GraphicsPipelineCreateInfo pCreateInfos, in AllocationCallbacks pAllocator, Silk.NET.Vulkan.Pipeline* pPipelines)
    {
        return VK.CreateGraphicsPipelines(_device, pipelineCache, createInfoCount, in pCreateInfos, in pAllocator, pPipelines);
    }

    public Result CreateGraphicsPipelines(PipelineCache pipelineCache, uint createInfoCount, in GraphicsPipelineCreateInfo pCreateInfos, in AllocationCallbacks pAllocator,
        out Silk.NET.Vulkan.Pipeline pPipelines)
    {
        return VK.CreateGraphicsPipelines(_device, pipelineCache, createInfoCount, in pCreateInfos, in pAllocator, out pPipelines);
    }

    public unsafe Result CreateImage(ImageCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, Silk.NET.Vulkan.Image* pImage)
    {
        return VK.CreateImage(_device, pCreateInfo, pAllocator, pImage);
    }

    public unsafe Result CreateImage(ImageCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, out Silk.NET.Vulkan.Image pImage)
    {
        return VK.CreateImage(_device, pCreateInfo, pAllocator, out pImage);
    }

    public unsafe Result CreateImage(ImageCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, Silk.NET.Vulkan.Image* pImage)
    {
        return VK.CreateImage(_device, pCreateInfo, in pAllocator, pImage);
    }

    public unsafe Result CreateImage(ImageCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, out Silk.NET.Vulkan.Image pImage)
    {
        return VK.CreateImage(_device, pCreateInfo, in pAllocator, out pImage);
    }

    public unsafe Result CreateImage(in ImageCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, Silk.NET.Vulkan.Image* pImage)
    {
        return VK.CreateImage(_device, in pCreateInfo, pAllocator, pImage);
    }

    public unsafe Result CreateImage(in ImageCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, out Silk.NET.Vulkan.Image pImage)
    {
        return VK.CreateImage(_device, in pCreateInfo, pAllocator, out pImage);
    }

    public unsafe Result CreateImage(in ImageCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, Silk.NET.Vulkan.Image* pImage)
    {
        return VK.CreateImage(_device, in pCreateInfo, in pAllocator, pImage);
    }

    public Result CreateImage(in ImageCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, out Silk.NET.Vulkan.Image pImage)
    {
        return VK.CreateImage(_device, in pCreateInfo, in pAllocator, out pImage);
    }

    public unsafe Result CreateImageView(ImageViewCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, Silk.NET.Vulkan.ImageView* pView)
    {
        return VK.CreateImageView(_device, pCreateInfo, pAllocator, pView);
    }

    public unsafe Result CreateImageView(ImageViewCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, out Silk.NET.Vulkan.ImageView pView)
    {
        return VK.CreateImageView(_device, pCreateInfo, pAllocator, out pView);
    }

    public unsafe Result CreateImageView(ImageViewCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, Silk.NET.Vulkan.ImageView* pView)
    {
        return VK.CreateImageView(_device, pCreateInfo, in pAllocator, pView);
    }

    public unsafe Result CreateImageView(ImageViewCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, out Silk.NET.Vulkan.ImageView pView)
    {
        return VK.CreateImageView(_device, pCreateInfo, in pAllocator, out pView);
    }

    public unsafe Result CreateImageView(in ImageViewCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, Silk.NET.Vulkan.ImageView* pView)
    {
        return VK.CreateImageView(_device, in pCreateInfo, pAllocator, pView);
    }

    public unsafe Result CreateImageView(in ImageViewCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, out Silk.NET.Vulkan.ImageView pView)
    {
        return VK.CreateImageView(_device, in pCreateInfo, pAllocator, out pView);
    }

    public unsafe Result CreateImageView(in ImageViewCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, Silk.NET.Vulkan.ImageView* pView)
    {
        return VK.CreateImageView(_device, in pCreateInfo, in pAllocator, pView);
    }

    public Result CreateImageView(in ImageViewCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, out Silk.NET.Vulkan.ImageView pView)
    {
        return VK.CreateImageView(_device, in pCreateInfo, in pAllocator, out pView);
    }

    public unsafe Result CreatePipelineCache(PipelineCacheCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, PipelineCache* pPipelineCache)
    {
        return VK.CreatePipelineCache(_device, pCreateInfo, pAllocator, pPipelineCache);
    }

    public unsafe Result CreatePipelineCache(PipelineCacheCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, out PipelineCache pPipelineCache)
    {
        return VK.CreatePipelineCache(_device, pCreateInfo, pAllocator, out pPipelineCache);
    }

    public unsafe Result CreatePipelineCache(PipelineCacheCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, PipelineCache* pPipelineCache)
    {
        return VK.CreatePipelineCache(_device, pCreateInfo, in pAllocator, pPipelineCache);
    }

    public unsafe Result CreatePipelineCache(PipelineCacheCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, out PipelineCache pPipelineCache)
    {
        return VK.CreatePipelineCache(_device, pCreateInfo, in pAllocator, out pPipelineCache);
    }

    public unsafe Result CreatePipelineCache(in PipelineCacheCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, PipelineCache* pPipelineCache)
    {
        return VK.CreatePipelineCache(_device, in pCreateInfo, pAllocator, pPipelineCache);
    }

    public unsafe Result CreatePipelineCache(in PipelineCacheCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, out PipelineCache pPipelineCache)
    {
        return VK.CreatePipelineCache(_device, in pCreateInfo, pAllocator, out pPipelineCache);
    }

    public unsafe Result CreatePipelineCache(in PipelineCacheCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, PipelineCache* pPipelineCache)
    {
        return VK.CreatePipelineCache(_device, in pCreateInfo, in pAllocator, pPipelineCache);
    }

    public Result CreatePipelineCache(in PipelineCacheCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, out PipelineCache pPipelineCache)
    {
        return VK.CreatePipelineCache(_device, in pCreateInfo, in pAllocator, out pPipelineCache);
    }

    public unsafe Result CreatePipelineLayout(PipelineLayoutCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, PipelineLayout* pPipelineLayout)
    {
        return VK.CreatePipelineLayout(_device, pCreateInfo, pAllocator, pPipelineLayout);
    }

    public unsafe Result CreatePipelineLayout(PipelineLayoutCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, out PipelineLayout pPipelineLayout)
    {
        return VK.CreatePipelineLayout(_device, pCreateInfo, pAllocator, out pPipelineLayout);
    }

    public unsafe Result CreatePipelineLayout(PipelineLayoutCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, PipelineLayout* pPipelineLayout)
    {
        return VK.CreatePipelineLayout(_device, pCreateInfo, in pAllocator, pPipelineLayout);
    }

    public unsafe Result CreatePipelineLayout(PipelineLayoutCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, out PipelineLayout pPipelineLayout)
    {
        return VK.CreatePipelineLayout(_device, pCreateInfo, in pAllocator, out pPipelineLayout);
    }

    public unsafe Result CreatePipelineLayout(in PipelineLayoutCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, PipelineLayout* pPipelineLayout)
    {
        return VK.CreatePipelineLayout(_device, in pCreateInfo, pAllocator, pPipelineLayout);
    }

    public unsafe Result CreatePipelineLayout(in PipelineLayoutCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, out PipelineLayout pPipelineLayout)
    {
        return VK.CreatePipelineLayout(_device, in pCreateInfo, pAllocator, out pPipelineLayout);
    }

    public unsafe Result CreatePipelineLayout(in PipelineLayoutCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, PipelineLayout* pPipelineLayout)
    {
        return VK.CreatePipelineLayout(_device, in pCreateInfo, in pAllocator, pPipelineLayout);
    }

    public Result CreatePipelineLayout(in PipelineLayoutCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, out PipelineLayout pPipelineLayout)
    {
        return VK.CreatePipelineLayout(_device, in pCreateInfo, in pAllocator, out pPipelineLayout);
    }

    public unsafe Result CreateQueryPool(QueryPoolCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, QueryPool* pQueryPool)
    {
        return VK.CreateQueryPool(_device, pCreateInfo, pAllocator, pQueryPool);
    }

    public unsafe Result CreateQueryPool(QueryPoolCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, out QueryPool pQueryPool)
    {
        return VK.CreateQueryPool(_device, pCreateInfo, pAllocator, out pQueryPool);
    }

    public unsafe Result CreateQueryPool(QueryPoolCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, QueryPool* pQueryPool)
    {
        return VK.CreateQueryPool(_device, pCreateInfo, in pAllocator, pQueryPool);
    }

    public unsafe Result CreateQueryPool(QueryPoolCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, out QueryPool pQueryPool)
    {
        return VK.CreateQueryPool(_device, pCreateInfo, in pAllocator, out pQueryPool);
    }

    public unsafe Result CreateQueryPool(in QueryPoolCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, QueryPool* pQueryPool)
    {
        return VK.CreateQueryPool(_device, in pCreateInfo, pAllocator, pQueryPool);
    }

    public unsafe Result CreateQueryPool(in QueryPoolCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, out QueryPool pQueryPool)
    {
        return VK.CreateQueryPool(_device, in pCreateInfo, pAllocator, out pQueryPool);
    }

    public unsafe Result CreateQueryPool(in QueryPoolCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, QueryPool* pQueryPool)
    {
        return VK.CreateQueryPool(_device, in pCreateInfo, in pAllocator, pQueryPool);
    }

    public Result CreateQueryPool(in QueryPoolCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, out QueryPool pQueryPool)
    {
        return VK.CreateQueryPool(_device, in pCreateInfo, in pAllocator, out pQueryPool);
    }

    public unsafe Result CreateRenderPass(RenderPassCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, Silk.NET.Vulkan.RenderPass* pRenderPass)
    {
        return VK.CreateRenderPass(_device, pCreateInfo, pAllocator, pRenderPass);
    }

    public unsafe Result CreateRenderPass(RenderPassCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, out Silk.NET.Vulkan.RenderPass pRenderPass)
    {
        return VK.CreateRenderPass(_device, pCreateInfo, pAllocator, out pRenderPass);
    }

    public unsafe Result CreateRenderPass(RenderPassCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, Silk.NET.Vulkan.RenderPass* pRenderPass)
    {
        return VK.CreateRenderPass(_device, pCreateInfo, in pAllocator, pRenderPass);
    }

    public unsafe Result CreateRenderPass(RenderPassCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, out Silk.NET.Vulkan.RenderPass pRenderPass)
    {
        return VK.CreateRenderPass(_device, pCreateInfo, in pAllocator, out pRenderPass);
    }

    public unsafe Result CreateRenderPass(in RenderPassCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, Silk.NET.Vulkan.RenderPass* pRenderPass)
    {
        return VK.CreateRenderPass(_device, in pCreateInfo, pAllocator, pRenderPass);
    }

    public unsafe Result CreateRenderPass(in RenderPassCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, out Silk.NET.Vulkan.RenderPass pRenderPass)
    {
        return VK.CreateRenderPass(_device, in pCreateInfo, pAllocator, out pRenderPass);
    }

    public unsafe Result CreateRenderPass(in RenderPassCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, Silk.NET.Vulkan.RenderPass* pRenderPass)
    {
        return VK.CreateRenderPass(_device, in pCreateInfo, in pAllocator, pRenderPass);
    }

    public Result CreateRenderPass(in RenderPassCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, out Silk.NET.Vulkan.RenderPass pRenderPass)
    {
        return VK.CreateRenderPass(_device, in pCreateInfo, in pAllocator, out pRenderPass);
    }

    public unsafe Result CreateSampler(SamplerCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, Silk.NET.Vulkan.Sampler* pSampler)
    {
        return VK.CreateSampler(_device, pCreateInfo, pAllocator, pSampler);
    }

    public unsafe Result CreateSampler(SamplerCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, out Silk.NET.Vulkan.Sampler pSampler)
    {
        return VK.CreateSampler(_device, pCreateInfo, pAllocator, out pSampler);
    }

    public unsafe Result CreateSampler(SamplerCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, Silk.NET.Vulkan.Sampler* pSampler)
    {
        return VK.CreateSampler(_device, pCreateInfo, in pAllocator, pSampler);
    }

    public unsafe Result CreateSampler(SamplerCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, out Silk.NET.Vulkan.Sampler pSampler)
    {
        return VK.CreateSampler(_device, pCreateInfo, in pAllocator, out pSampler);
    }

    public unsafe Result CreateSampler(in SamplerCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, Silk.NET.Vulkan.Sampler* pSampler)
    {
        return VK.CreateSampler(_device, in pCreateInfo, pAllocator, pSampler);
    }

    public unsafe Result CreateSampler(in SamplerCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, out Silk.NET.Vulkan.Sampler pSampler)
    {
        return VK.CreateSampler(_device, in pCreateInfo, pAllocator, out pSampler);
    }

    public unsafe Result CreateSampler(in SamplerCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, Silk.NET.Vulkan.Sampler* pSampler)
    {
        return VK.CreateSampler(_device, in pCreateInfo, in pAllocator, pSampler);
    }

    public Result CreateSampler(in SamplerCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, out Silk.NET.Vulkan.Sampler pSampler)
    {
        return VK.CreateSampler(_device, in pCreateInfo, in pAllocator, out pSampler);
    }

    public unsafe Result CreateSemaphore(SemaphoreCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, Semaphore* pSemaphore)
    {
        return VK.CreateSemaphore(_device, pCreateInfo, pAllocator, pSemaphore);
    }

    public unsafe Result CreateSemaphore(SemaphoreCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, out Semaphore pSemaphore)
    {
        return VK.CreateSemaphore(_device, pCreateInfo, pAllocator, out pSemaphore);
    }

    public unsafe Result CreateSemaphore(SemaphoreCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, Semaphore* pSemaphore)
    {
        return VK.CreateSemaphore(_device, pCreateInfo, in pAllocator, pSemaphore);
    }

    public unsafe Result CreateSemaphore(SemaphoreCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, out Semaphore pSemaphore)
    {
        return VK.CreateSemaphore(_device, pCreateInfo, in pAllocator, out pSemaphore);
    }

    public unsafe Result CreateSemaphore(in SemaphoreCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, Semaphore* pSemaphore)
    {
        return VK.CreateSemaphore(_device, in pCreateInfo, pAllocator, pSemaphore);
    }

    public unsafe Result CreateSemaphore(in SemaphoreCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, out Semaphore pSemaphore)
    {
        return VK.CreateSemaphore(_device, in pCreateInfo, pAllocator, out pSemaphore);
    }

    public unsafe Result CreateSemaphore(in SemaphoreCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, Semaphore* pSemaphore)
    {
        return VK.CreateSemaphore(_device, in pCreateInfo, in pAllocator, pSemaphore);
    }

    public Result CreateSemaphore(in SemaphoreCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, out Semaphore pSemaphore)
    {
        return VK.CreateSemaphore(_device, in pCreateInfo, in pAllocator, out pSemaphore);
    }

    public unsafe Result CreateShaderModule(ShaderModuleCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, ShaderModule* pShaderModule)
    {
        return VK.CreateShaderModule(_device, pCreateInfo, pAllocator, pShaderModule);
    }

    public unsafe Result CreateShaderModule(ShaderModuleCreateInfo* pCreateInfo, AllocationCallbacks* pAllocator, out ShaderModule pShaderModule)
    {
        return VK.CreateShaderModule(_device, pCreateInfo, pAllocator, out pShaderModule);
    }

    public unsafe Result CreateShaderModule(ShaderModuleCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, ShaderModule* pShaderModule)
    {
        return VK.CreateShaderModule(_device, pCreateInfo, in pAllocator, pShaderModule);
    }

    public unsafe Result CreateShaderModule(ShaderModuleCreateInfo* pCreateInfo, in AllocationCallbacks pAllocator, out ShaderModule pShaderModule)
    {
        return VK.CreateShaderModule(_device, pCreateInfo, in pAllocator, out pShaderModule);
    }

    public unsafe Result CreateShaderModule(in ShaderModuleCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, ShaderModule* pShaderModule)
    {
        return VK.CreateShaderModule(_device, in pCreateInfo, pAllocator, pShaderModule);
    }

    public unsafe Result CreateShaderModule(in ShaderModuleCreateInfo pCreateInfo, AllocationCallbacks* pAllocator, out ShaderModule pShaderModule)
    {
        return VK.CreateShaderModule(_device, in pCreateInfo, pAllocator, out pShaderModule);
    }

    public unsafe Result CreateShaderModule(in ShaderModuleCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, ShaderModule* pShaderModule)
    {
        return VK.CreateShaderModule(_device, in pCreateInfo, in pAllocator, pShaderModule);
    }

    public Result CreateShaderModule(in ShaderModuleCreateInfo pCreateInfo, in AllocationCallbacks pAllocator, out ShaderModule pShaderModule)
    {
        return VK.CreateShaderModule(_device, in pCreateInfo, in pAllocator, out pShaderModule);
    }

    public unsafe Result FlushMappedMemoryRanges(uint memoryRangeCount, MappedMemoryRange* pMemoryRanges)
    {
        return VK.FlushMappedMemoryRanges(_device, memoryRangeCount, pMemoryRanges);
    }

    public Result FlushMappedMemoryRanges(uint memoryRangeCount, in MappedMemoryRange pMemoryRanges)
    {
        return VK.FlushMappedMemoryRanges(_device, memoryRangeCount, in pMemoryRanges);
    }

    public unsafe Result FreeDescriptorSets(Silk.NET.Vulkan.DescriptorPool descriptorPool, uint descriptorSetCount, Silk.NET.Vulkan.DescriptorSet* pDescriptorSets)
    {
        return VK.FreeDescriptorSets(_device, descriptorPool, descriptorSetCount, pDescriptorSets);
    }

    public Result FreeDescriptorSets(Silk.NET.Vulkan.DescriptorPool descriptorPool, uint descriptorSetCount, in Silk.NET.Vulkan.DescriptorSet pDescriptorSets)
    {
        return VK.FreeDescriptorSets(_device, descriptorPool, descriptorSetCount, in pDescriptorSets);
    }

    public unsafe Result GetPipelineCacheData(PipelineCache pipelineCache, UIntPtr* pDataSize, void* pData)
    {
        return VK.GetPipelineCacheData(_device, pipelineCache, pDataSize, pData);
    }

    public unsafe Result GetPipelineCacheData<T0>(PipelineCache pipelineCache, UIntPtr* pDataSize, ref T0 pData) where T0 : unmanaged
    {
        return VK.GetPipelineCacheData(_device, pipelineCache, pDataSize, ref pData);
    }

    public unsafe Result GetPipelineCacheData(PipelineCache pipelineCache, ref UIntPtr pDataSize, void* pData)
    {
        return VK.GetPipelineCacheData(_device, pipelineCache, ref pDataSize, pData);
    }

    public Result GetPipelineCacheData<T0>(PipelineCache pipelineCache, ref UIntPtr pDataSize, ref T0 pData) where T0 : unmanaged
    {
        return VK.GetPipelineCacheData(_device, pipelineCache, ref pDataSize, ref pData);
    }

    public unsafe Result GetQueryPoolResults(QueryPool queryPool, uint firstQuery, uint queryCount, UIntPtr dataSize, void* pData, ulong stride, QueryResultFlags flags)
    {
        return VK.GetQueryPoolResults(_device, queryPool, firstQuery, queryCount, dataSize, pData, stride, flags);
    }

    public Result GetQueryPoolResults<T0>(QueryPool queryPool, uint firstQuery, uint queryCount, UIntPtr dataSize, ref T0 pData, ulong stride, QueryResultFlags flags) where T0 : unmanaged
    {
        return VK.GetQueryPoolResults(_device, queryPool, firstQuery, queryCount, dataSize, ref pData, stride, flags);
    }

    public unsafe Result InvalidateMappedMemoryRanges(uint memoryRangeCount, MappedMemoryRange* pMemoryRanges)
    {
        return VK.InvalidateMappedMemoryRanges(_device, memoryRangeCount, pMemoryRanges);
    }

    public Result InvalidateMappedMemoryRanges(uint memoryRangeCount, in MappedMemoryRange pMemoryRanges)
    {
        return VK.InvalidateMappedMemoryRanges(_device, memoryRangeCount, in pMemoryRanges);
    }

    public unsafe Result MapMemory(DeviceMemory memory, ulong offset, ulong size, uint flags, void** ppData)
    {
        return VK.MapMemory(_device, memory, offset, size, flags, ppData);
    }

    public unsafe Result MapMemory(DeviceMemory memory, ulong offset, ulong size, uint flags, ref void* ppData)
    {
        return VK.MapMemory(_device, memory, offset, size, flags, ref ppData);
    }

    public unsafe Result MergePipelineCaches(PipelineCache dstCache, uint srcCacheCount, PipelineCache* pSrcCaches)
    {
        return VK.MergePipelineCaches(_device, dstCache, srcCacheCount, pSrcCaches);
    }

    public Result MergePipelineCaches(PipelineCache dstCache, uint srcCacheCount, in PipelineCache pSrcCaches)
    {
        return VK.MergePipelineCaches(_device, dstCache, srcCacheCount, in pSrcCaches);
    }

    public unsafe Result ResetFences(uint fenceCount, Silk.NET.Vulkan.Fence* pFences)
    {
        return VK.ResetFences(_device, fenceCount, pFences);
    }

    public Result ResetFences(uint fenceCount, in Silk.NET.Vulkan.Fence pFences)
    {
        return VK.ResetFences(_device, fenceCount, in pFences);
    }

    public unsafe Result WaitForFences(uint fenceCount, Silk.NET.Vulkan.Fence* pFences, Bool32 waitAll, ulong timeout)
    {
        return VK.WaitForFences(_device, fenceCount, pFences, waitAll, timeout);
    }

    public Result WaitForFences(uint fenceCount, in Silk.NET.Vulkan.Fence pFences, Bool32 waitAll, ulong timeout)
    {
        return VK.WaitForFences(_device, fenceCount, in pFences, waitAll, timeout);
    }

    public unsafe ulong GetBufferDeviceAddress(BufferDeviceAddressInfo* pInfo)
    {
        return VK.GetBufferDeviceAddress(_device, pInfo);
    }

    public ulong GetBufferDeviceAddress(in BufferDeviceAddressInfo pInfo)
    {
        return VK.GetBufferDeviceAddress(_device, in pInfo);
    }

    public unsafe ulong GetBufferOpaqueCaptureAddress(BufferDeviceAddressInfo* pInfo)
    {
        return VK.GetBufferOpaqueCaptureAddress(_device, pInfo);
    }

    public ulong GetBufferOpaqueCaptureAddress(in BufferDeviceAddressInfo pInfo)
    {
        return VK.GetBufferOpaqueCaptureAddress(_device, in pInfo);
    }

    public unsafe ulong GetDeviceMemoryOpaqueCaptureAddress(DeviceMemoryOpaqueCaptureAddressInfo* pInfo)
    {
        return VK.GetDeviceMemoryOpaqueCaptureAddress(_device, pInfo);
    }

    public ulong GetDeviceMemoryOpaqueCaptureAddress(in DeviceMemoryOpaqueCaptureAddressInfo pInfo)
    {
        return VK.GetDeviceMemoryOpaqueCaptureAddress(_device, in pInfo);
    }

    public Result SetPrivateData(ObjectType objectType, ulong objectHandle, PrivateDataSlot privateDataSlot, ulong data)
    {
        return VK.SetPrivateData(_device, objectType, objectHandle, privateDataSlot, data);
    }

    public Result BindBufferMemory(Silk.NET.Vulkan.Buffer buffer, DeviceMemory memory, ulong memoryOffset)
    {
        return VK.BindBufferMemory(_device, buffer, memory, memoryOffset);
    }

    public Result BindImageMemory(Silk.NET.Vulkan.Image image, DeviceMemory memory, ulong memoryOffset)
    {
        return VK.BindImageMemory(_device, image, memory, memoryOffset);
    }

    public Result DeviceWaitIdle()
    {
        return VK.DeviceWaitIdle(_device);
    }

    public Result GetEventStatus(Event @event)
    {
        return VK.GetEventStatus(_device, @event);
    }

    public Result GetFenceStatus(Silk.NET.Vulkan.Fence fence)
    {
        return VK.GetFenceStatus(_device, fence);
    }

    public Result ResetCommandPool(Silk.NET.Vulkan.CommandPool commandPool, CommandPoolResetFlags flags)
    {
        return VK.ResetCommandPool(_device, commandPool, flags);
    }

    public Result ResetDescriptorPool(Silk.NET.Vulkan.DescriptorPool descriptorPool, uint flags)
    {
        return VK.ResetDescriptorPool(_device, descriptorPool, flags);
    }

    public Result ResetEvent(Event @event)
    {
        return VK.ResetEvent(_device, @event);
    }

    public Result SetEvent(Event @event)
    {
        return VK.SetEvent(_device, @event);
    }

    public unsafe PfnVoidFunction GetDeviceProcAddr(byte* pName)
    {
        return VK.GetDeviceProcAddr(_device, pName);
    }

    public PfnVoidFunction GetDeviceProcAddr(in byte pName)
    {
        return VK.GetDeviceProcAddr(_device, in pName);
    }

    public PfnVoidFunction GetDeviceProcAddr(string pName)
    {
        return VK.GetDeviceProcAddr(_device, pName);
    }
}

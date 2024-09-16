using Silk.NET.Core;
using Silk.NET.Vulkan;
using static Istok.Rendering.Engine;
// ReSharper disable UnusedMember.Global

namespace Istok.Rendering;
public unsafe partial class CommandBuffer
{
    public void CmdSetCullMode(CullModeFlags cullMode)
    {
        VK.CmdSetCullMode(_commandBuffer, cullMode);
    }

    public void CmdSetDepthBiasEnable(Bool32 depthBiasEnable)
    {
        VK.CmdSetDepthBiasEnable(_commandBuffer, depthBiasEnable);
    }

    public void CmdSetDepthBoundsTestEnable(Bool32 depthBoundsTestEnable)
    {
        VK.CmdSetDepthBoundsTestEnable(_commandBuffer, depthBoundsTestEnable);
    }

    public void CmdSetDepthCompareOp(CompareOp depthCompareOp)
    {
        VK.CmdSetDepthCompareOp(_commandBuffer, depthCompareOp);
    }

    public void CmdSetDepthTestEnable(Bool32 depthTestEnable)
    {
        VK.CmdSetDepthTestEnable(_commandBuffer, depthTestEnable);
    }

    public void CmdSetDepthWriteEnable(Bool32 depthWriteEnable)
    {
        VK.CmdSetDepthWriteEnable(_commandBuffer, depthWriteEnable);
    }

    public void CmdSetFrontFace(FrontFace frontFace)
    {
        VK.CmdSetFrontFace(_commandBuffer, frontFace);
    }

    public void CmdSetPrimitiveRestartEnable(Bool32 primitiveRestartEnable)
    {
        VK.CmdSetPrimitiveRestartEnable(_commandBuffer, primitiveRestartEnable);
    }

    public void CmdSetPrimitiveTopology(PrimitiveTopology primitiveTopology)
    {
        VK.CmdSetPrimitiveTopology(_commandBuffer, primitiveTopology);
    }

    public void CmdSetRasterizerDiscardEnable(Bool32 rasterizerDiscardEnable)
    {
        VK.CmdSetRasterizerDiscardEnable(_commandBuffer, rasterizerDiscardEnable);
    }

    public void CmdSetStencilTestEnable(Bool32 stencilTestEnable)
    {
        VK.CmdSetStencilTestEnable(_commandBuffer, stencilTestEnable);
    }

    public void CmdSetDeviceMask(uint deviceMask)
    {
        VK.CmdSetDeviceMask(_commandBuffer, deviceMask);
    }

    public void CmdNextSubpass(SubpassContents contents)
    {
        VK.CmdNextSubpass(_commandBuffer, contents);
    }

    public void CmdSetLineWidth(float lineWidth)
    {
        VK.CmdSetLineWidth(_commandBuffer, lineWidth);
    }

    public void CmdEndRenderPass2(SubpassEndInfo* pSubpassEndInfo)
    {
        VK.CmdEndRenderPass2(_commandBuffer, pSubpassEndInfo);
    }

    public void CmdBeginRendering(RenderingInfo* pRenderingInfo)
    {
        VK.CmdBeginRendering(_commandBuffer, pRenderingInfo);
    }

    public void CmdBlitImage2(BlitImageInfo2* pBlitImageInfo)
    {
        VK.CmdBlitImage2(_commandBuffer, pBlitImageInfo);
    }

    public void CmdCopyBuffer2(CopyBufferInfo2* pCopyBufferInfo)
    {
        VK.CmdCopyBuffer2(_commandBuffer, pCopyBufferInfo);
    }

    public void CmdCopyBufferToImage2(CopyBufferToImageInfo2* pCopyBufferToImageInfo)
    {
        VK.CmdCopyBufferToImage2(_commandBuffer, pCopyBufferToImageInfo);
    }

    public void CmdCopyImage2(CopyImageInfo2* pCopyImageInfo)
    {
        VK.CmdCopyImage2(_commandBuffer, pCopyImageInfo);
    }

    public void CmdCopyImageToBuffer2(CopyImageToBufferInfo2* pCopyImageToBufferInfo)
    {
        VK.CmdCopyImageToBuffer2(_commandBuffer, pCopyImageToBufferInfo);
    }

    public void CmdPipelineBarrier2(DependencyInfo* pDependencyInfo)
    {
        VK.CmdPipelineBarrier2(_commandBuffer, pDependencyInfo);
    }

    public void CmdResolveImage2(ResolveImageInfo2* pResolveImageInfo)
    {
        VK.CmdResolveImage2(_commandBuffer, pResolveImageInfo);
    }

    public void CmdSetBlendConstants(float* blendConstants)
    {
        VK.CmdSetBlendConstants(_commandBuffer, blendConstants);
    }

    public void CmdResetEvent2(Event @event, PipelineStageFlags2 stageMask)
    {
        VK.CmdResetEvent2(_commandBuffer, @event, stageMask);
    }

    public void CmdBindPipeline(PipelineBindPoint pipelineBindPoint, Silk.NET.Vulkan.Pipeline pipeline)
    {
        VK.CmdBindPipeline(_commandBuffer, pipelineBindPoint, pipeline);
    }

    public void CmdDispatchIndirect(Silk.NET.Vulkan.Buffer buffer, ulong offset)
    {
        VK.CmdDispatchIndirect(_commandBuffer, buffer, offset);
    }

    public void CmdEndQuery(QueryPool queryPool, uint query)
    {
        VK.CmdEndQuery(_commandBuffer, queryPool, query);
    }

    public void CmdResetEvent(Event @event, PipelineStageFlags stageMask)
    {
        VK.CmdResetEvent(_commandBuffer, @event, stageMask);
    }

    public void CmdSetDepthBounds(float minDepthBounds, float maxDepthBounds)
    {
        VK.CmdSetDepthBounds(_commandBuffer, minDepthBounds, maxDepthBounds);
    }

    public void CmdSetEvent(Event @event, PipelineStageFlags stageMask)
    {
        VK.CmdSetEvent(_commandBuffer, @event, stageMask);
    }

    public void CmdSetStencilCompareMask(StencilFaceFlags faceMask, uint compareMask)
    {
        VK.CmdSetStencilCompareMask(_commandBuffer, faceMask, compareMask);
    }

    public void CmdSetStencilReference(StencilFaceFlags faceMask, uint reference)
    {
        VK.CmdSetStencilReference(_commandBuffer, faceMask, reference);
    }

    public void CmdSetStencilWriteMask(StencilFaceFlags faceMask, uint writeMask)
    {
        VK.CmdSetStencilWriteMask(_commandBuffer, faceMask, writeMask);
    }

    public void CmdEndRendering()
    {
        VK.CmdEndRendering(_commandBuffer);
    }

    public void CmdEndRenderPass()
    {
        VK.CmdEndRenderPass(_commandBuffer);
    }

    public void CmdWriteTimestamp2(PipelineStageFlags2 stage, QueryPool queryPool, uint query)
    {
        VK.CmdWriteTimestamp2(_commandBuffer, stage, queryPool, query);
    }

    public void CmdBeginQuery(QueryPool queryPool, uint query, QueryControlFlags flags)
    {
        VK.CmdBeginQuery(_commandBuffer, queryPool, query, flags);
    }

    public void CmdBindIndexBuffer(Silk.NET.Vulkan.Buffer buffer, ulong offset, IndexType indexType)
    {
        VK.CmdBindIndexBuffer(_commandBuffer, buffer, offset, indexType);
    }

    public void CmdDispatch(uint groupCountX, uint groupCountY, uint groupCountZ)
    {
        VK.CmdDispatch(_commandBuffer, groupCountX, groupCountY, groupCountZ);
    }

    public void CmdResetQueryPool(QueryPool queryPool, uint firstQuery, uint queryCount)
    {
        VK.CmdResetQueryPool(_commandBuffer, queryPool, firstQuery, queryCount);
    }

    public void CmdSetDepthBias(float depthBiasConstantFactor, float depthBiasClamp, float depthBiasSlopeFactor)
    {
        VK.CmdSetDepthBias(_commandBuffer, depthBiasConstantFactor, depthBiasClamp, depthBiasSlopeFactor);
    }

    public void CmdWriteTimestamp(PipelineStageFlags pipelineStage, QueryPool queryPool, uint query)
    {
        VK.CmdWriteTimestamp(_commandBuffer, pipelineStage, queryPool, query);
    }

    public void CmdDraw(uint vertexCount, uint instanceCount, uint firstVertex, uint firstInstance)
    {
        VK.CmdDraw(_commandBuffer, vertexCount, instanceCount, firstVertex, firstInstance);
    }

    public void CmdDrawIndexedIndirect(Silk.NET.Vulkan.Buffer buffer, ulong offset, uint drawCount, uint stride)
    {
        VK.CmdDrawIndexedIndirect(_commandBuffer, buffer, offset, drawCount, stride);
    }

    public void CmdDrawIndirect(Silk.NET.Vulkan.Buffer buffer, ulong offset, uint drawCount, uint stride)
    {
        VK.CmdDrawIndirect(_commandBuffer, buffer, offset, drawCount, stride);
    }

    public void CmdFillBuffer(Silk.NET.Vulkan.Buffer dstBuffer, ulong dstOffset, ulong size, uint data)
    {
        VK.CmdFillBuffer(_commandBuffer, dstBuffer, dstOffset, size, data);
    }

    public void CmdSetStencilOp(StencilFaceFlags faceMask, StencilOp failOp, StencilOp passOp, StencilOp depthFailOp, CompareOp compareOp)
    {
        VK.CmdSetStencilOp(_commandBuffer, faceMask, failOp, passOp, depthFailOp, compareOp);
    }

    public void CmdDrawIndexed(uint indexCount, uint instanceCount, uint firstIndex, int vertexOffset, uint firstInstance)
    {
        VK.CmdDrawIndexed(_commandBuffer, indexCount, instanceCount, firstIndex, vertexOffset, firstInstance);
    }

    public void CmdDrawIndexedIndirectCount(Silk.NET.Vulkan.Buffer buffer, ulong offset, Silk.NET.Vulkan.Buffer countBuffer, ulong countBufferOffset, uint maxDrawCount, uint stride)
    {
        VK.CmdDrawIndexedIndirectCount(_commandBuffer, buffer, offset, countBuffer, countBufferOffset, maxDrawCount, stride);
    }

    public void CmdDrawIndirectCount(Silk.NET.Vulkan.Buffer buffer, ulong offset, Silk.NET.Vulkan.Buffer countBuffer, ulong countBufferOffset, uint maxDrawCount, uint stride)
    {
        VK.CmdDrawIndirectCount(_commandBuffer, buffer, offset, countBuffer, countBufferOffset, maxDrawCount, stride);
    }

    public void CmdDispatchBase(uint baseGroupX, uint baseGroupY, uint baseGroupZ, uint groupCountX, uint groupCountY, uint groupCountZ)
    {
        VK.CmdDispatchBase(_commandBuffer, baseGroupX, baseGroupY, baseGroupZ, groupCountX, groupCountY, groupCountZ);
    }

    public void CmdCopyQueryPoolResults(QueryPool queryPool, uint firstQuery, uint queryCount, Silk.NET.Vulkan.Buffer dstBuffer, ulong dstOffset, ulong stride, QueryResultFlags flags)
    {
        VK.CmdCopyQueryPoolResults(_commandBuffer, queryPool, firstQuery, queryCount, dstBuffer, dstOffset, stride, flags);
    }

    public void CmdBeginRenderPass2(RenderPassBeginInfo* pRenderPassBegin, SubpassBeginInfo* pSubpassBeginInfo)
    {
        VK.CmdBeginRenderPass2(_commandBuffer, pRenderPassBegin, pSubpassBeginInfo);
    }

    public void CmdNextSubpass2(SubpassBeginInfo* pSubpassBeginInfo, SubpassEndInfo* pSubpassEndInfo)
    {
        VK.CmdNextSubpass2(_commandBuffer, pSubpassBeginInfo, pSubpassEndInfo);
    }

    public void CmdSetEvent2(Event @event, DependencyInfo* pDependencyInfo)
    {
        VK.CmdSetEvent2(_commandBuffer, @event, pDependencyInfo);
    }

    public void CmdSetScissorWithCount(uint scissorCount, Rect2D* pScissors)
    {
        VK.CmdSetScissorWithCount(_commandBuffer, scissorCount, pScissors);
    }

    public void CmdSetViewportWithCount(uint viewportCount, Viewport* pViewports)
    {
        VK.CmdSetViewportWithCount(_commandBuffer, viewportCount, pViewports);
    }

    public void CmdBeginRenderPass(RenderPassBeginInfo* pRenderPassBegin, SubpassContents contents)
    {
        VK.CmdBeginRenderPass(_commandBuffer, pRenderPassBegin, contents);
    }

    public void CmdExecuteCommands(uint commandBufferCount, Silk.NET.Vulkan.CommandBuffer* pCommandBuffers)
    {
        VK.CmdExecuteCommands(_commandBuffer, commandBufferCount, pCommandBuffers);
    }

    public void CmdWaitEvents2(uint eventCount, Event* pEvents, DependencyInfo* pDependencyInfos)
    {
        VK.CmdWaitEvents2(_commandBuffer, eventCount, pEvents, pDependencyInfos);
    }

    public void CmdSetScissor(uint firstScissor, uint scissorCount, Rect2D* pScissors)
    {
        VK.CmdSetScissor(_commandBuffer, firstScissor, scissorCount, pScissors);
    }

    public void CmdSetViewport(uint firstViewport, uint viewportCount, Viewport* pViewports)
    {
        VK.CmdSetViewport(_commandBuffer, firstViewport, viewportCount, pViewports);
    }

    public void CmdBindVertexBuffers(uint firstBinding, uint bindingCount, Silk.NET.Vulkan.Buffer* pBuffers, ulong* pOffsets)
    {
        VK.CmdBindVertexBuffers(_commandBuffer, firstBinding, bindingCount, pBuffers, pOffsets);
    }

    public void CmdClearAttachments(uint attachmentCount, ClearAttachment* pAttachments, uint rectCount, ClearRect* pRects)
    {
        VK.CmdClearAttachments(_commandBuffer, attachmentCount, pAttachments, rectCount, pRects);
    }

    public void CmdCopyBuffer(Silk.NET.Vulkan.Buffer srcBuffer, Silk.NET.Vulkan.Buffer dstBuffer, uint regionCount, BufferCopy* pRegions)
    {
        VK.CmdCopyBuffer(_commandBuffer, srcBuffer, dstBuffer, regionCount, pRegions);
    }

    public void CmdUpdateBuffer(Silk.NET.Vulkan.Buffer dstBuffer, ulong dstOffset, ulong dataSize, void* pData)
    {
        VK.CmdUpdateBuffer(_commandBuffer, dstBuffer, dstOffset, dataSize, pData);
    }

    public void CmdClearColorImage(Silk.NET.Vulkan.Image image, ImageLayout imageLayout, ClearColorValue* pColor, uint rangeCount, ImageSubresourceRange* pRanges)
    {
        VK.CmdClearColorImage(_commandBuffer, image, imageLayout, pColor, rangeCount, pRanges);
    }

    public void CmdClearDepthStencilImage(Silk.NET.Vulkan.Image image, ImageLayout imageLayout, ClearDepthStencilValue* pDepthStencil, uint rangeCount,
        ImageSubresourceRange* pRanges)
    {
        VK.CmdClearDepthStencilImage(_commandBuffer, image, imageLayout, pDepthStencil, rangeCount, pRanges);
    }

    public void CmdCopyBufferToImage(Silk.NET.Vulkan.Buffer srcBuffer, Silk.NET.Vulkan.Image dstImage, ImageLayout dstImageLayout, uint regionCount, BufferImageCopy* pRegions)
    {
        VK.CmdCopyBufferToImage(_commandBuffer, srcBuffer, dstImage, dstImageLayout, regionCount, pRegions);
    }

    public void CmdCopyImageToBuffer(Silk.NET.Vulkan.Image srcImage, ImageLayout srcImageLayout, Silk.NET.Vulkan.Buffer dstBuffer, uint regionCount, BufferImageCopy* pRegions)
    {
        VK.CmdCopyImageToBuffer(_commandBuffer, srcImage, srcImageLayout, dstBuffer, regionCount, pRegions);
    }

    public void CmdPushConstants(PipelineLayout layout, ShaderStageFlags stageFlags, uint offset, uint size, void* pValues)
    {
        VK.CmdPushConstants(_commandBuffer, layout, stageFlags, offset, size, pValues);
    }

    public void CmdBindVertexBuffers2(uint firstBinding, uint bindingCount, Silk.NET.Vulkan.Buffer* pBuffers, ulong* pOffsets, ulong* pSizes, ulong* pStrides)
    {
        VK.CmdBindVertexBuffers2(_commandBuffer, firstBinding, bindingCount, pBuffers, pOffsets, pSizes, pStrides);
    }

    public void CmdCopyImage(Silk.NET.Vulkan.Image srcImage, ImageLayout srcImageLayout, Silk.NET.Vulkan.Image dstImage, ImageLayout dstImageLayout, uint regionCount, ImageCopy* pRegions)
    {
        VK.CmdCopyImage(_commandBuffer, srcImage, srcImageLayout, dstImage, dstImageLayout, regionCount, pRegions);
    }

    public void CmdResolveImage(Silk.NET.Vulkan.Image srcImage, ImageLayout srcImageLayout, Silk.NET.Vulkan.Image dstImage, ImageLayout dstImageLayout, uint regionCount, ImageResolve* pRegions)
    {
        VK.CmdResolveImage(_commandBuffer, srcImage, srcImageLayout, dstImage, dstImageLayout, regionCount, pRegions);
    }

    public void CmdBindDescriptorSets(PipelineBindPoint pipelineBindPoint, PipelineLayout layout, uint firstSet, uint descriptorSetCount,
        Silk.NET.Vulkan.DescriptorSet* pDescriptorSets, uint dynamicOffsetCount, uint* pDynamicOffsets)
    {
        VK.CmdBindDescriptorSets(_commandBuffer, pipelineBindPoint, layout, firstSet, descriptorSetCount, pDescriptorSets, dynamicOffsetCount, pDynamicOffsets);
    }

    public void CmdBlitImage(Silk.NET.Vulkan.Image srcImage, ImageLayout srcImageLayout, Silk.NET.Vulkan.Image dstImage, ImageLayout dstImageLayout, uint regionCount, ImageBlit* pRegions,
        Filter filter)
    {
        VK.CmdBlitImage(_commandBuffer, srcImage, srcImageLayout, dstImage, dstImageLayout, regionCount, pRegions, filter);
    }

    public void CmdWaitEvents(uint eventCount, Event* pEvents, PipelineStageFlags srcStageMask, PipelineStageFlags dstStageMask, uint memoryBarrierCount,
        MemoryBarrier* pMemoryBarriers, uint bufferMemoryBarrierCount, BufferMemoryBarrier* pBufferMemoryBarriers, uint imageMemoryBarrierCount, ImageMemoryBarrier* pImageMemoryBarriers)
    {
        VK.CmdWaitEvents(_commandBuffer, eventCount, pEvents, srcStageMask, dstStageMask, memoryBarrierCount, pMemoryBarriers, bufferMemoryBarrierCount, pBufferMemoryBarriers, imageMemoryBarrierCount,
            pImageMemoryBarriers);
    }

    public void CmdPipelineBarrier(PipelineStageFlags srcStageMask, PipelineStageFlags dstStageMask, DependencyFlags dependencyFlags, uint memoryBarrierCount,
        MemoryBarrier* pMemoryBarriers, uint bufferMemoryBarrierCount, BufferMemoryBarrier* pBufferMemoryBarriers, uint imageMemoryBarrierCount, ImageMemoryBarrier* pImageMemoryBarriers)
    {
        VK.CmdPipelineBarrier(_commandBuffer, srcStageMask, dstStageMask, dependencyFlags, memoryBarrierCount, pMemoryBarriers, bufferMemoryBarrierCount, pBufferMemoryBarriers, imageMemoryBarrierCount,
            pImageMemoryBarriers);
    }

    public void CmdBeginRenderPass2(RenderPassBeginInfo* pRenderPassBegin, in SubpassBeginInfo pSubpassBeginInfo)
    {
        VK.CmdBeginRenderPass2(_commandBuffer, pRenderPassBegin, in pSubpassBeginInfo);
    }

    public void CmdBeginRenderPass2(in RenderPassBeginInfo pRenderPassBegin, SubpassBeginInfo* pSubpassBeginInfo)
    {
        VK.CmdBeginRenderPass2(_commandBuffer, in pRenderPassBegin, pSubpassBeginInfo);
    }

    public void CmdBeginRenderPass2(in RenderPassBeginInfo pRenderPassBegin, in SubpassBeginInfo pSubpassBeginInfo)
    {
        VK.CmdBeginRenderPass2(_commandBuffer, in pRenderPassBegin, in pSubpassBeginInfo);
    }

    public void CmdEndRenderPass2(in SubpassEndInfo pSubpassEndInfo)
    {
        VK.CmdEndRenderPass2(_commandBuffer, in pSubpassEndInfo);
    }

    public void CmdNextSubpass2(SubpassBeginInfo* pSubpassBeginInfo, in SubpassEndInfo pSubpassEndInfo)
    {
        VK.CmdNextSubpass2(_commandBuffer, pSubpassBeginInfo, in pSubpassEndInfo);
    }

    public void CmdNextSubpass2(in SubpassBeginInfo pSubpassBeginInfo, SubpassEndInfo* pSubpassEndInfo)
    {
        VK.CmdNextSubpass2(_commandBuffer, in pSubpassBeginInfo, pSubpassEndInfo);
    }

    public void CmdNextSubpass2(in SubpassBeginInfo pSubpassBeginInfo, in SubpassEndInfo pSubpassEndInfo)
    {
        VK.CmdNextSubpass2(_commandBuffer, in pSubpassBeginInfo, in pSubpassEndInfo);
    }

    public void CmdBeginRendering(in RenderingInfo pRenderingInfo)
    {
        VK.CmdBeginRendering(_commandBuffer, in pRenderingInfo);
    }

    public void CmdBindVertexBuffers2(uint firstBinding, uint bindingCount, Silk.NET.Vulkan.Buffer* pBuffers, ulong* pOffsets, ulong* pSizes, in ulong pStrides)
    {
        VK.CmdBindVertexBuffers2(_commandBuffer, firstBinding, bindingCount, pBuffers, pOffsets, pSizes, in pStrides);
    }

    public void CmdBindVertexBuffers2(uint firstBinding, uint bindingCount, Silk.NET.Vulkan.Buffer* pBuffers, ulong* pOffsets, in ulong pSizes, ulong* pStrides)
    {
        VK.CmdBindVertexBuffers2(_commandBuffer, firstBinding, bindingCount, pBuffers, pOffsets, in pSizes, pStrides);
    }

    public void CmdBindVertexBuffers2(uint firstBinding, uint bindingCount, Silk.NET.Vulkan.Buffer* pBuffers, ulong* pOffsets, in ulong pSizes, in ulong pStrides)
    {
        VK.CmdBindVertexBuffers2(_commandBuffer, firstBinding, bindingCount, pBuffers, pOffsets, in pSizes, in pStrides);
    }

    public void CmdBindVertexBuffers2(uint firstBinding, uint bindingCount, Silk.NET.Vulkan.Buffer* pBuffers, in ulong pOffsets, ulong* pSizes, ulong* pStrides)
    {
        VK.CmdBindVertexBuffers2(_commandBuffer, firstBinding, bindingCount, pBuffers, in pOffsets, pSizes, pStrides);
    }

    public void CmdBindVertexBuffers2(uint firstBinding, uint bindingCount, Silk.NET.Vulkan.Buffer* pBuffers, in ulong pOffsets, ulong* pSizes, in ulong pStrides)
    {
        VK.CmdBindVertexBuffers2(_commandBuffer, firstBinding, bindingCount, pBuffers, in pOffsets, pSizes, in pStrides);
    }

    public void CmdBindVertexBuffers2(uint firstBinding, uint bindingCount, Silk.NET.Vulkan.Buffer* pBuffers, in ulong pOffsets, in ulong pSizes, ulong* pStrides)
    {
        VK.CmdBindVertexBuffers2(_commandBuffer, firstBinding, bindingCount, pBuffers, in pOffsets, in pSizes, pStrides);
    }

    public void CmdBindVertexBuffers2(uint firstBinding, uint bindingCount, Silk.NET.Vulkan.Buffer* pBuffers, in ulong pOffsets, in ulong pSizes, in ulong pStrides)
    {
        VK.CmdBindVertexBuffers2(_commandBuffer, firstBinding, bindingCount, pBuffers, in pOffsets, in pSizes, in pStrides);
    }

    public void CmdBindVertexBuffers2(uint firstBinding, uint bindingCount, in Silk.NET.Vulkan.Buffer pBuffers, ulong* pOffsets, ulong* pSizes, ulong* pStrides)
    {
        VK.CmdBindVertexBuffers2(_commandBuffer, firstBinding, bindingCount, in pBuffers, pOffsets, pSizes, pStrides);
    }

    public void CmdBindVertexBuffers2(uint firstBinding, uint bindingCount, in Silk.NET.Vulkan.Buffer pBuffers, ulong* pOffsets, ulong* pSizes, in ulong pStrides)
    {
        VK.CmdBindVertexBuffers2(_commandBuffer, firstBinding, bindingCount, in pBuffers, pOffsets, pSizes, in pStrides);
    }

    public void CmdBindVertexBuffers2(uint firstBinding, uint bindingCount, in Silk.NET.Vulkan.Buffer pBuffers, ulong* pOffsets, in ulong pSizes, ulong* pStrides)
    {
        VK.CmdBindVertexBuffers2(_commandBuffer, firstBinding, bindingCount, in pBuffers, pOffsets, in pSizes, pStrides);
    }

    public void CmdBindVertexBuffers2(uint firstBinding, uint bindingCount, in Silk.NET.Vulkan.Buffer pBuffers, ulong* pOffsets, in ulong pSizes, in ulong pStrides)
    {
        VK.CmdBindVertexBuffers2(_commandBuffer, firstBinding, bindingCount, in pBuffers, pOffsets, in pSizes, in pStrides);
    }

    public void CmdBindVertexBuffers2(uint firstBinding, uint bindingCount, in Silk.NET.Vulkan.Buffer pBuffers, in ulong pOffsets, ulong* pSizes, ulong* pStrides)
    {
        VK.CmdBindVertexBuffers2(_commandBuffer, firstBinding, bindingCount, in pBuffers, in pOffsets, pSizes, pStrides);
    }

    public void CmdBindVertexBuffers2(uint firstBinding, uint bindingCount, in Silk.NET.Vulkan.Buffer pBuffers, in ulong pOffsets, ulong* pSizes, in ulong pStrides)
    {
        VK.CmdBindVertexBuffers2(_commandBuffer, firstBinding, bindingCount, in pBuffers, in pOffsets, pSizes, in pStrides);
    }

    public void CmdBindVertexBuffers2(uint firstBinding, uint bindingCount, in Silk.NET.Vulkan.Buffer pBuffers, in ulong pOffsets, in ulong pSizes, ulong* pStrides)
    {
        VK.CmdBindVertexBuffers2(_commandBuffer, firstBinding, bindingCount, in pBuffers, in pOffsets, in pSizes, pStrides);
    }

    public void CmdBindVertexBuffers2(uint firstBinding, uint bindingCount, in Silk.NET.Vulkan.Buffer pBuffers, in ulong pOffsets, in ulong pSizes, in ulong pStrides)
    {
        VK.CmdBindVertexBuffers2(_commandBuffer, firstBinding, bindingCount, in pBuffers, in pOffsets, in pSizes, in pStrides);
    }

    public void CmdBlitImage2(in BlitImageInfo2 pBlitImageInfo)
    {
        VK.CmdBlitImage2(_commandBuffer, in pBlitImageInfo);
    }

    public void CmdCopyBuffer2(in CopyBufferInfo2 pCopyBufferInfo)
    {
        VK.CmdCopyBuffer2(_commandBuffer, in pCopyBufferInfo);
    }

    public void CmdCopyBufferToImage2(in CopyBufferToImageInfo2 pCopyBufferToImageInfo)
    {
        VK.CmdCopyBufferToImage2(_commandBuffer, in pCopyBufferToImageInfo);
    }

    public void CmdCopyImage2(in CopyImageInfo2 pCopyImageInfo)
    {
        VK.CmdCopyImage2(_commandBuffer, in pCopyImageInfo);
    }

    public void CmdCopyImageToBuffer2(in CopyImageToBufferInfo2 pCopyImageToBufferInfo)
    {
        VK.CmdCopyImageToBuffer2(_commandBuffer, in pCopyImageToBufferInfo);
    }

    public void CmdPipelineBarrier2(in DependencyInfo pDependencyInfo)
    {
        VK.CmdPipelineBarrier2(_commandBuffer, in pDependencyInfo);
    }

    public void CmdResolveImage2(in ResolveImageInfo2 pResolveImageInfo)
    {
        VK.CmdResolveImage2(_commandBuffer, in pResolveImageInfo);
    }

    public void CmdSetEvent2(Event @event, in DependencyInfo pDependencyInfo)
    {
        VK.CmdSetEvent2(_commandBuffer, @event, in pDependencyInfo);
    }

    public void CmdSetScissorWithCount(uint scissorCount, in Rect2D pScissors)
    {
        VK.CmdSetScissorWithCount(_commandBuffer, scissorCount, in pScissors);
    }

    public void CmdSetViewportWithCount(uint viewportCount, in Viewport pViewports)
    {
        VK.CmdSetViewportWithCount(_commandBuffer, viewportCount, in pViewports);
    }

    public void CmdWaitEvents2(uint eventCount, Event* pEvents, in DependencyInfo pDependencyInfos)
    {
        VK.CmdWaitEvents2(_commandBuffer, eventCount, pEvents, in pDependencyInfos);
    }

    public void CmdWaitEvents2(uint eventCount, in Event pEvents, DependencyInfo* pDependencyInfos)
    {
        VK.CmdWaitEvents2(_commandBuffer, eventCount, in pEvents, pDependencyInfos);
    }

    public void CmdWaitEvents2(uint eventCount, in Event pEvents, in DependencyInfo pDependencyInfos)
    {
        VK.CmdWaitEvents2(_commandBuffer, eventCount, in pEvents, in pDependencyInfos);
    }

    public void CmdBeginRenderPass(in RenderPassBeginInfo pRenderPassBegin, SubpassContents contents)
    {
        VK.CmdBeginRenderPass(_commandBuffer, in pRenderPassBegin, contents);
    }

    public void CmdBindDescriptorSets(PipelineBindPoint pipelineBindPoint, PipelineLayout layout, uint firstSet, uint descriptorSetCount,
        Silk.NET.Vulkan.DescriptorSet* pDescriptorSets, uint dynamicOffsetCount, in uint pDynamicOffsets)
    {
        VK.CmdBindDescriptorSets(_commandBuffer, pipelineBindPoint, layout, firstSet, descriptorSetCount, pDescriptorSets, dynamicOffsetCount, in pDynamicOffsets);
    }

    public void CmdBindDescriptorSets(PipelineBindPoint pipelineBindPoint, PipelineLayout layout, uint firstSet, uint descriptorSetCount,
        in Silk.NET.Vulkan.DescriptorSet pDescriptorSets, uint dynamicOffsetCount, uint* pDynamicOffsets)
    {
        VK.CmdBindDescriptorSets(_commandBuffer, pipelineBindPoint, layout, firstSet, descriptorSetCount, in pDescriptorSets, dynamicOffsetCount, pDynamicOffsets);
    }

    public void CmdBindDescriptorSets(PipelineBindPoint pipelineBindPoint, PipelineLayout layout, uint firstSet, uint descriptorSetCount,
        in Silk.NET.Vulkan.DescriptorSet pDescriptorSets, uint dynamicOffsetCount, in uint pDynamicOffsets)
    {
        VK.CmdBindDescriptorSets(_commandBuffer, pipelineBindPoint, layout, firstSet, descriptorSetCount, in pDescriptorSets, dynamicOffsetCount, in pDynamicOffsets);
    }

    public void CmdBindVertexBuffers(uint firstBinding, uint bindingCount, Silk.NET.Vulkan.Buffer* pBuffers, in ulong pOffsets)
    {
        VK.CmdBindVertexBuffers(_commandBuffer, firstBinding, bindingCount, pBuffers, in pOffsets);
    }

    public void CmdBindVertexBuffers(uint firstBinding, uint bindingCount, in Silk.NET.Vulkan.Buffer pBuffers, ulong* pOffsets)
    {
        VK.CmdBindVertexBuffers(_commandBuffer, firstBinding, bindingCount, in pBuffers, pOffsets);
    }

    public void CmdBindVertexBuffers(uint firstBinding, uint bindingCount, in Silk.NET.Vulkan.Buffer pBuffers, in ulong pOffsets)
    {
        VK.CmdBindVertexBuffers(_commandBuffer, firstBinding, bindingCount, in pBuffers, in pOffsets);
    }

    public void CmdBlitImage(Silk.NET.Vulkan.Image srcImage, ImageLayout srcImageLayout, Silk.NET.Vulkan.Image dstImage, ImageLayout dstImageLayout, uint regionCount, in ImageBlit pRegions,
        Filter filter)
    {
        VK.CmdBlitImage(_commandBuffer, srcImage, srcImageLayout, dstImage, dstImageLayout, regionCount, in pRegions, filter);
    }

    public void CmdClearAttachments(uint attachmentCount, ClearAttachment* pAttachments, uint rectCount, in ClearRect pRects)
    {
        VK.CmdClearAttachments(_commandBuffer, attachmentCount, pAttachments, rectCount, in pRects);
    }

    public void CmdClearAttachments(uint attachmentCount, in ClearAttachment pAttachments, uint rectCount, ClearRect* pRects)
    {
        VK.CmdClearAttachments(_commandBuffer, attachmentCount, in pAttachments, rectCount, pRects);
    }

    public void CmdClearAttachments(uint attachmentCount, in ClearAttachment pAttachments, uint rectCount, in ClearRect pRects)
    {
        VK.CmdClearAttachments(_commandBuffer, attachmentCount, in pAttachments, rectCount, in pRects);
    }

    public void CmdClearColorImage(Silk.NET.Vulkan.Image image, ImageLayout imageLayout, ClearColorValue* pColor, uint rangeCount, in ImageSubresourceRange pRanges)
    {
        VK.CmdClearColorImage(_commandBuffer, image, imageLayout, pColor, rangeCount, in pRanges);
    }

    public void CmdClearColorImage(Silk.NET.Vulkan.Image image, ImageLayout imageLayout, in ClearColorValue pColor, uint rangeCount, ImageSubresourceRange* pRanges)
    {
        VK.CmdClearColorImage(_commandBuffer, image, imageLayout, in pColor, rangeCount, pRanges);
    }

    public void CmdClearColorImage(Silk.NET.Vulkan.Image image, ImageLayout imageLayout, in ClearColorValue pColor, uint rangeCount, in ImageSubresourceRange pRanges)
    {
        VK.CmdClearColorImage(_commandBuffer, image, imageLayout, in pColor, rangeCount, in pRanges);
    }

    public void CmdClearDepthStencilImage(Silk.NET.Vulkan.Image image, ImageLayout imageLayout, ClearDepthStencilValue* pDepthStencil, uint rangeCount,
        in ImageSubresourceRange pRanges)
    {
        VK.CmdClearDepthStencilImage(_commandBuffer, image, imageLayout, pDepthStencil, rangeCount, in pRanges);
    }

    public void CmdClearDepthStencilImage(Silk.NET.Vulkan.Image image, ImageLayout imageLayout, in ClearDepthStencilValue pDepthStencil, uint rangeCount,
        ImageSubresourceRange* pRanges)
    {
        VK.CmdClearDepthStencilImage(_commandBuffer, image, imageLayout, in pDepthStencil, rangeCount, pRanges);
    }

    public void CmdClearDepthStencilImage(Silk.NET.Vulkan.Image image, ImageLayout imageLayout, in ClearDepthStencilValue pDepthStencil, uint rangeCount,
        in ImageSubresourceRange pRanges)
    {
        VK.CmdClearDepthStencilImage(_commandBuffer, image, imageLayout, in pDepthStencil, rangeCount, in pRanges);
    }

    public void CmdCopyBuffer(Silk.NET.Vulkan.Buffer srcBuffer, Silk.NET.Vulkan.Buffer dstBuffer, uint regionCount, in BufferCopy pRegions)
    {
        VK.CmdCopyBuffer(_commandBuffer, srcBuffer, dstBuffer, regionCount, in pRegions);
    }

    public void CmdCopyBufferToImage(Silk.NET.Vulkan.Buffer srcBuffer, Silk.NET.Vulkan.Image dstImage, ImageLayout dstImageLayout, uint regionCount, in BufferImageCopy pRegions)
    {
        VK.CmdCopyBufferToImage(_commandBuffer, srcBuffer, dstImage, dstImageLayout, regionCount, in pRegions);
    }

    public void CmdCopyImage(Silk.NET.Vulkan.Image srcImage, ImageLayout srcImageLayout, Silk.NET.Vulkan.Image dstImage, ImageLayout dstImageLayout, uint regionCount, in ImageCopy pRegions)
    {
        VK.CmdCopyImage(_commandBuffer, srcImage, srcImageLayout, dstImage, dstImageLayout, regionCount, in pRegions);
    }

    public void CmdCopyImageToBuffer(Silk.NET.Vulkan.Image srcImage, ImageLayout srcImageLayout, Silk.NET.Vulkan.Buffer dstBuffer, uint regionCount, in BufferImageCopy pRegions)
    {
        VK.CmdCopyImageToBuffer(_commandBuffer, srcImage, srcImageLayout, dstBuffer, regionCount, in pRegions);
    }

    public void CmdExecuteCommands(uint commandBufferCount, in Silk.NET.Vulkan.CommandBuffer pCommandBuffers)
    {
        VK.CmdExecuteCommands(_commandBuffer, commandBufferCount, in pCommandBuffers);
    }

    public void CmdPipelineBarrier(PipelineStageFlags srcStageMask, PipelineStageFlags dstStageMask, DependencyFlags dependencyFlags, uint memoryBarrierCount,
        MemoryBarrier* pMemoryBarriers, uint bufferMemoryBarrierCount, BufferMemoryBarrier* pBufferMemoryBarriers, uint imageMemoryBarrierCount, in ImageMemoryBarrier pImageMemoryBarriers)
    {
        VK.CmdPipelineBarrier(_commandBuffer, srcStageMask, dstStageMask, dependencyFlags, memoryBarrierCount, pMemoryBarriers, bufferMemoryBarrierCount, pBufferMemoryBarriers, imageMemoryBarrierCount,
            in pImageMemoryBarriers);
    }

    public void CmdPipelineBarrier(PipelineStageFlags srcStageMask, PipelineStageFlags dstStageMask, DependencyFlags dependencyFlags, uint memoryBarrierCount,
        MemoryBarrier* pMemoryBarriers, uint bufferMemoryBarrierCount, in BufferMemoryBarrier pBufferMemoryBarriers, uint imageMemoryBarrierCount, ImageMemoryBarrier* pImageMemoryBarriers)
    {
        VK.CmdPipelineBarrier(_commandBuffer, srcStageMask, dstStageMask, dependencyFlags, memoryBarrierCount, pMemoryBarriers, bufferMemoryBarrierCount, in pBufferMemoryBarriers,
            imageMemoryBarrierCount, pImageMemoryBarriers);
    }

    public void CmdPipelineBarrier(PipelineStageFlags srcStageMask, PipelineStageFlags dstStageMask, DependencyFlags dependencyFlags, uint memoryBarrierCount,
        MemoryBarrier* pMemoryBarriers, uint bufferMemoryBarrierCount, in BufferMemoryBarrier pBufferMemoryBarriers, uint imageMemoryBarrierCount, in ImageMemoryBarrier pImageMemoryBarriers)
    {
        VK.CmdPipelineBarrier(_commandBuffer, srcStageMask, dstStageMask, dependencyFlags, memoryBarrierCount, pMemoryBarriers, bufferMemoryBarrierCount, in pBufferMemoryBarriers,
            imageMemoryBarrierCount, in pImageMemoryBarriers);
    }

    public void CmdPipelineBarrier(PipelineStageFlags srcStageMask, PipelineStageFlags dstStageMask, DependencyFlags dependencyFlags, uint memoryBarrierCount,
        in MemoryBarrier pMemoryBarriers, uint bufferMemoryBarrierCount, BufferMemoryBarrier* pBufferMemoryBarriers, uint imageMemoryBarrierCount, ImageMemoryBarrier* pImageMemoryBarriers)
    {
        VK.CmdPipelineBarrier(_commandBuffer, srcStageMask, dstStageMask, dependencyFlags, memoryBarrierCount, in pMemoryBarriers, bufferMemoryBarrierCount, pBufferMemoryBarriers,
            imageMemoryBarrierCount, pImageMemoryBarriers);
    }

    public void CmdPipelineBarrier(PipelineStageFlags srcStageMask, PipelineStageFlags dstStageMask, DependencyFlags dependencyFlags, uint memoryBarrierCount,
        in MemoryBarrier pMemoryBarriers, uint bufferMemoryBarrierCount, BufferMemoryBarrier* pBufferMemoryBarriers, uint imageMemoryBarrierCount, in ImageMemoryBarrier pImageMemoryBarriers)
    {
        VK.CmdPipelineBarrier(_commandBuffer, srcStageMask, dstStageMask, dependencyFlags, memoryBarrierCount, in pMemoryBarriers, bufferMemoryBarrierCount, pBufferMemoryBarriers,
            imageMemoryBarrierCount, in pImageMemoryBarriers);
    }

    public void CmdPipelineBarrier(PipelineStageFlags srcStageMask, PipelineStageFlags dstStageMask, DependencyFlags dependencyFlags, uint memoryBarrierCount,
        in MemoryBarrier pMemoryBarriers, uint bufferMemoryBarrierCount, in BufferMemoryBarrier pBufferMemoryBarriers, uint imageMemoryBarrierCount, ImageMemoryBarrier* pImageMemoryBarriers)
    {
        VK.CmdPipelineBarrier(_commandBuffer, srcStageMask, dstStageMask, dependencyFlags, memoryBarrierCount, in pMemoryBarriers, bufferMemoryBarrierCount, in pBufferMemoryBarriers,
            imageMemoryBarrierCount, pImageMemoryBarriers);
    }

    public void CmdPipelineBarrier(PipelineStageFlags srcStageMask, PipelineStageFlags dstStageMask, DependencyFlags dependencyFlags, uint memoryBarrierCount,
        in MemoryBarrier pMemoryBarriers, uint bufferMemoryBarrierCount, in BufferMemoryBarrier pBufferMemoryBarriers, uint imageMemoryBarrierCount, in ImageMemoryBarrier pImageMemoryBarriers)
    {
        VK.CmdPipelineBarrier(_commandBuffer, srcStageMask, dstStageMask, dependencyFlags, memoryBarrierCount, in pMemoryBarriers, bufferMemoryBarrierCount, in pBufferMemoryBarriers,
            imageMemoryBarrierCount, in pImageMemoryBarriers);
    }

    public void CmdPushConstants<T0>(PipelineLayout layout, ShaderStageFlags stageFlags, uint offset, uint size, ref T0 pValues) where T0 : unmanaged
    {
        VK.CmdPushConstants(_commandBuffer, layout, stageFlags, offset, size, ref pValues);
    }

    public void CmdResolveImage(Silk.NET.Vulkan.Image srcImage, ImageLayout srcImageLayout, Silk.NET.Vulkan.Image dstImage, ImageLayout dstImageLayout, uint regionCount, in ImageResolve pRegions)
    {
        VK.CmdResolveImage(_commandBuffer, srcImage, srcImageLayout, dstImage, dstImageLayout, regionCount, in pRegions);
    }

    public void CmdSetBlendConstants(in float blendConstants)
    {
        VK.CmdSetBlendConstants(_commandBuffer, in blendConstants);
    }

    public void CmdSetScissor(uint firstScissor, uint scissorCount, in Rect2D pScissors)
    {
        VK.CmdSetScissor(_commandBuffer, firstScissor, scissorCount, in pScissors);
    }

    public void CmdSetViewport(uint firstViewport, uint viewportCount, in Viewport pViewports)
    {
        VK.CmdSetViewport(_commandBuffer, firstViewport, viewportCount, in pViewports);
    }

    public void CmdUpdateBuffer<T0>(Silk.NET.Vulkan.Buffer dstBuffer, ulong dstOffset, ulong dataSize, ref T0 pData) where T0 : unmanaged
    {
        VK.CmdUpdateBuffer(_commandBuffer, dstBuffer, dstOffset, dataSize, ref pData);
    }

    public void CmdWaitEvents(uint eventCount, Event* pEvents, PipelineStageFlags srcStageMask, PipelineStageFlags dstStageMask, uint memoryBarrierCount,
        MemoryBarrier* pMemoryBarriers, uint bufferMemoryBarrierCount, BufferMemoryBarrier* pBufferMemoryBarriers, uint imageMemoryBarrierCount, in ImageMemoryBarrier pImageMemoryBarriers)
    {
        VK.CmdWaitEvents(_commandBuffer, eventCount, pEvents, srcStageMask, dstStageMask, memoryBarrierCount, pMemoryBarriers, bufferMemoryBarrierCount, pBufferMemoryBarriers, imageMemoryBarrierCount,
            in pImageMemoryBarriers);
    }

    public void CmdWaitEvents(uint eventCount, Event* pEvents, PipelineStageFlags srcStageMask, PipelineStageFlags dstStageMask, uint memoryBarrierCount,
        MemoryBarrier* pMemoryBarriers, uint bufferMemoryBarrierCount, in BufferMemoryBarrier pBufferMemoryBarriers, uint imageMemoryBarrierCount, ImageMemoryBarrier* pImageMemoryBarriers)
    {
        VK.CmdWaitEvents(_commandBuffer, eventCount, pEvents, srcStageMask, dstStageMask, memoryBarrierCount, pMemoryBarriers, bufferMemoryBarrierCount, in pBufferMemoryBarriers,
            imageMemoryBarrierCount, pImageMemoryBarriers);
    }

    public void CmdWaitEvents(uint eventCount, Event* pEvents, PipelineStageFlags srcStageMask, PipelineStageFlags dstStageMask, uint memoryBarrierCount,
        MemoryBarrier* pMemoryBarriers, uint bufferMemoryBarrierCount, in BufferMemoryBarrier pBufferMemoryBarriers, uint imageMemoryBarrierCount, in ImageMemoryBarrier pImageMemoryBarriers)
    {
        VK.CmdWaitEvents(_commandBuffer, eventCount, pEvents, srcStageMask, dstStageMask, memoryBarrierCount, pMemoryBarriers, bufferMemoryBarrierCount, in pBufferMemoryBarriers,
            imageMemoryBarrierCount, in pImageMemoryBarriers);
    }

    public void CmdWaitEvents(uint eventCount, Event* pEvents, PipelineStageFlags srcStageMask, PipelineStageFlags dstStageMask, uint memoryBarrierCount,
        in MemoryBarrier pMemoryBarriers, uint bufferMemoryBarrierCount, BufferMemoryBarrier* pBufferMemoryBarriers, uint imageMemoryBarrierCount, ImageMemoryBarrier* pImageMemoryBarriers)
    {
        VK.CmdWaitEvents(_commandBuffer, eventCount, pEvents, srcStageMask, dstStageMask, memoryBarrierCount, in pMemoryBarriers, bufferMemoryBarrierCount, pBufferMemoryBarriers,
            imageMemoryBarrierCount, pImageMemoryBarriers);
    }

    public void CmdWaitEvents(uint eventCount, Event* pEvents, PipelineStageFlags srcStageMask, PipelineStageFlags dstStageMask, uint memoryBarrierCount,
        in MemoryBarrier pMemoryBarriers, uint bufferMemoryBarrierCount, BufferMemoryBarrier* pBufferMemoryBarriers, uint imageMemoryBarrierCount, in ImageMemoryBarrier pImageMemoryBarriers)
    {
        VK.CmdWaitEvents(_commandBuffer, eventCount, pEvents, srcStageMask, dstStageMask, memoryBarrierCount, in pMemoryBarriers, bufferMemoryBarrierCount, pBufferMemoryBarriers,
            imageMemoryBarrierCount, in pImageMemoryBarriers);
    }

    public void CmdWaitEvents(uint eventCount, Event* pEvents, PipelineStageFlags srcStageMask, PipelineStageFlags dstStageMask, uint memoryBarrierCount,
        in MemoryBarrier pMemoryBarriers, uint bufferMemoryBarrierCount, in BufferMemoryBarrier pBufferMemoryBarriers, uint imageMemoryBarrierCount, ImageMemoryBarrier* pImageMemoryBarriers)
    {
        VK.CmdWaitEvents(_commandBuffer, eventCount, pEvents, srcStageMask, dstStageMask, memoryBarrierCount, in pMemoryBarriers, bufferMemoryBarrierCount, in pBufferMemoryBarriers,
            imageMemoryBarrierCount, pImageMemoryBarriers);
    }

    public void CmdWaitEvents(uint eventCount, Event* pEvents, PipelineStageFlags srcStageMask, PipelineStageFlags dstStageMask, uint memoryBarrierCount,
        in MemoryBarrier pMemoryBarriers, uint bufferMemoryBarrierCount, in BufferMemoryBarrier pBufferMemoryBarriers, uint imageMemoryBarrierCount, in ImageMemoryBarrier pImageMemoryBarriers)
    {
        VK.CmdWaitEvents(_commandBuffer, eventCount, pEvents, srcStageMask, dstStageMask, memoryBarrierCount, in pMemoryBarriers, bufferMemoryBarrierCount, in pBufferMemoryBarriers,
            imageMemoryBarrierCount, in pImageMemoryBarriers);
    }

    public void CmdWaitEvents(uint eventCount, in Event pEvents, PipelineStageFlags srcStageMask, PipelineStageFlags dstStageMask, uint memoryBarrierCount,
        MemoryBarrier* pMemoryBarriers, uint bufferMemoryBarrierCount, BufferMemoryBarrier* pBufferMemoryBarriers, uint imageMemoryBarrierCount, ImageMemoryBarrier* pImageMemoryBarriers)
    {
        VK.CmdWaitEvents(_commandBuffer, eventCount, in pEvents, srcStageMask, dstStageMask, memoryBarrierCount, pMemoryBarriers, bufferMemoryBarrierCount, pBufferMemoryBarriers,
            imageMemoryBarrierCount, pImageMemoryBarriers);
    }

    public void CmdWaitEvents(uint eventCount, in Event pEvents, PipelineStageFlags srcStageMask, PipelineStageFlags dstStageMask, uint memoryBarrierCount,
        MemoryBarrier* pMemoryBarriers, uint bufferMemoryBarrierCount, BufferMemoryBarrier* pBufferMemoryBarriers, uint imageMemoryBarrierCount, in ImageMemoryBarrier pImageMemoryBarriers)
    {
        VK.CmdWaitEvents(_commandBuffer, eventCount, in pEvents, srcStageMask, dstStageMask, memoryBarrierCount, pMemoryBarriers, bufferMemoryBarrierCount, pBufferMemoryBarriers,
            imageMemoryBarrierCount, in pImageMemoryBarriers);
    }

    public void CmdWaitEvents(uint eventCount, in Event pEvents, PipelineStageFlags srcStageMask, PipelineStageFlags dstStageMask, uint memoryBarrierCount,
        MemoryBarrier* pMemoryBarriers, uint bufferMemoryBarrierCount, in BufferMemoryBarrier pBufferMemoryBarriers, uint imageMemoryBarrierCount, ImageMemoryBarrier* pImageMemoryBarriers)
    {
        VK.CmdWaitEvents(_commandBuffer, eventCount, in pEvents, srcStageMask, dstStageMask, memoryBarrierCount, pMemoryBarriers, bufferMemoryBarrierCount, in pBufferMemoryBarriers,
            imageMemoryBarrierCount, pImageMemoryBarriers);
    }

    public void CmdWaitEvents(uint eventCount, in Event pEvents, PipelineStageFlags srcStageMask, PipelineStageFlags dstStageMask, uint memoryBarrierCount,
        MemoryBarrier* pMemoryBarriers, uint bufferMemoryBarrierCount, in BufferMemoryBarrier pBufferMemoryBarriers, uint imageMemoryBarrierCount, in ImageMemoryBarrier pImageMemoryBarriers)
    {
        VK.CmdWaitEvents(_commandBuffer, eventCount, in pEvents, srcStageMask, dstStageMask, memoryBarrierCount, pMemoryBarriers, bufferMemoryBarrierCount, in pBufferMemoryBarriers,
            imageMemoryBarrierCount, in pImageMemoryBarriers);
    }

    public void CmdWaitEvents(uint eventCount, in Event pEvents, PipelineStageFlags srcStageMask, PipelineStageFlags dstStageMask, uint memoryBarrierCount,
        in MemoryBarrier pMemoryBarriers, uint bufferMemoryBarrierCount, BufferMemoryBarrier* pBufferMemoryBarriers, uint imageMemoryBarrierCount, ImageMemoryBarrier* pImageMemoryBarriers)
    {
        VK.CmdWaitEvents(_commandBuffer, eventCount, in pEvents, srcStageMask, dstStageMask, memoryBarrierCount, in pMemoryBarriers, bufferMemoryBarrierCount, pBufferMemoryBarriers,
            imageMemoryBarrierCount, pImageMemoryBarriers);
    }

    public void CmdWaitEvents(uint eventCount, in Event pEvents, PipelineStageFlags srcStageMask, PipelineStageFlags dstStageMask, uint memoryBarrierCount,
        in MemoryBarrier pMemoryBarriers, uint bufferMemoryBarrierCount, BufferMemoryBarrier* pBufferMemoryBarriers, uint imageMemoryBarrierCount, in ImageMemoryBarrier pImageMemoryBarriers)
    {
        VK.CmdWaitEvents(_commandBuffer, eventCount, in pEvents, srcStageMask, dstStageMask, memoryBarrierCount, in pMemoryBarriers, bufferMemoryBarrierCount, pBufferMemoryBarriers,
            imageMemoryBarrierCount, in pImageMemoryBarriers);
    }

    public void CmdWaitEvents(uint eventCount, in Event pEvents, PipelineStageFlags srcStageMask, PipelineStageFlags dstStageMask, uint memoryBarrierCount,
        in MemoryBarrier pMemoryBarriers, uint bufferMemoryBarrierCount, in BufferMemoryBarrier pBufferMemoryBarriers, uint imageMemoryBarrierCount, ImageMemoryBarrier* pImageMemoryBarriers)
    {
        VK.CmdWaitEvents(_commandBuffer, eventCount, in pEvents, srcStageMask, dstStageMask, memoryBarrierCount, in pMemoryBarriers, bufferMemoryBarrierCount, in pBufferMemoryBarriers,
            imageMemoryBarrierCount, pImageMemoryBarriers);
    }

    public void CmdWaitEvents(uint eventCount, in Event pEvents, PipelineStageFlags srcStageMask, PipelineStageFlags dstStageMask, uint memoryBarrierCount,
        in MemoryBarrier pMemoryBarriers, uint bufferMemoryBarrierCount, in BufferMemoryBarrier pBufferMemoryBarriers, uint imageMemoryBarrierCount, in ImageMemoryBarrier pImageMemoryBarriers)
    {
        VK.CmdWaitEvents(_commandBuffer, eventCount, in pEvents, srcStageMask, dstStageMask, memoryBarrierCount, in pMemoryBarriers, bufferMemoryBarrierCount, in pBufferMemoryBarriers,
            imageMemoryBarrierCount, in pImageMemoryBarriers);
    }

    public Result BeginCommandBuffer(CommandBufferBeginInfo* pBeginInfo)
    {
        return VK.BeginCommandBuffer(_commandBuffer, pBeginInfo);
    }

    public Result BeginCommandBuffer(in CommandBufferBeginInfo pBeginInfo)
    {
        return VK.BeginCommandBuffer(_commandBuffer, in pBeginInfo);
    }

    public Result EndCommandBuffer()
    {
        return VK.EndCommandBuffer(_commandBuffer);
    }

    public Result ResetCommandBuffer(CommandBufferResetFlags flags)
    {
        return VK.ResetCommandBuffer(_commandBuffer, flags);
    }
}

using Silk.NET.Vulkan;
using System.Runtime.InteropServices;
using Silk.NET.Vulkan.Extensions.Helpers;

namespace Istok.Rendering;

public unsafe class Pipeline : IDisposable
{
    readonly LogicalDevice _logicalDevice;
    readonly Silk.NET.Vulkan.Pipeline _devicePipeline;
    readonly PipelineLayout _pipelineLayout;
    string _name;

    public Silk.NET.Vulkan.Pipeline DevicePipeline => _devicePipeline;

    public PipelineLayout PipelineLayout => _pipelineLayout;

    public uint ResourceSetCount { get; }
    public int DynamicOffsetsCount { get; }


    public Pipeline(LogicalDevice logicalDevice, in GraphicsPipelineDescription description)
    {
        _logicalDevice = logicalDevice;
        IsComputePipeline = false;

        int attachmentsCount = description.BlendState.AttachmentStates.Length;
        PipelineColorBlendAttachmentState* attachmentsPtr = stackalloc PipelineColorBlendAttachmentState[attachmentsCount];
        for (int i = 0; i < attachmentsCount; i++)
            attachmentsPtr[i] = description.BlendState.AttachmentStates[i];

        Color blendFactor = description.BlendState.BlendFactor;

        PipelineColorBlendStateCreateInfo blendStateCI = new PipelineColorBlendStateCreateInfo
        {
            SType = StructureType.PipelineColorBlendStateCreateInfo,
            AttachmentCount = (uint)attachmentsCount,
            PAttachments = attachmentsPtr,
        };
        blendStateCI.BlendConstants[0] = blendFactor.R;
        blendStateCI.BlendConstants[1] = blendFactor.G;
        blendStateCI.BlendConstants[2] = blendFactor.B;
        blendStateCI.BlendConstants[3] = blendFactor.A;


        PipelineRasterizationStateCreateInfo rasterizerStateCI = description.RasterizerState;


        DynamicState* dynamicStates = stackalloc DynamicState[2];
        dynamicStates[0] = DynamicState.Viewport;
        dynamicStates[1] = DynamicState.Scissor;

        PipelineDynamicStateCreateInfo dynamicStateCI = new PipelineDynamicStateCreateInfo
        {
            SType = StructureType.PipelineDynamicStateCreateInfo,
            DynamicStateCount = 2,
            PDynamicStates = dynamicStates,
        };


        PipelineDepthStencilStateCreateInfo depthStencilStateCI = description.DepthStencilState;


        PipelineMultisampleStateCreateInfo multisampleCI = new PipelineMultisampleStateCreateInfo
        {
            SType = StructureType.PipelineMultisampleStateCreateInfo,
            RasterizationSamples = description.Outputs.SampleCount,
            AlphaToCoverageEnable = description.BlendState.AlphaToCoverageEnabled,
        };

        PipelineInputAssemblyStateCreateInfo inputAssemblyCI = new PipelineInputAssemblyStateCreateInfo
        {
            SType = StructureType.PipelineInputAssemblyStateCreateInfo,
            Topology = description.PrimitiveTopology,
        };

        VertexLayoutDescription[] inputDescriptions = description.ShaderSet.VertexLayouts;
        int bindingCount = inputDescriptions.Length;
        int attributeCount = 0;
        for (int i = 0; i < bindingCount; i++)
            attributeCount += inputDescriptions[i].Elements.Length;
        VertexInputBindingDescription* bindingDescriptions = stackalloc VertexInputBindingDescription[bindingCount];
        VertexInputAttributeDescription* attributeDescriptions = stackalloc VertexInputAttributeDescription[attributeCount];

        int targetIndex = 0;
        int targetLocation = 0;
        for (int binding = 0; binding < bindingCount; binding++)
        {
            VertexLayoutDescription inputDesc = inputDescriptions[binding];
            bindingDescriptions[binding] = inputDesc.ToVertexInputBindingDescription((uint)binding);

            uint currentOffset = 0;
            for (int location = 0; location < inputDesc.Elements.Length; location++)
            {
                VertexElementDescription inputElement = inputDesc.Elements[location];
                attributeDescriptions[targetIndex] = new VertexInputAttributeDescription
                {
                    Format = inputElement.Format, Binding = (uint)binding, Location = (uint)(targetLocation + location), Offset = currentOffset,
                };
                targetIndex += 1;
                currentOffset += inputElement.Format.ElementSize();
            }

            targetLocation += inputDesc.Elements.Length;
        }

        PipelineVertexInputStateCreateInfo vertexInputCI = new PipelineVertexInputStateCreateInfo
        {
            SType = StructureType.PipelineVertexInputStateCreateInfo,
            VertexBindingDescriptionCount = (uint)bindingCount,
            PVertexBindingDescriptions = bindingDescriptions,
            VertexAttributeDescriptionCount = (uint)attributeCount,
            PVertexAttributeDescriptions = attributeDescriptions,
        };

        FillSpecializationConstants(description.ShaderSet.Specializations, out byte[] fullSpecData, out SpecializationMapEntry[] mapEntries);

        GCHandle? ptrFullSpecData = null;
        GCHandle? ptrMapEntries = null;
        SpecializationInfo specializationInfo;

        if (fullSpecData != null && mapEntries != null)
        {
            ptrFullSpecData = GCHandle.Alloc(fullSpecData, GCHandleType.Pinned);
            ptrMapEntries = GCHandle.Alloc(mapEntries, GCHandleType.Pinned);
            specializationInfo = new SpecializationInfo
            {
                DataSize = (uint)fullSpecData.Length,
                PData = ptrFullSpecData.Value.AddrOfPinnedObject().ToPointer(),
                MapEntryCount = (uint)mapEntries.Length,
                PMapEntries = (SpecializationMapEntry*)ptrMapEntries.Value.AddrOfPinnedObject().ToPointer(),
            };
        }

        ShaderModule[] shaders = description.ShaderSet.Shaders;
        PipelineShaderStageCreateInfo* stages = stackalloc PipelineShaderStageCreateInfo[shaders.Length];
        for (int i = 0; i < shaders.Length; i++)
        {
            stages[i] = new PipelineShaderStageCreateInfo
            {
                SType = StructureType.PipelineShaderStageCreateInfo,
                Module = shaders[i].DeviceShaderModule,
                Stage = shaders[i].Stage,
                PName = MarshaledStringRegistry.Get(shaders[i].EntryPoint),
                PSpecializationInfo = &specializationInfo,
            };
        }

        PipelineViewportStateCreateInfo viewportStateCI = new PipelineViewportStateCreateInfo
        {
            SType = StructureType.PipelineViewportStateCreateInfo,
            ViewportCount = 1,
            ScissorCount = 1,
        };

        DescriptorSetLayout[] descriptorSetLayouts = description.Layouts;

        Silk.NET.Vulkan.DescriptorSetLayout* descriptorSetLayout = stackalloc Silk.NET.Vulkan.DescriptorSetLayout[descriptorSetLayouts.Length];
        for (int i = 0; i < descriptorSetLayouts.Length; i++)
            descriptorSetLayout[i] = descriptorSetLayouts[i].DeviceDescriptorSetLayout;

        PipelineLayoutCreateInfo pipelineLayoutCI = new PipelineLayoutCreateInfo
        {
            SType = StructureType.PipelineLayoutCreateInfo,
            SetLayoutCount = (uint)descriptorSetLayouts.Length,
            PSetLayouts = descriptorSetLayout,
        };

        _logicalDevice.CreatePipelineLayout(in pipelineLayoutCI, null, out _pipelineLayout);

        RenderPass renderPass = description.Outputs;

        GraphicsPipelineCreateInfo pipelineCI = new GraphicsPipelineCreateInfo
        {
            SType = StructureType.GraphicsPipelineCreateInfo,
            PColorBlendState = &blendStateCI,
            PRasterizationState = &rasterizerStateCI,
            PDynamicState = &dynamicStateCI,
            PDepthStencilState = &depthStencilStateCI,
            PMultisampleState = &multisampleCI,
            PInputAssemblyState = &inputAssemblyCI,
            PVertexInputState = &vertexInputCI,
            StageCount = (uint)shaders.Length,
            PStages = stages,
            PViewportState = &viewportStateCI,
            Layout = _pipelineLayout,
            RenderPass = renderPass.DeviceRenderPass,
        };

        Result result = _logicalDevice.CreateGraphicsPipelines(new PipelineCache(), 1, in pipelineCI, null, out _devicePipeline);
        Helpers.CheckErrors(result);

        ptrFullSpecData?.Free();
        ptrMapEntries?.Free();

        ResourceSetCount = (uint)description.Layouts.Length;
        DynamicOffsetsCount = 0;
        foreach (DescriptorSetLayout layout in description.Layouts)
        {
            DynamicOffsetsCount += layout.DynamicBufferCount;
        }
    }

    public Pipeline(LogicalDevice logicalDevice, in ComputePipelineDescription description)
    {
        _logicalDevice = logicalDevice;
        IsComputePipeline = true;

        DescriptorSetLayout[] resourceLayouts = description.DescriptorSetLayouts;
        Silk.NET.Vulkan.DescriptorSetLayout* descriptorSetLayouts = stackalloc Silk.NET.Vulkan.DescriptorSetLayout[resourceLayouts.Length];
        for (int i = 0; i < resourceLayouts.Length; i++)
        {
            descriptorSetLayouts[i] = resourceLayouts[i].DeviceDescriptorSetLayout;
        }

        PipelineLayoutCreateInfo pipelineLayoutCI = new PipelineLayoutCreateInfo
        {
            SType = StructureType.PipelineLayoutCreateInfo,
            SetLayoutCount = (uint)resourceLayouts.Length,
            PSetLayouts = descriptorSetLayouts,
        };

        _logicalDevice.CreatePipelineLayout(in pipelineLayoutCI, null, out _pipelineLayout);

        SpecializationConstant[] specializations = description.Specializations;

        FillSpecializationConstants(specializations, out byte[] fullSpecData, out SpecializationMapEntry[] mapEntries);
        GCHandle? ptrFullSpecData = null;
        GCHandle? ptrMapEntries = null;
        SpecializationInfo specializationInfo;

        if (fullSpecData != null && mapEntries != null)
        {
            ptrFullSpecData = GCHandle.Alloc(fullSpecData, GCHandleType.Pinned);
            ptrMapEntries = GCHandle.Alloc(mapEntries, GCHandleType.Pinned);
            specializationInfo = new SpecializationInfo
            {
                DataSize = (uint)fullSpecData.Length,
                PData = ptrFullSpecData.Value.AddrOfPinnedObject().ToPointer(),
                MapEntryCount = (uint)mapEntries.Length,
                PMapEntries = (SpecializationMapEntry*)ptrMapEntries.Value.AddrOfPinnedObject().ToPointer(),
            };
        }

        ShaderModule shaderModule = description.ComputeShaderModule;
        PipelineShaderStageCreateInfo stageCI = new PipelineShaderStageCreateInfo
        {
            SType = StructureType.PipelineShaderStageCreateInfo,
            Module = shaderModule.DeviceShaderModule,
            Stage = shaderModule.Stage,
            PName = MarshaledStringRegistry.Get("main"),
            PSpecializationInfo = &specializationInfo,
        };

        ComputePipelineCreateInfo pipelineCI = new ComputePipelineCreateInfo
        {
            SType = StructureType.ComputePipelineCreateInfo,
            Layout = _pipelineLayout,
            Stage = stageCI,
        };

        Result result = _logicalDevice.CreateComputePipelines(
            new PipelineCache(),
            1,
            in pipelineCI,
            null,
            out _devicePipeline);
        Helpers.CheckErrors(result);

        ptrFullSpecData?.Free();
        ptrMapEntries?.Free();

        ResourceSetCount = (uint)description.DescriptorSetLayouts.Length;
        DynamicOffsetsCount = 0;
        foreach (DescriptorSetLayout layout in description.DescriptorSetLayouts)
        {
            DynamicOffsetsCount += layout.DynamicBufferCount;
        }
    }

    static void FillSpecializationConstants(SpecializationConstant[] specializations, out byte[] fullSpecData, out SpecializationMapEntry[] mapEntries)
    {
        if (specializations != null)
        {
            uint specDataSize = 0;
            foreach (SpecializationConstant spec in specializations)
                specDataSize += spec.Size;
            fullSpecData = new byte[(int)specDataSize];
            int specializationCount = specializations.Length;
            mapEntries = new SpecializationMapEntry[specializationCount];
            uint specOffset = 0;
            for (int i = 0; i < specializationCount; i++)
            {
                SpecializationConstant.Bytes data = specializations[i].Data;
                uint dataSize = specializations[i].Size;
                data[..(int)dataSize].CopyTo(fullSpecData.AsSpan((int)specOffset..(int)(specOffset + dataSize)));
                mapEntries[i].ConstantID = specializations[i].ID;
                mapEntries[i].Offset = specOffset;
                mapEntries[i].Size = dataSize;
                specOffset += dataSize;
            }
        }
        else
        {
            fullSpecData = null;
            mapEntries = null;
        }
    }

    public bool IsComputePipeline { get; }

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            _logicalDevice.SetObjectName(ObjectType.Pipeline, DevicePipeline.Handle, value);
            _logicalDevice.SetObjectName(ObjectType.PipelineLayout, PipelineLayout.Handle, value);
        }
    }

    public bool IsDisposed { get; private set; }

    public void Dispose()
    {
        if (!IsDisposed)
        {
            IsDisposed = true;
            _logicalDevice.DestroyPipelineLayout(_pipelineLayout, null);
            _logicalDevice.DestroyPipeline(_devicePipeline, null);
        }
    }
}

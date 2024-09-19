using Istok.Rendering;
using Silk.NET.Vulkan;
using Buffer = Istok.Rendering.Buffer;
using DescriptorSetLayout = Istok.Rendering.DescriptorSetLayout;
using Framebuffer = Istok.Rendering.Framebuffer;
using Image = Istok.Rendering.Image;
using ImageView = Istok.Rendering.ImageView;
using Pipeline = Istok.Rendering.Pipeline;
using RenderPass = Istok.Rendering.RenderPass;
using Sampler = Istok.Rendering.Sampler;
using ShaderModule = Istok.Rendering.ShaderModule;

namespace Istok;

public partial class Graphics
{
    public CommandList CreateCommandList()
    {
        return new CommandList(LogicalDevice, StagingBuffersPool, CommandPoolsStorage);
    }

    public Framebuffer CreateFramebuffer(in FramebufferDescription description, RenderPass renderPass)
    {
        return new Framebuffer(LogicalDevice, in description, renderPass);
    }

    public Pipeline CreateGraphicsPipeline(in GraphicsPipelineDescription description)
    {
        return new Pipeline(LogicalDevice, in description);
    }

    public Pipeline CreateComputePipeline(in ComputePipelineDescription description)
    {
        return new Pipeline(LogicalDevice, in description);
    }

    public DescriptorSetLayout CreateResourceLayout(in DescriptorSetLayoutBindings description)
    {
        return DescriptorSetLayout.Create(LogicalDevice, in description);
    }

    public ResourceSet CreateResourceSet(in ResourceSetDescription description)
    {
        return new ResourceSet(LogicalDevice, DescriptorPoolManager, in description);
    }

    public Image CreateImage(in ImageCreateInfo description)
    {
        return Image.Create(LogicalDevice, StagingBuffersPool, CommandPoolsStorage, in description);
    }

    public ImageView CreateImageView(Image target) => CreateImageView(target, target.GetImageViewCreateInfo());

    public ImageView CreateImageView(Image target, in ImageViewCreateInfo description)
    {
        return ImageView.Create(LogicalDevice, target, in description);
    }

    public Buffer CreateBuffer(in BufferDescription description)
    {
        return new Buffer(LogicalDevice, StagingBuffersPool, CommandPoolsStorage, description.SizeInBytes, description.Usage, description.Property);
    }

    public Sampler CreateSampler(in SamplerCreateInfo description)
    {
        return new Sampler(LogicalDevice, in description);
    }

    public ShaderModule CreateShaderModule(in ShaderDescription description)
    {
        return ShaderModule.Create(LogicalDevice, in description);
    }
}

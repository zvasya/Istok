using Silk.NET.Vulkan;

namespace Istok.Rendering;

/// <summary>
/// Represent the state of an image sampler which is used by the implementation to read image data and apply filtering and other transformations for the shader.
/// </summary>
public unsafe class Sampler : IDisposable
{
    readonly LogicalDevice _logicalDevice;
    readonly Silk.NET.Vulkan.Sampler _sampler;
    string _name;

    public Sampler(LogicalDevice logicalDevice, in SamplerCreateInfo description)
    {
        _logicalDevice = logicalDevice;
        _logicalDevice.CreateSampler(in description, null, out _sampler);
    }

    public Silk.NET.Vulkan.Sampler DeviceSampler => _sampler;

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            _logicalDevice.SetObjectName(ObjectType.Sampler, DeviceSampler.Handle, value);
        }
    }

    public bool IsDisposed { get; private set; }

    public void Dispose()
    {
        if (!IsDisposed)
        {
            _logicalDevice.DestroySampler(_sampler, null);
            IsDisposed = true;
        }
    }

    public ResourcesSetBindingSampler GetResourcesSetBound()
    {
        return new ResourcesSetBindingSampler(this);
    }
}

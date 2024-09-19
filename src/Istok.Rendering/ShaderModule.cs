using Silk.NET.Vulkan;

namespace Istok.Rendering;

/// <summary>
/// Specifying parameters for creating a pipeline shader stage
/// </summary>
public unsafe class ShaderModule : IDisposable
{
    readonly LogicalDevice _logicalDevice;
    string _name;

    ShaderModule(LogicalDevice logicalDevice, ShaderStageFlags stage, string entryPoint, Silk.NET.Vulkan.ShaderModule shaderModule)
    {
        _logicalDevice = logicalDevice;
        Stage = stage;
        EntryPoint = entryPoint;
        DeviceShaderModule = shaderModule;
    }

    public Silk.NET.Vulkan.ShaderModule DeviceShaderModule { get; }


    /// <summary>
    /// Specifying a single pipeline stage
    /// </summary>
    public ShaderStageFlags Stage { get; }

    /// <summary>
    /// Specifying the entry point name of the shader for this stage
    /// </summary>
    public string EntryPoint { get; }

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            _logicalDevice.SetObjectName(ObjectType.ShaderModule, DeviceShaderModule.Handle, value);
        }
    }

    public bool IsDisposed { get; private set; }

    public void Dispose()
    {
        if (!IsDisposed)
        {
            IsDisposed = true;
            _logicalDevice.DestroyShaderModule(DeviceShaderModule, null);
        }
    }

    public static ShaderModule Create(LogicalDevice logicalDevice, in ShaderDescription description)
    {
        Silk.NET.Vulkan.ShaderModule shaderModule;

        fixed (byte* codePtr = description.ShaderBytes)
        {
            ShaderModuleCreateInfo shaderModuleCI = new ShaderModuleCreateInfo
            {
                SType = StructureType.ShaderModuleCreateInfo,
                CodeSize = (UIntPtr)description.ShaderBytes.Length,
                PCode = (uint*)codePtr,
            };
            Result result = logicalDevice.CreateShaderModule(in shaderModuleCI, null, out shaderModule);
            Helpers.CheckErrors(result);
        }

        return new ShaderModule(logicalDevice, description.Stage, description.EntryPoint, shaderModule);
    }
}

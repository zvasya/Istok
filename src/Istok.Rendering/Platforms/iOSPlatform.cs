using Silk.NET.Vulkan.Extensions.EXT;
using Silk.NET.Vulkan.Extensions.KHR;
using Silk.NET.Vulkan.Extensions.Missed;

namespace Istok.Rendering.Platforms;

public class iOSPlatform : IPlatform
{
    public virtual bool Debug => false;
    public virtual string[] RequiredInstanceExtensions => [KhrSurface.ExtensionName, ExtMetalSurface.ExtensionName];

    public virtual string[] DeviceExtensions => [KhrPortabilitySubset.ExtensionName];
    public virtual string[][] ValidationLayerNamesPriorityList { get; } = [[]];
}

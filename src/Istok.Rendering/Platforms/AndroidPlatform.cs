using Silk.NET.Vulkan.Extensions.KHR;

namespace Istok.Rendering.Platforms;

public class AndroidPlatform : IPlatform
{
    public virtual bool Debug => false;

    public virtual string[] RequiredInstanceExtensions => [KhrSurface.ExtensionName, KhrAndroidSurface.ExtensionName];
    public virtual string[] DeviceExtensions => [];

    public virtual string[][] ValidationLayerNamesPriorityList { get; } = [[]];
}

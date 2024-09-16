using Silk.NET.Vulkan.Extensions.KHR;

namespace Istok.Rendering.Platforms;

public class WindowsPlatform : IPlatform
{
    public virtual bool Debug => false;

    public virtual string[] RequiredInstanceExtensions => [KhrSurface.ExtensionName, KhrWin32Surface.ExtensionName];
    public virtual string[] DeviceExtensions => [];
    public virtual string[][] ValidationLayerNamesPriorityList { get; } = [[]];
}

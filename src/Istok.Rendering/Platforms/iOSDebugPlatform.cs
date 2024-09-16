using Silk.NET.Vulkan.Extensions.EXT;

namespace Istok.Rendering.Platforms;

public class iOSDebugPlatform : iOSPlatform
{
    public override bool Debug => true;

    public override string[] RequiredInstanceExtensions => [ExtDebugUtils.ExtensionName, .. base.RequiredInstanceExtensions];

    public override string[][] ValidationLayerNamesPriorityList { get; } =
    [
        ["MoltenVK"],
    ];
}

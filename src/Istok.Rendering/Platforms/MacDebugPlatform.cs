using Silk.NET.Vulkan.Extensions.EXT;
using Silk.NET.Vulkan.Extensions.Missed;

namespace Istok.Rendering.Platforms;

public class MacDebugPlatform : MacPlatform
{
    public override bool Debug => true;

    public override string[] RequiredInstanceExtensions => [ExtDebugUtils.ExtensionName, .. base.RequiredInstanceExtensions];
    public override string[][] ValidationLayerNamesPriorityList { get; } =
    [
        [LayerKhronosValidation.ExtensionName],
        [LayerLunargStandardValidation.ExtensionName],
    ];
}

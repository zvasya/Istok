namespace Silk.NET.Vulkan.Extensions.Helpers;

public static class ColorComponentFlagsExt
{
    public static ColorComponentFlags All => ColorComponentFlags.RBit | ColorComponentFlags.GBit | ColorComponentFlags.BBit | ColorComponentFlags.ABit;
}

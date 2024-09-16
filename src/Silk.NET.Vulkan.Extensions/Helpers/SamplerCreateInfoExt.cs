using Silk.NET.Vulkan;

namespace Silk.NET.Vulkan.Extensions.Helpers
{
    public static class SamplerCreateInfoExt
    {
        public static readonly SamplerCreateInfo Point = new SamplerCreateInfo
        {
            SType = StructureType.SamplerCreateInfo,
            AddressModeU = SamplerAddressMode.Repeat,
            AddressModeV = SamplerAddressMode.Repeat,
            AddressModeW = SamplerAddressMode.Repeat,
            MinFilter = Filter.Nearest,
            MagFilter = Filter.Nearest,
            MipmapMode = SamplerMipmapMode.Nearest,
            MipLodBias = 0,
            MinLod = 0,
            MaxLod = uint.MaxValue,
            MaxAnisotropy = 0,
        };

        public static readonly SamplerCreateInfo Linear = new SamplerCreateInfo
        {
            SType = StructureType.SamplerCreateInfo,
            AddressModeU = SamplerAddressMode.Repeat,
            AddressModeV = SamplerAddressMode.Repeat,
            AddressModeW = SamplerAddressMode.Repeat,
            MinFilter = Filter.Linear,
            MagFilter = Filter.Linear,
            MipmapMode = SamplerMipmapMode.Linear,
            MipLodBias = 0,
            MinLod = 0,
            MaxLod = uint.MaxValue,
            MaxAnisotropy = 0,
        };

        public static readonly SamplerCreateInfo Aniso4X = new SamplerCreateInfo
        {
            SType = StructureType.SamplerCreateInfo,
            AddressModeU = SamplerAddressMode.Repeat,
            AddressModeV = SamplerAddressMode.Repeat,
            AddressModeW = SamplerAddressMode.Repeat,
            MinFilter = Filter.Linear,
            MagFilter = Filter.Linear,
            MipmapMode = SamplerMipmapMode.Linear,
            MipLodBias = 0,
            MinLod = 0,
            MaxLod = uint.MaxValue,
            AnisotropyEnable = true,
            MaxAnisotropy = 4,
        };
    }
}

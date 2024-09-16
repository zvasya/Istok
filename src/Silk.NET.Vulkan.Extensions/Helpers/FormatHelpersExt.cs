using System.Collections.Generic;
using System.Diagnostics;
using Silk.NET.Vulkan;

namespace Silk.NET.Vulkan.Extensions.Helpers;

public static class FormatExt
{
    const int VkMultiplaneFormatMaxPlanes = 3;

    public enum VkFormatCompatibilityClass
    {
        ClassNoneBit = 0,
        Class8Bit = 1,
        Class16Bit = 2,
        Class24Bit = 3,
        Class32Bit = 4,
        Class48Bit = 5,
        Class64Bit = 6,
        Class96Bit = 7,
        Class128Bit = 8,
        Class192Bit = 9,
        Class256Bit = 10,
        ClassBc1RgbBit = 11,
        ClassBc1RgbaBit = 12,
        ClassBc2Bit = 13,
        ClassBc3Bit = 14,
        ClassBc4Bit = 15,
        ClassBc5Bit = 16,
        ClassBc6HBit = 17,
        ClassBc7Bit = 18,
        ClassEtc2RGBBit = 19,
        ClassEtc2RGBABit = 20,
        ClassEtc2EacRGBABit = 21,
        ClassEacRBit = 22,
        ClassEacRGBit = 23,
        ClassAstc4X4Bit = 24,
        ClassAstc5X4Bit = 25,
        ClassAstc5X5Bit = 26,
        ClassAstc6X5Bit = 27,
        ClassAstc6X6Bit = 28,
        ClassAstc8X5Bit = 29,
        ClassAstc8X6Bit = 20,
        ClassAstc8X8Bit = 31,
        ClassAstc10X5Bit = 32,
        ClassAstc10X6Bit = 33,
        ClassAstc10X8Bit = 34,
        ClassAstc10X10Bit = 35,
        ClassAstc12X10Bit = 36,
        ClassAstc12X12Bit = 37,
        ClassD16Bit = 38,
        ClassD24Bit = 39,
        ClassD32Bit = 30,
        ClassS8Bit = 41,
        ClassD16S8Bit = 42,
        ClassD24S8Bit = 43,
        ClassD32S8Bit = 44,
        ClassPvrtc12BppBit = 45,
        ClassPvrtc14BppBit = 46,
        ClassPvrtc22BppBit = 47,
        ClassPvrtc24BppBit = 48,

        /* KHR_sampler_YCbCr_conversion */
        Class32BitG8B8G8R8 = 49,
        Class32BitB8G8R8G8 = 50,
        Class64BitR10G10B10A10 = 51,
        Class64BitG10B10G10R10 = 52,
        Class64BitB10G10R10G10 = 53,
        Class64BitR12G12B12A12 = 54,
        Class64BitG12B12G12R12 = 55,
        Class64BitB12G12R12G12 = 56,
        Class64BitG16B16G16R16 = 57,
        Class64BitB16G16R16G16 = 58,
        Class8Bit3Plane420 = 59,
        Class8Bit2Plane420 = 60,
        Class8Bit3Plane422 = 61,
        Class8Bit2Plane422 = 62,
        Class8Bit3Plane444 = 63,
        Class10Bit3Plane420 = 64,
        Class10Bit2Plane420 = 65,
        Class10Bit3Plane422 = 66,
        Class10Bit2Plane422 = 67,
        Class10Bit3Plane444 = 68,
        Class12Bit3Plane420 = 69,
        Class12Bit2Plane420 = 70,
        Class12Bit3Plane422 = 71,
        Class12Bit2Plane422 = 72,
        Class12Bit3Plane444 = 73,
        Class16Bit3Plane420 = 74,
        Class16Bit2Plane420 = 75,
        Class16Bit3Plane422 = 76,
        Class16Bit2Plane422 = 77,
        Class16Bit3Plane444 = 78,
    }

    public enum VkFormatNumericalType
    {
        None,
        UInt,
        SInt,
        UNorm,
        SNorm,
        UScaled,
        SScaled,
        UFloat,
        SFloat,
        SRGB,
    }

    /// <summary>
    /// Data structure with size(bytes) and number of channels for each Vulkan format
    /// For compressed and multi-plane formats, size is bytes per compressed or shared block
    /// </summary>
    /// <param name="Size">size in bytes</param>
    /// <param name="ChannelCount">number of channels</param>
    /// <param name="FormatClass"></param>
    record struct VulkanFormatInfo(uint Size, uint ChannelCount, VkFormatCompatibilityClass FormatClass);

    record struct PerPlaneCompatibility(uint widthDivisor, uint heightDivisor, Format compatibleFormat);

    // [System.Runtime.CompilerServices.InlineArray(VK_MULTIPLANE_FORMAT_MAX_PLANES)]
    readonly struct MultiplaneCompatibility
    {
        readonly PerPlaneCompatibility[] per_plane;

        public MultiplaneCompatibility(PerPlaneCompatibility p1, PerPlaneCompatibility p2, PerPlaneCompatibility p3)
        {
            per_plane = new PerPlaneCompatibility[3];
            per_plane[0] = p1;
            per_plane[1] = p2;
            per_plane[2] = p3;
        }

        public PerPlaneCompatibility this[int index] => per_plane[index];
    }

    static readonly Dictionary<Format, VulkanFormatInfo> FormatTable = new Dictionary<Format, VulkanFormatInfo>
    {
        { Format.Undefined, new VulkanFormatInfo(0, 0, VkFormatCompatibilityClass.ClassNoneBit) },
        { Format.R4G4UnormPack8, new VulkanFormatInfo(1, 2, VkFormatCompatibilityClass.Class8Bit) },
        { Format.R4G4B4A4UnormPack16, new VulkanFormatInfo(2, 4, VkFormatCompatibilityClass.Class16Bit) },
        { Format.B4G4R4A4UnormPack16, new VulkanFormatInfo(2, 4, VkFormatCompatibilityClass.Class16Bit) },
        { Format.R5G6B5UnormPack16, new VulkanFormatInfo(2, 3, VkFormatCompatibilityClass.Class16Bit) },
        { Format.B5G6R5UnormPack16, new VulkanFormatInfo(2, 3, VkFormatCompatibilityClass.Class16Bit) },
        { Format.R5G5B5A1UnormPack16, new VulkanFormatInfo(2, 4, VkFormatCompatibilityClass.Class16Bit) },
        { Format.B5G5R5A1UnormPack16, new VulkanFormatInfo(2, 4, VkFormatCompatibilityClass.Class16Bit) },
        { Format.A1R5G5B5UnormPack16, new VulkanFormatInfo(2, 4, VkFormatCompatibilityClass.Class16Bit) },
        { Format.R8Unorm, new VulkanFormatInfo(1, 1, VkFormatCompatibilityClass.Class8Bit) },
        { Format.R8SNorm, new VulkanFormatInfo(1, 1, VkFormatCompatibilityClass.Class8Bit) },
        { Format.R8Uscaled, new VulkanFormatInfo(1, 1, VkFormatCompatibilityClass.Class8Bit) },
        { Format.R8Sscaled, new VulkanFormatInfo(1, 1, VkFormatCompatibilityClass.Class8Bit) },
        { Format.R8Uint, new VulkanFormatInfo(1, 1, VkFormatCompatibilityClass.Class8Bit) },
        { Format.R8Sint, new VulkanFormatInfo(1, 1, VkFormatCompatibilityClass.Class8Bit) },
        { Format.R8Srgb, new VulkanFormatInfo(1, 1, VkFormatCompatibilityClass.Class8Bit) },
        { Format.R8G8Unorm, new VulkanFormatInfo(2, 2, VkFormatCompatibilityClass.Class16Bit) },
        { Format.R8G8SNorm, new VulkanFormatInfo(2, 2, VkFormatCompatibilityClass.Class16Bit) },
        { Format.R8G8Uscaled, new VulkanFormatInfo(2, 2, VkFormatCompatibilityClass.Class16Bit) },
        { Format.R8G8Sscaled, new VulkanFormatInfo(2, 2, VkFormatCompatibilityClass.Class16Bit) },
        { Format.R8G8Uint, new VulkanFormatInfo(2, 2, VkFormatCompatibilityClass.Class16Bit) },
        { Format.R8G8Sint, new VulkanFormatInfo(2, 2, VkFormatCompatibilityClass.Class16Bit) },
        { Format.R8G8Srgb, new VulkanFormatInfo(2, 2, VkFormatCompatibilityClass.Class16Bit) },
        { Format.R8G8B8Unorm, new VulkanFormatInfo(3, 3, VkFormatCompatibilityClass.Class24Bit) },
        { Format.R8G8B8SNorm, new VulkanFormatInfo(3, 3, VkFormatCompatibilityClass.Class24Bit) },
        { Format.R8G8B8Uscaled, new VulkanFormatInfo(3, 3, VkFormatCompatibilityClass.Class24Bit) },
        { Format.R8G8B8Sscaled, new VulkanFormatInfo(3, 3, VkFormatCompatibilityClass.Class24Bit) },
        { Format.R8G8B8Uint, new VulkanFormatInfo(3, 3, VkFormatCompatibilityClass.Class24Bit) },
        { Format.R8G8B8Sint, new VulkanFormatInfo(3, 3, VkFormatCompatibilityClass.Class24Bit) },
        { Format.R8G8B8Srgb, new VulkanFormatInfo(3, 3, VkFormatCompatibilityClass.Class24Bit) },
        { Format.B8G8R8Unorm, new VulkanFormatInfo(3, 3, VkFormatCompatibilityClass.Class24Bit) },
        { Format.B8G8R8SNorm, new VulkanFormatInfo(3, 3, VkFormatCompatibilityClass.Class24Bit) },
        { Format.B8G8R8Uscaled, new VulkanFormatInfo(3, 3, VkFormatCompatibilityClass.Class24Bit) },
        { Format.B8G8R8Sscaled, new VulkanFormatInfo(3, 3, VkFormatCompatibilityClass.Class24Bit) },
        { Format.B8G8R8Uint, new VulkanFormatInfo(3, 3, VkFormatCompatibilityClass.Class24Bit) },
        { Format.B8G8R8Sint, new VulkanFormatInfo(3, 3, VkFormatCompatibilityClass.Class24Bit) },
        { Format.B8G8R8Srgb, new VulkanFormatInfo(3, 3, VkFormatCompatibilityClass.Class24Bit) },
        { Format.R8G8B8A8Unorm, new VulkanFormatInfo(4, 4, VkFormatCompatibilityClass.Class32Bit) },
        { Format.R8G8B8A8SNorm, new VulkanFormatInfo(4, 4, VkFormatCompatibilityClass.Class32Bit) },
        { Format.R8G8B8A8Uscaled, new VulkanFormatInfo(4, 4, VkFormatCompatibilityClass.Class32Bit) },
        { Format.R8G8B8A8Sscaled, new VulkanFormatInfo(4, 4, VkFormatCompatibilityClass.Class32Bit) },
        { Format.R8G8B8A8Uint, new VulkanFormatInfo(4, 4, VkFormatCompatibilityClass.Class32Bit) },
        { Format.R8G8B8A8Sint, new VulkanFormatInfo(4, 4, VkFormatCompatibilityClass.Class32Bit) },
        { Format.R8G8B8A8Srgb, new VulkanFormatInfo(4, 4, VkFormatCompatibilityClass.Class32Bit) },
        { Format.B8G8R8A8Unorm, new VulkanFormatInfo(4, 4, VkFormatCompatibilityClass.Class32Bit) },
        { Format.B8G8R8A8SNorm, new VulkanFormatInfo(4, 4, VkFormatCompatibilityClass.Class32Bit) },
        { Format.B8G8R8A8Uscaled, new VulkanFormatInfo(4, 4, VkFormatCompatibilityClass.Class32Bit) },
        { Format.B8G8R8A8Sscaled, new VulkanFormatInfo(4, 4, VkFormatCompatibilityClass.Class32Bit) },
        { Format.B8G8R8A8Uint, new VulkanFormatInfo(4, 4, VkFormatCompatibilityClass.Class32Bit) },
        { Format.B8G8R8A8Sint, new VulkanFormatInfo(4, 4, VkFormatCompatibilityClass.Class32Bit) },
        { Format.B8G8R8A8Srgb, new VulkanFormatInfo(4, 4, VkFormatCompatibilityClass.Class32Bit) },
        { Format.A8B8G8R8UnormPack32, new VulkanFormatInfo(4, 4, VkFormatCompatibilityClass.Class32Bit) },
        { Format.A8B8G8R8SNormPack32, new VulkanFormatInfo(4, 4, VkFormatCompatibilityClass.Class32Bit) },
        { Format.A8B8G8R8UscaledPack32, new VulkanFormatInfo(4, 4, VkFormatCompatibilityClass.Class32Bit) },
        { Format.A8B8G8R8SscaledPack32, new VulkanFormatInfo(4, 4, VkFormatCompatibilityClass.Class32Bit) },
        { Format.A8B8G8R8UintPack32, new VulkanFormatInfo(4, 4, VkFormatCompatibilityClass.Class32Bit) },
        { Format.A8B8G8R8SintPack32, new VulkanFormatInfo(4, 4, VkFormatCompatibilityClass.Class32Bit) },
        { Format.A8B8G8R8SrgbPack32, new VulkanFormatInfo(4, 4, VkFormatCompatibilityClass.Class32Bit) },
        { Format.A2R10G10B10UnormPack32, new VulkanFormatInfo(4, 4, VkFormatCompatibilityClass.Class32Bit) },
        { Format.A2R10G10B10SNormPack32, new VulkanFormatInfo(4, 4, VkFormatCompatibilityClass.Class32Bit) },
        { Format.A2R10G10B10UscaledPack32, new VulkanFormatInfo(4, 4, VkFormatCompatibilityClass.Class32Bit) },
        { Format.A2R10G10B10SscaledPack32, new VulkanFormatInfo(4, 4, VkFormatCompatibilityClass.Class32Bit) },
        { Format.A2R10G10B10UintPack32, new VulkanFormatInfo(4, 4, VkFormatCompatibilityClass.Class32Bit) },
        { Format.A2R10G10B10SintPack32, new VulkanFormatInfo(4, 4, VkFormatCompatibilityClass.Class32Bit) },
        { Format.A2B10G10R10UnormPack32, new VulkanFormatInfo(4, 4, VkFormatCompatibilityClass.Class32Bit) },
        { Format.A2B10G10R10SNormPack32, new VulkanFormatInfo(4, 4, VkFormatCompatibilityClass.Class32Bit) },
        { Format.A2B10G10R10UscaledPack32, new VulkanFormatInfo(4, 4, VkFormatCompatibilityClass.Class32Bit) },
        { Format.A2B10G10R10SscaledPack32, new VulkanFormatInfo(4, 4, VkFormatCompatibilityClass.Class32Bit) },
        { Format.A2B10G10R10UintPack32, new VulkanFormatInfo(4, 4, VkFormatCompatibilityClass.Class32Bit) },
        { Format.A2B10G10R10SintPack32, new VulkanFormatInfo(4, 4, VkFormatCompatibilityClass.Class32Bit) },
        { Format.R16Unorm, new VulkanFormatInfo(2, 1, VkFormatCompatibilityClass.Class16Bit) },
        { Format.R16SNorm, new VulkanFormatInfo(2, 1, VkFormatCompatibilityClass.Class16Bit) },
        { Format.R16Uscaled, new VulkanFormatInfo(2, 1, VkFormatCompatibilityClass.Class16Bit) },
        { Format.R16Sscaled, new VulkanFormatInfo(2, 1, VkFormatCompatibilityClass.Class16Bit) },
        { Format.R16Uint, new VulkanFormatInfo(2, 1, VkFormatCompatibilityClass.Class16Bit) },
        { Format.R16Sint, new VulkanFormatInfo(2, 1, VkFormatCompatibilityClass.Class16Bit) },
        { Format.R16Sfloat, new VulkanFormatInfo(2, 1, VkFormatCompatibilityClass.Class16Bit) },
        { Format.R16G16Unorm, new VulkanFormatInfo(4, 2, VkFormatCompatibilityClass.Class32Bit) },
        { Format.R16G16SNorm, new VulkanFormatInfo(4, 2, VkFormatCompatibilityClass.Class32Bit) },
        { Format.R16G16Uscaled, new VulkanFormatInfo(4, 2, VkFormatCompatibilityClass.Class32Bit) },
        { Format.R16G16Sscaled, new VulkanFormatInfo(4, 2, VkFormatCompatibilityClass.Class32Bit) },
        { Format.R16G16Uint, new VulkanFormatInfo(4, 2, VkFormatCompatibilityClass.Class32Bit) },
        { Format.R16G16Sint, new VulkanFormatInfo(4, 2, VkFormatCompatibilityClass.Class32Bit) },
        { Format.R16G16Sfloat, new VulkanFormatInfo(4, 2, VkFormatCompatibilityClass.Class32Bit) },
        { Format.R16G16B16Unorm, new VulkanFormatInfo(6, 3, VkFormatCompatibilityClass.Class48Bit) },
        { Format.R16G16B16SNorm, new VulkanFormatInfo(6, 3, VkFormatCompatibilityClass.Class48Bit) },
        { Format.R16G16B16Uscaled, new VulkanFormatInfo(6, 3, VkFormatCompatibilityClass.Class48Bit) },
        { Format.R16G16B16Sscaled, new VulkanFormatInfo(6, 3, VkFormatCompatibilityClass.Class48Bit) },
        { Format.R16G16B16Uint, new VulkanFormatInfo(6, 3, VkFormatCompatibilityClass.Class48Bit) },
        { Format.R16G16B16Sint, new VulkanFormatInfo(6, 3, VkFormatCompatibilityClass.Class48Bit) },
        { Format.R16G16B16Sfloat, new VulkanFormatInfo(6, 3, VkFormatCompatibilityClass.Class48Bit) },
        { Format.R16G16B16A16Unorm, new VulkanFormatInfo(8, 4, VkFormatCompatibilityClass.Class64Bit) },
        { Format.R16G16B16A16SNorm, new VulkanFormatInfo(8, 4, VkFormatCompatibilityClass.Class64Bit) },
        { Format.R16G16B16A16Uscaled, new VulkanFormatInfo(8, 4, VkFormatCompatibilityClass.Class64Bit) },
        { Format.R16G16B16A16Sscaled, new VulkanFormatInfo(8, 4, VkFormatCompatibilityClass.Class64Bit) },
        { Format.R16G16B16A16Uint, new VulkanFormatInfo(8, 4, VkFormatCompatibilityClass.Class64Bit) },
        { Format.R16G16B16A16Sint, new VulkanFormatInfo(8, 4, VkFormatCompatibilityClass.Class64Bit) },
        { Format.R16G16B16A16Sfloat, new VulkanFormatInfo(8, 4, VkFormatCompatibilityClass.Class64Bit) },
        { Format.R32Uint, new VulkanFormatInfo(4, 1, VkFormatCompatibilityClass.Class32Bit) },
        { Format.R32Sint, new VulkanFormatInfo(4, 1, VkFormatCompatibilityClass.Class32Bit) },
        { Format.R32Sfloat, new VulkanFormatInfo(4, 1, VkFormatCompatibilityClass.Class32Bit) },
        { Format.R32G32Uint, new VulkanFormatInfo(8, 2, VkFormatCompatibilityClass.Class64Bit) },
        { Format.R32G32Sint, new VulkanFormatInfo(8, 2, VkFormatCompatibilityClass.Class64Bit) },
        { Format.R32G32Sfloat, new VulkanFormatInfo(8, 2, VkFormatCompatibilityClass.Class64Bit) },
        { Format.R32G32B32Uint, new VulkanFormatInfo(12, 3, VkFormatCompatibilityClass.Class96Bit) },
        { Format.R32G32B32Sint, new VulkanFormatInfo(12, 3, VkFormatCompatibilityClass.Class96Bit) },
        { Format.R32G32B32Sfloat, new VulkanFormatInfo(12, 3, VkFormatCompatibilityClass.Class96Bit) },
        { Format.R32G32B32A32Uint, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.Class128Bit) },
        { Format.R32G32B32A32Sint, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.Class128Bit) },
        { Format.R32G32B32A32Sfloat, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.Class128Bit) },
        { Format.R64Uint, new VulkanFormatInfo(8, 1, VkFormatCompatibilityClass.Class64Bit) },
        { Format.R64Sint, new VulkanFormatInfo(8, 1, VkFormatCompatibilityClass.Class64Bit) },
        { Format.R64Sfloat, new VulkanFormatInfo(8, 1, VkFormatCompatibilityClass.Class64Bit) },
        { Format.R64G64Uint, new VulkanFormatInfo(16, 2, VkFormatCompatibilityClass.Class128Bit) },
        { Format.R64G64Sint, new VulkanFormatInfo(16, 2, VkFormatCompatibilityClass.Class128Bit) },
        { Format.R64G64Sfloat, new VulkanFormatInfo(16, 2, VkFormatCompatibilityClass.Class128Bit) },
        { Format.R64G64B64Uint, new VulkanFormatInfo(24, 3, VkFormatCompatibilityClass.Class192Bit) },
        { Format.R64G64B64Sint, new VulkanFormatInfo(24, 3, VkFormatCompatibilityClass.Class192Bit) },
        { Format.R64G64B64Sfloat, new VulkanFormatInfo(24, 3, VkFormatCompatibilityClass.Class192Bit) },
        { Format.R64G64B64A64Uint, new VulkanFormatInfo(32, 4, VkFormatCompatibilityClass.Class256Bit) },
        { Format.R64G64B64A64Sint, new VulkanFormatInfo(32, 4, VkFormatCompatibilityClass.Class256Bit) },
        { Format.R64G64B64A64Sfloat, new VulkanFormatInfo(32, 4, VkFormatCompatibilityClass.Class256Bit) },
        { Format.B10G11R11UfloatPack32, new VulkanFormatInfo(4, 3, VkFormatCompatibilityClass.Class32Bit) },
        { Format.E5B9G9R9UfloatPack32, new VulkanFormatInfo(4, 3, VkFormatCompatibilityClass.Class32Bit) },
        { Format.D16Unorm, new VulkanFormatInfo(2, 1, VkFormatCompatibilityClass.ClassNoneBit) },
        { Format.X8D24UnormPack32, new VulkanFormatInfo(4, 1, VkFormatCompatibilityClass.ClassNoneBit) },
        { Format.D32Sfloat, new VulkanFormatInfo(4, 1, VkFormatCompatibilityClass.ClassNoneBit) },
        { Format.S8Uint, new VulkanFormatInfo(1, 1, VkFormatCompatibilityClass.ClassNoneBit) },
        { Format.D16UnormS8Uint, new VulkanFormatInfo(3, 2, VkFormatCompatibilityClass.ClassNoneBit) },
        { Format.D24UnormS8Uint, new VulkanFormatInfo(4, 2, VkFormatCompatibilityClass.ClassNoneBit) },
        { Format.D32SfloatS8Uint, new VulkanFormatInfo(8, 2, VkFormatCompatibilityClass.ClassNoneBit) },
        { Format.BC1RgbUnormBlock, new VulkanFormatInfo(8, 4, VkFormatCompatibilityClass.ClassBc1RgbBit) },
        { Format.BC1RgbSrgbBlock, new VulkanFormatInfo(8, 4, VkFormatCompatibilityClass.ClassBc1RgbBit) },
        { Format.BC1RgbaUnormBlock, new VulkanFormatInfo(8, 4, VkFormatCompatibilityClass.ClassBc1RgbaBit) },
        { Format.BC1RgbaSrgbBlock, new VulkanFormatInfo(8, 4, VkFormatCompatibilityClass.ClassBc1RgbaBit) },
        { Format.BC2UnormBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassBc2Bit) },
        { Format.BC2SrgbBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassBc2Bit) },
        { Format.BC3UnormBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassBc3Bit) },
        { Format.BC3SrgbBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassBc3Bit) },
        { Format.BC4UnormBlock, new VulkanFormatInfo(8, 4, VkFormatCompatibilityClass.ClassBc4Bit) },
        { Format.BC4SNormBlock, new VulkanFormatInfo(8, 4, VkFormatCompatibilityClass.ClassBc4Bit) },
        { Format.BC5UnormBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassBc5Bit) },
        { Format.BC5SNormBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassBc5Bit) },
        { Format.BC6HUfloatBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassBc6HBit) },
        { Format.BC6HSfloatBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassBc6HBit) },
        { Format.BC7UnormBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassBc7Bit) },
        { Format.BC7SrgbBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassBc7Bit) },
        { Format.Etc2R8G8B8UnormBlock, new VulkanFormatInfo(8, 3, VkFormatCompatibilityClass.ClassEtc2RGBBit) },
        { Format.Etc2R8G8B8SrgbBlock, new VulkanFormatInfo(8, 3, VkFormatCompatibilityClass.ClassEtc2RGBBit) },
        { Format.Etc2R8G8B8A1UnormBlock, new VulkanFormatInfo(8, 4, VkFormatCompatibilityClass.ClassEtc2RGBABit) },
        { Format.Etc2R8G8B8A1SrgbBlock, new VulkanFormatInfo(8, 4, VkFormatCompatibilityClass.ClassEtc2RGBABit) },
        { Format.Etc2R8G8B8A8UnormBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassEtc2EacRGBABit) },
        { Format.Etc2R8G8B8A8SrgbBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassEtc2EacRGBABit) },
        { Format.EacR11UnormBlock, new VulkanFormatInfo(8, 1, VkFormatCompatibilityClass.ClassEacRBit) },
        { Format.EacR11SNormBlock, new VulkanFormatInfo(8, 1, VkFormatCompatibilityClass.ClassEacRBit) },
        { Format.EacR11G11UnormBlock, new VulkanFormatInfo(16, 2, VkFormatCompatibilityClass.ClassEacRGBit) },
        { Format.EacR11G11SNormBlock, new VulkanFormatInfo(16, 2, VkFormatCompatibilityClass.ClassEacRGBit) },
        { Format.Astc4x4UnormBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassAstc4X4Bit) },
        { Format.Astc4x4SrgbBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassAstc4X4Bit) },
        { Format.Astc5x4UnormBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassAstc5X4Bit) },
        { Format.Astc5x4SrgbBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassAstc5X4Bit) },
        { Format.Astc5x5UnormBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassAstc5X5Bit) },
        { Format.Astc5x5SrgbBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassAstc5X5Bit) },
        { Format.Astc6x5UnormBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassAstc6X5Bit) },
        { Format.Astc6x5SrgbBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassAstc6X5Bit) },
        { Format.Astc6x6UnormBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassAstc6X6Bit) },
        { Format.Astc6x6SrgbBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassAstc6X6Bit) },
        { Format.Astc8x5UnormBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassAstc8X5Bit) },
        { Format.Astc8x5SrgbBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassAstc8X5Bit) },
        { Format.Astc8x6UnormBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassAstc8X6Bit) },
        { Format.Astc8x6SrgbBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassAstc8X6Bit) },
        { Format.Astc8x8UnormBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassAstc8X8Bit) },
        { Format.Astc8x8SrgbBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassAstc8X8Bit) },
        { Format.Astc10x5UnormBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassAstc10X5Bit) },
        { Format.Astc10x5SrgbBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassAstc10X5Bit) },
        { Format.Astc10x6UnormBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassAstc10X6Bit) },
        { Format.Astc10x6SrgbBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassAstc10X6Bit) },
        { Format.Astc10x8UnormBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassAstc10X8Bit) },
        { Format.Astc10x8SrgbBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassAstc10X8Bit) },
        { Format.Astc10x10UnormBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassAstc10X10Bit) },
        { Format.Astc10x10SrgbBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassAstc10X10Bit) },
        { Format.Astc12x10UnormBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassAstc12X10Bit) },
        { Format.Astc12x10SrgbBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassAstc12X10Bit) },
        { Format.Astc12x12UnormBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassAstc12X12Bit) },
        { Format.Astc12x12SrgbBlock, new VulkanFormatInfo(16, 4, VkFormatCompatibilityClass.ClassAstc12X12Bit) },
        { Format.Pvrtc12BppUnormBlockImg, new VulkanFormatInfo(8, 4, VkFormatCompatibilityClass.ClassPvrtc12BppBit) },
        { Format.Pvrtc14BppUnormBlockImg, new VulkanFormatInfo(8, 4, VkFormatCompatibilityClass.ClassPvrtc14BppBit) },
        { Format.Pvrtc22BppUnormBlockImg, new VulkanFormatInfo(8, 4, VkFormatCompatibilityClass.ClassPvrtc22BppBit) },
        { Format.Pvrtc24BppUnormBlockImg, new VulkanFormatInfo(8, 4, VkFormatCompatibilityClass.ClassPvrtc24BppBit) },
        { Format.Pvrtc12BppSrgbBlockImg, new VulkanFormatInfo(8, 4, VkFormatCompatibilityClass.ClassPvrtc12BppBit) },
        { Format.Pvrtc14BppSrgbBlockImg, new VulkanFormatInfo(8, 4, VkFormatCompatibilityClass.ClassPvrtc14BppBit) },
        { Format.Pvrtc22BppSrgbBlockImg, new VulkanFormatInfo(8, 4, VkFormatCompatibilityClass.ClassPvrtc22BppBit) },
        { Format.Pvrtc24BppSrgbBlockImg, new VulkanFormatInfo(8, 4, VkFormatCompatibilityClass.ClassPvrtc24BppBit) },
        // KHR_sampler_YCbCr_conversion extension - single-plane variants
        // 'PACK' formats are normal, uncompressed
        { Format.R10X6UnormPack16, new VulkanFormatInfo(2, 1, VkFormatCompatibilityClass.Class16Bit) },
        { Format.R10X6G10X6Unorm2Pack16, new VulkanFormatInfo(4, 2, VkFormatCompatibilityClass.Class32Bit) },
        { Format.R10X6G10X6B10X6A10X6Unorm4Pack16, new VulkanFormatInfo(8, 4, VkFormatCompatibilityClass.Class64BitR10G10B10A10) },
        { Format.R12X4UnormPack16, new VulkanFormatInfo(2, 1, VkFormatCompatibilityClass.Class16Bit) },
        { Format.R12X4G12X4Unorm2Pack16, new VulkanFormatInfo(4, 2, VkFormatCompatibilityClass.Class16Bit) },
        { Format.R12X4G12X4B12X4A12X4Unorm4Pack16, new VulkanFormatInfo(8, 4, VkFormatCompatibilityClass.Class64BitR12G12B12A12) },
        // _422 formats encode 2 texels per entry with B, R components shared - treated as compressed w/ 2x1 block size
        { Format.G8B8G8R8422Unorm, new VulkanFormatInfo(4, 4, VkFormatCompatibilityClass.Class32BitG8B8G8R8) },
        { Format.B8G8R8G8422Unorm, new VulkanFormatInfo(4, 4, VkFormatCompatibilityClass.Class32BitB8G8R8G8) },
        { Format.G10X6B10X6G10X6R10X6422Unorm4Pack16, new VulkanFormatInfo(8, 4, VkFormatCompatibilityClass.Class64BitG10B10G10R10) },
        { Format.B10X6G10X6R10X6G10X6422Unorm4Pack16, new VulkanFormatInfo(8, 4, VkFormatCompatibilityClass.Class64BitB10G10R10G10) },
        { Format.G12X4B12X4G12X4R12X4422Unorm4Pack16, new VulkanFormatInfo(8, 4, VkFormatCompatibilityClass.Class64BitG12B12G12R12) },
        { Format.B12X4G12X4R12X4G12X4422Unorm4Pack16, new VulkanFormatInfo(8, 4, VkFormatCompatibilityClass.Class64BitB12G12R12G12) },
        { Format.G16B16G16R16422Unorm, new VulkanFormatInfo(8, 4, VkFormatCompatibilityClass.Class64BitG16B16G16R16) },
        { Format.B16G16R16G16422Unorm, new VulkanFormatInfo(8, 4, VkFormatCompatibilityClass.Class64BitB16G16R16G16) },
        // KHR_sampler_YCbCr_conversion extension - multi-plane variants
        // Formats that 'share' components among texels (_420 and _422), size represents total bytes for the smallest possible texel block
        // _420 share B, R components within a 2x2 texel block
        { Format.G8B8R83Plane420Unorm, new VulkanFormatInfo(6, 3, VkFormatCompatibilityClass.Class8Bit3Plane420) },
        { Format.G8B8R82Plane420Unorm, new VulkanFormatInfo(6, 3, VkFormatCompatibilityClass.Class8Bit2Plane420) },
        { Format.G10X6B10X6R10X63Plane420Unorm3Pack16, new VulkanFormatInfo(12, 3, VkFormatCompatibilityClass.Class10Bit3Plane420) },
        { Format.G10X6B10X6R10X62Plane420Unorm3Pack16, new VulkanFormatInfo(12, 3, VkFormatCompatibilityClass.Class10Bit2Plane420) },
        { Format.G12X4B12X4R12X43Plane420Unorm3Pack16, new VulkanFormatInfo(12, 3, VkFormatCompatibilityClass.Class12Bit3Plane420) },
        { Format.G12X4B12X4R12X42Plane420Unorm3Pack16, new VulkanFormatInfo(12, 3, VkFormatCompatibilityClass.Class12Bit2Plane420) },
        { Format.G16B16R163Plane420Unorm, new VulkanFormatInfo(12, 3, VkFormatCompatibilityClass.Class16Bit3Plane420) },
        { Format.G16B16R162Plane420Unorm, new VulkanFormatInfo(12, 3, VkFormatCompatibilityClass.Class16Bit2Plane420) },
        // _422 share B, R components within a 2x1 texel block
        { Format.G8B8R83Plane422Unorm, new VulkanFormatInfo(4, 3, VkFormatCompatibilityClass.Class8Bit3Plane422) },
        { Format.G8B8R82Plane422Unorm, new VulkanFormatInfo(4, 3, VkFormatCompatibilityClass.Class8Bit2Plane422) },
        { Format.G10X6B10X6R10X63Plane422Unorm3Pack16, new VulkanFormatInfo(8, 3, VkFormatCompatibilityClass.Class10Bit3Plane422) },
        { Format.G10X6B10X6R10X62Plane422Unorm3Pack16, new VulkanFormatInfo(8, 3, VkFormatCompatibilityClass.Class10Bit2Plane422) },
        { Format.G12X4B12X4R12X43Plane422Unorm3Pack16, new VulkanFormatInfo(8, 3, VkFormatCompatibilityClass.Class12Bit3Plane422) },
        { Format.G12X4B12X4R12X42Plane422Unorm3Pack16, new VulkanFormatInfo(8, 3, VkFormatCompatibilityClass.Class12Bit2Plane422) },
        { Format.G16B16R163Plane422Unorm, new VulkanFormatInfo(8, 3, VkFormatCompatibilityClass.Class16Bit3Plane422) },
        { Format.G16B16R162Plane422Unorm, new VulkanFormatInfo(8, 3, VkFormatCompatibilityClass.Class16Bit2Plane422) },
        // _444 do not share
        { Format.G8B8R83Plane444Unorm, new VulkanFormatInfo(3, 3, VkFormatCompatibilityClass.Class8Bit3Plane444) },
        { Format.G10X6B10X6R10X63Plane444Unorm3Pack16, new VulkanFormatInfo(6, 3, VkFormatCompatibilityClass.Class10Bit3Plane444) },
        { Format.G12X4B12X4R12X43Plane444Unorm3Pack16, new VulkanFormatInfo(6, 3, VkFormatCompatibilityClass.Class12Bit3Plane444) },
        { Format.G16B16R163Plane444Unorm, new VulkanFormatInfo(6, 3, VkFormatCompatibilityClass.Class16Bit3Plane444) },
    };

    static readonly Dictionary<Format, MultiplaneCompatibility> MultiplaneCompatibilityMap = new Dictionary<Format, MultiplaneCompatibility>
    {
        {
            Format.G8B8R83Plane420Unorm, new MultiplaneCompatibility(
                new PerPlaneCompatibility(1, 1, Format.R8Unorm),
                new PerPlaneCompatibility(2, 2, Format.R8Unorm),
                new PerPlaneCompatibility(2, 2, Format.R8Unorm))
        },
        {
            Format.G8B8R82Plane420Unorm, new MultiplaneCompatibility(
                new PerPlaneCompatibility(1, 1, Format.R8Unorm),
                new PerPlaneCompatibility(2, 2, Format.R8G8Unorm),
                new PerPlaneCompatibility(1, 1, Format.Undefined))
        },
        {
            Format.G8B8R83Plane422Unorm, new MultiplaneCompatibility(
                new PerPlaneCompatibility(1, 1, Format.R8Unorm),
                new PerPlaneCompatibility(2, 1, Format.R8Unorm),
                new PerPlaneCompatibility(2, 1, Format.R8Unorm))
        },
        {
            Format.G8B8R82Plane422Unorm, new MultiplaneCompatibility(
                new PerPlaneCompatibility(1, 1, Format.R8Unorm),
                new PerPlaneCompatibility(2, 1, Format.R8G8Unorm),
                new PerPlaneCompatibility(1, 1, Format.Undefined))
        },
        {
            Format.G8B8R83Plane444Unorm, new MultiplaneCompatibility(
                new PerPlaneCompatibility(1, 1, Format.R8Unorm),
                new PerPlaneCompatibility(1, 1, Format.R8Unorm),
                new PerPlaneCompatibility(1, 1, Format.R8Unorm))
        },
        {
            Format.G10X6B10X6R10X63Plane420Unorm3Pack16, new MultiplaneCompatibility(
                new PerPlaneCompatibility(1, 1, Format.R10X6UnormPack16),
                new PerPlaneCompatibility(2, 2, Format.R10X6UnormPack16),
                new PerPlaneCompatibility(2, 2, Format.R10X6UnormPack16))
        },
        {
            Format.G10X6B10X6R10X62Plane420Unorm3Pack16, new MultiplaneCompatibility(
                new PerPlaneCompatibility(1, 1, Format.R10X6UnormPack16),
                new PerPlaneCompatibility(2, 2, Format.R10X6G10X6Unorm2Pack16),
                new PerPlaneCompatibility(1, 1, Format.Undefined))
        },
        {
            Format.G10X6B10X6R10X63Plane422Unorm3Pack16, new MultiplaneCompatibility(
                new PerPlaneCompatibility(1, 1, Format.R10X6UnormPack16),
                new PerPlaneCompatibility(2, 1, Format.R10X6UnormPack16),
                new PerPlaneCompatibility(2, 1, Format.R10X6UnormPack16))
        },
        {
            Format.G10X6B10X6R10X62Plane422Unorm3Pack16, new MultiplaneCompatibility(
                new PerPlaneCompatibility(1, 1, Format.R10X6UnormPack16),
                new PerPlaneCompatibility(2, 1, Format.R10X6G10X6Unorm2Pack16),
                new PerPlaneCompatibility(1, 1, Format.Undefined))
        },
        {
            Format.G10X6B10X6R10X63Plane444Unorm3Pack16, new MultiplaneCompatibility(
                new PerPlaneCompatibility(1, 1, Format.R10X6UnormPack16),
                new PerPlaneCompatibility(1, 1, Format.R10X6UnormPack16),
                new PerPlaneCompatibility(1, 1, Format.R10X6UnormPack16))
        },
        {
            Format.G12X4B12X4R12X43Plane420Unorm3Pack16, new MultiplaneCompatibility(
                new PerPlaneCompatibility(1, 1, Format.R12X4UnormPack16),
                new PerPlaneCompatibility(2, 2, Format.R12X4UnormPack16),
                new PerPlaneCompatibility(2, 2, Format.R12X4UnormPack16))
        },
        {
            Format.G12X4B12X4R12X42Plane420Unorm3Pack16, new MultiplaneCompatibility(
                new PerPlaneCompatibility(1, 1, Format.R12X4UnormPack16),
                new PerPlaneCompatibility(2, 2, Format.R12X4G12X4Unorm2Pack16),
                new PerPlaneCompatibility(1, 1, Format.Undefined))
        },
        {
            Format.G12X4B12X4R12X43Plane422Unorm3Pack16, new MultiplaneCompatibility(
                new PerPlaneCompatibility(1, 1, Format.R12X4UnormPack16),
                new PerPlaneCompatibility(2, 1, Format.R12X4UnormPack16),
                new PerPlaneCompatibility(2, 1, Format.R12X4UnormPack16))
        },
        {
            Format.G12X4B12X4R12X42Plane422Unorm3Pack16, new MultiplaneCompatibility(
                new PerPlaneCompatibility(1, 1, Format.R12X4UnormPack16),
                new PerPlaneCompatibility(2, 1, Format.R12X4G12X4Unorm2Pack16),
                new PerPlaneCompatibility(1, 1, Format.Undefined))
        },
        {
            Format.G12X4B12X4R12X43Plane444Unorm3Pack16, new MultiplaneCompatibility(
                new PerPlaneCompatibility(1, 1, Format.R12X4UnormPack16),
                new PerPlaneCompatibility(1, 1, Format.R12X4UnormPack16),
                new PerPlaneCompatibility(1, 1, Format.R12X4UnormPack16))
        },
        {
            Format.G16B16R163Plane420Unorm, new MultiplaneCompatibility(
                new PerPlaneCompatibility(1, 1, Format.R16Unorm),
                new PerPlaneCompatibility(2, 2, Format.R16Unorm),
                new PerPlaneCompatibility(2, 2, Format.R16Unorm))
        },
        {
            Format.G16B16R162Plane420Unorm, new MultiplaneCompatibility(
                new PerPlaneCompatibility(1, 1, Format.R16Unorm),
                new PerPlaneCompatibility(2, 2, Format.R16G16Unorm),
                new PerPlaneCompatibility(1, 1, Format.Undefined))
        },
        {
            Format.G16B16R163Plane422Unorm, new MultiplaneCompatibility(
                new PerPlaneCompatibility(1, 1, Format.R16Unorm),
                new PerPlaneCompatibility(2, 1, Format.R16Unorm),
                new PerPlaneCompatibility(2, 1, Format.R16Unorm))
        },
        {
            Format.G16B16R162Plane422Unorm, new MultiplaneCompatibility(
                new PerPlaneCompatibility(1, 1, Format.R16Unorm),
                new PerPlaneCompatibility(2, 1, Format.R16G16Unorm),
                new PerPlaneCompatibility(1, 1, Format.Undefined))
        },
        {
            Format.G16B16R163Plane444Unorm, new MultiplaneCompatibility(
                new PerPlaneCompatibility(1, 1, Format.R16Unorm),
                new PerPlaneCompatibility(1, 1, Format.R16Unorm),
                new PerPlaneCompatibility(1, 1, Format.R16Unorm))
        },
    };

    public static bool IsUndef(this Format format) => format == Format.Undefined;
    public static bool HasDepth(this Format format) => format.IsDepthOnly() || format.IsDepthAndStencil();
    public static bool HasStencil(this Format format) => format.IsStencilOnly() || format.IsDepthAndStencil();
    public static bool IsMultiplane(this Format format) => format.PlaneCount() > 1u;
    public static bool IsColor(this Format format) => !(format.IsUndef() || format.IsDepthOrStencil() || format.IsMultiplane());

    public static bool IsCompressedEtc2Eac(this Format format)
    {
        switch (format)
        {
            case Format.Etc2R8G8B8UnormBlock:
            case Format.Etc2R8G8B8SrgbBlock:
            case Format.Etc2R8G8B8A1UnormBlock:
            case Format.Etc2R8G8B8A1SrgbBlock:
            case Format.Etc2R8G8B8A8UnormBlock:
            case Format.Etc2R8G8B8A8SrgbBlock:
            case Format.EacR11UnormBlock:
            case Format.EacR11SNormBlock:
            case Format.EacR11G11UnormBlock:
            case Format.EacR11G11SNormBlock:
                return true;
            default:
                return false;
        }
    }

    public static bool IsCompressedAstcLdr(this Format format)
    {
        switch (format)
        {
            case Format.Astc4x4UnormBlock:
            case Format.Astc4x4SrgbBlock:
            case Format.Astc5x4UnormBlock:
            case Format.Astc5x4SrgbBlock:
            case Format.Astc5x5UnormBlock:
            case Format.Astc5x5SrgbBlock:
            case Format.Astc6x5UnormBlock:
            case Format.Astc6x5SrgbBlock:
            case Format.Astc6x6UnormBlock:
            case Format.Astc6x6SrgbBlock:
            case Format.Astc8x5UnormBlock:
            case Format.Astc8x5SrgbBlock:
            case Format.Astc8x6UnormBlock:
            case Format.Astc8x6SrgbBlock:
            case Format.Astc8x8UnormBlock:
            case Format.Astc8x8SrgbBlock:
            case Format.Astc10x5UnormBlock:
            case Format.Astc10x5SrgbBlock:
            case Format.Astc10x6UnormBlock:
            case Format.Astc10x6SrgbBlock:
            case Format.Astc10x8UnormBlock:
            case Format.Astc10x8SrgbBlock:
            case Format.Astc10x10UnormBlock:
            case Format.Astc10x10SrgbBlock:
            case Format.Astc12x10UnormBlock:
            case Format.Astc12x10SrgbBlock:
            case Format.Astc12x12UnormBlock:
            case Format.Astc12x12SrgbBlock:
                return true;
            default:
                return false;
        }
    }

    public static bool IsCompressedBc(this Format format)
    {
        switch (format)
        {
            case Format.BC1RgbUnormBlock:
            case Format.BC1RgbSrgbBlock:
            case Format.BC1RgbaUnormBlock:
            case Format.BC1RgbaSrgbBlock:
            case Format.BC2UnormBlock:
            case Format.BC2SrgbBlock:
            case Format.BC3UnormBlock:
            case Format.BC3SrgbBlock:
            case Format.BC4UnormBlock:
            case Format.BC4SNormBlock:
            case Format.BC5UnormBlock:
            case Format.BC5SNormBlock:
            case Format.BC6HUfloatBlock:
            case Format.BC6HSfloatBlock:
            case Format.BC7UnormBlock:
            case Format.BC7SrgbBlock:
                return true;
            default:
                return false;
        }
    }

    public static bool IsCompressedPvrtc(this Format format)
    {
        switch (format)
        {
            case Format.Pvrtc12BppUnormBlockImg:
            case Format.Pvrtc14BppUnormBlockImg:
            case Format.Pvrtc22BppUnormBlockImg:
            case Format.Pvrtc24BppUnormBlockImg:
            case Format.Pvrtc12BppSrgbBlockImg:
            case Format.Pvrtc14BppSrgbBlockImg:
            case Format.Pvrtc22BppSrgbBlockImg:
            case Format.Pvrtc24BppSrgbBlockImg:
                return true;
            default:
                return false;
        }
    }

    // Single-plane "_422" formats are treated as 2x1 compressed (for copies)
    public static bool IsSinglePlane422(this Format format)
    {
        switch (format)
        {
            case Format.G8B8G8R8422UnormKhr:
            case Format.B8G8R8G8422UnormKhr:
            case Format.G10X6B10X6G10X6R10X6422Unorm4Pack16Khr:
            case Format.B10X6G10X6R10X6G10X6422Unorm4Pack16Khr:
            case Format.G12X4B12X4G12X4R12X4422Unorm4Pack16Khr:
            case Format.B12X4G12X4R12X4G12X4422Unorm4Pack16Khr:
            case Format.G16B16G16R16422UnormKhr:
            case Format.B16G16R16G16422UnormKhr:
                return true;
            default:
                return false;
        }
    }

    // Return true if format is compressed
    public static bool IsCompressed(this Format format) =>
        format.IsCompressedAstcLdr() || format.IsCompressedBc() || format.IsCompressedEtc2Eac() ||
        format.IsCompressedPvrtc();

    // Return true if format is packed
    public static bool IsPacked(this Format format)
    {
        switch (format)
        {
            case Format.R4G4UnormPack8:
            case Format.R4G4B4A4UnormPack16:
            case Format.B4G4R4A4UnormPack16:
            case Format.R5G6B5UnormPack16:
            case Format.B5G6R5UnormPack16:
            case Format.R5G5B5A1UnormPack16:
            case Format.B5G5R5A1UnormPack16:
            case Format.A1R5G5B5UnormPack16:
            case Format.A8B8G8R8UnormPack32:
            case Format.A8B8G8R8SNormPack32:
            case Format.A8B8G8R8UscaledPack32:
            case Format.A8B8G8R8SscaledPack32:
            case Format.A8B8G8R8UintPack32:
            case Format.A8B8G8R8SintPack32:
            case Format.A8B8G8R8SrgbPack32:
            case Format.A2R10G10B10UnormPack32:
            case Format.A2R10G10B10SNormPack32:
            case Format.A2R10G10B10UscaledPack32:
            case Format.A2R10G10B10SscaledPack32:
            case Format.A2R10G10B10UintPack32:
            case Format.A2R10G10B10SintPack32:
            case Format.A2B10G10R10UnormPack32:
            case Format.A2B10G10R10SNormPack32:
            case Format.A2B10G10R10UscaledPack32:
            case Format.A2B10G10R10SscaledPack32:
            case Format.A2B10G10R10UintPack32:
            case Format.A2B10G10R10SintPack32:
            case Format.B10G11R11UfloatPack32:
            case Format.E5B9G9R9UfloatPack32:
            case Format.X8D24UnormPack32:
            case Format.R10X6UnormPack16:
            case Format.R10X6G10X6Unorm2Pack16:
            case Format.R10X6G10X6B10X6A10X6Unorm4Pack16:
            case Format.G10X6B10X6G10X6R10X6422Unorm4Pack16:
            case Format.B10X6G10X6R10X6G10X6422Unorm4Pack16:
            case Format.R12X4UnormPack16:
            case Format.R12X4G12X4Unorm2Pack16:
            case Format.R12X4G12X4B12X4A12X4Unorm4Pack16:
            case Format.G12X4B12X4G12X4R12X4422Unorm4Pack16:
            case Format.B12X4G12X4R12X4G12X4422Unorm4Pack16:
            case Format.G10X6B10X6R10X63Plane420Unorm3Pack16:
            case Format.G10X6B10X6R10X62Plane420Unorm3Pack16:
            case Format.G10X6B10X6R10X63Plane422Unorm3Pack16:
            case Format.G10X6B10X6R10X62Plane422Unorm3Pack16:
            case Format.G10X6B10X6R10X63Plane444Unorm3Pack16:
            case Format.G12X4B12X4R12X43Plane420Unorm3Pack16:
            case Format.G12X4B12X4R12X42Plane420Unorm3Pack16:
            case Format.G12X4B12X4R12X43Plane422Unorm3Pack16:
            case Format.G12X4B12X4R12X42Plane422Unorm3Pack16:
            case Format.G12X4B12X4R12X43Plane444Unorm3Pack16:
                return true;
            default:
                return false;
        }
    }

    // Return true if format is 'normal', with one texel per format element
    public static bool ElementIsTexel(this Format format) => !(format.IsPacked() || format.IsCompressed() || format.IsSinglePlane422() || format.IsMultiplane());

    // Return true if format is a depth or stencil format
    public static bool IsDepthOrStencil(this Format format) => format.IsDepthAndStencil() || format.IsDepthOnly() || format.IsStencilOnly();

    // Return true if format contains depth and stencil information
    public static bool IsDepthAndStencil(this Format format)
    {
        switch (format)
        {
            case Format.D16UnormS8Uint:
            case Format.D24UnormS8Uint:
            case Format.D32SfloatS8Uint:
                return true;
            default:
                return false;
        }
    }

    // Return true if format is a stencil-only format
    public static bool IsStencilOnly(this Format format) => format == Format.S8Uint;

    // Return true if format is a depth-only format
    public static bool IsDepthOnly(this Format format)
    {
        switch (format)
        {
            case Format.D16Unorm:
            case Format.X8D24UnormPack32:
            case Format.D32Sfloat:
                return true;
            default:
                return false;
        }
    }

    // Return true if format is of type NORM
    public static bool IsNorm(this Format format)
    {
        switch (format)
        {
            case Format.R4G4UnormPack8:
            case Format.R4G4B4A4UnormPack16:
            case Format.R5G6B5UnormPack16:
            case Format.R5G5B5A1UnormPack16:
            case Format.A1R5G5B5UnormPack16:
            case Format.R8Unorm:
            case Format.R8SNorm:
            case Format.R8G8Unorm:
            case Format.R8G8SNorm:
            case Format.R8G8B8Unorm:
            case Format.R8G8B8SNorm:
            case Format.R8G8B8A8Unorm:
            case Format.R8G8B8A8SNorm:
            case Format.A8B8G8R8UnormPack32:
            case Format.A8B8G8R8SNormPack32:
            case Format.A2B10G10R10UnormPack32:
            case Format.A2B10G10R10SNormPack32:
            case Format.R16Unorm:
            case Format.R16SNorm:
            case Format.R16G16Unorm:
            case Format.R16G16SNorm:
            case Format.R16G16B16Unorm:
            case Format.R16G16B16SNorm:
            case Format.R16G16B16A16Unorm:
            case Format.R16G16B16A16SNorm:
            case Format.BC1RgbUnormBlock:
            case Format.BC2UnormBlock:
            case Format.BC3UnormBlock:
            case Format.BC4UnormBlock:
            case Format.BC4SNormBlock:
            case Format.BC5UnormBlock:
            case Format.BC5SNormBlock:
            case Format.BC7UnormBlock:
            case Format.Etc2R8G8B8UnormBlock:
            case Format.Etc2R8G8B8A1UnormBlock:
            case Format.Etc2R8G8B8A8UnormBlock:
            case Format.EacR11UnormBlock:
            case Format.EacR11SNormBlock:
            case Format.EacR11G11UnormBlock:
            case Format.EacR11G11SNormBlock:
            case Format.Astc4x4UnormBlock:
            case Format.Astc5x4UnormBlock:
            case Format.Astc5x5UnormBlock:
            case Format.Astc6x5UnormBlock:
            case Format.Astc6x6UnormBlock:
            case Format.Astc8x5UnormBlock:
            case Format.Astc8x6UnormBlock:
            case Format.Astc8x8UnormBlock:
            case Format.Astc10x5UnormBlock:
            case Format.Astc10x6UnormBlock:
            case Format.Astc10x8UnormBlock:
            case Format.Astc10x10UnormBlock:
            case Format.Astc12x10UnormBlock:
            case Format.Astc12x12UnormBlock:
            case Format.B5G6R5UnormPack16:
            case Format.B8G8R8Unorm:
            case Format.B8G8R8SNorm:
            case Format.B8G8R8A8Unorm:
            case Format.B8G8R8A8SNorm:
            case Format.A2R10G10B10UnormPack32:
            case Format.A2R10G10B10SNormPack32:
                return true;
            default:
                return false;
        }
    }

    // Return true if format is of type UNORM
    public static bool IsUNorm(this Format format)
    {
        switch (format)
        {
            case Format.R4G4UnormPack8:
            case Format.R4G4B4A4UnormPack16:
            case Format.R5G6B5UnormPack16:
            case Format.R5G5B5A1UnormPack16:
            case Format.A1R5G5B5UnormPack16:
            case Format.R8Unorm:
            case Format.R8G8Unorm:
            case Format.R8G8B8Unorm:
            case Format.R8G8B8A8Unorm:
            case Format.A8B8G8R8UnormPack32:
            case Format.A2B10G10R10UnormPack32:
            case Format.R16Unorm:
            case Format.R16G16Unorm:
            case Format.R16G16B16Unorm:
            case Format.R16G16B16A16Unorm:
            case Format.BC1RgbUnormBlock:
            case Format.BC2UnormBlock:
            case Format.BC3UnormBlock:
            case Format.BC4UnormBlock:
            case Format.BC5UnormBlock:
            case Format.BC7UnormBlock:
            case Format.Etc2R8G8B8UnormBlock:
            case Format.Etc2R8G8B8A1UnormBlock:
            case Format.Etc2R8G8B8A8UnormBlock:
            case Format.EacR11UnormBlock:
            case Format.EacR11G11UnormBlock:
            case Format.Astc4x4UnormBlock:
            case Format.Astc5x4UnormBlock:
            case Format.Astc5x5UnormBlock:
            case Format.Astc6x5UnormBlock:
            case Format.Astc6x6UnormBlock:
            case Format.Astc8x5UnormBlock:
            case Format.Astc8x6UnormBlock:
            case Format.Astc8x8UnormBlock:
            case Format.Astc10x5UnormBlock:
            case Format.Astc10x6UnormBlock:
            case Format.Astc10x8UnormBlock:
            case Format.Astc10x10UnormBlock:
            case Format.Astc12x10UnormBlock:
            case Format.Astc12x12UnormBlock:
            case Format.B5G6R5UnormPack16:
            case Format.B8G8R8Unorm:
            case Format.B8G8R8A8Unorm:
            case Format.A2R10G10B10UnormPack32:
                return true;
            default:
                return false;
        }
    }

    // Return true if format is of type SNORM
    public static bool IsSNorm(this Format format)
    {
        switch (format)
        {
            case Format.R8SNorm:
            case Format.R8G8SNorm:
            case Format.R8G8B8SNorm:
            case Format.R8G8B8A8SNorm:
            case Format.A8B8G8R8SNormPack32:
            case Format.A2B10G10R10SNormPack32:
            case Format.R16SNorm:
            case Format.R16G16SNorm:
            case Format.R16G16B16SNorm:
            case Format.R16G16B16A16SNorm:
            case Format.BC4SNormBlock:
            case Format.BC5SNormBlock:
            case Format.EacR11SNormBlock:
            case Format.EacR11G11SNormBlock:
            case Format.B8G8R8SNorm:
            case Format.B8G8R8A8SNorm:
            case Format.A2R10G10B10SNormPack32:
                return true;
            default:
                return false;
        }
    }

    // Return true if format is an integer format
    public static bool IsInt(this Format format) => format.IsSInt() || format.IsUInt();

    // Return true if format is an unsigned integer format
    public static bool IsUInt(this Format format)
    {
        switch (format)
        {
            case Format.R8Uint:
            case Format.S8Uint:
            case Format.R8G8Uint:
            case Format.R8G8B8Uint:
            case Format.R8G8B8A8Uint:
            case Format.A8B8G8R8UintPack32:
            case Format.A2B10G10R10UintPack32:
            case Format.R16Uint:
            case Format.R16G16Uint:
            case Format.R16G16B16Uint:
            case Format.R16G16B16A16Uint:
            case Format.R32Uint:
            case Format.R32G32Uint:
            case Format.R32G32B32Uint:
            case Format.R32G32B32A32Uint:
            case Format.R64Uint:
            case Format.R64G64Uint:
            case Format.R64G64B64Uint:
            case Format.R64G64B64A64Uint:
            case Format.B8G8R8Uint:
            case Format.B8G8R8A8Uint:
            case Format.A2R10G10B10UintPack32:
                return true;
            default:
                return false;
        }
    }

    // Return true if format is a signed integer format
    public static bool IsSInt(this Format format)
    {
        switch (format)
        {
            case Format.R8Sint:
            case Format.R8G8Sint:
            case Format.R8G8B8Sint:
            case Format.R8G8B8A8Sint:
            case Format.A8B8G8R8SintPack32:
            case Format.A2B10G10R10SintPack32:
            case Format.R16Sint:
            case Format.R16G16Sint:
            case Format.R16G16B16Sint:
            case Format.R16G16B16A16Sint:
            case Format.R32Sint:
            case Format.R32G32Sint:
            case Format.R32G32B32Sint:
            case Format.R32G32B32A32Sint:
            case Format.R64Sint:
            case Format.R64G64Sint:
            case Format.R64G64B64Sint:
            case Format.R64G64B64A64Sint:
            case Format.B8G8R8Sint:
            case Format.B8G8R8A8Sint:
            case Format.A2R10G10B10SintPack32:
                return true;
            default:
                return false;
        }
    }

    // Return true if format is a floating-point format
    public static bool IsFloat(this Format format)
    {
        switch (format)
        {
            case Format.R16Sfloat:
            case Format.R16G16Sfloat:
            case Format.R16G16B16Sfloat:
            case Format.R16G16B16A16Sfloat:
            case Format.R32Sfloat:
            case Format.R32G32Sfloat:
            case Format.R32G32B32Sfloat:
            case Format.R32G32B32A32Sfloat:
            case Format.R64Sfloat:
            case Format.R64G64Sfloat:
            case Format.R64G64B64Sfloat:
            case Format.R64G64B64A64Sfloat:
            case Format.B10G11R11UfloatPack32:
            case Format.E5B9G9R9UfloatPack32:
            case Format.BC6HUfloatBlock:
            case Format.BC6HSfloatBlock:
                return true;
            default:
                return false;
        }
    }

    // Return true if format is in the SRGB colorspace
    public static bool IsSRGB(this Format format)
    {
        switch (format)
        {
            case Format.R8Srgb:
            case Format.R8G8Srgb:
            case Format.R8G8B8Srgb:
            case Format.R8G8B8A8Srgb:
            case Format.A8B8G8R8SrgbPack32:
            case Format.BC1RgbSrgbBlock:
            case Format.BC2SrgbBlock:
            case Format.BC3SrgbBlock:
            case Format.BC7SrgbBlock:
            case Format.Etc2R8G8B8SrgbBlock:
            case Format.Etc2R8G8B8A1SrgbBlock:
            case Format.Etc2R8G8B8A8SrgbBlock:
            case Format.Astc4x4SrgbBlock:
            case Format.Astc5x4SrgbBlock:
            case Format.Astc5x5SrgbBlock:
            case Format.Astc6x5SrgbBlock:
            case Format.Astc6x6SrgbBlock:
            case Format.Astc8x5SrgbBlock:
            case Format.Astc8x6SrgbBlock:
            case Format.Astc8x8SrgbBlock:
            case Format.Astc10x5SrgbBlock:
            case Format.Astc10x6SrgbBlock:
            case Format.Astc10x8SrgbBlock:
            case Format.Astc10x10SrgbBlock:
            case Format.Astc12x10SrgbBlock:
            case Format.Astc12x12SrgbBlock:
            case Format.B8G8R8Srgb:
            case Format.B8G8R8A8Srgb:
                return true;
            default:
                return false;
        }
    }

    // Return true if format is a USCALED format
    public static bool IsUScaled(this Format format)
    {
        switch (format)
        {
            case Format.R8Uscaled:
            case Format.R8G8Uscaled:
            case Format.R8G8B8Uscaled:
            case Format.B8G8R8Uscaled:
            case Format.R8G8B8A8Uscaled:
            case Format.B8G8R8A8Uscaled:
            case Format.A8B8G8R8UscaledPack32:
            case Format.A2R10G10B10UscaledPack32:
            case Format.A2B10G10R10UscaledPack32:
            case Format.R16Uscaled:
            case Format.R16G16Uscaled:
            case Format.R16G16B16Uscaled:
            case Format.R16G16B16A16Uscaled:
                return true;
            default:
                return false;
        }
    }

    // Return true if format is a SSCALED format
    public static bool IsSScaled(this Format format)
    {
        switch (format)
        {
            case Format.R8Sscaled:
            case Format.R8G8Sscaled:
            case Format.R8G8B8Sscaled:
            case Format.B8G8R8Sscaled:
            case Format.R8G8B8A8Sscaled:
            case Format.B8G8R8A8Sscaled:
            case Format.A8B8G8R8SscaledPack32:
            case Format.A2R10G10B10SscaledPack32:
            case Format.A2B10G10R10SscaledPack32:
            case Format.R16Sscaled:
            case Format.R16G16Sscaled:
            case Format.R16G16B16Sscaled:
            case Format.R16G16B16A16Sscaled:
                return true;
            default:
                return false;
        }
    }

    // Return texel block sizes for all formats
    // Uncompressed formats return {1, 1, 1}
    // Compressed formats return the compression block extents
    // Multiplane formats return the 'shared' extent of their low-res channel(s)
    public static Extent3D TexelBlockExtent(this Format format)
    {
        switch (format)
        {
            case Format.BC1RgbUnormBlock:
            case Format.BC1RgbSrgbBlock:
            case Format.BC1RgbaUnormBlock:
            case Format.BC1RgbaSrgbBlock:
            case Format.BC2UnormBlock:
            case Format.BC2SrgbBlock:
            case Format.BC3UnormBlock:
            case Format.BC3SrgbBlock:
            case Format.BC4UnormBlock:
            case Format.BC4SNormBlock:
            case Format.BC5UnormBlock:
            case Format.BC5SNormBlock:
            case Format.BC6HUfloatBlock:
            case Format.BC6HSfloatBlock:
            case Format.BC7UnormBlock:
            case Format.BC7SrgbBlock:
            case Format.Etc2R8G8B8UnormBlock:
            case Format.Etc2R8G8B8SrgbBlock:
            case Format.Etc2R8G8B8A1UnormBlock:
            case Format.Etc2R8G8B8A1SrgbBlock:
            case Format.Etc2R8G8B8A8UnormBlock:
            case Format.Etc2R8G8B8A8SrgbBlock:
            case Format.EacR11UnormBlock:
            case Format.EacR11SNormBlock:
            case Format.EacR11G11UnormBlock:
            case Format.EacR11G11SNormBlock:
            case Format.Astc4x4UnormBlock:
            case Format.Astc4x4SrgbBlock:
                return new Extent3D(4, 4, 1);
            case Format.Astc5x4UnormBlock:
            case Format.Astc5x4SrgbBlock:
                return new Extent3D(5, 4, 1);
            case Format.Astc5x5UnormBlock:
            case Format.Astc5x5SrgbBlock:
                return new Extent3D(5, 5, 1);
            case Format.Astc6x5UnormBlock:
            case Format.Astc6x5SrgbBlock:
                return new Extent3D(6, 5, 1);
            case Format.Astc6x6UnormBlock:
            case Format.Astc6x6SrgbBlock:
                return new Extent3D(6, 6, 1);
            case Format.Astc8x5UnormBlock:
            case Format.Astc8x5SrgbBlock:
                return new Extent3D(8, 5, 1);
            case Format.Astc8x6UnormBlock:
            case Format.Astc8x6SrgbBlock:
                return new Extent3D(8, 6, 1);
            case Format.Astc8x8UnormBlock:
            case Format.Astc8x8SrgbBlock:
                return new Extent3D(8, 8, 1);
            case Format.Astc10x5UnormBlock:
            case Format.Astc10x5SrgbBlock:
                return new Extent3D(10, 5, 1);
            case Format.Astc10x6UnormBlock:
            case Format.Astc10x6SrgbBlock:
                return new Extent3D(10, 6, 1);
            case Format.Astc10x8UnormBlock:
            case Format.Astc10x8SrgbBlock:
                return new Extent3D(10, 8, 1);
            case Format.Astc10x10UnormBlock:
            case Format.Astc10x10SrgbBlock:
                return new Extent3D(10, 10, 1);
            case Format.Astc12x10UnormBlock:
            case Format.Astc12x10SrgbBlock:
                return new Extent3D(12, 10, 1);
            case Format.Astc12x12UnormBlock:
            case Format.Astc12x12SrgbBlock:
                return new Extent3D(12, 12, 1);
            case Format.Pvrtc12BppUnormBlockImg:
            case Format.Pvrtc22BppUnormBlockImg:
            case Format.Pvrtc12BppSrgbBlockImg:
            case Format.Pvrtc22BppSrgbBlockImg:
                return new Extent3D(8, 4, 1);
            case Format.Pvrtc14BppUnormBlockImg:
            case Format.Pvrtc24BppUnormBlockImg:
            case Format.Pvrtc14BppSrgbBlockImg:
            case Format.Pvrtc24BppSrgbBlockImg:
                return new Extent3D(4, 4, 1);
            // (KHR_sampler_ycbcr_conversion) _422 single-plane formats are treated as 2x1 compressed (for copies)
            case Format.G8B8G8R8422Unorm:
            case Format.B8G8R8G8422Unorm:
            case Format.G10X6B10X6G10X6R10X6422Unorm4Pack16:
            case Format.B10X6G10X6R10X6G10X6422Unorm4Pack16:
            case Format.G12X4B12X4G12X4R12X4422Unorm4Pack16:
            case Format.B12X4G12X4R12X4G12X4422Unorm4Pack16:
            case Format.G16B16G16R16422Unorm:
            case Format.B16G16R16G16422Unorm:
                return new Extent3D(2, 1, 1);
            // _422 multi-plane formats are not considered compressed, but shared components form a logical 2x1 block
            case Format.G8B8R83Plane422Unorm:
            case Format.G8B8R82Plane422Unorm:
            case Format.G10X6B10X6R10X63Plane422Unorm3Pack16:
            case Format.G10X6B10X6R10X62Plane422Unorm3Pack16:
            case Format.G12X4B12X4R12X43Plane422Unorm3Pack16:
            case Format.G12X4B12X4R12X42Plane422Unorm3Pack16:
            case Format.G16B16R163Plane422Unorm:
            case Format.G16B16R162Plane422Unorm:
                return new Extent3D(2, 1, 1);
            // _420 formats are not considered compressed, but shared components form a logical 2x2 block
            case Format.G8B8R83Plane420Unorm:
            case Format.G8B8R82Plane420Unorm:
            case Format.G10X6B10X6R10X63Plane420Unorm3Pack16:
            case Format.G10X6B10X6R10X62Plane420Unorm3Pack16:
            case Format.G12X4B12X4R12X43Plane420Unorm3Pack16:
            case Format.G12X4B12X4R12X42Plane420Unorm3Pack16:
            case Format.G16B16R163Plane420Unorm:
            case Format.G16B16R162Plane420Unorm:
                return new Extent3D(2, 2, 1);
            // _444 multi-plane formats do not share components, default to 1x1
            case Format.G10X6B10X6R10X63Plane444Unorm3Pack16:
            case Format.G8B8R83Plane444Unorm:
            case Format.G12X4B12X4R12X43Plane444Unorm3Pack16:
            case Format.G16B16R163Plane444Unorm:
            default:
                return new Extent3D(1, 1, 1);
        }
    }

    public static uint DepthSize(this Format format)
    {
        switch (format)
        {
            case Format.D16Unorm:
            case Format.D16UnormS8Uint:
                return 16;
            case Format.X8D24UnormPack32:
            case Format.D24UnormS8Uint:
                return 24;
            case Format.D32Sfloat:
            case Format.D32SfloatS8Uint:
                return 32;
            default:
                return 0;
        }
    }

    public static VkFormatNumericalType DepthNumericalType(this Format format)
    {
        switch (format)
        {
            case Format.D16Unorm:
            case Format.D16UnormS8Uint:
            case Format.X8D24UnormPack32:
            case Format.D24UnormS8Uint:
                return VkFormatNumericalType.UNorm;
            case Format.D32Sfloat:
            case Format.D32SfloatS8Uint:
                return VkFormatNumericalType.SFloat;
            default:
                return VkFormatNumericalType.None;
        }
    }

    public static uint StencilSize(this Format format)
    {
        switch (format)
        {
            case Format.S8Uint:
            case Format.D16UnormS8Uint:
            case Format.D24UnormS8Uint:
            case Format.D32SfloatS8Uint:
                return 8;
            default:
                return 0;
        }
    }

    public static VkFormatNumericalType StencilNumericalType(this Format format)
    {
        switch (format)
        {
            case Format.S8Uint:
            case Format.D16UnormS8Uint:
            case Format.D24UnormS8Uint:
            case Format.D32SfloatS8Uint:
                return VkFormatNumericalType.UInt;
            default:
                return VkFormatNumericalType.None;
        }
    }

    public static uint PlaneCount(this Format format)
    {
        switch (format)
        {
            case Format.G8B8R83Plane420Unorm:
            case Format.G8B8R83Plane422Unorm:
            case Format.G8B8R83Plane444Unorm:
            case Format.G10X6B10X6R10X63Plane420Unorm3Pack16:
            case Format.G10X6B10X6R10X63Plane422Unorm3Pack16:
            case Format.G10X6B10X6R10X63Plane444Unorm3Pack16:
            case Format.G12X4B12X4R12X43Plane420Unorm3Pack16:
            case Format.G12X4B12X4R12X43Plane422Unorm3Pack16:
            case Format.G12X4B12X4R12X43Plane444Unorm3Pack16:
            case Format.G16B16R163Plane420Unorm:
            case Format.G16B16R163Plane422Unorm:
            case Format.G16B16R163Plane444Unorm:
                return 3;
            case Format.G8B8R82Plane420Unorm:
            case Format.G8B8R82Plane422Unorm:
            case Format.G10X6B10X6R10X62Plane420Unorm3Pack16:
            case Format.G10X6B10X6R10X62Plane422Unorm3Pack16:
            case Format.G12X4B12X4R12X42Plane420Unorm3Pack16:
            case Format.G12X4B12X4R12X42Plane422Unorm3Pack16:
            case Format.G16B16R162Plane420Unorm:
            case Format.G16B16R162Plane422Unorm:
                return 2;
            default:
                return 1;
        }
    }

    // Return format class of the specified format
    public static VkFormatCompatibilityClass CompatibilityClass(this Format format) => FormatTable.TryGetValue(format, out var item) ? item.FormatClass : VkFormatCompatibilityClass.ClassNoneBit;

    // Return size, in bytes, of one element of the specified format
    // For uncompressed this is one texel, for compressed it is one block
    public static uint ElementSize(this Format format, ImageAspectFlags aspectMask = ImageAspectFlags.ColorBit)
    {
        // Handle special buffer packing rules for specific depth/stencil formats
        if (aspectMask == ImageAspectFlags.StencilBit)
        {
            format = Format.S8Uint;
        }
        else if (aspectMask == ImageAspectFlags.DepthBit)
        {
            switch (format)
            {
                case Format.D16UnormS8Uint:
                    format = Format.D16Unorm;
                    break;
                case Format.D32SfloatS8Uint:
                    format = Format.D32Sfloat;
                    break;
            }
        }
        else if (format.IsMultiplane())
        {
            format = FindMultiplaneCompatibleFormat(format, aspectMask);
        }

        return FormatTable.TryGetValue(format, out var item) ? item.Size : 0;
    }

    // Return the size in bytes of one texel of given foramt
    // For compressed or multi-plane, this may be a fractional number
    public static double TexelSize(this Format format)
    {
        double texelSize = ElementSize(format);
        var blockExtent = TexelBlockExtent(format);
        var texelsPerBlock = blockExtent.Width * blockExtent.Height * blockExtent.Depth;
        if (1.0 < texelsPerBlock)
        {
            texelSize /= texelsPerBlock;
        }

        return texelSize;
    }

    // Return the number of channels for a given format
    public static uint ChannelCount(this Format format) => FormatTable.TryGetValue(format, out var item) ? item.ChannelCount : 0;


    static int GetPlaneIndex(this ImageAspectFlags aspect)
    {
        // Returns an out-of-bounds index on error
        return aspect switch
        {
            ImageAspectFlags.Plane0Bit => 0,
            ImageAspectFlags.Plane1Bit => 1,
            ImageAspectFlags.Plane2Bit => 2,
            _ => VkMultiplaneFormatMaxPlanes,
        };
    }

    public static Format FindMultiplaneCompatibleFormat(this Format format, ImageAspectFlags planeAspect)
    {
        var planeIdx = planeAspect.GetPlaneIndex();

        if (!MultiplaneCompatibilityMap.TryGetValue(format, out var it) || planeIdx >= VkMultiplaneFormatMaxPlanes)
        {
            return Format.Undefined;
        }

        return it[planeIdx].compatibleFormat;
    }

    public static Extent2D FindMultiplaneExtentDivisors(this Format format, ImageAspectFlags planeAspect)
    {
        var divisors = new Extent2D(1, 1);
        var planeIdx = planeAspect.GetPlaneIndex();

        if (!MultiplaneCompatibilityMap.TryGetValue(format, out var it) || planeIdx >= VkMultiplaneFormatMaxPlanes)
            return divisors;

        divisors.Width = it[planeIdx].widthDivisor;
        divisors.Height = it[planeIdx].heightDivisor;
        return divisors;
    }

    public static bool SizesAreEqual(this Format srcFormat, Format dstFormat, ImageCopy[] regions)
    {
        uint srcSize, dstSize;
        if (IsMultiplane(srcFormat) || IsMultiplane(dstFormat))
        {
            for (uint i = 0; i < regions.Length; i++)
            {
                if (IsMultiplane(srcFormat))
                {
                    var planeFormat = FindMultiplaneCompatibleFormat(srcFormat, regions[i].SrcSubresource.AspectMask);
                    srcSize = planeFormat.ElementSize();
                }
                else
                {
                    srcSize = ElementSize(srcFormat);
                }

                if (IsMultiplane(dstFormat))
                {
                    var planeFormat = FindMultiplaneCompatibleFormat(dstFormat, regions[i].DstSubresource.AspectMask);
                    dstSize = ElementSize(planeFormat);
                }
                else
                {
                    dstSize = ElementSize(dstFormat);
                }

                if (dstSize != srcSize)
                    return false;
            }

            return true;
        }

        srcSize = ElementSize(srcFormat);
        dstSize = ElementSize(dstFormat);
        return dstSize == srcSize;
    }

    public static bool RequiresYcbcrConversion(this Format format) => format is >= Format.G8B8G8R8422Unorm and <= Format.G16B16R163Plane444Unorm;

    public static uint GetRegionSize(this Format format, uint width, uint height, uint depth)
    {
        uint blockSizeInBytes;
        if (format.IsCompressed())
        {
            Extent3D blockSize = format.TexelBlockExtent();
            Debug.Assert((width % blockSize.Width == 0 || width < blockSize.Width) && (height % blockSize.Height == 0 || height < blockSize.Height));
            blockSizeInBytes = format.ElementSize();
            width /= blockSize.Width;
            height /= blockSize.Height;
        }
        else
        {
            blockSizeInBytes = format.ElementSize();
        }

        return width * height * depth * blockSizeInBytes;
    }
}

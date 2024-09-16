using System;

namespace Silk.NET.Vulkan.Extensions.Helpers;

/// <summary>
/// Describes a Structure specifying parameters of a newly created image view
/// </summary>
public static class ImageViewCreateInfoExt
{

    /// <summary>
    /// Constructs a new ImageViewCreateInfo
    /// </summary>
    /// <param name="image">Image on which the view will be created</param>
    /// <param name="imageCreateInfo">Parameters of an image</param>
    /// <returns>new ImageViewCreateInfo</returns>
    public static ImageViewCreateInfo Create(Image image, ImageCreateInfo imageCreateInfo)
    {
        return new ImageViewCreateInfo
        {
            SType = StructureType.ImageViewCreateInfo,
            Image = image,
            Format = imageCreateInfo.Format,
            SubresourceRange = new ImageSubresourceRange(
                imageCreateInfo.Usage.HasFlag(ImageUsageFlags.DepthStencilAttachmentBit)
                    ? imageCreateInfo.Format.HasStencil() ? ImageAspectFlags.DepthBit | ImageAspectFlags.StencilBit : ImageAspectFlags.DepthBit
                    : ImageAspectFlags.ColorBit,
                0u,
                imageCreateInfo.MipLevels,
                0,
                imageCreateInfo.ArrayLayers),
            ViewType = GetImageViewType(imageCreateInfo, imageCreateInfo.ArrayLayers),
        };
    }

    /// <summary>
    /// Constructs a new ImageViewCreateInfo
    /// </summary>
    /// <param name="image">Image on which the view will be created</param>
    /// <param name="imageCreateInfo">Parameters of an image</param>
    /// <param name="format">override format of image (should be compatible with the image format)</param>
    /// <returns>new ImageViewCreateInfo</returns>
    public static ImageViewCreateInfo Create(Image image, ImageCreateInfo imageCreateInfo, Format format)
    {
        return new ImageViewCreateInfo
        {
            SType = StructureType.ImageViewCreateInfo,
            Image = image,
            Format = format,
            SubresourceRange = new ImageSubresourceRange(
                imageCreateInfo.Usage.HasFlag(ImageUsageFlags.DepthStencilAttachmentBit) ? ImageAspectFlags.DepthBit : ImageAspectFlags.ColorBit,
                0u,
                imageCreateInfo.MipLevels,
                0,
                imageCreateInfo.ArrayLayers),
            ViewType = GetImageViewType(imageCreateInfo, imageCreateInfo.ArrayLayers),
        };
    }

    /// <summary>
    /// Constructs a new TextureViewDescription.
    /// </summary>
    /// <param name="image">Image on which the view will be created</param>
    /// <param name="imageCreateInfo">Parameters of an image</param>
    /// <param name="baseMipLevel">The first mipmap level accessible to the view (should be less than <see cref="ImageCreateInfo.MipLevels"/>)</param>
    /// <param name="mipLevels">The number of mipmap levels (starting from baseMipLevel) accessible to the view</param>
    /// <param name="baseArrayLayer">The first array layer accessible to the view</param>
    /// <param name="arrayLayers">The number of array layers (starting from baseArrayLayer) accessible to the view</param>
    /// <returns>new ImageViewCreateInfo</returns>
    public static ImageViewCreateInfo Create(Image image, ImageCreateInfo imageCreateInfo, uint baseMipLevel, uint mipLevels, uint baseArrayLayer, uint arrayLayers)
    {
        return new ImageViewCreateInfo
        {
            SType = StructureType.ImageViewCreateInfo,
            Image = image,
            Format = imageCreateInfo.Format,
            SubresourceRange = new ImageSubresourceRange(
                imageCreateInfo.Usage.HasFlag(ImageUsageFlags.DepthStencilAttachmentBit) ? ImageAspectFlags.DepthBit : ImageAspectFlags.ColorBit,
                baseMipLevel,
                mipLevels,
                baseArrayLayer,
                arrayLayers),
            ViewType = GetImageViewType(imageCreateInfo, arrayLayers),
        };
    }

    /// <summary>
    /// Constructs a new ImageViewCreateInfo
    /// </summary>
    /// <param name="image">Image on which the view will be created</param>
    /// <param name="imageCreateInfo">Parameters of an image</param>
    /// <param name="format">Override format of image (should be compatible with the image format)</param>
    /// <param name="baseMipLevel">The first mipmap level accessible to the view (should be less than <see cref="ImageCreateInfo.MipLevels"/>)</param>
    /// <param name="mipLevels">The number of mipmap levels (starting from baseMipLevel) accessible to the view</param>
    /// <param name="baseArrayLayer">The first array layer accessible to the view</param>
    /// <param name="arrayLayers">The number of array layers (starting from baseArrayLayer) accessible to the view</param>
    /// <returns>new ImageViewCreateInfo</returns>
    public static ImageViewCreateInfo Create(Image image, ImageCreateInfo imageCreateInfo, Format format, uint baseMipLevel, uint mipLevels, uint baseArrayLayer, uint arrayLayers)
    {
        return new ImageViewCreateInfo
        {
            SType = StructureType.ImageViewCreateInfo,
            Image = image,
            Format = format,
            SubresourceRange = new ImageSubresourceRange(
                imageCreateInfo.Usage.HasFlag(ImageUsageFlags.DepthStencilAttachmentBit) ? ImageAspectFlags.DepthBit : ImageAspectFlags.ColorBit,
                baseMipLevel,
                mipLevels,
                baseArrayLayer,
                arrayLayers),
            ViewType = GetImageViewType(imageCreateInfo, arrayLayers),
        };
    }

    static ImageViewType GetImageViewType(ImageCreateInfo tex, uint arrayLayers)
    {
        if (tex.Flags.HasFlag(ImageCreateFlags.CreateCubeCompatibleBit))
        {
            return arrayLayers == 6 ? ImageViewType.TypeCube : ImageViewType.TypeCubeArray;
        }

        return tex.ImageType switch
        {
            ImageType.Type1D when arrayLayers == 1 => ImageViewType.Type1D,
            ImageType.Type1D => ImageViewType.Type1DArray,
            ImageType.Type2D when arrayLayers == 1 => ImageViewType.Type2D,
            ImageType.Type2D => ImageViewType.Type2DArray,
            ImageType.Type3D => ImageViewType.Type3D,
            _ => throw new ArgumentOutOfRangeException(nameof(tex.ImageType)),
        };
    }

}

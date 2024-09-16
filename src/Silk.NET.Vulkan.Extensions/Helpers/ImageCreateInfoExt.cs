namespace Silk.NET.Vulkan.Extensions.Helpers;

/// <summary>
/// Describes a Structure specifying the parameters of a newly created image object/>.
/// </summary>
public static class ImageCreateInfoExt
{
    static ImageCreateInfo Create(uint width,
        uint height,
        uint depth,
        uint mipLevels,
        uint arrayLayers,
        Format format,
        ImageUsageFlags usage,
        ImageCreateFlags createFlags,
        ImageType type,
        SampleCountFlags sampleCount,
        ImageLayout initialLayout,
        ImageTiling tiling)
    {
        return new ImageCreateInfo
        {
            SType = StructureType.ImageCreateInfo,
            Extent = new Extent3D(width, height, depth),
            MipLevels = mipLevels,
            ArrayLayers = arrayLayers,
            Format = format,
            Usage = usage,
            Flags = createFlags,
            ImageType = type,
            Samples = sampleCount,
            InitialLayout = initialLayout,
            Tiling = tiling,
        };
    }

    /// <summary>
    /// Constructs a structure specifying a non-multisampled 1D Texture
    /// </summary>
    /// <param name="width">Describing the number of data elements of the base level</param>
    /// <param name="mipLevels">Describes the number of levels of detail available for minified sampling of the image</param>
    /// <param name="arrayLayers">Specifying the number of samples per texel</param>
    /// <param name="format">Describing the format and type of the texel blocks that will be contained in the image</param>
    /// <param name="usage">Describing the intended usage of the image</param>
    /// <param name="createFlags">Describing additional parameters of the image</param>
    /// <returns>A new ImageCreateInfo for a non-multisampled 1D Texture.</returns>
    public static ImageCreateInfo Texture1D(
        uint width,
        uint mipLevels,
        uint arrayLayers,
        Format format,
        ImageUsageFlags usage,
        ImageCreateFlags createFlags)
    {
        return Create(width,
            1,
            1,
            mipLevels,
            arrayLayers,
            format,
            usage,
            createFlags,
            ImageType.Type1D,
            SampleCountFlags.Count1Bit,
            ImageLayout.Preinitialized,
            ImageTiling.Optimal);
    }

    /// <summary>
    /// Constructs a structure specifying a non-multisampled 2D Texture
    /// </summary>
    /// <param name="width">Describing the number of data elements in X dimension of the base level</param>
    /// <param name="height">Describing the number of data elements in Y dimension of the base level</param>
    /// <param name="mipLevels">Describes the number of levels of detail available for minified sampling of the image</param>
    /// <param name="arrayLayers">Specifying the number of samples per texel</param>
    /// <param name="format">Describing the format and type of the texel blocks that will be contained in the image</param>
    /// <param name="usage">Describing the intended usage of the image</param>
    /// <param name="createFlags">Describing additional parameters of the image</param>
    /// <returns>A new ImageCreateInfo for a non-multisampled 2D Texture.</returns>
    public static ImageCreateInfo Texture2D(
        uint width,
        uint height,
        uint mipLevels,
        uint arrayLayers,
        Format format,
        ImageUsageFlags usage,
        ImageCreateFlags createFlags = ImageCreateFlags.CreateMutableFormatBit)
    {
        return Create(width,
            height,
            1,
            mipLevels,
            arrayLayers,
            format,
            usage,
            createFlags,
            ImageType.Type2D,
            SampleCountFlags.Count1Bit,
            ImageLayout.Preinitialized,
            ImageTiling.Optimal);
    }

    /// <summary>
    /// Constructs a structure specifying a 2D Texture
    /// </summary>
    /// <param name="width">Describing the number of data elements in X dimension of the base level</param>
    /// <param name="height">Describing the number of data elements in Y dimension of the base level</param>
    /// <param name="mipLevels">Describes the number of levels of detail available for minified sampling of the image</param>
    /// <param name="arrayLayers">Specifying the number of samples per texel</param>
    /// <param name="format">Describing the format and type of the texel blocks that will be contained in the image</param>
    /// <param name="usage">Describing the intended usage of the image</param>
    /// <param name="createFlags">Describing additional parameters of the image</param>
    /// <param name="sampleCount">Specifying the number of samples per texel</param>
    /// <returns>A new ImageCreateInfo for a 2D Texture.</returns>
    public static ImageCreateInfo Texture2D(
        uint width,
        uint height,
        uint mipLevels,
        uint arrayLayers,
        Format format,
        ImageUsageFlags usage,
        ImageCreateFlags createFlags,
        SampleCountFlags sampleCount)
    {
        return Create(width,
            height,
            1,
            mipLevels,
            arrayLayers,
            format,
            usage,
            createFlags,
            ImageType.Type2D,
            sampleCount,
            ImageLayout.Preinitialized,
            ImageTiling.Optimal);
    }

    /// <summary>
    /// Constructs a structure specifying a 3D Texture
    /// </summary>
    /// <param name="width">Describing the number of data elements in X dimension of the base level</param>
    /// <param name="height">Describing the number of data elements in Y dimension of the base level</param>
    /// <param name="depth">Describing the number of data elements in Z dimension of the base level</param>
    /// <param name="mipLevels">Describes the number of levels of detail available for minified sampling of the image</param>
    /// <param name="format">Describing the format and type of the texel blocks that will be contained in the image</param>
    /// <param name="usage">Describing the intended usage of the image</param>
    /// <param name="createFlags">Describing additional parameters of the image</param>
    /// <returns>A new ImageCreateInfo for a 3D Texture.</returns>
    public static ImageCreateInfo Texture3D(
        uint width,
        uint height,
        uint depth,
        uint mipLevels,
        Format format,
        ImageCreateFlags createFlags,
        ImageUsageFlags usage)
    {
        return Create(width,
            height,
            depth,
            mipLevels,
            1,
            format,
            usage,
            createFlags,
            ImageType.Type3D,
            SampleCountFlags.Count1Bit,
            ImageLayout.Preinitialized,
            ImageTiling.Optimal);
    }
}

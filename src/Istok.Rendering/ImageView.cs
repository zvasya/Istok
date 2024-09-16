using Silk.NET.Vulkan;

namespace Istok.Rendering;

/// <summary>
/// Representing contiguous ranges of the image subresources and containing additional metadata are used for accessing by pipeline shaders for reading or writing image data
/// </summary>
public unsafe class ImageView : IDisposable
{
    readonly LogicalDevice _logicalDevice;
    readonly Silk.NET.Vulkan.ImageView _imageView;
    string _name;

    public Silk.NET.Vulkan.ImageView DeviceImageView => _imageView;

    /// <summary>
    /// The target <see cref="Image"/>
    /// </summary>
    public Image Target { get; }
    /// <summary>
    /// The base number of levels of detail available for minified sampling of the image for this view
    /// </summary>

    public ImageAspectFlags AspectMask { get; }
    public ImageViewType ViewType { get; }
    public uint BaseMipLevel { get; }
    /// <summary>
    /// The number of levels of detail available for minified sampling of the image for this view
    /// </summary>
    public uint MipLevels { get; }
    /// <summary>
    /// The base number of sample per texel for this view
    /// </summary>
    public uint BaseArrayLayer { get; }
    /// <summary>
    /// The number of samples per texel for this view
    /// </summary>
    public uint ArrayLayers { get; }
    /// <summary>
    /// The format and type of the texel blocks that will be contained in the image
    /// </summary>
    public Format Format { get; }

    ImageView(LogicalDevice logicalDevice, Silk.NET.Vulkan.ImageView imageView, Image target, in ImageViewCreateInfo description)
    {
        _logicalDevice = logicalDevice;
        Target = target;

        ViewType = description.ViewType;
        AspectMask = description.SubresourceRange.AspectMask;
        BaseMipLevel = description.SubresourceRange.BaseMipLevel;
        MipLevels = description.SubresourceRange.LevelCount;
        BaseArrayLayer = description.SubresourceRange.BaseArrayLayer;
        ArrayLayers = description.SubresourceRange.LayerCount;
        Format = description.Format;

        _imageView = imageView;
    }

    public static ImageView Create(LogicalDevice logicalDevice, Image target, in ImageViewCreateInfo description)
    {
        ImageViewCreateInfo imageViewCI = description;
        logicalDevice.CreateImageView(in imageViewCI, null, out Silk.NET.Vulkan.ImageView imageView);
        return new ImageView(logicalDevice, imageView, target, in description);
    }

    public static ImageView Create(LogicalDevice logicalDevice, Image target)
    {
        ImageViewCreateInfo imageViewCI = target.GetImageViewCreateInfo();
        logicalDevice.CreateImageView(in imageViewCI, null, out Silk.NET.Vulkan.ImageView imageView);
        return new ImageView(logicalDevice, imageView, target, ref imageViewCI);
    }

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            _logicalDevice.SetObjectName(ObjectType.ImageView, _imageView.Handle, value);
        }
    }

    public bool IsDisposed { get; private set; }

    public void Dispose()
    {
        if (!IsDisposed)
        {
            IsDisposed = true;
            _logicalDevice.DestroyImageView(_imageView, null);
        }
    }

    public ResourcesSetBindingSampledImage GetResourcesSetBoundSampled()
    {
        return new ResourcesSetBindingSampledImage(this);
    }

    public ResourcesSetBindingStorageImage GetResourcesSetBoundStorage()
    {
        return new ResourcesSetBindingStorageImage(this);
    }
}

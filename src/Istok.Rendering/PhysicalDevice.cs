using Silk.NET.Vulkan.Extensions.Helpers;
using Silk.NET.Core;
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.KHR;
using Silk.NET.Vulkan.Extensions.Missed;

namespace Istok.Rendering;

using DeviceProperties2 = (PhysicalDeviceProperties2? deviceProperties, PhysicalDeviceDriverProperties? driverProperties);

public unsafe partial class PhysicalDevice
{
    readonly string[] _deviceExtensions = [KhrSwapchain.ExtensionName];

    readonly Engine _engine;

    readonly Silk.NET.Vulkan.PhysicalDevice _physicalDevice;

    ConformanceVersion? _apiVersion;

    string _deviceName;

    DeviceProperties2? _deviceProperties2;

    string _driverInfo;

    string _driverName;


    Dictionary<string, uint> _extensionProperties;
    ExtensionProperties[] _extensionPropertiesRaw;

    PhysicalDeviceFeatures? _features;

    uint? _graphicsQueueIndex;

    PhysicalDeviceMemoryProperties? _memoryProperties;

    PhysicalDeviceProperties? _properties;

    QueueFamilyProperties[]? _queueFamilyProperties;

    string _vendorName;

    internal PhysicalDevice(Silk.NET.Vulkan.PhysicalDevice physicalDevice, Engine engine)
    {
        _physicalDevice = physicalDevice;
        _engine = engine;
    }

    public PhysicalDeviceProperties Properties => _properties ??= GetProperties();
    public string DeviceName => _deviceName ??= Properties.GetDeviceName();
    public string VendorName =>  _vendorName ??= Properties.VendorID.ToString("x8");
    public string DriverInfo => _driverInfo ??= DeviceProperties2.driverProperties?.GetDriverInfo() ?? Properties.DriverVersion.ToString("x8");
    public ConformanceVersion ApiVersion => _apiVersion ??= DeviceProperties2.driverProperties?.ConformanceVersion ?? new ConformanceVersion();
    public Dictionary<string, uint> ExtensionProperties => _extensionProperties ??= ExtensionPropertiesRaw.ToDictionary(prop => prop.GetName(), prop => prop.SpecVersion);
    public ExtensionProperties[] ExtensionPropertiesRaw => _extensionPropertiesRaw ??= GetDeviceExtensionProperties();
    DeviceProperties2 DeviceProperties2 => _deviceProperties2 ??= GetDriverProperties();
    public string DriverName => _driverName ??= DeviceProperties2.driverProperties?.GetDriverName() ?? "Driver name undefined";
    public PhysicalDeviceFeatures Features => _features ??= GetFeatures();
    public PhysicalDeviceMemoryProperties MemoryProperties => _memoryProperties ??= GetMemoryProperties();
    QueueFamilyProperties[] QueueFamilyProperties => _queueFamilyProperties ??= GetQueueFamilyProperties();
    public uint GraphicsQueueIndex => _graphicsQueueIndex ??= GetGraphicsQueueIndex();

    DeviceProperties2 GetDriverProperties()
    {
        if (!ExtensionProperties.ContainsKey(KhrDriverProperties.ExtensionName))
            return (null, null);

        PhysicalDeviceDriverProperties driverProps = new PhysicalDeviceDriverProperties
        {
            SType = StructureType.PhysicalDeviceDriverProperties,
        };

        PhysicalDeviceProperties2 deviceProps = new PhysicalDeviceProperties2
        {
            SType = StructureType.PhysicalDeviceProperties2,
            PNext = &driverProps,
        };

        GetPhysicalDeviceProperties2(&deviceProps);

        return (deviceProps,driverProps);
    }

    ExtensionProperties[] GetDeviceExtensionProperties()
    {
        uint propertyCount = 0;
        Result result = EnumerateDeviceExtensionProperties((byte*)null, &propertyCount, null);
        Helpers.CheckErrors(result);
        ExtensionProperties[] props = new ExtensionProperties[(int)propertyCount];
        fixed (ExtensionProperties* properties = props)
        {
            result = EnumerateDeviceExtensionProperties((byte*)null, &propertyCount, properties);
            Helpers.CheckErrors(result);
        }
        return props;
    }

    PhysicalDeviceMemoryProperties GetMemoryProperties()
    {
        GetPhysicalDeviceMemoryProperties(out PhysicalDeviceMemoryProperties physicalDeviceMemProperties);
        return physicalDeviceMemProperties;
    }

    PhysicalDeviceFeatures GetFeatures()
    {
        GetPhysicalDeviceFeatures(out PhysicalDeviceFeatures deviceFeatures);
        return deviceFeatures;
    }

    PhysicalDeviceProperties GetProperties()
    {
        if (DeviceProperties2.deviceProperties.HasValue)
            return DeviceProperties2.deviceProperties.Value.Properties;

        GetPhysicalDeviceProperties(out PhysicalDeviceProperties properties);
        return properties;
    }

    public LogicalDevice CreateLogicalDevice(uint graphicsQueueIndex, uint presentQueueIndex, IPlatform platform)
    {
        return LogicalDevice.CreateLogicalDevice(_engine, this, graphicsQueueIndex, presentQueueIndex, platform);
    }

    public bool IsPhysicalDeviceSuitable(SurfaceKHR surface)
    {
        if (GraphicsQueueIndex == uint.MaxValue)
            return false;

        if (GetPresentQueueIndex(surface) == uint.MaxValue)
            return false;

        if (!CheckPhysicalDeviceExtensionSupport())
            return false;

        if (GetSurfaceFormats(surface).Length == 0 || GetSurfacePresentModes(surface).Length == 0)
            return false;

        PhysicalDeviceFeatures supportedFeatures;
        GetPhysicalDeviceFeatures(&supportedFeatures);

        return supportedFeatures.SamplerAnisotropy;
    }

    bool CheckPhysicalDeviceExtensionSupport()
    {
        return _deviceExtensions.All(ExtensionProperties.ContainsKey);
    }

    public bool TryFindMemoryType(uint typeFilter, MemoryPropertyFlags properties, out uint typeIndex)
    {
        typeIndex = 0;
        PhysicalDeviceMemoryProperties memProperties = MemoryProperties;

        for (int i = 0; i < memProperties.MemoryTypeCount; i++)
        {
            if ((typeFilter & (1 << i)) != 0 && (memProperties.MemoryTypes[i].PropertyFlags & properties) == properties)
            {
                typeIndex = (uint)i;
                return true;
            }
        }

        return false;
    }

    public uint FindMemoryType(uint typeFilter, MemoryPropertyFlags properties)
    {
        if (!TryFindMemoryType(typeFilter, properties, out uint typeIndex))
            throw new Exception("failed to find suitable memory type!");
        return typeIndex;
    }

    Format FindSupportedFormat(IEnumerable<Format> candidates, ImageTiling tiling, FormatFeatureFlags features)
    {
        foreach (Format format in candidates)
        {
            FormatProperties props;
            GetPhysicalDeviceFormatProperties(format, &props);

            if (tiling == ImageTiling.Linear && (props.LinearTilingFeatures & features) == features) {
                return format;
            } else if (tiling == ImageTiling.Optimal && (props.OptimalTilingFeatures & features) == features) {
                return format;
            }
        }

        throw new Exception("failed to find supported format!");
    }

    public Format FindDepthFormat()
    {
        return FindSupportedFormat(
            [Format.D32Sfloat, Format.D32SfloatS8Uint, Format.D24UnormS8Uint],
            ImageTiling.Optimal,
            FormatFeatureFlags.DepthStencilAttachmentBit
        );
    }

    public SurfaceCapabilitiesKHR GetSurfaceCapabilities(SurfaceKHR surface)
    {
        // Capabilities
        Result result = GetPhysicalDeviceSurfaceCapabilities(surface, out SurfaceCapabilitiesKHR surfaceCapabilities);
        if (result == Result.ErrorSurfaceLostKhr)
        {
            throw new Exception("The Swapchain's underlying surface has been lost.");
        }
        return surfaceCapabilities;
    }

    public SurfaceFormatKHR[] GetSurfaceFormats(SurfaceKHR surface)
    {
        // Formats
        uint formatCount;
        Helpers.CheckErrors(GetPhysicalDeviceSurfaceFormats(surface, &formatCount, null));

        if (formatCount != 0)
        {
            SurfaceFormatKHR[] formats = new SurfaceFormatKHR[formatCount];
            fixed (SurfaceFormatKHR* formatsPtr = &formats[0])
            {
                Helpers.CheckErrors(GetPhysicalDeviceSurfaceFormats(surface, &formatCount, formatsPtr));
            }

            return formats;
        }

        return [];
    }

    public PresentModeKHR[] GetSurfacePresentModes(SurfaceKHR surface)
    {
        uint presentModeCount;
        Helpers.CheckErrors(GetPhysicalDeviceSurfacePresentModes(surface, &presentModeCount, null));

        if (presentModeCount != 0)
        {
            PresentModeKHR[] presentModes = new PresentModeKHR[presentModeCount];
            fixed (PresentModeKHR* presentModesPtr = &presentModes[0])
            {
                Helpers.CheckErrors(GetPhysicalDeviceSurfacePresentModes(surface, &presentModeCount, presentModesPtr));
            }

            return presentModes;
        }

        return [];
    }

    // public Surface.SwapChainSupportDetails QuerySwapChainSupport(SurfaceKHR surface)
    // {
    //     return new Surface.SwapChainSupportDetails()
    //     {
    //         _capabilities = GetSurfaceCapabilities(surface),
    //         _formats = GetSurfaceFormats(surface),
    //         _presentModes = GetSurfacePresentModes(surface),
    //     };
    // }

    public static SurfaceFormatKHR ChooseSwapSurfaceFormat(SurfaceFormatKHR[] availableFormats)
    {
        foreach (SurfaceFormatKHR availableFormat in availableFormats)
        {
            if (availableFormat.Format == Format.B8G8R8A8Srgb && availableFormat.ColorSpace == ColorSpaceKHR.SpaceSrgbNonlinearKhr)
            {
                return availableFormat;
            }
        }

        return availableFormats[0];
    }

    QueueFamilyProperties[] GetQueueFamilyProperties()
    {
        uint queueFamilyCount = 0;
        GetPhysicalDeviceQueueFamilyProperties(ref queueFamilyCount, null);
        QueueFamilyProperties[] qfp = new QueueFamilyProperties[queueFamilyCount];
        GetPhysicalDeviceQueueFamilyProperties(ref queueFamilyCount, out qfp[0]);
        return qfp;
    }

    uint GetGraphicsQueueIndex()
    {
        QueueFamilyProperties[] queueFamilyPropertiesArray = QueueFamilyProperties;
        for (uint i = 0; i < queueFamilyPropertiesArray.Length; i++)
        {
            if (queueFamilyPropertiesArray[i].QueueFlags.HasFlag(QueueFlags.GraphicsBit))
                return i;
        }

        return uint.MaxValue;
    }

    public uint GetPresentQueueIndex(SurfaceKHR surface)
    {
        if (surface.Handle == default)
            return uint.MaxValue;

        for (uint i = 0; i < QueueFamilyProperties.Length; i++)
        {
            GetPhysicalDeviceSurfaceSupport(i, surface, out Bool32 presentSupported);
            if (presentSupported)
                return i;
        }

        return uint.MaxValue;
    }

    /// <summary>
    /// If format is not a supported image format, or if the combination of format, type, tiling, usage, and flags is not supported for images, then returns false
    /// tiling = ImageTiling.Optimal
    /// flags = ImageCreateFlags.None
    /// </summary>
    /// <param name="format">The image format</param>
    /// <param name="type">The image type</param>
    /// <param name="usage">The intended usage of the image</param>
    /// <param name="flags">Additional parameters of the image</param>
    /// <param name="properties">Returned capabilities if the combination of format, type, tiling, usage, and flags is supported</param>
    /// <returns>True if the combination of format, type, tiling, usage, and flags is supported</returns>
    public bool GetImageFormatProperties(
        Format format,
        ImageType type,
        ImageUsageFlags usage,
        out ImageFormatProperties properties
    ) => GetImageFormatProperties(format, type, ImageTiling.Optimal, usage, ImageCreateFlags.None, out properties);

    /// <summary>
    /// If format is not a supported image format, or if the combination of format, type, tiling, usage, and flags is not supported for images, then returns false
    /// </summary>
    /// <param name="format">The image format</param>
    /// <param name="type">The image type</param>
    /// <param name="tiling">The image tiling</param>
    /// <param name="usage">The intended usage of the image</param>
    /// <param name="flags">Additional parameters of the image</param>
    /// <param name="properties">Returned capabilities if the combination of format, type, tiling, usage, and flags is supported</param>
    /// <returns>True if the combination of format, type, tiling, usage, and flags is supported</returns>
    public bool GetImageFormatProperties(
        Format format,
        ImageType type,
        ImageTiling tiling,
        ImageUsageFlags usage,
        ImageCreateFlags flags,
        out ImageFormatProperties properties)
    {
        Result result = GetPhysicalDeviceImageFormatProperties(
            format,
            type,
            tiling,
            usage,
            flags,
            out properties);

        if (result == Result.ErrorFormatNotSupported)
            return false;

        Helpers.CheckErrors(result);

        return true;
    }
}

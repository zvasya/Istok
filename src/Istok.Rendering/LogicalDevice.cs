using System.Collections.Concurrent;
using Silk.NET.Core.Native;
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.KHR;

namespace Istok.Rendering;

public unsafe partial class LogicalDevice : IDisposable
{
    internal KhrSwapchain KhrSwapchain = null;

    readonly Engine _engine;
    readonly PhysicalDevice _physicalDevice;
    readonly Device _device;
    readonly ConcurrentDictionary<Format, Filter> _filters = new ConcurrentDictionary<Format, Filter>();

    LogicalDevice(Engine engine, PhysicalDevice physicalDevice, Device device)
    {
        _device = device;
        _engine = engine;
        _physicalDevice = physicalDevice;
    }

    public PhysicalDevice PhysicalDevice => _physicalDevice;
    public Engine Engine => _engine;

    public void Dispose()
    {
        DestroyDevice(null);
    }

    public static LogicalDevice CreateLogicalDevice(Engine engine, PhysicalDevice physicalDevice, uint graphicsQueueIndex, uint presentQueueIndex, IPlatform platform)
    {
        HashSet<uint> uniqueQueueFamilies = [graphicsQueueIndex, presentQueueIndex];
        DeviceQueueCreateInfo* queueCreateInfos = stackalloc DeviceQueueCreateInfo[uniqueQueueFamilies.Count];

        float priority = 1f;
        FillQueueCreateInfos(uniqueQueueFamilies, queueCreateInfos, &priority);

        PhysicalDeviceFeatures deviceFeatures = physicalDevice.Features;

        List<string> activeExtensions = CollectActiveExtensions(physicalDevice, platform);

        IntPtr activeExtensionsPtr = SilkMarshal.StringArrayToPtr(activeExtensions);
        DeviceCreateInfo deviceCreateInfo = new DeviceCreateInfo
        {
            SType = StructureType.DeviceCreateInfo,
            QueueCreateInfoCount = (uint)uniqueQueueFamilies.Count,
            PQueueCreateInfos = queueCreateInfos,
            PEnabledFeatures = &deviceFeatures,
            EnabledExtensionCount = (uint)activeExtensions.Count,
            PpEnabledExtensionNames = (byte**)activeExtensionsPtr,
        };

        Result result = physicalDevice.CreateDevice(in deviceCreateInfo, null, out Device device);

        Helpers.CheckErrors(result);

        LogicalDevice logicalDevice = new LogicalDevice(engine, physicalDevice, device);

        SilkMarshal.Free(activeExtensionsPtr);

        if (!logicalDevice.TryGetDeviceExtension(out logicalDevice.KhrSwapchain))
        {
            throw new NotSupportedException($"{KhrSwapchain.ExtensionName} extension not found.");
        }

        return logicalDevice;
    }

    public void GetSwapchainImages(SwapchainKHR deviceSwapchain, ref Silk.NET.Vulkan.Image[] images)
    {
        uint scImageCount = 0;
        Result result = GetSwapchainImages(deviceSwapchain, ref scImageCount, null);
        Helpers.CheckErrors(result);
        ArrayExtensions.EnsureArrayMinimumSize(ref images, scImageCount);
        result = GetSwapchainImages(deviceSwapchain, ref scImageCount, out images[0]);
        Helpers.CheckErrors(result);
    }

    static List<string> CollectActiveExtensions(PhysicalDevice physicalDevice, IPlatform platform)
    {
        List<string> requiredInstanceExtensions = [KhrSwapchain.ExtensionName, ..platform.DeviceExtensions];

        if (!requiredInstanceExtensions.All(physicalDevice.ExtensionProperties.ContainsKey))
        {
            IEnumerable<string> missingList = requiredInstanceExtensions.Except(physicalDevice.ExtensionProperties.Keys);
            throw new Exception(
                $"The following Vulkan device extensions were not available: {string.Join(", ", missingList)}");
        }

        return requiredInstanceExtensions;
    }

    static void FillQueueCreateInfos(HashSet<uint> uniqueQueueFamilies, DeviceQueueCreateInfo* queueCreateInfos, float* priority)
    {
        int i = 0;
        foreach (uint index in uniqueQueueFamilies)
        {
            queueCreateInfos[i] = new DeviceQueueCreateInfo
            {
                SType = StructureType.DeviceQueueCreateInfo,
                QueueFamilyIndex = index,
                QueueCount = 1,
                PQueuePriorities = priority,
            };
            i += 1;
        }
    }

    public Queue GetDeviceQueue(uint queueFamilyIndex, uint queueIndex)
    {
        GetDeviceQueue(queueFamilyIndex, queueIndex, out Silk.NET.Vulkan.Queue queue);
        return Queue.Create(queue, KhrSwapchain, queueFamilyIndex, queueIndex);
    }

    public Filter GetFormatFilter(Format format)
    {
        return _filters.GetOrAdd(format, GetFormatInternal);

        Filter GetFormatInternal(Format requestedFormat)
        {
            _physicalDevice.GetPhysicalDeviceFormatProperties(requestedFormat, out FormatProperties formatProperties);
            return formatProperties.OptimalTilingFeatures.HasFlag(FormatFeatureFlags.SampledImageFilterLinearBit)
                ? Filter.Linear
                : Filter.Nearest;
        }
    }

    public void SetObjectName(ObjectType type, ulong target, string name)
    {
        _engine.Debug?.SetObjectName(_device, type, target, name);
    }
}

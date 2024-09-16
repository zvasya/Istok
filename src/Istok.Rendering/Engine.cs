using Silk.NET.Vulkan.Extensions.Helpers;
using Silk.NET.Core.Native;
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.EXT;
using Silk.NET.Vulkan.Extensions.KHR;
using Silk.NET.Vulkan.Extensions.Missed;
using Silk.NET.Windowing;

namespace Istok.Rendering;

public unsafe partial class Engine : IDisposable
{
    const string EngineName = "Istok";
    internal static Vk VK = null!;

    static KhrSurface _khrSurface = null!;
    readonly HashSet<string> _extensions;

    readonly Instance _instance;
    readonly IPlatform _platform;
    readonly HashSet<string> _validationLayers;


    Engine(IPlatform platform, Instance instance, IEnumerable<string> validationLayers, IEnumerable<string> requestedExtension)
    {
        _platform = platform;
        _instance = instance;
        _validationLayers = new HashSet<string>(validationLayers);
        _extensions = new HashSet<string>(requestedExtension);

        VK.CurrentInstance = _instance;
    }

    internal static KhrSurface KhrSurfaceExtension => _khrSurface;

    public DebugUtils? Debug { get; private set; }

    public IPlatform Platform => _platform;
    public Instance Instance => _instance;


    public void Dispose()
    {
        Debug?.Dispose();
        DestroyInstance(null);
    }


    static HashSet<string> GetAvailableLayers()
    {
        uint layerCount = 0u;
        VK.EnumerateInstanceLayerProperties(&layerCount, null);

        LayerProperties[] availableLayers = new LayerProperties[layerCount];
        if (layerCount > 0)
            VK.EnumerateInstanceLayerProperties(&layerCount, ref availableLayers[0]);

        return new HashSet<string>(availableLayers.Select(LayerPropertiesExt.GetName));
    }

    public bool HasLayer(string name)
    {
        return _validationLayers.Contains(name);
    }

    public bool HasExtension(string name)
    {
        return _extensions.Contains(name);
    }

    static HashSet<string> GetAvailableExtensions()
    {
        uint availableExtensionCount = 0;
        VK.EnumerateInstanceExtensionProperties((string)null!, ref availableExtensionCount, null);

        ExtensionProperties[] availableExtensionProperties = new ExtensionProperties[availableExtensionCount];
        VK.EnumerateInstanceExtensionProperties((string)null!, &availableExtensionCount, ref availableExtensionProperties[0]);

        return new HashSet<string>(availableExtensionProperties.Select(ExtensionPropertiesExt.GetName));
    }

    public static Engine Create(IPlatform platform, string applicationName)
    {
        VK = Vk.GetApi();

        HashSet<string> availableLayerNames = GetAvailableLayers();

        string[]? validationLayers = platform.ValidationLayerNamesPriorityList.FirstOrDefault(validationLayerNameSet => validationLayerNameSet.All(availableLayerNames.Contains));

        if (validationLayers is null)
        {
            throw new NotSupportedException("Validation layers requested, but not available!");
        }

        ApplicationInfo appInfo = new ApplicationInfo
        {
            SType = StructureType.ApplicationInfo,
            PApplicationName = MarshaledStringRegistry.Get(applicationName),
            ApplicationVersion = Vk.Version10,
            PEngineName = MarshaledStringRegistry.Get(EngineName),
            EngineVersion = Vk.Version10,
            ApiVersion = Vk.Version11,
        };

        HashSet<string> availableInstanceExtensions = GetAvailableExtensions();

        if (!platform.RequiredInstanceExtensions.All(availableInstanceExtensions.Contains))
        {
            throw new NotSupportedException("Extensions requested, but not available!");
        }

        string[] platformRequestedExtension = platform.RequiredInstanceExtensions;

        bool portability = platformRequestedExtension.Contains(KhrPortabilityEnumeration.ExtensionName);

        List<string> requestedExtension = new List<string>(platformRequestedExtension);

        IntPtr instanceLayersPtr = validationLayers.Length > 0 ? SilkMarshal.StringArrayToPtr(validationLayers) : IntPtr.Zero;
        IntPtr instanceExtensionsPtr = SilkMarshal.StringArrayToPtr(requestedExtension);

        InstanceCreateInfo instanceCI = new InstanceCreateInfo
        {
            SType = StructureType.InstanceCreateInfo,
            PApplicationInfo = &appInfo,
            EnabledExtensionCount = (uint)requestedExtension.Count,
            PpEnabledExtensionNames = (byte**)instanceExtensionsPtr,
            EnabledLayerCount = (uint)validationLayers.Length,
            PpEnabledLayerNames = (byte**)instanceLayersPtr,
            Flags = portability ? InstanceCreateFlags.EnumeratePortabilityBitKhr : InstanceCreateFlags.None,
        };

        if (VK.CreateInstance(&instanceCI, null, out Instance instance) != Result.Success)
        {
            throw new Exception("Failed to create instance!");
        }

        Engine engine = new Engine(platform, instance, validationLayers, requestedExtension);
        SilkMarshal.Free(instanceExtensionsPtr);

        if (instanceLayersPtr != IntPtr.Zero)
            SilkMarshal.Free(instanceLayersPtr);

        if (!engine.TryGetInstanceExtension(out _khrSurface))
        {
            engine.Dispose();
            throw new NotSupportedException($"{KhrSurface.ExtensionName} extension not found.");
        }

        if (platform.Debug && requestedExtension.Contains(ExtDebugUtils.ExtensionName))
        {
            engine.Debug = DebugUtils.Create(engine);
        }

        return engine;
    }

    public SurfaceKHR CreateSurface(IView view)
    {
        return view.VkSurface!.Create<AllocationCallbacks>(_instance.ToHandle(), null).ToSurface();
    }


    public PhysicalDevice[] GetPhysicalDevices()
    {
        uint deviceCount = 0;
        Helpers.CheckErrors(EnumeratePhysicalDevices(&deviceCount, null));
        if (deviceCount == 0)
        {
            throw new Exception("Failed to find GPUs with Vulkan support!");
        }

        Silk.NET.Vulkan.PhysicalDevice* devices = stackalloc Silk.NET.Vulkan.PhysicalDevice[(int)deviceCount];
        Helpers.CheckErrors(EnumeratePhysicalDevices(&deviceCount, devices));

        PhysicalDevice[] gpus = new PhysicalDevice[deviceCount];
        for (int i = 0; i < deviceCount; i++)
        {
            gpus[i] = new PhysicalDevice(devices[i], this);
        }

        return gpus;
    }
}

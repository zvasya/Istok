#define BREAK_ON_ERROR
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Silk.NET.Core.Native;
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.EXT;

namespace Istok.Rendering;

public unsafe class DebugUtils : IDisposable
{
    readonly DebugUtilsMessengerEXT? _debugMessenger;
    readonly ExtDebugUtils _debugUtils;
    readonly Engine _engine;

    DebugUtils(Engine engine, ExtDebugUtils debugUtils, DebugUtilsMessengerEXT? debugMessenger)
    {
        _engine = engine;
        _debugUtils = debugUtils;
        _debugMessenger = debugMessenger;
    }


    public void Dispose()
    {
        if (_debugMessenger.HasValue)
            _debugUtils.DestroyDebugUtilsMessenger(_engine.Instance, _debugMessenger.Value, null);
    }

    public static DebugUtils Create(Engine engine)
    {
        if (!engine.TryGetInstanceExtension(out ExtDebugUtils debugUtils))
        {
            return null;
        }

        if (!RuntimeFeature.IsDynamicCodeSupported)
        {
            Console.WriteLine("AOT Runtime does not support DebugUtilsMessenger for now");
            return new DebugUtils(engine, debugUtils, null);
        }

        DebugUtilsMessengerCreateInfoEXT createInfo = new DebugUtilsMessengerCreateInfoEXT
        {
            SType = StructureType.DebugUtilsMessengerCreateInfoExt,
            MessageSeverity = DebugUtilsMessageSeverityFlagsEXT.VerboseBitExt |
                              DebugUtilsMessageSeverityFlagsEXT.WarningBitExt |
                              DebugUtilsMessageSeverityFlagsEXT.ErrorBitExt,
            MessageType = DebugUtilsMessageTypeFlagsEXT.GeneralBitExt |
                          DebugUtilsMessageTypeFlagsEXT.PerformanceBitExt |
                          DebugUtilsMessageTypeFlagsEXT.ValidationBitExt,
            PfnUserCallback = new PfnDebugUtilsMessengerCallbackEXT(DebugCallback),
        };


        if (debugUtils.CreateDebugUtilsMessenger(engine.Instance, &createInfo, null, out DebugUtilsMessengerEXT debugMessenger) != Result.Success)
        {
            throw new Exception("Failed to create debug messenger.");
        }

        DebugUtils debug = new DebugUtils(engine, debugUtils, debugMessenger);

        return debug;
    }


    static uint DebugCallback(DebugUtilsMessageSeverityFlagsEXT messageSeverity, DebugUtilsMessageTypeFlagsEXT messageTypes, DebugUtilsMessengerCallbackDataEXT* pCallbackData, void* pUserData)
    {
        if (messageSeverity > DebugUtilsMessageSeverityFlagsEXT.VerboseBitExt)
        {
            string message = Marshal.PtrToStringAnsi((nint)pCallbackData->PMessage) ?? string.Empty;
            Console.WriteLine($"{messageSeverity} {messageTypes} {message}");
#if DEBUG && BREAK_ON_ERROR
            if (messageSeverity.HasFlag(DebugUtilsMessageSeverityFlagsEXT.ErrorBitExt))
            {
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    System.Diagnostics.Debugger.Break();
                }
            }
#endif
        }

        return Vk.False;
    }

    public void SetObjectName(Device device, ObjectType type, ulong target, string name)
    {
        IntPtr namePtr = SilkMarshal.StringToPtr(name);
        DebugUtilsObjectNameInfoEXT nameInfo = new DebugUtilsObjectNameInfoEXT
        {
            SType = StructureType.DebugUtilsObjectNameInfoExt, ObjectType = type, ObjectHandle = target, PObjectName = (byte*)namePtr,
        };

        Result result = _debugUtils.SetDebugUtilsObjectName(device, in nameInfo);
        SilkMarshal.Free(namePtr);

        if (result != Result.Success)
        {
            throw new Exception("Fail to set debug utils object name. Result: " + result);
        }
    }

    public void BeginLabel(CommandBuffer cb, string name)
    {
        IntPtr namePtr = SilkMarshal.StringToPtr(name);
        DebugUtilsLabelEXT markerInfo = new DebugUtilsLabelEXT { SType = StructureType.DebugUtilsLabelExt, PLabelName = (byte*)namePtr };
        _debugUtils.CmdBeginDebugUtilsLabel(cb.Buffer, in markerInfo);
        SilkMarshal.Free(namePtr);
    }

    public void EndLabel(CommandBuffer cb)
    {
        _debugUtils.CmdEndDebugUtilsLabel(cb.Buffer);
    }

    public void InsertLabel(CommandBuffer cb, string name)
    {
        IntPtr namePtr = SilkMarshal.StringToPtr(name);
        DebugUtilsLabelEXT markerInfo = new DebugUtilsLabelEXT { SType = StructureType.DebugUtilsLabelExt, PLabelName = (byte*)namePtr };
        _debugUtils.CmdInsertDebugUtilsLabel(cb.Buffer, in markerInfo);
        SilkMarshal.Free(namePtr);
    }
}

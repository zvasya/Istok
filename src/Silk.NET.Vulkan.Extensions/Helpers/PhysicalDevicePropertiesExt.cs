using System;
using Silk.NET.Core.Native;
using Silk.NET.Vulkan;

namespace Silk.NET.Vulkan.Extensions.Helpers;

public static class PhysicalDevicePropertiesExt
{
    public static unsafe string GetDeviceName(this PhysicalDeviceProperties properties) => SilkMarshal.PtrToString((IntPtr)properties.DeviceName, NativeStringEncoding.LPTStr) ?? throw new Exception("Can't get DeviceName");
}

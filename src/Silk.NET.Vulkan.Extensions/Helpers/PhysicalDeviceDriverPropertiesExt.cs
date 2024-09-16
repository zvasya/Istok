using System;
using Silk.NET.Core.Native;
using Silk.NET.Vulkan;

namespace Silk.NET.Vulkan.Extensions.Helpers;

public static class PhysicalDeviceDriverPropertiesExt
{
    public static unsafe string GetDriverName(this PhysicalDeviceDriverProperties properties) => SilkMarshal.PtrToString((IntPtr)properties.DriverName, NativeStringEncoding.LPTStr) ?? throw new Exception("Can't get DriverName");
    public static unsafe string GetDriverInfo(this PhysicalDeviceDriverProperties properties) => SilkMarshal.PtrToString((IntPtr)properties.DriverInfo, NativeStringEncoding.LPTStr) ?? throw new Exception("Can't get DriverInfo");
}

using System;
using Silk.NET.Core.Native;
using Silk.NET.Vulkan;

namespace Silk.NET.Vulkan.Extensions.Helpers;

public static class ExtensionPropertiesExt
{
    public static unsafe string GetName(this ExtensionProperties property) => SilkMarshal.PtrToString((IntPtr)property.ExtensionName, NativeStringEncoding.LPTStr) ?? throw new Exception("Can't get ExtensionName");
}

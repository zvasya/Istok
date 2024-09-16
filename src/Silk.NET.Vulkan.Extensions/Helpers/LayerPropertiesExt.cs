using System;
using Silk.NET.Core.Native;
using Silk.NET.Vulkan;

namespace Silk.NET.Vulkan.Extensions.Helpers;

public static class LayerPropertiesExt
{
    public static unsafe string GetName(this LayerProperties property) => SilkMarshal.PtrToString((IntPtr)property.LayerName, NativeStringEncoding.LPTStr) ?? throw new Exception("Can't get LayerName");
}

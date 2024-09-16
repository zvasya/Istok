using Silk.NET.Core.Native;

namespace Istok.Rendering;

public static unsafe class MarshaledStringRegistry
{
    static readonly Dictionary<string, IntPtr> Cache = new Dictionary<string, IntPtr>();

    public static byte* Get(string str)
    {
        if (!Cache.TryGetValue(str, out IntPtr ptr))
        {
            Cache.Add(str, ptr = SilkMarshal.StringToPtr(str));
        }

        return (byte*)ptr;
    }
}

namespace Istok.Rendering.Platforms;

public static class Utils
{
#if DEBUG
    static bool Debug => true;
#else
    static bool Debug => false;
#endif

    public static IPlatform SelectPlatform()
    {
        IPlatform platform = null;
        if (OperatingSystem.IsAndroid())
            platform = Debug ? new AndroidDebugPlatform() : new AndroidPlatform();
        else if (OperatingSystem.IsIOS())
            platform = Debug ? new iOSDebugPlatform() : new iOSPlatform();
        else if (OperatingSystem.IsMacCatalyst())
            platform = Debug ? new iOSDebugPlatform() : new iOSPlatform();
        else if (OperatingSystem.IsMacOS())
            platform = Debug ? new MacDebugPlatform() : new MacPlatform();
        else if (OperatingSystem.IsWindows())
            platform = Debug ? new WindowsDebugPlatform() : new WindowsPlatform();
        return platform;
    }
}

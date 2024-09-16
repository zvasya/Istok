using Silk.NET.Windowing;
using Silk.NET.Windowing.Sdl.Android;
using Android.Content;
using Android.Views;
using Java.Interop;
using System.Runtime.InteropServices;
using Android.Content.PM;
using Android.Graphics;
using Examples;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Xamarin.Essentials;


namespace AndroidExamplesRunner;

[Activity(Label = "@string/app_name", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
public class MainActivity : SilkActivity
{

    protected override void OnRun()
    {
        const bool colorSrgb = true;

        Silk.NET.Vulkan.Vk.GetApi();

        IView window = Silk.NET.Windowing.Window.GetView(ViewOptions.DefaultVulkan); // note also GetView, instead of Window.Create.
        window.Initialize();

        // new Examples.TriangleExample(window, colorSrgb, Load).Run();
        // new Examples.QuadExample(window, colorSrgb, Load).Run();
        // new Examples.TexturedQuadExample(window, colorSrgb, Load).Run();
        // new Examples.UniformBufferExample(window, colorSrgb, Load).Run();
        // new Examples.SimpleCameraAndRendererComponentsExample(window, colorSrgb, Load).Run();
        // new Examples.GltfExample(window, colorSrgb, Load).Run();
        // new Examples.GltfExampleAnimation(window, colorSrgb, Load).Run();
        new Examples.GltfSkinnedMeshExample(window, colorSrgb, Load).Run();

        window.Dispose();
    }

    static byte[] Load(string path)
    {
        using Stream? stream = FileSystem.OpenAppPackageFileAsync(path).Result;
        using MemoryStream memStream = new MemoryStream();
        stream.CopyTo(memStream);
        return memStream.ToArray();
    }
}

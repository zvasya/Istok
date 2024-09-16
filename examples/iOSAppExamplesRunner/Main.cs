using Istok;
using Istok.Rendering;
using Istok.Rendering.Platforms;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Sdl.iOS;

// This is the main entry point of the application.
// If you want to use a different Application Delegate class from "AppDelegate"
// you can specify it here.
SilkMobile.RunApp([], strings =>
{
    const bool colorSrgb = true;

    Silk.NET.Vulkan.Vk.GetApi();
    Window.PrioritizeSdl();
    IView window = Window.GetView(ViewOptions.DefaultVulkan);
    window.Initialize();

    new Examples.GltfSkinnedMeshExample(window, colorSrgb, File.ReadAllBytes).Run();
});

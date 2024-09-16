using Silk.NET.Maths;
using Silk.NET.Windowing;
using Silk.NET.Vulkan;

const bool colorSrgb = true;

WindowOptions options = WindowOptions.DefaultVulkan with
{
    Position = new Vector2D<int>(50, 50),
    Size = new Vector2D<int>(960, 540),
    WindowState = WindowState.Normal,
    Title = "Example",
};

Vk.GetApi();
IWindow window = Window.Create(options);
window.Initialize();

// new Examples.TriangleExample(window, colorSrgb, File.ReadAllBytes).Run();
// new Examples.QuadExample(window, colorSrgb, File.ReadAllBytes).Run();
// new Examples.TexturedQuadExample(window, colorSrgb, File.ReadAllBytes).Run();
// new Examples.UniformBufferExample(window, colorSrgb, File.ReadAllBytes).Run();
// new Examples.SimpleCameraAndRendererComponentsExample(window, colorSrgb, File.ReadAllBytes).Run();
// new Examples.GltfExample(window, colorSrgb, File.ReadAllBytes).Run();
// new Examples.GltfExampleAnimation(window, colorSrgb, File.ReadAllBytes).Run();
new Examples.GltfSkinnedMeshExample(window, colorSrgb, File.ReadAllBytes).Run();

window.Dispose();

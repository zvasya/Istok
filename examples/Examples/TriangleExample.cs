using System.Numerics;
using System.Runtime.CompilerServices;
using Istok;
using Istok.Rendering;
using Istok.Rendering.Platforms;
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.Helpers;
using Silk.NET.Windowing;
using Buffer = Istok.Rendering.Buffer;
using Pipeline = Istok.Rendering.Pipeline;

namespace Examples;

public class TriangleExample
{
    readonly IView _window;
    readonly Graphics _graphics;
    readonly CommandList _commandList;
    readonly Shader _vertexShader;
    readonly Shader _fragmentShader;
    readonly Pipeline _pipeline;

    bool _windowResized;

    readonly Func<string, byte[]> _getBytes;

    public TriangleExample(IView window, bool colorSrgb, Func<string, byte[]> getBytes)
    {
        _getBytes = getBytes;
        _window = window;

        _graphics = new Graphics(Utils.SelectPlatform(), new Swapchain.Description(window, null, false, colorSrgb));
        _commandList = _graphics.CreateCommandList();
        _commandList.Name = "Frame Commands List";

        _vertexShader = LoadShader(_graphics, "Shaders/triangle_example_vert.spv", ShaderStageFlags.VertexBit);
        _fragmentShader = LoadShader(_graphics, "Shaders/triangle_example_frag.spv", ShaderStageFlags.FragmentBit);

        GraphicsPipelineDescription pd = new GraphicsPipelineDescription(
            BlendStateDescription.SingleOverrideBlend,
            PipelineDepthStencilStateCreateInfoExt.Disabled,
            PipelineRasterizationStateCreateInfoExt.Default,
            PrimitiveTopology.TriangleList,
            new ShaderSetDescription([], [_vertexShader, _fragmentShader]),
            _graphics.MainSwapchain?.Framebuffer?.RenderPass);

        _pipeline = _graphics.CreateGraphicsPipeline(in pd);
    }

    public void Run()
    {
        _window.Render += Draw;
        _window.Resize += _ => _windowResized = true;
        _window.Closing += OnClosing;
        _window.Run();
    }


    Shader LoadShader(Graphics graphics, string name, ShaderStageFlags shaderStage)
    {
        byte[] shaderBytes = _getBytes(name);
        ShaderDescription shaderDescription = new ShaderDescription(shaderStage, shaderBytes, "main");
        return graphics.CreateShader(in shaderDescription);
    }

    unsafe void Draw(double _)
    {
        if (_windowResized)
        {
            _windowResized = false;
            _graphics.ResizeMainWindow();
        }

        using (_commandList.Begin())
        {
            using (_commandList.SetFramebuffer(_graphics.MainSwapchain!.Framebuffer!.CurrentFramebuffer))
            {
                using (_commandList.BeginRenderPass())
                {
                    _commandList.SetPipeline(_pipeline);
                    _commandList.ClearColorTarget(0, Color.Black);
                    _commandList.Draw(3);
                }
            }
        }

        _commandList.SubmitCommandList(0, null, 0, null);

        _graphics.SwapBuffers();
    }

    void OnClosing()
    {
        _graphics.WaitForIdle();
        _pipeline.Dispose();
        _vertexShader.Dispose();
        _fragmentShader.Dispose();
        _commandList.Dispose();
        _graphics.Dispose();
    }
}

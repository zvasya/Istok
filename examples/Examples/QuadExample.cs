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

public class QuadExample
{
    readonly IView _window;
    readonly Graphics _graphics;
    readonly CommandList _commandList;
    readonly Shader _vertexShader;
    readonly Shader _fragmentShader;
    readonly Pipeline _pipeline;

    readonly Buffer _vertexBuffer;
    readonly Buffer _indexBuffer;

    bool _windowResized;

    readonly Func<string, byte[]> _getBytes;

    public struct Vertex
    {
        public Vector3 Pos;
        public Vector3 Color;
    }

    public QuadExample(IView window, bool colorSrgb, Func<string, byte[]> getBytes)
    {
        _getBytes = getBytes;
        _window = window;

        _graphics = new Graphics(Utils.SelectPlatform(), new Swapchain.Description(window, null, false, colorSrgb));
        _commandList = _graphics.CreateCommandList();
        _commandList.Name = "Frame Commands List";

        _vertexShader = LoadShader(_graphics, "Shaders/quad_example_vert.spv", ShaderStageFlags.VertexBit);
        _fragmentShader = LoadShader(_graphics, "Shaders/quad_example_frag.spv", ShaderStageFlags.FragmentBit);

        //        Y
        //  (2)      ^     (3)
        // -1, 1     │     1, 1
        //     ┌┄┄┄┄┄+┄┄┄┄┄┐
        //     ┆     │   ╱ ┆
        //     ┆     │ ╱   ┆
        //   ──+─────┼─────+──> X
        //     ┆   ╱ │     ┆
        //     ┆ ╱   │     ┆
        //     └┄┄┄┄┄+┄┄┄┄┄┘
        // -1,-1     │     1,-1
        //  (0)            (1)

        Vertex[] vertices =
        [
            new Vertex { Pos = new Vector3(-0.5f, -0.5f, 0.0f), Color = new Vector3(1.0f, 0.0f, 0.0f) },
            new Vertex { Pos = new Vector3( 0.5f, -0.5f, 0.0f), Color = new Vector3(1.0f, 1.0f, 1.0f) },
            new Vertex { Pos = new Vector3(-0.5f,  0.5f, 0.0f), Color = new Vector3(0.0f, 0.0f, 1.0f) },
            new Vertex { Pos = new Vector3( 0.5f,  0.5f, 0.0f), Color = new Vector3(0.0f, 1.0f, 0.0f) },
        ];
        ushort[] indices = [0, 1, 3, 0, 3, 2];

        _vertexBuffer = _graphics.CreateBuffer(BufferDescription.Vertex((uint)(vertices.Length * Unsafe.SizeOf<Vertex>())));
        _vertexBuffer.Fill(0, new ReadOnlySpan<Vertex>(vertices));

        _indexBuffer = _graphics.CreateBuffer(BufferDescription.Index((uint)(indices.Length * Unsafe.SizeOf<ushort>())));
        _indexBuffer.Fill(0, new ReadOnlySpan<ushort>(indices));

        GraphicsPipelineDescription pd = new GraphicsPipelineDescription(
            BlendStateDescription.SingleOverrideBlend,
            PipelineDepthStencilStateCreateInfoExt.Disabled,
            PipelineRasterizationStateCreateInfoExt.Default,
            PrimitiveTopology.TriangleList,
            new ShaderSetDescription([
                new VertexLayoutDescription(
                    new VertexElementDescription("Position", Format.R32G32B32Sfloat),
                    new VertexElementDescription("Color", Format.R32G32B32Sfloat)),
            ], [_vertexShader, _fragmentShader]),
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
                    _commandList.SetVertexBuffer(0, _vertexBuffer);
                    _commandList.SetIndexBuffer(_indexBuffer, IndexType.Uint16);
                    _commandList.DrawIndexed(6, 1, 0, 0, 0);
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
        _vertexBuffer.Dispose();
        _indexBuffer.Dispose();
        _vertexShader.Dispose();
        _fragmentShader.Dispose();
        _commandList.Dispose();
        _graphics.Dispose();
    }
}

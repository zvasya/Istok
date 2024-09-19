using System.Numerics;
using System.Runtime.CompilerServices;
using Istok;
using Istok.Core;
using Istok.Rendering;
using Istok.Rendering.Platforms;
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.Helpers;
using Silk.NET.Windowing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Buffer = Istok.Rendering.Buffer;
using Color = Istok.Color;
using DescriptorSetLayout = Istok.Rendering.DescriptorSetLayout;
using Image = Istok.Rendering.Image;
using ImageView = Istok.Rendering.ImageView;
using Pipeline = Istok.Rendering.Pipeline;
using ShaderModule = Istok.Rendering.ShaderModule;

namespace Examples;

public class UniformBufferExample
{
    readonly IView _window;
    readonly Graphics _graphics;
    readonly CommandList _commandList;
    readonly ShaderModule _vertexShader;
    readonly ShaderModule _fragmentShader;
    readonly Pipeline _pipeline;

    readonly Buffer _vertexBuffer;
    readonly Buffer _indexBuffer;
    readonly DescriptorSetLayout _textureDescriptorSetLayout;
    readonly ResourceSet _textureResourceSet;
    readonly Image _image;
    readonly ImageView _imageView;
    readonly DescriptorSetLayout _uniformBufferDescriptorSetLayout;
    readonly ResourceSet _uniformBufferResourceSet;
    readonly Buffer _uniformBuffer;

    bool _windowResized;

    readonly Func<string, byte[]> _getBytes;

    public struct Vertex
    {
        public Vector3 Pos;
        public Vector3 Color;
        public Vector2 UV;
    }

    public struct UniformBuffer
    {
        public Matrix4x4 Model;
        public Matrix4x4 View;
        public Matrix4x4 Proj;
    }

    public UniformBufferExample(IView window, bool colorSrgb, Func<string, byte[]> getBytes)
    {
        _getBytes = getBytes;
        _window = window;

        _graphics = new Graphics(Utils.SelectPlatform(), new Swapchain.Description(window, null, false, colorSrgb));
        _commandList = _graphics.CreateCommandList();
        _commandList.Name = "Frame Commands List";

        _vertexShader = LoadShader(_graphics, "Shaders/uniform_buffer_example_vert.spv", ShaderStageFlags.VertexBit);
        _fragmentShader = LoadShader(_graphics, "Shaders/uniform_buffer_example_frag.spv", ShaderStageFlags.FragmentBit);

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
            new Vertex { Pos = new Vector3(-0.5f, -0.5f, 0.0f), Color = new Vector3(1.0f, 1.0f, 1.0f), UV = new Vector2(0.0f, 1.0f)},
            new Vertex { Pos = new Vector3( 0.5f, -0.5f, 0.0f), Color = new Vector3(1.0f, 1.0f, 1.0f), UV = new Vector2(1.0f, 1.0f) },
            new Vertex { Pos = new Vector3(-0.5f,  0.5f, 0.0f), Color = new Vector3(1.0f, 1.0f, 1.0f), UV = new Vector2(0.0f, 0.0f) },
            new Vertex { Pos = new Vector3( 0.5f,  0.5f, 0.0f), Color = new Vector3(1.0f, 1.0f, 1.0f), UV = new Vector2(1.0f, 0.0f) },
        ];
        ushort[] indices = [0, 1, 3, 0, 3, 2];

        _vertexBuffer = _graphics.CreateBuffer(BufferDescription.Vertex((uint)(vertices.Length * Unsafe.SizeOf<Vertex>())));
        _vertexBuffer.Fill(0, new ReadOnlySpan<Vertex>(vertices));

        _indexBuffer = _graphics.CreateBuffer(BufferDescription.Index((uint)(indices.Length * Unsafe.SizeOf<ushort>())));
        _indexBuffer.Fill(0, new ReadOnlySpan<ushort>(indices));

        _uniformBufferDescriptorSetLayout = _graphics.CreateResourceLayout(new DescriptorSetLayoutBindings(
        new DescriptorSetLayoutBindingEntry("ModelViewProjection", DescriptorType.UniformBuffer, ShaderStageFlags.VertexBit)));

        _uniformBuffer = _graphics.CreateBuffer(BufferDescription.Uniform((uint)Unsafe.SizeOf<UniformBuffer>()));

        _uniformBufferResourceSet = _graphics.CreateResourceSet(new ResourceSetDescription(_uniformBufferDescriptorSetLayout, _uniformBuffer.GetResourcesSetBound()));

        _textureDescriptorSetLayout = _graphics.CreateResourceLayout(
            new DescriptorSetLayoutBindings(
                new DescriptorSetLayoutBindingEntry("SourceTexture", DescriptorType.CombinedImageSampler, ShaderStageFlags.FragmentBit)));

        using (Image<Rgba32>? img = SixLabors.ImageSharp.Image.Load<Rgba32>(_getBytes("Textures/dungeon_room.png")))
        {
            _image = _graphics.CreateImage(ImageCreateInfoExt.Texture2D((uint)img.Width, (uint)img.Height, 1, 1, Format.R8G8B8A8Srgb, ImageUsageFlags.SampledBit | ImageUsageFlags.TransferDstBit | ImageUsageFlags.TransferSrcBit));
            byte[] data = new byte[Format.R8G8B8A8Unorm.GetRegionSize((uint)img.Width, (uint)img.Height, 1)];
            img.CopyPixelDataTo(data);

            _image.Fill( new ReadOnlySpan<byte>(data));
        }

        _imageView = _graphics.CreateImageView(_image);

        _textureResourceSet = _graphics.CreateResourceSet(new ResourceSetDescription(_textureDescriptorSetLayout, new ResourcesSetBindingCombinedImageSampler(_imageView, _graphics.LinearSampler)));

        GraphicsPipelineDescription pd = new GraphicsPipelineDescription(
            BlendStateDescription.SingleOverrideBlend,
            PipelineDepthStencilStateCreateInfoExt.Disabled,
            PipelineRasterizationStateCreateInfoExt.Default,
            PrimitiveTopology.TriangleList,
            new ShaderSetDescription([
                new VertexLayoutDescription(
                    new VertexElementDescription("Position", Format.R32G32B32Sfloat),
                    new VertexElementDescription("Color", Format.R32G32B32Sfloat),
                    new VertexElementDescription("TexCoords", Format.R32G32Sfloat)
                ),
            ], [_vertexShader, _fragmentShader]),
            _graphics.MainSwapchain?.Framebuffer?.RenderPass,
            _uniformBufferDescriptorSetLayout,
                _textureDescriptorSetLayout);

        _pipeline = _graphics.CreateGraphicsPipeline(in pd);
    }

    public void Run()
    {
        _window.Render += Draw;
        _window.Resize += _ => _windowResized = true;
        _window.Closing += OnClosing;
        _window.Run();
    }


    ShaderModule LoadShader(Graphics graphics, string name, ShaderStageFlags shaderStage)
    {
        byte[] shaderBytes = _getBytes(name);
        ShaderDescription shaderDescription = new ShaderDescription(shaderStage, shaderBytes, "main");
        return graphics.CreateShaderModule(in shaderDescription);
    }

    unsafe void Draw(double deltaTime)
    {
        if (_windowResized)
        {
            _windowResized = false;
            _graphics.ResizeMainWindow();
        }

        using (_commandList.Begin())
        {
            UniformBuffer uniformBuffer = UpdateUniformBuffer(deltaTime);
            _commandList.UpdateBuffer(_uniformBuffer, 0, uniformBuffer);

            using (_commandList.SetFramebuffer(_graphics.MainSwapchain!.Framebuffer!.CurrentFramebuffer))
            {
                using (_commandList.BeginRenderPass())
                {
                    _commandList.SetPipeline(_pipeline);
                    _commandList.ClearColorTarget(0, Color.Black);
                    _commandList.SetGraphicsResourceSet(0, _uniformBufferResourceSet, Span<uint>.Empty);
                    _commandList.SetGraphicsResourceSet(1, _textureResourceSet, Span<uint>.Empty);
                    _commandList.SetVertexBuffer(0, _vertexBuffer);
                    _commandList.SetIndexBuffer(_indexBuffer, IndexType.Uint16);
                    _commandList.DrawIndexed(6, 1, 0, 0, 0);
                }
            }
        }

        _commandList.SubmitCommandList(0, null, 0, null);

        _graphics.SwapBuffers();
    }

    double p;
    UniformBuffer UpdateUniformBuffer(double deltaTime)
    {
        const double degToRad = Math.PI / 180.0;
        p = (p+deltaTime / 2) % 1;

        Vector3 objectScale = new Vector3(1, 1, 1);
        Quaternion objectRotation = Quaternion.CreateFromYawPitchRoll(0, (float)(Math.Sin(p * 2 * MathF.PI) * 60 * degToRad), 0);
        Vector3 objectPosition = new Vector3(1, 0, 0);
        Matrix4x4 objectPositionMatrix = Matrix4x4.CreateScale(objectScale)
                                         * Matrix4x4.CreateFromQuaternion(objectRotation)
                                         * Matrix4x4.CreateTranslation(objectPosition);

        Quaternion cameraRotation = Quaternion.CreateFromYawPitchRoll((float)(-25 * degToRad), 0, 0);
        Vector3 cameraPosition = new Vector3(0, 0, 2f);
        Matrix4x4 cameraPositionMatrix = Matrix4x4.CreateFromQuaternion(cameraRotation)
                                         * Matrix4x4.CreateTranslation(cameraPosition);
        Matrix4x4.Invert(cameraPositionMatrix, out Matrix4x4 cameraViewMatrix);

        float width = _window.Size.X;
        float height = _window.Size.Y;
        float farPlane = 1000f;
        float nearPlane = 0.1f;
        float fieldOfView = (float)(30 * degToRad);

        // Matrix4x4 proj = Matrix4x4.CreateOrthographic(width/height, 1f, nearPlane, farPlane);
        Matrix4x4 proj = Matrix4x4.CreatePerspectiveFieldOfView(fieldOfView, width / height, nearPlane, farPlane);

        UniformBuffer uniformBuffer = new UniformBuffer
        {
            Model = objectPositionMatrix,
            View = cameraViewMatrix,
            Proj = proj,
        };

        return uniformBuffer;
    }

    void OnClosing()
    {
        _graphics.WaitForIdle();
        _pipeline.Dispose();
        _vertexBuffer.Dispose();
        _indexBuffer.Dispose();
        _vertexShader.Dispose();
        _uniformBufferDescriptorSetLayout.Dispose();
        _uniformBufferResourceSet.Dispose();
        _uniformBuffer.Dispose();
        _textureResourceSet.Dispose();
        _image.Dispose();
        _imageView.Dispose();
        _textureDescriptorSetLayout.Dispose();
        _fragmentShader.Dispose();
        _commandList.Dispose();
        _graphics.Dispose();
    }
}

using System.Numerics;
using System.Runtime.CompilerServices;
using Istok;
using Istok.Core;
using Istok.Core.PlayerLoop;
using Istok.Core.PlayerLoopStages;
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

public class SimpleCameraAndRendererComponentsExample
{
    readonly IView _window;
    readonly Graphics _graphics;
    readonly CommandList _commandList;
    readonly ShaderModule _vertexShader;
    readonly ShaderModule _fragmentShader;
    readonly Pipeline _pipeline;

    readonly Mesh _mesh;
    readonly DescriptorSetLayout _textureDescriptorSetLayout;
    readonly ResourceSet _textureResourceSet;
    readonly Image _image;
    readonly ImageView _imageView;
    readonly DescriptorSetLayout _uniformBufferDescriptorSetLayout;

    bool _windowResized;


    readonly PlayerLoop _playerLoop;
    readonly Camera _camera;
    readonly List<SimpleRenderer> _renderers = [];

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

    public SimpleCameraAndRendererComponentsExample(IView window, bool colorSrgb, Func<string, byte[]> getBytes)
    {
        _getBytes = getBytes;
        _window = window;

        _graphics = new Graphics(Utils.SelectPlatform(), new Swapchain.Description(window, null, false, colorSrgb));
        _playerLoop = CreatePlayerLoop();

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
            new Vertex { Pos = new Vector3(-0.5f, -0.5f, 0.0f), Color = new Vector3(1.0f, 1.0f, 1.0f), UV = new Vector2(0.0f, 1.0f) },
            new Vertex { Pos = new Vector3(0.5f, -0.5f, 0.0f), Color = new Vector3(1.0f, 1.0f, 1.0f), UV = new Vector2(1.0f, 1.0f) },
            new Vertex { Pos = new Vector3(-0.5f, 0.5f, 0.0f), Color = new Vector3(1.0f, 1.0f, 1.0f), UV = new Vector2(0.0f, 0.0f) },
            new Vertex { Pos = new Vector3(0.5f, 0.5f, 0.0f), Color = new Vector3(1.0f, 1.0f, 1.0f), UV = new Vector2(1.0f, 0.0f) },
        ];
        ushort[] indices = [0, 1, 3, 0, 3, 2];


        Buffer vertexBuffer = _graphics.CreateBuffer(BufferDescription.Vertex((uint)(vertices.Length * Unsafe.SizeOf<Vertex>())));
        vertexBuffer.Fill(0, new ReadOnlySpan<Vertex>(vertices));

        Buffer indexBuffer = _graphics.CreateBuffer(BufferDescription.Index((uint)(indices.Length * Unsafe.SizeOf<ushort>())));
        indexBuffer.Fill(0, new ReadOnlySpan<ushort>(indices));
        _mesh = new Mesh(vertexBuffer, indexBuffer, (uint)indices.Length, IndexType.Uint16);

        _uniformBufferDescriptorSetLayout = _graphics.CreateResourceLayout(new DescriptorSetLayoutBindings(
            new DescriptorSetLayoutBindingEntry("ModelViewProjection", DescriptorType.UniformBuffer, ShaderStageFlags.VertexBit)));

        _textureDescriptorSetLayout = _graphics.CreateResourceLayout(
            new DescriptorSetLayoutBindings(
                new DescriptorSetLayoutBindingEntry("SourceTexture", DescriptorType.CombinedImageSampler, ShaderStageFlags.FragmentBit)));

        using (Image<Rgba32>? img = SixLabors.ImageSharp.Image.Load<Rgba32>(_getBytes("Textures/dungeon_room.png")))
        {
            _image = _graphics.CreateImage(ImageCreateInfoExt.Texture2D((uint)img.Width, (uint)img.Height, 1, 1, Format.R8G8B8A8Srgb, ImageUsageFlags.SampledBit | ImageUsageFlags.TransferDstBit | ImageUsageFlags.TransferSrcBit));
            byte[] data = new byte[Format.R8G8B8A8Unorm.GetRegionSize((uint)img.Width, (uint)img.Height, 1)];
            img.CopyPixelDataTo(data);

            _image.Fill(new ReadOnlySpan<byte>(data));
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

        Node cameraNode = new Node("Camera")
        {
            LocalPosition = new Vector3(-1, -1, 2),
            LocalRotation = new Quaternion(0.277816f, -0.115075f, -0.364972f, 0.88112f),
        };
        _camera = new Camera()
        {
            FieldOfView = 30,
            ScreenSize = new Vector2(window.Size.X, window.Size.Y),
            // Orthographic = true
        };
        cameraNode.AddComponent(_camera);

        Node plane2 = new Node("Plane2") { LocalPosition = new Vector3(0, 0, -0.2f) };
        SimpleRenderer simpleRenderer2 = new SimpleRenderer(_graphics, _mesh, _uniformBufferDescriptorSetLayout, _textureResourceSet, _pipeline);
        plane2.AddComponent(simpleRenderer2);
        plane2.AddComponent(new TwistRotator() { Axis = new Vector3(0, 0, 1), Speed = 0.15f });
        _renderers.Add(simpleRenderer2);

        Node plane1 = new Node("Plane1");
        SimpleRenderer simpleRenderer1 = new SimpleRenderer(_graphics, _mesh, _uniformBufferDescriptorSetLayout, _textureResourceSet, _pipeline);
        plane1.AddComponent(simpleRenderer1);
        plane1.AddComponent(new TwistRotator() { Axis = new Vector3(0, 0, 1), Speed = 0.1f });
        _renderers.Add(simpleRenderer1);
    }

    public void Run()
    {
        _window.Render += deltaTime =>
        {
            _playerLoop.Run();
            Draw(deltaTime);
        };
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
            _camera.ScreenSize = new Vector2(_window.Size.X, _window.Size.Y);
            _graphics.ResizeMainWindow();
        }

        using (_commandList.Begin())
        {
            foreach (SimpleRenderer simpleRenderer in _renderers)
            {
                simpleRenderer.UpdateUniformBuffer(_commandList, _camera);
            }

            using (_commandList.SetFramebuffer(_graphics.MainSwapchain!.Framebuffer!.CurrentFramebuffer))
            {
                using (_commandList.BeginRenderPass())
                {
                    _commandList.ClearColorTarget(0, Color.Black);
                    foreach (SimpleRenderer simpleRenderer in _renderers)
                    {
                        simpleRenderer.Draw(_commandList);
                    }
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
        _mesh.Dispose();
        _vertexShader.Dispose();
        _uniformBufferDescriptorSetLayout.Dispose();
        foreach (SimpleRenderer simpleRenderer in _renderers)
        {
            simpleRenderer.Dispose();
        }

        _textureResourceSet.Dispose();
        _image.Dispose();
        _imageView.Dispose();
        _textureDescriptorSetLayout.Dispose();
        _fragmentShader.Dispose();
        _commandList.Dispose();
        _graphics.Dispose();
    }

    static PlayerLoop CreatePlayerLoop()
    {
        PlayerLoop playerLoop = new PlayerLoop();
        playerLoop.Add(new UpdateStage());
        playerLoop.Add(new RenderableStage());
        return playerLoop;
    }

    public class TwistRotator : Component, IUpdateble
    {
        public required float Speed { get; init; }
        public required Vector3 Axis { get; init; }

        protected override void OnConnect()
        {
            base.OnConnect();
            UpdateStage.Register(this);
        }

        protected override void OnDisconnect()
        {
            base.OnDisconnect();
            UpdateStage.Unregister(this);
        }

        public void Update(double time)
        {
            SceneNode!.LocalRotation = Quaternion.CreateFromAxisAngle(Axis, (float)Math.Sin(MathExt.Radians(time * Speed)));
        }
    }

    public class Mesh(Buffer vertexBuffer, Buffer indexBuffers, uint indexCount, IndexType indexType) : IDisposable
    {
        public Buffer VertexBuffer { get; } = vertexBuffer;
        public Buffer IndexBuffers { get; } = indexBuffers;
        public uint IndexCount { get; } = indexCount;
        public IndexType IndexType { get; } = indexType;

        public void Dispose()
        {
            VertexBuffer.Dispose();
            IndexBuffers.Dispose();
        }
    }

    public class SimpleRenderer : Component, IDisposable
    {
        readonly Mesh _mesh;
        readonly ResourceSet _textureResourceSet;
        readonly Pipeline _pipeline;

        readonly ResourceSet _uniformBufferResourceSet;
        readonly Buffer _uniformBuffer;


        public SimpleRenderer(Graphics graphics, Mesh mesh, DescriptorSetLayout uniformBufferDescriptorSetLayout, ResourceSet textureResourceSet, Pipeline pipeline)
        {
            _mesh = mesh;
            _textureResourceSet = textureResourceSet;
            _pipeline = pipeline;

            _uniformBuffer = graphics.CreateBuffer(BufferDescription.Uniform((uint)Unsafe.SizeOf<UniformBuffer>()));
            _uniformBufferResourceSet = graphics.CreateResourceSet(new ResourceSetDescription(uniformBufferDescriptorSetLayout, _uniformBuffer.GetResourcesSetBound()));
        }

        public void UpdateUniformBuffer(CommandList commandList, Camera camera)
        {
            UniformBuffer uniformBuffer = new UniformBuffer
            {
                Model = SceneNode!.WorldMatrix4X4,
                View = camera.ViewMatrix,
                Proj = camera.Projection,
            };

            commandList.UpdateBuffer(_uniformBuffer, 0, uniformBuffer);
        }

        public void Draw(CommandList commandList)
        {
            commandList.SetPipeline(_pipeline);
            commandList.SetGraphicsResourceSet(0, _uniformBufferResourceSet, Span<uint>.Empty);
            commandList.SetGraphicsResourceSet(1, _textureResourceSet, Span<uint>.Empty);
            commandList.SetVertexBuffer(0, _mesh.VertexBuffer);
            commandList.SetIndexBuffer(_mesh.IndexBuffers, _mesh.IndexType);
            commandList.DrawIndexed(_mesh.IndexCount, 1, 0, 0, 0);
        }

        public void Dispose()
        {
            _uniformBufferResourceSet.Dispose();
            _uniformBuffer.Dispose();
        }
    }

    public class Camera : Component
    {
        public float FieldOfView { get; set; } = 45;
        public bool Orthographic { get; set; } = false;
        public float OrthographicSize { get; set; } = 5;
        public float NearPlane { get; set; } = 0.1f;
        public float FarPlane { get; set; } = 200.0f;
        public Vector2 ScreenSize { get; set; } = new Vector2(1, 1);

        public Matrix4x4 ViewMatrix
        {
            get
            {
                Matrix4x4.Invert(SceneNode!.WorldMatrix4X4, out Matrix4x4 view);
                return view;
            }
        }

        public Matrix4x4 Projection
        {
            get
            {
                float aspect = ScreenSize.X / ScreenSize.Y;
                return Orthographic
                    ? Matrix4x4.CreateOrthographic(OrthographicSize * aspect, OrthographicSize, NearPlane, FarPlane)
                    : Matrix4x4.CreatePerspectiveFieldOfView(MathExt.Radians(FieldOfView), aspect, NearPlane, FarPlane);
            }
        }
    }
}

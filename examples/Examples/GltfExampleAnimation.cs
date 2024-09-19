using System.Numerics;
using System.Runtime.CompilerServices;
using Istok;
using Istok.Core;
using Istok.Core.Animation;
using Istok.Core.PlayerLoop;
using Istok.Core.PlayerLoopStages;
using Istok.Rendering;
using Istok.Rendering.Platforms;
using SharpGLTF.Schema2;
using SharpGLTF.Transforms;
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
using Node = Istok.Core.Node;
using Pipeline = Istok.Rendering.Pipeline;
using ShaderModule = Istok.Rendering.ShaderModule;

namespace Examples;

public class GltfExampleAnimation
{
    readonly IView _window;
    readonly Graphics _graphics;
    readonly CommandList _commandList;
    readonly ShaderModule _vertexShader;
    readonly ShaderModule _fragmentShader;
    readonly Pipeline _pipeline;

    readonly List<List<Mesh>> _meshes = [];
    readonly List<Node> _roots = [];
    readonly DescriptorSetLayout _textureDescriptorSetLayout;
    readonly ResourceSet _errorTextureResourceSet;
    readonly Image _errorImage;
    readonly ImageView _errorImageView;
    readonly List<ResourceSet> _textureResourceSets = [];
    readonly List<Image> _images = [];
    readonly List<ImageView> _imageViews = [];
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

    public class MeshData(Vertex[] vertices, uint[] indices)
    {
        public Vertex[] Vertices { get; set; } = vertices;
        public uint[] Indices { get; set; } = indices;
    }

    public GltfExampleAnimation(IView window, bool colorSrgb, Func<string, byte[]> getBytes)
    {
        _getBytes = getBytes;
        _window = window;

        _graphics = new Graphics(Utils.SelectPlatform(), new Swapchain.Description(window, Format.D32Sfloat, false, colorSrgb));
        _playerLoop = CreatePlayerLoop();

        _commandList = _graphics.CreateCommandList();
        _commandList.Name = "Frame Commands List";

        _vertexShader = LoadShader(_graphics, "Shaders/uniform_buffer_example_vert.spv", ShaderStageFlags.VertexBit);
        _fragmentShader = LoadShader(_graphics, "Shaders/uniform_buffer_example_frag.spv", ShaderStageFlags.FragmentBit);

        _uniformBufferDescriptorSetLayout = _graphics.CreateResourceLayout(new DescriptorSetLayoutBindings(
            new DescriptorSetLayoutBindingEntry("ModelViewProjection", DescriptorType.UniformBuffer, ShaderStageFlags.VertexBit)));

        _textureDescriptorSetLayout = _graphics.CreateResourceLayout(
            new DescriptorSetLayoutBindings(
                new DescriptorSetLayoutBindingEntry("SourceTexture", DescriptorType.CombinedImageSampler, ShaderStageFlags.FragmentBit)));

        _errorImage = _graphics.CreateImage(ImageCreateInfoExt.Texture2D(1, 1, 1, 1, Silk.NET.Vulkan.Format.R8G8B8A8Unorm, ImageUsageFlags.SampledBit | ImageUsageFlags.TransferDstBit | ImageUsageFlags.TransferSrcBit));
        Color32 color = Color32.Magenta;
        _errorImage.Fill(new ReadOnlySpan<Color32>(ref color), 0, 0, 0, 1, 1, 1, 0, 0);

        _errorImageView = _graphics.CreateImageView(_errorImage);
        _errorTextureResourceSet = _graphics.CreateResourceSet(new ResourceSetDescription(_textureDescriptorSetLayout, new ResourcesSetBindingCombinedImageSampler(_errorImageView, _graphics.LinearSampler)));

        GraphicsPipelineDescription pd = new GraphicsPipelineDescription(
            BlendStateDescription.SingleOverrideBlend,
            PipelineDepthStencilStateCreateInfoExt.DepthOnlyLessEqual,
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

        LoadGlTFFile("Models/dungeon_scene_anim.glb");

        Node cameraNode = _roots.FirstOrDefault(node => node.Name == "Camera") ?? new Node("Camera") { LocalPosition = new Vector3(7, -7, 6), LocalRotation = new Quaternion(0.4964f, 0.205616f, 0.322752f, 0.779192f), };
        _camera = new Camera()
        {
            FieldOfView = 40,
            ScreenSize = new Vector2(window.Size.X, window.Size.Y),
            // Orthographic = true,
        };
        cameraNode.AddComponent(_camera);
    }

    void LoadGlTFFile(string name)
    {
        ModelRoot? model = ModelRoot.ParseGLB(_getBytes(name));

        LoadImages(model);
        LoadMaterials(model);
        LoadMeshes(model);

        Dictionary<int, Node> nodeMap = new Dictionary<int, Node>();

        Scene? scene = model.DefaultScene;
        foreach (SharpGLTF.Schema2.Node? nodeID in scene.VisualChildren)
        {
            Node node = LoadNode(nodeID, nodeMap);
            _roots.Add(node);
        }

        foreach (SharpGLTF.Schema2.Node? nodeID in scene.VisualChildren)
        {
            LoadComponents(nodeID, nodeMap);
        }

        LoadAnimation(model, nodeMap, true);
    }

    void LoadImages(ModelRoot model)
    {
        foreach (Texture texture in model.LogicalTextures)
        {
            using Image<Rgba32>? img = SixLabors.ImageSharp.Image.Load<Rgba32>(texture.PrimaryImage.Content.Content.Span);
            Image image = _graphics.CreateImage(ImageCreateInfoExt.Texture2D((uint)img.Width, (uint)img.Height, 1, 1, Format.R8G8B8A8Srgb, ImageUsageFlags.SampledBit | ImageUsageFlags.TransferDstBit | ImageUsageFlags.TransferSrcBit));
            byte[] data = new byte[Format.R8G8B8A8Unorm.GetRegionSize((uint)img.Width, (uint)img.Height, 1)];
            img.CopyPixelDataTo(data);

            image.Fill(new ReadOnlySpan<byte>(data));
            _images.Add(image);
            ImageView imageView = _graphics.CreateImageView(image);
            _imageViews.Add(imageView);
        }
    }

    void LoadMaterials(ModelRoot model)
    {
        foreach (Material? material in model.LogicalMaterials)
        {
            MaterialChannel? chanel = material.FindChannel("BaseColor");
            if (chanel != null && chanel.Value.Texture != null)
            {
                ResourceSet textureResourceSet = _graphics.CreateResourceSet(new ResourceSetDescription(_textureDescriptorSetLayout, new ResourcesSetBindingCombinedImageSampler(_imageViews[chanel.Value.Texture.LogicalIndex], _graphics.LinearSampler)));
                _textureResourceSets.Add(textureResourceSet);
            }
            else
            {
                _textureResourceSets.Add(_errorTextureResourceSet);
            }
        }

    }

    void LoadMeshes(ModelRoot model)
    {
        for (int i = 0; i < model.LogicalMeshes.Count; ++i)
        {
            List<MeshData> r = LoadMesh(i, model, false);
            List<Mesh> meshes = [];
            _meshes.Add(meshes);
            foreach (MeshData meshData in r)
            {
                Buffer vb = _graphics.CreateBuffer(BufferDescription.Vertex((uint)(meshData.Vertices.Length * Unsafe.SizeOf<Vertex>())));
                vb.Fill(0, new ReadOnlySpan<Vertex>(meshData.Vertices));

                Buffer ib = _graphics.CreateBuffer(BufferDescription.Index((uint)(meshData.Indices.Length * Unsafe.SizeOf<uint>())));
                ib.Fill(0, new ReadOnlySpan<uint>(meshData.Indices));

                meshes.Add(new Mesh(vb, ib, (uint)meshData.Indices.Length, IndexType.Uint32));
            }
        }
    }

    static List<MeshData> LoadMesh(int meshId, ModelRoot file, bool inverseV)
    {
        SharpGLTF.Schema2.Mesh? mesh = file.LogicalMeshes[meshId];
        List<MeshData> meshes = new List<MeshData>();
        // Iterate through all primitives of this node's mesh
        for (int i = 0; i < mesh.Primitives.Count; i++)
        {
            Vertex[] vertexBuffer;
            uint[] indexBuffer;
            MeshPrimitive? glTfPrimitive = mesh.Primitives[i];
            // Vertices
            {
                IList<Vector3>? positionBuffer = null;
                IList<Vector3>? normalsBuffer = null;
                IList<Vector2>? texCoordsBuffer = null;
                int vertexCount = 0;

                // Get buffer data for vertex positions
                Accessor? positionAccessor = glTfPrimitive.GetVertexAccessor("POSITION");
                if (positionAccessor != null)
                {
                    vertexCount = positionAccessor.Count;
                    positionBuffer = positionAccessor.AsVector3Array();
                }

                // Get buffer data for vertex texture coordinates
                // glTF supports multiple sets, we only load the first one
                Accessor? textureAccessor = glTfPrimitive.GetVertexAccessor("TEXCOORD_0");
                if (textureAccessor != null)
                {
                    texCoordsBuffer = textureAccessor.AsVector2Array();
                }

                vertexBuffer = new Vertex[vertexCount];
                // Append data to model's vertex buffer
                for (int v = 0; v < vertexCount; v++)
                {
                    Vertex vert = new Vertex
                    {
                        Pos = positionBuffer[v],
                        UV = texCoordsBuffer != null
                            ? inverseV
                                ? new Vector2(texCoordsBuffer[v].X, 1 - texCoordsBuffer[v].Y)
                                : texCoordsBuffer[v]
                            : Vector2.Zero,
                        Color = Vector3.One,
                    };
                    vertexBuffer[v] = vert;
                }
            }
            // Indices
            {
                Accessor? accessor = glTfPrimitive.IndexAccessor;

                indexBuffer = accessor.AsIndicesArray().ToArray();
            }
            meshes.Add(new MeshData(vertexBuffer, indexBuffer));
        }

        return meshes;
    }

    Node LoadNode(SharpGLTF.Schema2.Node gltfNode, Dictionary<int, Node> nodesMap)
    {
        Node node = new Node(gltfNode.Name);
        AffineTransform a = gltfNode.LocalTransform;

        node.LocalPosition = a.Translation;
        node.LocalRotation = a.Rotation;
        node.LocalScale = a.Scale;
        nodesMap.Add(gltfNode.LogicalIndex, node);

        foreach (SharpGLTF.Schema2.Node? childId in gltfNode.VisualChildren ?? [])
            node.AddChild(LoadNode(childId, nodesMap));

        return node;
    }

    void LoadComponents(SharpGLTF.Schema2.Node gltfNode, Dictionary<int, Node> nodesMap)
    {
        Node node = nodesMap[gltfNode.LogicalIndex];

        if (gltfNode.Mesh != null)
        {
            for (int i = 0; i < _meshes[gltfNode.Mesh.LogicalIndex].Count; i++)
            {
                Mesh meshData = _meshes[gltfNode.Mesh.LogicalIndex][i];
                SimpleRenderer renderer = new SimpleRenderer(_graphics, meshData, _uniformBufferDescriptorSetLayout, _textureResourceSets[gltfNode.Mesh.Primitives[i].Material.LogicalIndex], _pipeline);
                _renderers.Add(renderer);
                node.AddComponent(renderer);
            }
        }

        foreach (SharpGLTF.Schema2.Node? childId in gltfNode.VisualChildren ?? [])
            LoadComponents(childId, nodesMap);
    }


    void LoadAnimation(ModelRoot model, Dictionary<int, Node> nodesMap, bool correctCameraRotation)
    {
        if (model.LogicalAnimations != null && model.LogicalAnimations.Count > 0)
        {
            Animation logicalAnimation = model.LogicalAnimations.First();
            List<ITrack> tracks = [];
            Node? rootNode = null;
            foreach (AnimationChannel channel in logicalAnimation.Channels)
            {
                Node node = nodesMap[channel.TargetNode.LogicalIndex];
                rootNode ??= node;
                // Possible cause when use export from blender with Z up:
                // https://github.com/KhronosGroup/glTF-Blender-IO/blob/cc7ad343b2342c11cbd9a328f020bc5d0987fc9d/addons/io_scene_gltf2/blender/exp/animation/sampled/object/sampler.py#L108
                bool correctRotation = correctCameraRotation && channel.TargetNode.Camera != null;
                switch (channel.TargetNodePath)
                {
                    case PropertyPath.translation:
                    {
                        IAnimationSampler<Vector3>? translationSampler = channel.GetTranslationSampler();
                        if (translationSampler == null)
                            break;

                        Sampler<Vector3> sampler = GetVector3Sampler(translationSampler.InterpolationMode);
                        Curve<Vector3> curve = new Curve<Vector3>(sampler);

                        switch (translationSampler.InterpolationMode)
                        {
                            case AnimationInterpolationMode.LINEAR:
                            case AnimationInterpolationMode.STEP:
                                foreach ((float time, Vector3 value) in translationSampler.GetLinearKeys())
                                {
                                    curve.AddKeyframe(new Keyframe<Vector3>(time, value));
                                }
                                break;
                            case AnimationInterpolationMode.CUBICSPLINE:
                                foreach ((float time, (Vector3 TangentIn, Vector3 Value, Vector3 TangentOut) value) in translationSampler.GetCubicKeys())
                                {
                                    curve.AddKeyframe(new Keyframe<Vector3>(time, value.Value, value.TangentIn, value.TangentOut));
                                }

                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        tracks.Add(new Track<Vector3>(new StraightAnimationBinding<Vector3>(node.SetLocalPosition), curve));
                        break;
                    }
                    case PropertyPath.rotation:
                    {
                        IAnimationSampler<Quaternion>? rotationSampler = channel.GetRotationSampler();
                        if (rotationSampler == null)
                            break;

                        Sampler<Quaternion> sampler = GetQuaternionSampler(rotationSampler.InterpolationMode);
                        Curve<Quaternion> curve = new Curve<Quaternion>(sampler);


                        Quaternion rotateX90 = Quaternion.CreateFromYawPitchRoll(0, MathF.PI / 2f, 0);
                        switch (rotationSampler.InterpolationMode)
                        {
                            case AnimationInterpolationMode.LINEAR:
                            case AnimationInterpolationMode.STEP:
                                foreach ((float time, Quaternion value) in rotationSampler.GetLinearKeys())
                                {
                                    Quaternion rotation = correctRotation ? value * rotateX90 : value;
                                    curve.AddKeyframe(new Keyframe<Quaternion>(time, rotation));
                                }
                                break;
                            case AnimationInterpolationMode.CUBICSPLINE:
                                foreach ((float time, (Quaternion TangentIn, Quaternion Value, Quaternion TangentOut) value) in rotationSampler.GetCubicKeys())
                                {
                                    Quaternion rotation = correctRotation ? value.Value * rotateX90 : value.Value;
                                    Quaternion tangentIn = correctRotation ? value.TangentIn * rotateX90 : value.TangentIn;
                                    Quaternion tangentOut = correctRotation ? value.TangentOut * rotateX90 : value.TangentOut;
                                    curve.AddKeyframe(new Keyframe<Quaternion>(time, rotation, tangentIn, tangentOut));
                                }

                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        tracks.Add(new Track<Quaternion>(new StraightAnimationBinding<Quaternion>(node.SetLocalRotation), curve));
                        break;
                    }
                    case PropertyPath.scale:
                    {
                        IAnimationSampler<Vector3>? scaleSampler = channel.GetScaleSampler();
                        if (scaleSampler == null)
                            break;

                        Sampler<Vector3> sampler = GetVector3Sampler(scaleSampler.InterpolationMode);

                        Curve<Vector3> curve = new Curve<Vector3>(sampler);

                        switch (scaleSampler.InterpolationMode)
                        {
                            case AnimationInterpolationMode.LINEAR:
                            case AnimationInterpolationMode.STEP:
                                foreach ((float time, Vector3 value) in scaleSampler.GetLinearKeys())
                                {
                                    curve.AddKeyframe(new Keyframe<Vector3>(time, value));
                                }
                                break;
                            case AnimationInterpolationMode.CUBICSPLINE:
                                foreach ((float time, (Vector3 TangentIn, Vector3 Value, Vector3 TangentOut) value) in scaleSampler.GetCubicKeys())
                                {
                                    curve.AddKeyframe(new Keyframe<Vector3>(time, value.Value, value.TangentIn, value.TangentOut));
                                }

                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        tracks.Add(new Track<Vector3>(new StraightAnimationBinding<Vector3>(node.SetLocalScale), curve));
                        break;
                    }
                }
            }
            rootNode?.AddComponent(new SimpleAnimator()
            {
                Animation = new Clip
                {
                    Tracks = tracks,
                },
                Mode = SimpleAnimator.PlayMode.LOOP,
            });
        }
    }

    static Sampler<Vector3> GetVector3Sampler(AnimationInterpolationMode mode)
    {
        return mode switch
        {
            AnimationInterpolationMode.LINEAR => Samplers.Linear,
            AnimationInterpolationMode.STEP => Samplers.Step,
            AnimationInterpolationMode.CUBICSPLINE => Samplers.Cubic,
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };
    }

    static Sampler<Quaternion> GetQuaternionSampler(AnimationInterpolationMode mode)
    {
        return mode switch
        {
            AnimationInterpolationMode.LINEAR => Samplers.Linear,
            AnimationInterpolationMode.STEP => Samplers.Step,
            AnimationInterpolationMode.CUBICSPLINE => Samplers.Cubic,
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };
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
                    _commandList.ClearDepthStencil(1, 0);
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
        foreach (List<Mesh> meshes in _meshes)
        foreach (Mesh mesh in meshes)
            mesh.Dispose();
        _vertexShader.Dispose();
        _uniformBufferDescriptorSetLayout.Dispose();
        foreach (SimpleRenderer simpleRenderer in _renderers)
        {
            simpleRenderer.Dispose();
        }

        foreach (ResourceSet resourceSet in _textureResourceSets)
            resourceSet.Dispose();
        foreach (Image image in _images)
                image.Dispose();
        foreach (ImageView imageView in _imageViews)
            imageView.Dispose();
        _errorTextureResourceSet.Dispose();
        _errorImage.Dispose();
        _errorImageView.Dispose();
        _textureDescriptorSetLayout.Dispose();
        _fragmentShader.Dispose();
        _commandList.Dispose();
        _graphics.Dispose();
    }

    static PlayerLoop CreatePlayerLoop()
    {
        PlayerLoop playerLoop = new PlayerLoop();
        playerLoop.Add(new AnimationStage());
        playerLoop.Add(new UpdateStage());
        playerLoop.Add(new RenderableStage());
        return playerLoop;
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
            UniformBuffer uniformBuffer = new UniformBuffer { Model = SceneNode!.WorldMatrix4X4, View = camera.ViewMatrix, Proj = camera.Projection, };

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

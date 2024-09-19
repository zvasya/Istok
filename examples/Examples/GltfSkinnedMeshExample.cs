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
using Silk.NET.Maths;
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

namespace Examples;

public class GltfSkinnedMeshExample
{
    readonly IView _window;
    readonly Graphics _graphics;
    readonly CommandList _commandList;
    readonly Shader _vertexShader;
    readonly Shader _vertexShaderSkinned;
    readonly Shader _fragmentShader;
    readonly Pipeline _pipeline;
    readonly Pipeline _skinnedPipeline;

    readonly List<List<MeshData>> _meshesData = [];
    readonly List<Mesh> _meshes = [];
    readonly List<Node> _roots = [];
    readonly DescriptorSetLayout _textureDescriptorSetLayout;
    readonly ResourceSet _errorTextureResourceSet;
    readonly Image _errorImage;
    readonly ImageView _errorImageView;
    readonly List<ResourceSet> _textureResourceSets = [];
    readonly List<Image> _images = [];
    readonly List<ImageView> _imageViews = [];
    readonly DescriptorSetLayout _uniformBufferDescriptorSetLayout;
    readonly DescriptorSetLayout _bonesBufferDescriptorSetLayout;

    bool _windowResized;

    readonly PlayerLoop _playerLoop;
    readonly Camera _camera;
    readonly List<IRenderer> _renderers = [];

    readonly Func<string, byte[]> _getBytes;

    public struct Vertex
    {
        public Vector3 Pos;
        public Vector3 Color;
        public Vector2 UV;
    }

    public struct VertexSkinned
    {
        public Vector3 Pos;
        public Vector3 Color;
        public Vector2 UV;
        public Vector4D<byte> Joints;
        public Vector4 Weights;
    }

    public struct UniformBuffer
    {
        public Matrix4x4 Model;
        public Matrix4x4 View;
        public Matrix4x4 Proj;
    }

    public class MeshData()
    {
        public IList<Vector3>? PositionBuffer { get; set; }
        public IList<Vector3>? NormalsBuffer { get; set; }
        public IList<Vector2>? TexCoordsBuffer { get; set; }
        public IList<Vector4D<byte>>? JointsBuffer { get; set; }
        public IList<Vector4>? WeightsBuffer { get; set; }
        public uint[] Indices { get; set; }
    }

    public GltfSkinnedMeshExample(IView window, bool colorSrgb, Func<string, byte[]> getBytes)
    {
        _getBytes = getBytes;
        _window = window;

        _graphics = new Graphics(Utils.SelectPlatform(), new Swapchain.Description(window, Format.D32Sfloat, false, colorSrgb));
        _playerLoop = CreatePlayerLoop();

        _commandList = _graphics.CreateCommandList();
        _commandList.Name = "Frame Commands List";

        _vertexShader = LoadShader(_graphics, "Shaders/uniform_buffer_example_vert.spv", ShaderStageFlags.VertexBit);
        _vertexShaderSkinned = LoadShader(_graphics, "Shaders/skinned_example_vert.spv", ShaderStageFlags.VertexBit);
        _fragmentShader = LoadShader(_graphics, "Shaders/uniform_buffer_example_frag.spv", ShaderStageFlags.FragmentBit);

        _uniformBufferDescriptorSetLayout = _graphics.CreateResourceLayout(new DescriptorSetLayoutBindings(
            new DescriptorSetLayoutBindingEntry("ModelViewProjection", DescriptorType.UniformBuffer, ShaderStageFlags.VertexBit)));
        _bonesBufferDescriptorSetLayout = _graphics.CreateResourceLayout(new DescriptorSetLayoutBindings(
            new DescriptorSetLayoutBindingEntry("Bones", DescriptorType.StorageBuffer, ShaderStageFlags.VertexBit)));

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

        GraphicsPipelineDescription skinnedPd = new GraphicsPipelineDescription(
            BlendStateDescription.SingleOverrideBlend,
            PipelineDepthStencilStateCreateInfoExt.DepthOnlyLessEqual,
            PipelineRasterizationStateCreateInfoExt.Default,
            PrimitiveTopology.TriangleList,
            new ShaderSetDescription([
                new VertexLayoutDescription(
                    new VertexElementDescription("Position", Format.R32G32B32Sfloat),
                    new VertexElementDescription("Color", Format.R32G32B32Sfloat),
                    new VertexElementDescription("TexCoords", Format.R32G32Sfloat),
                    new VertexElementDescription("Joints", Format.R8G8B8A8Uint),
                    new VertexElementDescription("Weigts", Format.R32G32B32A32Sfloat)
                ),
            ], [_vertexShaderSkinned, _fragmentShader]),
            _graphics.MainSwapchain?.Framebuffer?.RenderPass,
            _uniformBufferDescriptorSetLayout,
            _textureDescriptorSetLayout,
            _bonesBufferDescriptorSetLayout);
        _skinnedPipeline = _graphics.CreateGraphicsPipeline(in skinnedPd);

        LoadGlTFFile("Models/character.glb");

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

        LoadAnimations(model, nodeMap, true, "Run");
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
            _meshesData.Add(LoadMesh(i, model, false));
        }
    }

    static List<MeshData> LoadMesh(int meshId, ModelRoot file, bool inverseV)
    {
        SharpGLTF.Schema2.Mesh? mesh = file.LogicalMeshes[meshId];
        List<MeshData> meshes = new List<MeshData>();
        // Iterate through all primitives of this node's mesh
        for (int i = 0; i < mesh.Primitives.Count; i++)
        {
            MeshData meshData = new MeshData();

            MeshPrimitive? glTfPrimitive = mesh.Primitives[i];
            // Vertices
            {
                int vertexCount = 0;

                // Get buffer data for vertex positions
                Accessor? positionAccessor = glTfPrimitive.GetVertexAccessor("POSITION");
                if (positionAccessor != null)
                {
                    vertexCount = positionAccessor.Count;
                    meshData.PositionBuffer = positionAccessor.AsVector3Array();
                }

                // Get buffer data for vertex texture coordinates
                // glTF supports multiple sets, we only load the first one
                Accessor? textureAccessor = glTfPrimitive.GetVertexAccessor("TEXCOORD_0");
                if (textureAccessor != null)
                {
                    meshData.TexCoordsBuffer = textureAccessor.AsVector2Array();
                }

                Accessor? jointsAccessor = glTfPrimitive.GetVertexAccessor("JOINTS_0");
                if (jointsAccessor != null)
                {
                    meshData.JointsBuffer = new List<Vector4D<byte>>(jointsAccessor.Count);
                    for (int j = 0; j < jointsAccessor.Count; j++)
                    {
                        ArraySegment<byte> segment = jointsAccessor.TryGetVertexBytes(j);
                        meshData.JointsBuffer.Add(new Vector4D<byte>(segment[0], segment[1], segment[2], segment[3]));
                    }
                }
                Accessor? weightsAccessor = glTfPrimitive.GetVertexAccessor("WEIGHTS_0");
                if (weightsAccessor != null)
                {
                    meshData.WeightsBuffer = weightsAccessor.AsVector4Array();
                }
            }
            // Indices
            {
                Accessor? accessor = glTfPrimitive.IndexAccessor;

                meshData.Indices = accessor.AsIndicesArray().ToArray();
            }
            meshes.Add(meshData);
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
            for (int i = 0; i < _meshesData[gltfNode.Mesh.LogicalIndex].Count; i++)
            {
                MeshData meshData = _meshesData[gltfNode.Mesh.LogicalIndex][i];

                Buffer ib = _graphics.CreateBuffer(BufferDescription.Index((uint)(meshData.Indices.Length * Unsafe.SizeOf<uint>())));
                ib.Fill(0, new ReadOnlySpan<uint>(meshData.Indices));

                bool skinned = meshData.JointsBuffer != null && gltfNode.Skin != null;
                if (skinned)
                {
                    Node[] joints = new Node[gltfNode.Skin.Joints.Count];
                    for (int j = 0; j < gltfNode.Skin.Joints.Count; j++)
                    {
                        joints[j] = nodesMap[gltfNode.Skin.Joints[j].LogicalIndex];
                    }

                    Matrix4x4[] inverseBindMatrices = new Matrix4x4[gltfNode.Skin.InverseBindMatrices.Count];
                    for (int j = 0; j < gltfNode.Skin.InverseBindMatrices.Count; j++)
                    {
                        inverseBindMatrices[j] = gltfNode.Skin.InverseBindMatrices[j];
                    }

                    Node rootNode = node;
                    if (gltfNode.Skin.Skeleton != null)
                        rootNode = nodesMap[gltfNode.Skin.Skeleton.LogicalIndex];


                    VertexSkinned[] vertices = new VertexSkinned[meshData.PositionBuffer.Count];
                    for (int v = 0; v < meshData.PositionBuffer.Count; v++)
                    {
                        VertexSkinned vert = new VertexSkinned
                        {
                            Pos = meshData.PositionBuffer[v],
                            UV = meshData.TexCoordsBuffer != null
                                ? meshData.TexCoordsBuffer[v]
                                : Vector2.Zero,
                            Color = Vector3.One,
                            Joints = meshData.JointsBuffer[v],
                            Weights = meshData.WeightsBuffer[v],
                        };
                        vertices[v] = vert;
                    }
                    Buffer vb = _graphics.CreateBuffer(BufferDescription.Vertex((uint)(vertices.Length * Unsafe.SizeOf<VertexSkinned>())));
                    vb.Fill(0, new ReadOnlySpan<VertexSkinned>(vertices));
                    Mesh mesh = new Mesh(vb, ib, (uint)meshData.Indices.Length, IndexType.Uint32);
                    _meshes.Add(mesh);
                    SkinnedRenderer renderer = new SkinnedRenderer(_graphics, mesh, _uniformBufferDescriptorSetLayout, _bonesBufferDescriptorSetLayout, _textureResourceSets[gltfNode.Mesh.Primitives[i].Material.LogicalIndex], _skinnedPipeline, joints, inverseBindMatrices, rootNode);
                    _renderers.Add(renderer);
                    node.AddComponent(renderer);
                }
                else
                {
                    Vertex[] vertices = new Vertex[meshData.PositionBuffer.Count];
                    for (int v = 0; v < meshData.PositionBuffer.Count; v++)
                    {
                        Vertex vert = new Vertex
                        {
                            Pos = meshData.PositionBuffer[v],
                            UV = meshData.TexCoordsBuffer != null
                                ? meshData.TexCoordsBuffer[v]
                                : Vector2.Zero,
                            Color = Vector3.One,
                        };
                        vertices[v] = vert;
                    }
                    Buffer vb = _graphics.CreateBuffer(BufferDescription.Vertex((uint)(vertices.Length * Unsafe.SizeOf<Vertex>())));
                    vb.Fill(0, new ReadOnlySpan<Vertex>(vertices));
                    Mesh mesh = new Mesh(vb, ib, (uint)meshData.Indices.Length, IndexType.Uint32);
                    _meshes.Add(mesh);
                    SimpleRenderer renderer = new SimpleRenderer(_graphics, mesh, _uniformBufferDescriptorSetLayout, _textureResourceSets[gltfNode.Mesh.Primitives[i].Material.LogicalIndex], _pipeline);
                    _renderers.Add(renderer);
                    node.AddComponent(renderer);
                }

            }
        }

        foreach (SharpGLTF.Schema2.Node? childId in gltfNode.VisualChildren ?? [])
            LoadComponents(childId, nodesMap);
    }


    void LoadAnimations(ModelRoot model, Dictionary<int, Node> nodesMap, bool correctCameraRotation, string? name = null)
    {
        if (model.LogicalAnimations != null && model.LogicalAnimations.Count > 0)
        {
            Animation logicalAnimation = string.IsNullOrWhiteSpace(name)
                ? model.LogicalAnimations.First()
                : model.LogicalAnimations.FirstOrDefault(animation => animation.Name == name) ?? model.LogicalAnimations.First();
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


    Shader LoadShader(Graphics graphics, string name, ShaderStageFlags shaderStage)
    {
        byte[] shaderBytes = _getBytes(name);
        ShaderDescription shaderDescription = new ShaderDescription(shaderStage, shaderBytes, "main");
        return graphics.CreateShader(in shaderDescription);
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
            foreach (IRenderer simpleRenderer in _renderers)
            {
                simpleRenderer.UpdateUniformBuffer(_commandList, _camera);
            }

            using (_commandList.SetFramebuffer(_graphics.MainSwapchain!.Framebuffer!.CurrentFramebuffer))
            {
                using (_commandList.BeginRenderPass())
                {
                    _commandList.ClearColorTarget(0, Color.Black);
                    _commandList.ClearDepthStencil(1, 0);
                    foreach (IRenderer simpleRenderer in _renderers)
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
        _skinnedPipeline.Dispose();
        foreach (Mesh mesh in _meshes)
            mesh.Dispose();
        _vertexShaderSkinned.Dispose();
        _vertexShader.Dispose();
        _fragmentShader.Dispose();
        _uniformBufferDescriptorSetLayout.Dispose();
        foreach (IRenderer simpleRenderer in _renderers)
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
        _bonesBufferDescriptorSetLayout.Dispose();
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

    public interface IRenderer : IDisposable
    {
        public void Draw(CommandList commandList);
        public void UpdateUniformBuffer(CommandList commandList, Camera camera);
    }

    public class SkinnedRenderer : Component, IRenderer, IDisposable
    {
        readonly Mesh _mesh;
        readonly ResourceSet _textureResourceSet;
        readonly Pipeline _pipeline;

        readonly ResourceSet _uniformBufferResourceSet;
        readonly ResourceSet _bonesBufferResourceSet;
        readonly Buffer _uniformBuffer;
        readonly Buffer _bonesBuffer;
        readonly Node[] _joints;
        readonly Matrix4x4[] _inverseBindMatrices;
        readonly Matrix4x4[] _bonesMatrices;
        readonly Node _rootNode;

        public SkinnedRenderer(Graphics graphics, Mesh mesh, DescriptorSetLayout uniformBufferDescriptorSetLayout, DescriptorSetLayout bonesBufferDescriptorSetLayout, ResourceSet textureResourceSet, Pipeline pipeline, Node[] joints, Matrix4x4[] inverseBindMatrices, Node rootNode)
        {
            _mesh = mesh;
            _textureResourceSet = textureResourceSet;
            _pipeline = pipeline;
            _joints = joints;
            _bonesMatrices = new Matrix4x4[_joints.Length];
            _inverseBindMatrices = inverseBindMatrices;
            _rootNode = rootNode;

            _uniformBuffer = graphics.CreateBuffer(BufferDescription.Uniform((uint)Unsafe.SizeOf<UniformBuffer>()));
            _uniformBufferResourceSet = graphics.CreateResourceSet(new ResourceSetDescription(uniformBufferDescriptorSetLayout, _uniformBuffer.GetResourcesSetBound()));

            _bonesBuffer = graphics.CreateBuffer(BufferDescription.Storage((uint)(_joints.Length * (uint)Unsafe.SizeOf<Matrix4x4>())));
            _bonesBufferResourceSet = graphics.CreateResourceSet(new ResourceSetDescription(bonesBufferDescriptorSetLayout, _bonesBuffer.GetResourcesSetBound()));
        }

        public void UpdateUniformBuffer(CommandList commandList, Camera camera)
        {
            UniformBuffer uniformBuffer = new UniformBuffer { Model = SceneNode!.WorldMatrix4X4, View = camera.ViewMatrix, Proj = camera.Projection, };

            commandList.UpdateBuffer(_uniformBuffer, 0, uniformBuffer);

            Matrix4x4.Invert(_rootNode.WorldMatrix4X4, out Matrix4x4 inverseRootMatrix);

            for (int i = 0; i < _bonesMatrices.Length; i++)
            {
                Matrix4x4 m = _joints[i].WorldMatrix4X4 * inverseRootMatrix;
                _bonesMatrices[i] = _inverseBindMatrices[i] * m;
            }

            commandList.UpdateBuffer(_bonesBuffer, 0, _bonesMatrices);
        }

        public void Draw(CommandList commandList)
        {
            commandList.SetPipeline(_pipeline);
            commandList.SetGraphicsResourceSet(0, _uniformBufferResourceSet, Span<uint>.Empty);
            commandList.SetGraphicsResourceSet(1, _textureResourceSet, Span<uint>.Empty);
            commandList.SetGraphicsResourceSet(2, _bonesBufferResourceSet, Span<uint>.Empty);
            commandList.SetVertexBuffer(0, _mesh.VertexBuffer);
            commandList.SetIndexBuffer(_mesh.IndexBuffers, _mesh.IndexType);
            commandList.DrawIndexed(_mesh.IndexCount, 1, 0, 0, 0);
        }

        public void Dispose()
        {
            _uniformBufferResourceSet.Dispose();
            _bonesBufferResourceSet.Dispose();
            _bonesBuffer.Dispose();
            _uniformBuffer.Dispose();
        }
    }

    public class SimpleRenderer : Component, IRenderer, IDisposable
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

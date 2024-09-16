using Silk.NET.Vulkan;

namespace Istok.Rendering;

public readonly record struct ShaderDescription(ShaderStageFlags Stage, byte[] ShaderBytes, string EntryPoint);

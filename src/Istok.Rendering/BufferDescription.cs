using Silk.NET.Vulkan;

namespace Istok.Rendering;

public readonly record struct BufferDescription(uint SizeInBytes, BufferUsageFlags Usage, MemoryPropertyFlags Property = MemoryPropertyFlags.DeviceLocalBit)
{
    public static BufferDescription Index(uint sizeInBytes, MemoryPropertyFlags property = MemoryPropertyFlags.DeviceLocalBit) => new BufferDescription(sizeInBytes, BufferUsageFlags.IndexBufferBit | BufferUsageFlags.TransferSrcBit | BufferUsageFlags.TransferDstBit, property);
    public static BufferDescription Uniform(uint sizeInBytes, MemoryPropertyFlags property = MemoryPropertyFlags.DeviceLocalBit) => new BufferDescription(sizeInBytes, BufferUsageFlags.UniformBufferBit | BufferUsageFlags.TransferSrcBit | BufferUsageFlags.TransferDstBit, property);
    public static BufferDescription Storage(uint sizeInBytes, MemoryPropertyFlags property = MemoryPropertyFlags.DeviceLocalBit) => new BufferDescription(sizeInBytes, BufferUsageFlags.StorageBufferBit | BufferUsageFlags.TransferSrcBit | BufferUsageFlags.TransferDstBit, property);
    public static BufferDescription Vertex(uint sizeInBytes, MemoryPropertyFlags property = MemoryPropertyFlags.DeviceLocalBit) => new BufferDescription(sizeInBytes, BufferUsageFlags.VertexBufferBit | BufferUsageFlags.TransferSrcBit | BufferUsageFlags.TransferDstBit, property);
}

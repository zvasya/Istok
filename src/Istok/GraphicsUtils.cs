using Istok.Rendering;

namespace Istok;

public static class GraphicsUtils
{
    static readonly byte[] SpirVHeader = [3, 2, 35, 7];

    public static ShaderModule[] CreateFromSpirV2(Graphics graphics, ShaderDescription vertexShaderDescription, ShaderDescription fragmentShaderDescription)
    {
        EnsureSpirVHeader(vertexShaderDescription.ShaderBytes);
        EnsureSpirVHeader(fragmentShaderDescription.ShaderBytes);

        return
        [
            graphics.CreateShaderModule(in vertexShaderDescription),
            graphics.CreateShaderModule(in fragmentShaderDescription),
        ];

        static void EnsureSpirVHeader(byte[] bytes)
        {
            if (bytes.Length > 4 && new ReadOnlySpan<byte>(bytes, 0, 4).SequenceEqual(SpirVHeader))
                return;
            throw new Exception("Not SpirV shader");
        }
    }
}

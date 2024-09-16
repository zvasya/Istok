using Silk.NET.Vulkan;

namespace Istok.Rendering;

public static class Helpers
{
    public static void CheckErrors(Result result)
    {
        if (result != Result.Success)
        {
            throw new InvalidOperationException(result.ToString());
        }
    }
}

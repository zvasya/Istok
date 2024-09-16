namespace Istok.Rendering;

public interface IPlatform
{
    bool Debug { get; }

    string[] RequiredInstanceExtensions { get; }
    string[] DeviceExtensions { get; }

    string[][] ValidationLayerNamesPriorityList { get; }
}

using System.Numerics;

namespace Istok.Core.Animation;

public class AnimationBindingVector3(string path, string property) : AnimationBinding<Vector3>(path, property)
{
    public override void Bind(Node sceneNode)
    {
        if (sceneNode.TryFindChildByPath(Path, out Node? node))
        {
            Setter = node!.GetSetterAnimVector3(Property);
        }
    }
}

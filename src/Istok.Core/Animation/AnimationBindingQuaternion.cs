using System.Numerics;

namespace Istok.Core.Animation;

public class AnimationBindingQuaternion(string path, string property) : AnimationBinding<Quaternion>(path, property)
{
    public override void Bind(Node sceneNode)
    {
        if (sceneNode.TryFindChildByPath(Path, out Node? node))
        {
            Setter = node!.GetSetterAnimQuaternion(Property);
        }
    }
}

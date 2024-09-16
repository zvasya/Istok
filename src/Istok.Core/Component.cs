namespace Istok.Core;

public abstract class Component
{
    public Node? SceneNode { get; private set; }

    internal void Connect(Node? sceneNode)
    {
        if (SceneNode != null)
            throw new Exception("Component already connected");

        SceneNode = sceneNode;
        OnConnect();
    }

    internal void Disconnect()
    {
        if (SceneNode == null)
            throw new Exception("Component not connected");

        SceneNode = null;
        OnDisconnect();
    }

    protected virtual void OnConnect() { }

    protected virtual void OnDisconnect() { }


}

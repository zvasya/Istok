namespace Istok.Core.PlayerLoop;

public abstract class PlayerLoopStage<T> : IPlayerLoopStage
{
    static EventList<T> _subscribers = new EventList<T>();

    protected abstract void Invoke(T subscriber);
    public virtual void Execute()
    {
        _subscribers.Execute(Invoke);
    }

    public static void Register(T subscriber) => EventList<T>.Add(ref _subscribers, subscriber);

    public static void Unregister(T subscriber) => EventList<T>.Remove(ref _subscribers, subscriber);
}

namespace Istok.Core.PlayerLoop;

public class EventList<T>
{
    readonly HashSet<T> _subscribers;

    bool _inUse;

    public EventList()
    {
        _subscribers = new HashSet<T>();
    }

    EventList(HashSet<T> subscribers)
    {
        _subscribers = subscribers;
    }

    public static void Add(ref EventList<T> list, T subscriber)
    {
        if (list._inUse)
            list = new EventList<T>(list._subscribers);

        list._subscribers.Add(subscriber);
    }

    public static void Remove(ref EventList<T> list, T subscriber)
    {
        if (list._inUse)
            list = new EventList<T>(list._subscribers);

        list._subscribers.Remove(subscriber);
    }

    public void Execute(Action<T> invoke)
    {
        _inUse = true;

        try
        {
            foreach (var subscriber in _subscribers)
            {
                invoke(subscriber);
            }
        }
        finally
        {
            _inUse = false;
        }
    }
}

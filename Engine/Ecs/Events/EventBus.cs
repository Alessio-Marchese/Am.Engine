namespace Engine.Ecs.Events;

/// <summary>
/// Provides a simple event bus for subscribing to, publishing, and dispatching events.
/// </summary>
/// <remarks>The EventBus enables decoupled communication between components by allowing event handlers to
/// subscribe to specific event types and receive notifications when those events are published. Events are queued and
/// dispatched in the order they are published.</remarks>
public class EventBus
{
    private readonly Dictionary<Type, List<Delegate>> _subscribers = new();

    private readonly Queue<IEvent> _eventQueue = new();

    public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent
    {
        var type = typeof(TEvent);

        if (!_subscribers.ContainsKey(type))
            _subscribers[type] = new List<Delegate>();

        _subscribers[type].Add(handler);
    }

    public void Unsubscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent
    {
        var type = typeof(TEvent);

        if (_subscribers.TryGetValue(type, out var handlers))
            handlers.Remove(handler);
    }

    public void Publish(IEvent ev)
    {
        _eventQueue.Enqueue(ev);
    }

    public void Dispatch()
    {
        while (_eventQueue.Count > 0)
        {
            var ev = _eventQueue.Dequeue();
            var type = ev.GetType();

            if (!_subscribers.ContainsKey(type))
                continue;

            foreach (var del in _subscribers[type])
            {
                del.DynamicInvoke(ev);
            }
        }
    }
}

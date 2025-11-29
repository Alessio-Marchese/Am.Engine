namespace Engine.Ecs.Events.Interfaces;

public interface IEventBus
{
    void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent;

    void Unsubscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent;

    void Publish(IEvent ev);

    void Dispatch();
}

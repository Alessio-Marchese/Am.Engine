namespace Engine.Ecs.Events.Interfaces;

/// <summary>
/// Defines a contract for subscribing to events within a specified world context.
/// </summary>
/// <remarks>Implementations of this interface should subscribe for an event in the world context
/// and provide a Delegate to execute whenever this event is published. </remarks>
public interface IEventSubscriber
{
    void Register(World world);
}
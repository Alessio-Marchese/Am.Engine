using Engine.Ecs;
using Engine.Ecs.Components;
using Engine.Ecs.Events;
using Engine.Ecs.Events.Interfaces;
using Engine.Ecs.Systems;

namespace Tests.Integration.Events;

public class EventsIntegration
{
    /// <summary>
    /// Creates a reusable environment:
    /// </summary>
    private (World world, EventCaptureListener subscriber, GameObject mover) CreateWorldWithSubscriber(
        Transform tA, Transform tB)
    {
        var subscriber = new EventCaptureListener();

        var world = new World()
            .AddSystem(new CollisionSystem())
            .RegisterSubscriber(subscriber);

        // Moving object
        var mover = world.AddGameObject()
            .AddComponent(tA)
            .AddComponent(new Collider(100, 100))
            .AddComponent(new Movement { DirX = 1, Speed = 100 });

        // Static object (just a collider)
        world.AddGameObject()
            .AddComponent(tB)
            .AddComponent(new Collider(100, 100));

        return (world, subscriber, mover);
    }

    private class EventCaptureListener : IEventSubscriber
    {
        public int CollisionCount = 0;
        public void Register(World world)
        {
            world.Events.Subscribe<CollisionEvent>(OnEvent);
        }

        private void OnEvent(IEvent e)
        {
            if (e is CollisionEvent)
                CollisionCount++;
        }
    }

    [Fact]
    public void Event_flow_should_publish_subscribe_and_react()
    {
        //Simulate touch between objectA (mover) and objectB (static object)
        var (world, listener, _) = CreateWorldWithSubscriber(
            new Transform { X = 0, Y = 0 },
            new Transform { X = 50, Y = 0 });

        world.Update(1f);

        Assert.Equal(1, listener.CollisionCount);
    }

    [Fact]
    public void Subscriber_should_not_react_if_event_is_not_published()
    { 
        var (world, listener, _) = CreateWorldWithSubscriber(
            new Transform { X = 0, Y = 0 },
            new Transform { X = 300, Y = 0 });

        world.Update(1f);

        Assert.Equal(0, listener.CollisionCount);
    }
}

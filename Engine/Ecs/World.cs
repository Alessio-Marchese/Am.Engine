using Engine.Ecs.Components.Interfaces;
using Engine.Ecs.Events;
using Engine.Ecs.Events.Listeners;
using Engine.Ecs.Systems.Interfaces;

namespace Engine.Ecs;

public class World
{
    public EventBus Events { get; } = new EventBus();

    private readonly List<GameObject> _gameObjects = new();
    private readonly List<ISystem> _systems = new();
    private int _nextId = 1;

    public IReadOnlyList<GameObject> Entities => _gameObjects;

    public GameObject AddGameObject()
    {
        var e = new GameObject(_nextId++);
        _gameObjects.Add(e);
        return e;
    }

    public World AddSystem(ISystem system)
    {
        _systems.Add(system);
        return this;
    }

    public World RegisterSubscriber(IEventSubscriber subscriber)
    {
        subscriber.Register(this);
        return this;
    }

    public void Update(float deltaTime)
    {
        foreach (var system in _systems)
            system.Update(this, deltaTime);

        Events.Dispatch();
    }

    public IEnumerable<GameObject> With<T>() where T : class, IComponent
    {
        foreach (var e in _gameObjects)
            if (e.HasComponent<T>())
                yield return e;
    }

    public IEnumerable<GameObject> WithAll(params Type[] componentTypes)
    {
        foreach (var e in _gameObjects)
        {
            bool ok = true;

            foreach (var type in componentTypes)
                if (!e.HasComponent(type))
                {
                    ok = false;
                    break;
                }

            if (ok) yield return e;
        }
    }
}

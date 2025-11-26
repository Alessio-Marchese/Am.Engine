namespace Am.Engine.Ecs;

public class World
{
    private readonly List<GameObject> _gameObjects = new();
    private readonly List<ISystem> _systems = new();
    private int _nextId = 1;

    public IReadOnlyList<GameObject> Entities => _gameObjects;

    public GameObject CreateEntity()
    {
        var e = new GameObject(_nextId++);
        _gameObjects.Add(e);
        return e;
    }

    public void AddSystem(ISystem system) => _systems.Add(system);

    public void Update(float deltaTime)
    {
        foreach (var system in _systems)
            system.Update(this, deltaTime);
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

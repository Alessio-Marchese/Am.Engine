using Engine.Ecs.Components.Interfaces;

namespace Engine.Ecs;

public class GameObject
{
    private readonly Dictionary<Type, IComponent> _components = new();

    public int Id { get; }

    internal GameObject(int id)
    {
        Id = id;
    }

    public GameObject AddComponent<T>(T component) where T : class, IComponent
    {
        _components[typeof(T)] = component;
        return this;
    }

    public bool HasComponent<T>() where T : class, IComponent
        => _components.ContainsKey(typeof(T));
    
    public bool HasComponent(Type type)
        => _components.ContainsKey(type);

    public T? GetComponent<T>() where T : class, IComponent
        => _components.TryGetValue(typeof(T), out var c) ? (T)c : null;
}

namespace Engine.Utils;

public class ComponentNotFoundException : Exception
{
    public ComponentNotFoundException(Type type)
        : base($"Component '{type.Name}' not found") { }
}

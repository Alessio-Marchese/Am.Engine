namespace Engine.Utils;

public class GameObjectNotFoundException : Exception
{
    public GameObjectNotFoundException(Type type)
        : base($"GameObject with component of type '{type.Name}' not found") { }
}

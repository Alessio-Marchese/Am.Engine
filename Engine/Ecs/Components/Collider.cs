using Engine.Ecs.Components.Interfaces;

namespace Engine.Ecs.Components;

public class Collider : IComponent
{
    public float Width;
    public float Height;
    public float Scale;

    public Collider(float width, float height, float scale = 1f)
    {
        Width = width;
        Height = height;
        Scale = scale;
    }
}

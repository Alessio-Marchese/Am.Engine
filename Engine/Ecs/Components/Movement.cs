using Engine.Ecs.Components.Interfaces;

namespace Engine.Ecs.Components;

public class Movement : IComponent
{
    public float Speed;
    public float DirX;
    public float DirY;
}

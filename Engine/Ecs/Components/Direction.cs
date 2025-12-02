using Engine.Ecs.Components.Enums;
using Engine.Ecs.Components.Interfaces;

namespace Engine.Ecs.Components;

public class Direction : IComponent
{
    public FacingDirection LastDirection { get; set; } = FacingDirection.Right;
}

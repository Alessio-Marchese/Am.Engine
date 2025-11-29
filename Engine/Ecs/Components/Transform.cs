using Engine.Ecs.Components.Interfaces;

namespace Engine.Ecs.Components;

public class Transform : IComponent
{
    public float X;
    public float Y;

    public Transform(float x = 0, float y = 0)
    {
        X = x; Y = y; 
    }
}

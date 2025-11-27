using Engine.Ecs.Components.Interfaces;

namespace Engine.Ecs.Components;
public class Camera : IComponent
{
    public float X, Y;
    public float Zoom = 1f;

    public float OffsetX = 0;
    public float OffsetY = 0;

    public float FollowSpeed = 5f;

    public Camera(float offsetX = 0, float offsetY = 0, float zoom = 1f)
    {
        OffsetX = offsetX;
        OffsetY = offsetY;
        Zoom = zoom;
    }
}

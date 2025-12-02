using Engine.Ecs.Components.Interfaces;

namespace Engine.Ecs.Components;
public class Camera : IComponent
{
    public float X, Y;
    public float Zoom;

    public float OffsetX;
    public float OffsetY;

    public float FollowSpeed;

    public Camera(float offsetX = 0, float offsetY = 0, float zoom = 1f, float followSpeed = 5f)
    {
        OffsetX = offsetX;
        OffsetY = offsetY;
        Zoom = zoom;
        FollowSpeed = followSpeed;
    }
}

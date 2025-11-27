using Engine.Ecs.Components;
using Engine.Ecs.Components.Tags;
using Engine.Ecs.Systems.Interfaces;
using Engine.Utils;

namespace Engine.Ecs.Systems;

public class CameraFollowSystem : ISystem
{
    private readonly Func<(int viewportW, int viewportH)> _viewportProvider;

    public CameraFollowSystem(Func<(int, int)> viewportProvider)
    {
        _viewportProvider = viewportProvider;
    }

    public void Update(World world, float dt)
    {
        var cameraEntity = world.With<Camera>().FirstOrDefault();
        var playerEntity = world.With<PlayerTag>().FirstOrDefault();

        if (cameraEntity == null) throw new GameObjectNotFoundException(typeof(Camera));
        if (playerEntity == null) throw new GameObjectNotFoundException(typeof(PlayerTag));

        var cam = cameraEntity.GetComponent<Camera>()!;
        var p = playerEntity.GetComponent<Transform>()!;

        var (vw, vh) = _viewportProvider();
        float halfW = vw * 0.5f;
        float halfH = vh * 0.5f;

        float targetX = (p.X - halfW) + cam.OffsetX;
        float targetY = (p.Y - halfH) + cam.OffsetY;

        cam.X += (targetX - cam.X) * cam.FollowSpeed * dt;
        cam.Y += (targetY - cam.Y) * cam.FollowSpeed * dt;
    }
}

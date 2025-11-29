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
        var camEntity = world.With<Camera>().FirstOrDefault();
        var player = world.With<PlayerTag>().FirstOrDefault();

        if (camEntity == null) throw new GameObjectNotFoundException(typeof(Camera));
        if (player == null) throw new GameObjectNotFoundException(typeof(PlayerTag));

        var cam = camEntity.GetComponent<Camera>()!;
        var t = player.GetComponent<Transform>()!;

        var (vw, vh) = _viewportProvider();

        float halfWWorld = (vw / cam.Zoom) * 0.5f;
        float halfHWorld = (vh / cam.Zoom) * 0.5f;

        float targetX = t.X - halfWWorld + cam.OffsetX;
        float targetY = t.Y - halfHWorld + cam.OffsetY;

        cam.X += (targetX - cam.X) * cam.FollowSpeed * dt;
        cam.Y += (targetY - cam.Y) * cam.FollowSpeed * dt;
    }
}

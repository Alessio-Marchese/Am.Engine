using Engine.Ecs;
using Engine.Ecs.Components;
using Engine.Ecs.Components.Enums;
using Engine.Ecs.Events;
using Engine.Ecs.Events.Interfaces;

namespace Sandbox.EngineExtensions;

public class IdleAnimationSubscriber : IEventSubscriber
{
    public void Register(World world)
    {
        world.Events.Subscribe<PlayerIdleEvent>(ev =>
        {
            var anim = ev.Player.GetComponent<Animation>();
            var dir = ev.Player.GetComponent<Direction>();

            if (dir.LastDirection == FacingDirection.Left)
            {
                anim?.TryPlay("idle_left");
            }
            else
            {
                anim?.TryPlay("idle_right");
            }
        });
    }
}

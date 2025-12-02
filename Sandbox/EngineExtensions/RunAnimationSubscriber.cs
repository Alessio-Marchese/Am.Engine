using Engine.Ecs.Components;
using Engine.Ecs.Components.Enums;
using Engine.Ecs.Events.Interfaces;
using Sandbox.EngineExtensions;

namespace Engine.Ecs.Events.Subscribers;

public class RunAnimationSubscriber : IEventSubscriber
{
    public void Register(World world)
    {
        world.Events.Subscribe<PlayerRunEvent>(ev =>
        {
            var anim = ev.Player.GetComponent<Animation>();
            var dir = ev.Player.GetComponent<Direction>();

            if(ev.IsLeft)
            {
                anim?.TryPlay("run_left");
                dir.LastDirection = FacingDirection.Left;
            }
            else
            {
                anim?.TryPlay("run_right");
                dir.LastDirection = FacingDirection.Right;
            }
        });
    }
}

using Engine.Ecs;
using Engine.Ecs.Components;
using Engine.Ecs.Components.Enums;
using Engine.Ecs.Events.Interfaces;

namespace Sandbox.EngineExtensions;

public class AttackAnimationSubscriber : IEventSubscriber
{
    public void Register(World world)
    {
        world.Events.Subscribe<PlayerAttackEvent>(ev =>
        {
            var anim = ev.Attacker.GetComponent<Animation>();
            var dir = ev.Attacker.GetComponent<Direction>();

            if (dir.LastDirection == FacingDirection.Left)
            {
                anim?.TryPlay("attack_left");
            }
            else
            {
                anim?.TryPlay("attack_right");
            }
        });
    }
}

using Engine.Ecs.Components;
using Engine.Ecs.Systems.Interfaces;

namespace Engine.Ecs.Systems;

public class MovementSystem : ISystem
{
    public void Update(World world, float deltaTime)
    {
        foreach (var e in world.WithAll(typeof(Transform), typeof(Movement)))
        {
            var t = e.GetComponent<Transform>()!;
            var m = e.GetComponent<Movement>()!;

            t.X += m.DirX * m.Speed * deltaTime;
            t.Y += m.DirY * m.Speed * deltaTime;
        }
    }
}

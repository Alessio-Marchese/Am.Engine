using Engine.Ecs.Components;
using Engine.Utils;

namespace Engine.Ecs.Events.Listeners;

public class PhysicsCollisionSubscriber : IEventSubscriber
{
    public void Register(World world)
    {
        world.Events.Subscribe<CollisionEvent>(OnCollision);
    }

    private void OnCollision(CollisionEvent ev)
    {
        // --- 1) Extract required components from collided GameObjects ---
        // These are mandatory for collision resolution. If any is missing,
        // something is structurally wrong → throw explicit exception.
        var tA = ev.A.GetComponent<Transform>() ?? throw new ComponentNotFoundException(typeof(Transform));
        var tB = ev.B.GetComponent<Transform>() ?? throw new ComponentNotFoundException(typeof(Transform));
        var cA = ev.A.GetComponent<Collider>() ?? throw new ComponentNotFoundException(typeof(Transform));
        var cB = ev.B.GetComponent<Collider>() ?? throw new ComponentNotFoundException(typeof(Transform));
        var mA = ev.A.GetComponent<Movement>() ?? throw new ComponentNotFoundException(typeof(Transform));

        // --- 2) Calculate distance between centers ---
        // dx > 0 means A is to the RIGHT of B
        // dy > 0 means A is BELOW B
        float dx = tA.X - tB.X;
        float dy = tA.Y - tB.Y;

        // --- 3) Compute how much objects overlap on each axis ---
        // If overlapX < overlapY → collision is more horizontal than vertical.
        float overlapX = (cA.Width / 2 + cB.Width / 2) - Math.Abs(dx);
        float overlapY = (cA.Height / 2 + cB.Height / 2) - Math.Abs(dy);

        // TODO (cA.Width / 2 + cB.Width / 2) it's the calculation of the PIVOT
        // i have to create a centralized way to manage PIVOTS

        // --- 4) Resolve X axis collision (horizontal push) ---
        if (overlapX < overlapY)
        {
            if (dx > 0) tA.X += overlapX;
            else tA.X -= overlapX;

            mA?.DirX = 0;
        }
        // --- 5) Resolve Y axis collision (vertical push) ---
        else
        {
            if (dy > 0) tA.Y += overlapY;
            else tA.Y -= overlapY;

            mA?.DirY = 0;
        }
    }
}

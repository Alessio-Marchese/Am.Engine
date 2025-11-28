using Engine.Ecs.Components;
using Engine.Ecs.Events;
using Engine.Ecs.Systems.Interfaces;

namespace Engine.Ecs.Systems;

public class CollisionSystem : ISystem
{
    public void Update(World world, float deltaTime)
    {
        var colliders = world.WithAll(typeof(Transform), typeof(Collider)).ToList();

        for (int i = 0; i < colliders.Count; i++)
            for (int j = i + 1; j < colliders.Count; j++)
            {
                var A = colliders[i];
                var B = colliders[j];

                var tA = A.GetComponent<Transform>()!;
                var cA = A.GetComponent<Collider>()!;

                var tB = B.GetComponent<Transform>()!;
                var cB = B.GetComponent<Collider>()!;

                if (Collide(tA, cA, tB, cB))
                    world.Events.Publish(new CollisionEvent(A, B));
            }
    }

    private bool Collide(Transform tA, Collider cA, Transform tB, Collider cB)
    {
        //Calculate the pivot of both axys for both objects
        float cAHalfW = cA.Width / 2f * cA.Scale;
        float cAHalfH = cA.Height / 2f * cA.Scale;
        float cBHalfW = cB.Width / 2f * cB.Scale;
        float cBHalfH = cB.Height / 2f * cB.Scale;

        //Calculate the Left, Right, Top, Bottom margin for both objects
        float aLeft = tA.X - cAHalfW;
        float aRight = tA.X + cAHalfW;
        float aTop = tA.Y - cAHalfH;
        float aBottom = tA.Y + cAHalfH;

        float bLeft = tB.X - cBHalfW;
        float bRight = tB.X + cBHalfW;
        float bTop = tB.Y - cBHalfH;
        float bBottom = tB.Y + cBHalfH;

        //Execute the AABB algorithm (Axis-aligned bounding box) to understand if the two objects are colliding
        return aLeft < bRight && aRight > bLeft &&
               aTop < bBottom && aBottom > bTop;
    }
}

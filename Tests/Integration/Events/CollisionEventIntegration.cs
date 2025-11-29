using Engine.Ecs;
using Engine.Ecs.Components;
using Engine.Ecs.Events.Subscribers;
using Engine.Ecs.Systems;

namespace Tests.Integration.Events
{
    public class CollisionEventIntegration
    {
        /// <summary>
        /// Creates a simple reusable environment:
        /// </summary>   
        private (World world, GameObject mover, GameObject target) CreateWorld(
            float moverX, float moverY,
            float targetX, float targetY,
            float dirX = 1, float dirY = 0,
            int colliderSizeA = 100, int colliderSizeB = 100)
        {
            var world = new World()
                .AddSystem(new CollisionSystem())
                .RegisterSubscriber(new PhysicsCollisionSubscriber());

            var mover = world.AddGameObject()
                .AddComponent(new Transform { X = moverX, Y = moverY })
                .AddComponent(new Collider(colliderSizeA, colliderSizeA))
                .AddComponent(new Movement { DirX = dirX, DirY = dirY });

            var target = world.AddGameObject()
                .AddComponent(new Transform { X = targetX, Y = targetY })
                .AddComponent(new Collider(colliderSizeB, colliderSizeB));

            return (world, mover, target);
        }

        [Fact]
        public void CollisionOnXAxis_ShouldStopHorizontalMovementOnly()
        {
            var (world, mover, _) = CreateWorld(
                moverX: 0, moverY: 0,
                targetX: 80, targetY: 0,
                dirX: 1, dirY: 1
            );

            world.Update(1f);

            var m = mover.GetComponent<Movement>()!;
            Assert.Equal(0, m.DirX);
            Assert.Equal(1, m.DirY); 
        }

        [Fact]
        public void CollisionOnYAxis_ShouldStopVerticalMovementOnly()
        {
            var (world, mover, _) = CreateWorld(
                moverX: 0, moverY: 0,
                targetX: 0, targetY: 80,
                dirX: 1, dirY: 1
            );

            world.Update(1f);

            var m = mover.GetComponent<Movement>()!;
            Assert.Equal(1, m.DirX);
            Assert.Equal(0, m.DirY); 
        }

        [Fact]
        public void NoCollision_ShouldNotAffectMovement()
        {
            var (world, mover, _) = CreateWorld(
                moverX: 0, moverY: 0,
                targetX: 400, targetY: 400,
                dirX: 1, dirY: 1
            );

            world.Update(1f);

            var m = mover.GetComponent<Movement>()!;
            Assert.Equal(1, m.DirX);
            Assert.Equal(1, m.DirY);
        }
    }
}

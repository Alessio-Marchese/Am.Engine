using Engine.Ecs;
using Engine.Ecs.Components;
using Engine.Ecs.Events;
using Engine.Ecs.Events.Interfaces;
using Engine.Ecs.Systems;

namespace Tests.Integration.Events
{
    public class CollisionSystemIntegration
    {
        // Minimal subscriber that helps us to check if the subscriber is called after the event is published
        private class CaptureCollisionSubscriber : IEventSubscriber
        {
            public int Count { get; private set; } = 0;
            public void Register(World world) =>
                world.Events.Subscribe<CollisionEvent>(_ => Count++);
        }

        private (World world, CaptureCollisionSubscriber listener) CreateWorld(
            float ax, float ay, float bx, float by,
            int sizeA = 100, int sizeB = 100,
            float scaleA = 1f, float scaleB = 1f)
        {
            var listener = new CaptureCollisionSubscriber();

            var world = new World()
                .AddSystem(new CollisionSystem())
                .RegisterSubscriber(listener);

            world.AddGameObject()
                .AddComponent(new Transform { X = ax, Y = ay })
                .AddComponent(new Collider(sizeA, sizeA) { Scale = scaleA });

            world.AddGameObject()
                .AddComponent(new Transform { X = bx, Y = by })
                .AddComponent(new Collider(sizeB, sizeB) { Scale = scaleB });

            return (world, listener);
        }

        [Fact]
        public void ShouldPublishCollisionEvent_WhenObjectsOverlap()
        {
            var (world, listener) = CreateWorld(
                ax: 0, ay: 0,
                bx: 50, by: 0 // close enough to overlap on X
            );

            world.Update(1f);

            Assert.Equal(1, listener.Count);
        }

        [Fact]
        public void ShouldNotPublishEvent_WhenObjectsDoNotOverlap()
        {
            var (world, listener) = CreateWorld(
                ax: 0, ay: 0,
                bx: 300, by: 0
            );

            world.Update(1f);

            Assert.Equal(0, listener.Count);
        }

        [Fact]
        public void ShouldPublishEvent_OnVerticalCollision()
        {
            var (world, listener) = CreateWorld(
                ax: 0, ay: 0,
                bx: 0, by: 70 // vertical overlap
            );

            world.Update(1f);

            Assert.Equal(1, listener.Count);
        }

        [Fact]
        public void ShouldNotPublishEvent_WhenEdgesJustTouch()
        {
            var (world, listener) = CreateWorld(
                ax: 0, ay: 0,
                bx: 100, by: 0 // touching edge exactly
            );

            world.Update(1f);

            Assert.Equal(0, listener.Count); // by AABB math: must not collide
        }

        [Fact]
        public void ShouldPublishEvent_ConsideringColliderScale()
        {
            var (world, listener) = CreateWorld(
                ax: 0, ay: 0, scaleA: 2f,
                bx: 120, by: 0 // normally too far, but scale=2 makes hit
            );

            world.Update(1f);

            Assert.Equal(1, listener.Count);
        }
    }
}

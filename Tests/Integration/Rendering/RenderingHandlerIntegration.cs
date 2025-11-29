using Engine.Ecs;
using Engine.Ecs.Components;
using Engine.Rendering;
using Engine.Rendering.Interfaces;
using Engine.Rendering.Models;
using Engine.Utils;

namespace Tests.Integration.Rendering
{
    public class RenderingHandlerIntegration
    {
        /// <summary>
        /// Creates a simple reusable environment:
        /// </summary>        
        private World CreateSimpleRenderWorld()
        {
            var world = new World();

            world.AddGameObject().AddComponent(new Camera()); // Required by renderer

            world.AddGameObject()
                .AddComponent(new Transform())
                .AddComponent(new Sprite(new TextureHandle("SPRITE"), 100, 100));

            return world;
        }

        // Fake renderer — records draw operations so we can assert on them
        private class FakeRenderer : IRenderer
        {
            public bool BeginCalled { get; private set; }
            public bool EndCalled { get; private set; }
            public readonly List<(TextureHandle texture, int x, int y, int w, int h)> DrawCalls = new();
            public (float r, float g, float b, float a)? ClearCall;

            public void Begin() => BeginCalled = true;
            public void End() => EndCalled = true;

            public void DrawSprite(TextureHandle tex, int x, int y, int w, int h)
                => DrawCalls.Add((tex, x, y, h, w));

            public void Clear(float r, float g, float b, float a = 1f)
                => ClearCall = (r, g, b, a);
        }

        [Fact]
        public void Rendering_calls_begin_and_end()
        {
            // Arrange → Set up fake renderer and a minimal world that can be rendered
            var renderer = new FakeRenderer();
            var rendering = new RenderingHandler(renderer);
            var world = CreateSimpleRenderWorld();

            // Act → simulate one rendering frame
            rendering.Update(world);

            // Assert → Begin() and End() must be called every render cycle
            Assert.True(renderer.BeginCalled);
            Assert.True(renderer.EndCalled);
        }

        [Fact]
        public void Rendering_draws_in_correct_zIndex_order()
        {
            // Arrange → World with a camera + two renderable entities with different zIndex
            var renderer = new FakeRenderer();
            var rendering = new RenderingHandler(renderer);
            var world = new World();

            world.AddGameObject().AddComponent(new Camera()); // rendering cannot work without camera

            // Background with lower zIndex → should be drawn first
            world.AddGameObject()
                .AddComponent(new Transform())
                .AddComponent(new Sprite(new TextureHandle("BG"), 500, 500, zIndex: 0));

            // Player with higher zIndex → should be drawn after BG
            world.AddGameObject()
                .AddComponent(new Transform())
                .AddComponent(new Sprite(new TextureHandle("PLAYER"), 100, 100, zIndex: 10));

            // Act
            rendering.Update(world);

            // Assert → first draw must be BG, second must be PLAYER
            Assert.Equal("BG", renderer.DrawCalls[0].texture.Id);
            Assert.Equal("PLAYER", renderer.DrawCalls[1].texture.Id);
        }

        [Fact]
        public void Rendering_throws_if_camera_missing()
        {
            // Arrange → world has renderable objects, but no camera
            var renderer = new FakeRenderer();
            var rendering = new RenderingHandler(renderer);
            var world = new World();

            world.AddGameObject() // entity exists but there's no Camera component anywhere
                .AddComponent(new Transform())
                .AddComponent(new Sprite(new TextureHandle("OBJ"), 10, 10));

            // Assert → renderer must fail fast with exception
            Assert.Throws<GameObjectNotFoundException>(() => rendering.Update(world));
        }
    }
}

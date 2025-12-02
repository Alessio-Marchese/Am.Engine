using Engine.Ecs;
using Engine.Ecs.Animations;
using Engine.Ecs.Animations.Abstract;
using Engine.Ecs.Components;
using Engine.Ecs.Systems;
using Engine.Rendering.Models;

namespace Tests.Integration.Animations
{
    public class AnimationSystemIntegration
    {
        private World CreateWorld(
            Clip clip,
            float frameDuration = 0.1f,
            int frames = 3)
        {
            var world = new World()
                .AddSystem(new AnimationSystem());

            for (int i = 0; i < frames; i++)
                clip.Frames.Add(new AnimationFrame(new TextureHandle($"f{i}"), duration: frameDuration));

            world.AddGameObject()
                .AddComponent(new Sprite(new TextureHandle("initial"), 100, 100))
                .AddComponent(new Animation()
                    .AddClip("test", clip));

            return world;
        }

        [Fact]
        public void Animation_should_progress_to_next_frame()
        {
            var world = CreateWorld(new LoopClip(), frameDuration: 0.2f);
            var anim = world.Entities[0].GetComponent<Animation>()!;
            var sprite = world.Entities[0].GetComponent<Sprite>()!;
            var clip = anim.CurrentClip;

            world.Update(0.25f); // > 0.2f → switch frame

            Assert.Equal(1, anim.FrameIndex);
            Assert.Equal(clip.Frames[1].Texture, sprite.Texture);
        }

        [Fact]
        public void LoopClip_animation_should_loop()
        {
            var world = CreateWorld(new LoopClip(), frames: 2);
            var anim = world.Entities[0].GetComponent<Animation>()!;

            //Simulate 2 world updates, clip should end and index must be 0
            world.Update(0.1f); 
            world.Update(0.1f);

            Assert.Equal(0, anim.FrameIndex);
        }

        [Fact]
        public void OneShotClip_animation_should_play_next_clip()
        {
            var world = CreateWorld(new OneShotClip("test"), frames: 2);
            var anim = world.Entities[0].GetComponent<Animation>()!;
            var sprite = world.Entities[0].GetComponent<Sprite>()!;
            var clip = anim.CurrentClip;

            world.Update(0.1f);
            world.Update(0.1f);

            Assert.Equal("test", anim.Current);
        }
    }
}

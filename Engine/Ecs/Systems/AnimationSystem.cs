using Engine.Ecs.Animations;
using Engine.Ecs.Components;
using Engine.Ecs.Systems.Interfaces;

namespace Engine.Ecs.Systems
{
    public class AnimationSystem : ISystem
    {
        public void Update(World world, float dt)
        {
            foreach (var e in world.WithAll(typeof(Animation), typeof(Sprite)))
            {
                var anim = e.GetComponent<Animation>()!;

                var sprite = e.GetComponent<Sprite>()!;
                var clip = anim.CurrentClip;

                anim.Time += dt;

                if (anim.Time >= clip.Frames[anim.FrameIndex].Duration)
                {
                    anim.Time = 0f;
                    anim.FrameIndex++;

                    if (anim.FrameIndex >= clip.Frames.Count)
                    {
                        if (clip.GetType() == typeof(LoopClip))
                        {
                            anim.FrameIndex = 0;
                        }
                        else
                        {
                            anim.PlayNext();
                        }
                    }
                }

                sprite.Texture = clip.Frames[anim.FrameIndex].Texture;
            }
        }
    }
}

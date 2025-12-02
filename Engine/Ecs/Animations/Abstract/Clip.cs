using Engine.Rendering.Models;

namespace Engine.Ecs.Animations.Abstract;

public abstract class Clip
{
    public List<AnimationFrame> Frames { get; }
    public int Priority { get; }

    protected Clip(int priority = 0)
    {
        Priority = priority;
        Frames = new List<AnimationFrame>();
    }
    public Clip AddFrame(TextureHandle tex, float duration)
    {
        Frames.Add(new AnimationFrame(tex, duration));
        return this;
    }
}

using Engine.Rendering.Models;

namespace Engine.Ecs.Animations;

public class AnimationFrame
{
    public TextureHandle Texture { get; }
    public float Duration { get; }

    public AnimationFrame(TextureHandle texture, float duration)
    {
        Texture = texture;
        Duration = duration;
    }
}

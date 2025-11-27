using Engine.Ecs.Components.Interfaces;
using Engine.Rendering.Models;

namespace Engine.Ecs.Components;
public class Sprite : IComponent
{
    public TextureHandle Texture { get; }
    public int Width { get; }
    public int Height { get; }
    public float Scale { get; set; } = 1f;
    public int ZIndex = 0;

    public Sprite(TextureHandle tex, int width, int height, float scale = 1f, int zIndex = 0)
    {
        Texture = tex;
        Width = width;
        Height = height;
        ZIndex = zIndex;
    }
}

using Engine.Ecs;
using Engine.Ecs.Components;
using Engine.Rendering.Interfaces;

namespace Engine.Rendering;

public class RenderingHandler
{
    private readonly IRenderer _renderer;

    public RenderingHandler(IRenderer renderer)
    {
        _renderer = renderer;
    }

    public void Update(World world)
    {
        _renderer.Begin();

        foreach (var e in world.WithAll(typeof(Transform), typeof(Sprite)))
        {
            var t = e.GetComponent<Transform>()!;
            var s = e.GetComponent<Sprite>()!;

            _renderer.DrawSprite(s.Texture, t.X, t.Y, s.Width, s.Height, s.Scale);
        }

        _renderer.End();
    }

    public void Clear(float r, float g, float b, float a = 1f)
    {
        _renderer.Clear(r, g, b, a);
    }
}

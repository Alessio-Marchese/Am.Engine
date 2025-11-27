using Engine.Ecs;
using Engine.Ecs.Components;
using Engine.Rendering.Interfaces;
using Engine.Utils;

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

        var camEntity = world.With<Camera>().FirstOrDefault() ?? throw new GameObjectNotFoundException(typeof(Camera));

        var cam = camEntity?.GetComponent<Camera>()!;

        var renderables = world.WithAll(typeof(Transform), typeof(Sprite))
                               .Select(e => new { e, s = e.GetComponent<Sprite>()! })
                               .OrderBy(x => x.s.ZIndex)
                               .ToList();

        foreach (var r in renderables)
        {
            var t = r.e.GetComponent<Transform>()!;
            var s = r.s;

            float px = (t.X - cam.X) * cam.Zoom;
            float py = (t.Y - cam.Y) * cam.Zoom;

            _renderer.DrawSprite(s.Texture, px, py, s.Width, s.Height, s.Scale * cam.Zoom);
        }

        _renderer.End();
    }

    public void Clear(float r, float g, float b, float a = 1f)
    {
        _renderer.Clear(r, g, b, a);
    }
}

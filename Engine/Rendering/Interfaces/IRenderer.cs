using Engine.Rendering.Models;

namespace Engine.Rendering.Interfaces;

public interface IRenderer
{
    void Begin();
    void End();

    void Clear(float r, float g, float b, float a = 1f);

    void DrawSprite(TextureHandle handle, float x, float y, int width, int height, float scale);
}

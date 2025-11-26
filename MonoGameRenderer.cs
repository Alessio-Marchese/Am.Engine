using System;

public class MonoGameRenderer : IRenderer
{
    private readonly GraphicsDevice _graphicsDevice;
    private readonly SpriteBatch _spriteBatch;
    private readonly Dictionary<string, Texture2D> _textures = new();

    public MonoGameRenderer(GraphicsDevice graphicsDevice)
    {
        _graphicsDevice = graphicsDevice;
        _spriteBatch = new SpriteBatch(graphicsDevice);
    }

    public void RegisterTexture(TextureHandle handle, Texture2D texture)
    {
        _textures[handle.Id] = texture;
    }

    public void Begin()
    {
        _spriteBatch.Begin();
    }

    public void End()
    {
        _spriteBatch.End();
    }

    public void Clear(float r, float g, float b, float a = 1f)
    {
        _graphicsDevice.Clear(new Color(r, g, b, a));
    }

    public void DrawSprite(TextureHandle texture, float x, float y)
    {
        if (!_textures.TryGetValue(texture.Id, out var tex))
            return; // oppure throw

        _spriteBatch.Draw(tex, new Vector2(x, y), Color.White);
    }
}

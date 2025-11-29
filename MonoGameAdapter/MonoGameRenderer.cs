using Engine.Rendering.Interfaces;
using Engine.Rendering.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameAdapter;

public class MonoGameRenderer : IRenderer
{
    private readonly GraphicsDevice _graphicsDevice;
    private readonly SpriteBatch _spriteBatch;
    private readonly ITextureRegistry _textureRegistry;

    public MonoGameRenderer(GraphicsDevice graphicsDevice, ITextureRegistry textureRegistry)
    {
        _graphicsDevice = graphicsDevice;
        _spriteBatch = new SpriteBatch(graphicsDevice);
        _textureRegistry = textureRegistry;
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

    public void DrawSprite(TextureHandle handle, int x, int y, int width, int height)
    {
        if (!_textureRegistry.Exists(handle))
            throw new InvalidOperationException($"Texture '{handle.Id}' was not registered in the renderer.");

        var tex = _textureRegistry.Get(handle);

        _spriteBatch.Draw(
            (Texture2D)tex,
            new Rectangle(
                x,
                y,
                width,
                height
            ),
            Color.White
        );
    }
}

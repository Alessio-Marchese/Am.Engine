using Engine.Rendering.Abstract;
using Engine.Rendering.Models;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameAdapter;
public class MonoGameTextureRegistry : TextureRegistry
{
    public override void Register(TextureHandle handle, object nativeTexture)
    {
        if (nativeTexture.GetType() != typeof(Texture2D))
            throw new InvalidOperationException("Invalid texture MonoGame needs Texture2D Type");

        base.Register(handle, nativeTexture);
    }

    public override Texture2D Get(TextureHandle handle)
    {
        return (Texture2D)base.Get(handle);
    }
}

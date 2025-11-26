using Engine.Rendering.Interfaces;
using Engine.Rendering.Models;

namespace Engine.Rendering.Abstract;

public abstract class TextureLoader : ITextureLoader
{
    private readonly ITextureRegistry _textureRegistry;

    public TextureLoader(ITextureRegistry textureRegistry)
    {
        _textureRegistry = textureRegistry;
    }

    public virtual TextureHandle LoadTexture(string path, object nativeTexture)
    {
        var handle = new TextureHandle(path);

        _textureRegistry.Register(handle, nativeTexture);

        return handle;
    }
}

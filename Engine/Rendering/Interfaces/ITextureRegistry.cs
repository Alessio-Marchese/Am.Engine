using Engine.Rendering.Models;

namespace Engine.Rendering.Interfaces;

public interface ITextureRegistry
{
    void Register(TextureHandle handle, object nativeTexture);

    object Get(TextureHandle handle);

    bool Exists(TextureHandle handle);
}

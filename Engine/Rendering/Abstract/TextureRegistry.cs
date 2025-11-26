using Engine.Rendering.Interfaces;
using Engine.Rendering.Models;

namespace Engine.Rendering.Abstract;

public abstract class TextureRegistry : ITextureRegistry
{
    protected readonly Dictionary<string, object> _textures = new();

    public virtual void Register(TextureHandle handle, object nativeTexture)
    {
        if (_textures.ContainsKey(handle.Id))
            throw new InvalidOperationException($"Texture '{handle.Id}' already registered");

        _textures.Add(handle.Id, nativeTexture);
    }

    public virtual object Get(TextureHandle handle)
    {
        if (!_textures.TryGetValue(handle.Id, out var tex))
            throw new KeyNotFoundException($"Texture '{handle.Id}' not found in registry.");

        return tex;
    }

    public virtual bool Exists(TextureHandle handle) =>
        _textures.ContainsKey(handle.Id);
}

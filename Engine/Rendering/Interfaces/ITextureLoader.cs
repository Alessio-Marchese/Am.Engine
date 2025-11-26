using Engine.Rendering.Models;

namespace Engine.Rendering.Interfaces;

public interface ITextureLoader
{
    TextureHandle LoadTexture(string path, object nativeTexture);
}

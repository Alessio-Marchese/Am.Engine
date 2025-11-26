using Engine.Rendering.Abstract;
using Engine.Rendering.Interfaces;

namespace MonoGameAdapter;

public class MonoGameTextureLoader : TextureLoader
{
    public MonoGameTextureLoader(ITextureRegistry textureRegistry) : base(textureRegistry)
    {
    }
}

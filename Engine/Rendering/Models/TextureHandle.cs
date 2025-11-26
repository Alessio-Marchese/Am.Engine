namespace Engine.Rendering.Models;
public readonly struct TextureHandle
{
    public string Id { get; }

    public TextureHandle(string id)
    {
        Id = id;
    }

    public override string ToString() => Id;
}

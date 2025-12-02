using Engine.Ecs.Animations;
using Engine.Ecs.Animations.Abstract;
using Engine.Ecs.Components.Interfaces;

namespace Engine.Ecs.Components;

public class Animation : IComponent
{
    private readonly Dictionary<string, Clip> _clips = new();

    public string Current { get; private set; } = "";
    public int FrameIndex { get; set; } = 0;
    public float Time { get; set; } = 0f;

    public Clip CurrentClip => _clips[Current];

    public Animation(string name = "")
    {
        Current = name;
    }

    public Animation AddClip(string name, Clip clip)
    {
        _clips[name] = clip;
        if (Current == "") Current = name; //Set this clip as current if current is empty
        return this;
    }

    public void TryPlay(string name)
    {
        if (name == Current) return;

        if (CurrentClip.Priority > _clips[name].Priority)
            return;

        Current = name;
        FrameIndex = 0;
        Time = 0f;
    }

    public void PlayNext()
    {
        if (CurrentClip.GetType() != typeof(OneShotClip))
            return;

        var oneShotClip = (OneShotClip)CurrentClip;

        Current = oneShotClip.FollowUpClip;
        FrameIndex = 0;
        Time = 0f;
    }
}

using Engine.Ecs.Animations.Abstract;

namespace Engine.Ecs.Animations;

public class OneShotClip : Clip
{
    public string FollowUpClip { get; set; }

    public OneShotClip(string followUpClip, int priority = 0) : base(priority)
    {
        FollowUpClip = followUpClip;
    }
}

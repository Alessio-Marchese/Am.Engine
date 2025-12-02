using Engine.Ecs;
using Engine.Ecs.Events.Interfaces;

namespace Sandbox.EngineExtensions;

public record PlayerRunEvent(GameObject Player, bool IsLeft) : IEvent { }
public record PlayerAttackEvent(GameObject Attacker) : IEvent { }
public record PlayerIdleEvent(GameObject Player) : IEvent { }


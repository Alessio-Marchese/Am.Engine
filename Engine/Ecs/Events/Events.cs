namespace Engine.Ecs.Events;

public record CollisionEvent(GameObject A, GameObject B) : IEvent { }


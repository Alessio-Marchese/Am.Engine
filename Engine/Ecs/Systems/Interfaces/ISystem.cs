namespace Engine.Ecs.Systems.Interfaces;

public interface ISystem
{
    void Update(World world, float deltaTime);
}

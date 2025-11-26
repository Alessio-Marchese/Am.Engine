namespace Am.Engine.Ecs;

public interface ISystem
{
    void Update(World world, float deltaTime);
}

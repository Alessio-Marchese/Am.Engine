using Am.Engine.Components;
using Am.Engine.Ecs;
using Am.Engine.Systems;

namespace Am.Engine.Tests.Integration.Systems;

public class MovementSystemIntegration
{
    private (World world, GameObject obj) SetupEnvironment(
        float x = 0, float y = 0,
        float speed = 1, float dirX = 0, float dirY = 0)
    {
        var world = new World()
            .AddSystem(new MovementSystem());

        var obj = world.AddGameObject()
                       .AddComponent(new Transform { X = x, Y = y })
                       .AddComponent(new Movement { Speed = speed, DirX = dirX, DirY = dirY });

        return (world, obj);
    }

    [Fact]
    public void Moves_right_when_dirX_positive()
    {
        var (world, obj) = SetupEnvironment(dirX: 1, dirY: 0);
        world.Update(1f);

        var t = obj.GetComponent<Transform>()!;
        Assert.Equal(1f, t.X);
        Assert.Equal(0f, t.Y);
    }

    [Fact]
    public void Moves_left_when_dirX_negative()
    {
        var (world, obj) = SetupEnvironment(dirX: -1);
        world.Update(1f);

        var t = obj.GetComponent<Transform>()!;
        Assert.Equal(-1f, t.X);
        Assert.Equal(0f, t.Y);
    }

    [Fact]
    public void Moves_top_when_ditY_positive()
    {
        var (world, obj) = SetupEnvironment(dirX: 0, dirY: 1);
        world.Update(1f);

        var t = obj.GetComponent<Transform>()!;
        Assert.Equal(0f, t.X);
        Assert.Equal(1f, t.Y);
    }

    [Fact]
    public void Moves_bottom_when_ditY_negative()
    {
        var (world, obj) = SetupEnvironment(dirX: 0, dirY: -1);
        world.Update(1f);

        var t = obj.GetComponent<Transform>()!;
        Assert.Equal(0f, t.X);
        Assert.Equal(-1f, t.Y);
    }

    [Fact]
    public void Movement_is_scaled_by_deltaTime()
    {
        var (world, obj) = SetupEnvironment(speed: 2, dirX: 1);
        world.Update(0.5f);

        var t = obj.GetComponent<Transform>()!;
        Assert.Equal(1f, t.X); // 2 speed * 0.5s = 1
    }

    [Fact]
    public void Multiple_updates_accumulate_correctly()
    {
        var (world, obj) = SetupEnvironment(dirX: 1);
        world.Update(1f);
        world.Update(1f);
        world.Update(1f);

        var t = obj.GetComponent<Transform>()!;
        Assert.Equal(3f, t.X);
    }

    [Fact]
    public void Does_not_move_if_speed_zero()
    {
        var (world, obj) = SetupEnvironment(speed: 0);
        world.Update(1f);

        var t = obj.GetComponent<Transform>()!;
        Assert.Equal(0f, t.X);
        Assert.Equal(0f, t.Y);
    }

    [Fact]
    public void Does_not_move_if_direction_zero()
    {
        var (world, obj) = SetupEnvironment(dirX: 0, dirY: 0);
        world.Update(1f);

        var t = obj.GetComponent<Transform>()!;
        Assert.Equal(0f, t.X);
        Assert.Equal(0f, t.Y);
    }

    [Fact]
    public void Moves_top_right()
    {
        var (world, obj) = SetupEnvironment(dirX: 1, dirY: 1, speed: 2);
        world.Update(1f);

        var t = obj.GetComponent<Transform>()!;
        Assert.Equal(2f, t.X);
        Assert.Equal(2f, t.Y);
    }

    [Fact]
    public void Moves_top_left()
    {
        var (world, obj) = SetupEnvironment(dirX: -1, dirY: 1, speed: 2);
        world.Update(1f);

        var t = obj.GetComponent<Transform>()!;
        Assert.Equal(-2f, t.X);
        Assert.Equal(2f, t.Y);
    }

    [Fact]
    public void Moves_bottom_right()
    {
        var (world, obj) = SetupEnvironment(dirX: 1, dirY: -1, speed: 3);
        world.Update(1f);

        var t = obj.GetComponent<Transform>()!;
        Assert.Equal(3f, t.X);
        Assert.Equal(-3f, t.Y);
    }

    [Fact]
    public void Moves_bottom_left()
    {
        var (world, obj) = SetupEnvironment(dirX: -1, dirY: -1, speed: 3);
        world.Update(1f);

        var t = obj.GetComponent<Transform>()!;
        Assert.Equal(-3f, t.X);
        Assert.Equal(-3f, t.Y);
    }
}

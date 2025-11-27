using Engine.Ecs;
using Engine.Ecs.Components;
using Engine.Ecs.Systems;

namespace Am.Engine.Tests.Integration.Systems;

public class MovementSystemIntegration
{
    /// <summary>
    /// Creates a World with MovementSystem active and a single GameObject
    /// containing Transform + Movement component. All test cases reuse this.
    /// </summary>
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
        // Movement (1,0) for 1 second → should move +1 on X axis
        var (world, obj) = SetupEnvironment(dirX: 1, dirY: 0);
        world.Update(1f); // simulate 1 second

        var t = obj.GetComponent<Transform>()!;
        Assert.Equal(1f, t.X);
        Assert.Equal(0f, t.Y);
    }

    [Fact]
    public void Moves_left_when_dirX_negative()
    {
        // Movement (-1,0) for 1 second → should move -1 on X axis
        var (world, obj) = SetupEnvironment(dirX: -1);
        world.Update(1f);

        var t = obj.GetComponent<Transform>()!;
        Assert.Equal(-1f, t.X);
        Assert.Equal(0f, t.Y);
    }

    [Fact]
    public void Moves_up_when_dirY_positive()
    {
        // Movement (0,1) for 1 second → should increase Y by 1
        var (world, obj) = SetupEnvironment(dirY: 1);
        world.Update(1f);

        var t = obj.GetComponent<Transform>()!;
        Assert.Equal(0f, t.X);
        Assert.Equal(1f, t.Y);
    }

    [Fact]
    public void Moves_down_when_dirY_negative()
    {
        // Movement (0,-1) for 1 second → should decrease Y by 1
        var (world, obj) = SetupEnvironment(dirY: -1);
        world.Update(1f);

        var t = obj.GetComponent<Transform>()!;
        Assert.Equal(0f, t.X);
        Assert.Equal(-1f, t.Y);
    }

    [Fact]
    public void Movement_is_scaled_by_deltaTime()
    {
        // speed 2 * dt 0.5 = movement of 1 unit → delta must affect movement
        var (world, obj) = SetupEnvironment(speed: 2, dirX: 1);
        world.Update(0.5f);

        var t = obj.GetComponent<Transform>()!;
        Assert.Equal(1f, t.X);
    }

    [Fact]
    public void Multiple_updates_accumulate_over_time()
    {
        // Movement (1,0) → 3 seconds total = position should end at X=3
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
        // speed=0 means movement should never occur regardless of direction
        var (world, obj) = SetupEnvironment(speed: 0);
        world.Update(1f);

        var t = obj.GetComponent<Transform>()!;
        Assert.Equal(0f, t.X);
        Assert.Equal(0f, t.Y);
    }

    [Fact]
    public void Does_not_move_if_direction_zero()
    {
        // No direction → no displacement even with speed >0
        var (world, obj) = SetupEnvironment(dirX: 0, dirY: 0);
        world.Update(1f);

        var t = obj.GetComponent<Transform>()!;
        Assert.Equal(0f, t.X);
        Assert.Equal(0f, t.Y);
    }

    [Fact]
    public void Moves_top_right()
    {
        // (DirX=1, DirY=1, Speed=2) → 1 second → both axes +2
        var (world, obj) = SetupEnvironment(dirX: 1, dirY: 1, speed: 2);
        world.Update(1f);

        var t = obj.GetComponent<Transform>()!;
        Assert.Equal(2f, t.X);
        Assert.Equal(2f, t.Y);
    }

    [Fact]
    public void Moves_top_left()
    {
        // (DirX=-1, DirY=1, Speed=2) → 1 second → X=-2, Y=+2
        var (world, obj) = SetupEnvironment(dirX: -1, dirY: 1, speed: 2);
        world.Update(1f);

        var t = obj.GetComponent<Transform>()!;
        Assert.Equal(-2f, t.X);
        Assert.Equal(2f, t.Y);
    }

    [Fact]
    public void Moves_bottom_right()
    {
        // (DirX=1, DirY=-1, Speed=3) → 1 second → X=3, Y=-3
        var (world, obj) = SetupEnvironment(dirX: 1, dirY: -1, speed: 3);
        world.Update(1f);

        var t = obj.GetComponent<Transform>()!;
        Assert.Equal(3f, t.X);
        Assert.Equal(-3f, t.Y);
    }

    [Fact]
    public void Moves_bottom_left()
    {
        // (DirX=-1, DirY=-1, Speed=3) → 1 second → X=-3, Y=-3
        var (world, obj) = SetupEnvironment(dirX: -1, dirY: -1, speed: 3);
        world.Update(1f);

        var t = obj.GetComponent<Transform>()!;
        Assert.Equal(-3f, t.X);
        Assert.Equal(-3f, t.Y);
    }
}

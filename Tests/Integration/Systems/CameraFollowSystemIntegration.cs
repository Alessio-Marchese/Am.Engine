using Engine.Ecs;
using Engine.Ecs.Components;
using Engine.Ecs.Components.Tags;
using Engine.Ecs.Systems;
using Engine.Utils;

namespace Tests.Integration.Systems;

public class CameraFollowSystemIntegration
{
    /// <summary>
    /// Creates a reusable environment for camera-follow testing:
    /// - a World instance
    /// - a Player entity with transform
    /// - a Camera entity configured with offset, speed and starting position
    /// - viewport is injected via Func (MonoGame independent)
    /// </summary>
    private (World world, GameObject camera, GameObject player) SetupCameraEnvironment(
        float playerX = 0, float playerY = 0,
        float camX = 0, float camY = 0,
        float offsetX = 0, float offsetY = 0,
        float followSpeed = 1f,
        int viewportW = 800, int viewportH = 600)
    {
        var world = new World();

        // Player entity: represents the follow target
        var player = world.AddGameObject()
            .AddComponent(new PlayerTag())
            .AddComponent(new Transform { X = playerX, Y = playerY });

        // Camera entity: contains position, offset, follow behavior
        var camera = world.AddGameObject()
            .AddComponent(new Camera(offsetX, offsetY)
            {
                X = camX,
                Y = camY,
                FollowSpeed = followSpeed
            });

        // Attach camera-follow system (injecting viewport size externally)
        var system = new CameraFollowSystem(() => (viewportW, viewportH));
        world.AddSystem(system);

        return (world, camera, player);
    }

    [Fact]
    public void Camera_moves_to_player_position_when_followSpeed_is_1()
    {
        // A followSpeed of 1 should snap the camera directly to the correct follow target in a single update

        const int viewportWidth = 800;
        const int viewportHeight = 600;
        const float playerStartX = 500;
        const float playerStartY = 300;

        var (world, cameraEntity, _) = SetupCameraEnvironment(
            playerX: playerStartX,
            playerY: playerStartY,
            viewportW: viewportWidth,
            viewportH: viewportHeight,
            followSpeed: 1f
        );

        world.Update(deltaTime: 1f); // simulate 1 second of world time

        var camera = cameraEntity.GetComponent<Camera>()!;
        float expectedX = playerStartX - (viewportWidth / 2f);
        float expectedY = playerStartY - (viewportHeight / 2f);

        Assert.Equal(expectedX, camera.X); // camera should center exactly on target
        Assert.Equal(expectedY, camera.Y);
    }

    [Fact]
    public void Camera_moves_only_fraction_of_distance_when_followSpeed_is_low()
    {
        // followSpeed < 1 means camera should NOT reach the target immediately, only move toward it

        const float playerStartX = 1000;
        const float playerStartY = 600;
        const float followSpeed = 0.25f;
        const int viewportWidth = 800;
        const int viewportHeight = 600;

        var (world, cameraEntity, _) = SetupCameraEnvironment(
            playerX: playerStartX,
            playerY: playerStartY,
            followSpeed: followSpeed,
            viewportW: viewportWidth,
            viewportH: viewportHeight
        );

        world.Update(deltaTime: 1f);

        var camera = cameraEntity.GetComponent<Camera>()!;
        float expectedTargetX = playerStartX - (viewportWidth / 2f);
        float expectedTargetY = playerStartY - (viewportHeight / 2f);

        // Camera should be BETWEEN initial and target position (partial movement)
        Assert.InRange(camera.X, 0, expectedTargetX);
        Assert.InRange(camera.Y, 0, expectedTargetY);
    }

    [Fact]
    public void Throws_if_camera_missing()
    {
        // System requires a Camera component; a missing camera should fail-fast

        var world = new World();
        world.AddGameObject().AddComponent(new PlayerTag()).AddComponent(new Transform());

        var system = new CameraFollowSystem(() => (800, 600));

        Assert.Throws<GameObjectNotFoundException>(() => system.Update(world, 1f));
    }

    [Fact]
    public void Throws_if_player_missing()
    {
        // If no PlayerTag entity exists, camera cannot follow → exception expected

        var world = new World();
        world.AddGameObject().AddComponent(new Camera());

        var system = new CameraFollowSystem(() => (800, 600));

        Assert.Throws<GameObjectNotFoundException>(() => system.Update(world, 1f));
    }
}

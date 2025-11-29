using Engine.Ecs;
using Engine.Ecs.Components;
using Engine.Ecs.Components.Tags;
using Engine.Ecs.Events.Subscribers;
using Engine.Ecs.Systems;
using Engine.Rendering;
using Engine.Rendering.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameAdapter;

namespace Sandbox
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private ITextureLoader _textureLoader;
        private RenderingHandler _renderSystem;
        private World _world;
        private GameObject _player;
        private GameObject _grassHandle;
        private GameObject _background;
        private GameObject _camera;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _world = new World()
                .AddSystem(new MovementSystem())
                .AddSystem(new CameraFollowSystem(() =>
                    (
                        GraphicsDevice.Viewport.Width,
                        GraphicsDevice.Viewport.Height
                    ))
                )
                .AddSystem(new CollisionSystem())
                .RegisterSubscriber(new PhysicsCollisionSubscriber());

            var registry = new MonoGameTextureRegistry();

            _renderSystem = new RenderingHandler(new MonoGameRenderer(GraphicsDevice, registry));
            _textureLoader = new MonoGameTextureLoader(registry);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            var playerHandle = _textureLoader.LoadTexture("alienGreen", Content.Load<Texture2D>("alienGreen")); 
            var grassHandle = _textureLoader.LoadTexture("grassBlock", Content.Load<Texture2D>("grassBlock")); 
            var backgroundHandle = _textureLoader.LoadTexture("backgroundColorForest", Content.Load<Texture2D>("backgroundColorForest")); 

            _player = _world.AddGameObject()
                .AddComponent(new Collider(100, 150))
                .AddComponent(new PlayerTag())
                .AddComponent(new Transform { X = 0, Y = 0 })
                .AddComponent(new Movement { Speed = 400 })
                .AddComponent(new Sprite(playerHandle, 100, 150, zIndex: 1));

            _grassHandle = _world.AddGameObject()
                .AddComponent(new Collider(100, 100))
                .AddComponent(new Transform { X = 200, Y = 0 })
                .AddComponent(new Sprite(grassHandle, 100, 100, zIndex: 1));

            _background = _world.AddGameObject()
                .AddComponent(new Transform { X = 0, Y = -150 })
                .AddComponent(new Sprite(backgroundHandle, 3000, GraphicsDevice.Viewport.Height * 2));

            _camera = _world.AddGameObject()
                .AddComponent(new Camera(0, 0, 1));
        }

        protected override void Update(GameTime gameTime)
        {
            var k = Keyboard.GetState();

            var movement = _player.GetComponent<Movement>()!;
            movement.DirX = (k.IsKeyDown(Keys.Right) ? 1 : 0) - (k.IsKeyDown(Keys.Left) ? 1 : 0);
            movement.DirY = (k.IsKeyDown(Keys.Down) ? 1 : 0) - (k.IsKeyDown(Keys.Up) ? 1 : 0);

            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _world.Update(dt);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _renderSystem.Clear(0.1f, 0.1f, 0.1f);
            _renderSystem.Update(_world);

            base.Draw(gameTime);
        }
    }
}

using Engine.Ecs;
using Engine.Ecs.Components;
using Engine.Ecs.Components.Tags;
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
                );

            var registry = new MonoGameTextureRegistry();

            _renderSystem = new RenderingHandler(new MonoGameRenderer(GraphicsDevice, registry));
            _textureLoader = new MonoGameTextureLoader(registry);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            var playerHandle = _textureLoader.LoadTexture("player", Content.Load<Texture2D>("player")); 
            var backgroundHandle = _textureLoader.LoadTexture("background", Content.Load<Texture2D>("background")); 

            _player = _world.AddGameObject()
                .AddComponent(new PlayerTag())
                .AddComponent(new Transform { X = 0, Y = 0 })
                .AddComponent(new Movement { Speed = 400 })
                .AddComponent(new Sprite(playerHandle, 200, 200, zIndex: 1));

            _background = _world.AddGameObject()
                .AddComponent(new Transform { X = 0, Y = -150 })
                .AddComponent(new Sprite(backgroundHandle, 3000, GraphicsDevice.Viewport.Height * 2));

            _camera = _world.AddGameObject()
                .AddComponent(new Camera(0, -100));
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

using Engine.Ecs;
using Engine.Ecs.Components;
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

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _world = new World()
                .AddSystem(new MovementSystem());

            var registry = new MonoGameTextureRegistry();

            _renderSystem = new RenderingHandler(new MonoGameRenderer(GraphicsDevice, registry));
            _textureLoader = new MonoGameTextureLoader(registry);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            var texHandle = _textureLoader.LoadTexture("player", Content.Load<Texture2D>("player")); 

            int centerX = GraphicsDevice.Viewport.Width / 2;
            int centerY = GraphicsDevice.Viewport.Height / 2;

            _player = _world.AddGameObject()
                .AddComponent(new Transform { X = centerX, Y = centerY })
                .AddComponent(new Movement { Speed = 1000 })
                .AddComponent(new Sprite(texHandle, 200, 200));
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

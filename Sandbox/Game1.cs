using Engine.Ecs;
using Engine.Ecs.Animations;
using Engine.Ecs.Components;
using Engine.Ecs.Components.Enums;
using Engine.Ecs.Components.Tags;
using Engine.Ecs.Events;
using Engine.Ecs.Events.Subscribers;
using Engine.Ecs.Systems;
using Engine.Rendering;
using Engine.Rendering.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameAdapter;
using Sandbox.EngineExtensions;

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
                .AddSystem(new AnimationSystem())
                .RegisterSubscriber(new PhysicsCollisionSubscriber())
                .RegisterSubscriber(new RunAnimationSubscriber())
                .RegisterSubscriber(new IdleAnimationSubscriber())
                .RegisterSubscriber(new AttackAnimationSubscriber());

            var registry = new MonoGameTextureRegistry();

            _renderSystem = new RenderingHandler(new MonoGameRenderer(GraphicsDevice, registry));
            _textureLoader = new MonoGameTextureLoader(registry);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            #region Load Textures
            var playerIdleRight0 = _textureLoader.LoadTexture("PlayerIdleRight/idle_right_0", Content.Load<Texture2D>("PlayerIdleRight/idle_right_0"));
            var playerIdleRight1 = _textureLoader.LoadTexture("PlayerIdleRight/idle_right_1", Content.Load<Texture2D>("PlayerIdleRight/idle_right_1"));
            var playerIdleRight2 = _textureLoader.LoadTexture("PlayerIdleRight/idle_right_2", Content.Load<Texture2D>("PlayerIdleRight/idle_right_2"));
            var playerIdleRight3 = _textureLoader.LoadTexture("PlayerIdleRight/idle_right_3", Content.Load<Texture2D>("PlayerIdleRight/idle_right_3"));
            var playerIdleRight4 = _textureLoader.LoadTexture("PlayerIdleRight/idle_right_4", Content.Load<Texture2D>("PlayerIdleRight/idle_right_4"));
            var playerIdleRight5 = _textureLoader.LoadTexture("PlayerIdleRight/idle_right_5", Content.Load<Texture2D>("PlayerIdleRight/idle_right_5"));
            var playerIdleRight6 = _textureLoader.LoadTexture("PlayerIdleRight/idle_right_6", Content.Load<Texture2D>("PlayerIdleRight/idle_right_6"));
            var playerIdleRight7 = _textureLoader.LoadTexture("PlayerIdleRight/idle_right_7", Content.Load<Texture2D>("PlayerIdleRight/idle_right_7"));
            var playerIdleLeft0 = _textureLoader.LoadTexture("PlayerIdleLeft/idle_left_0", Content.Load<Texture2D>("PlayerIdleLeft/idle_left_0"));
            var playerIdleLeft1 = _textureLoader.LoadTexture("PlayerIdleLeft/idle_left_1", Content.Load<Texture2D>("PlayerIdleLeft/idle_left_1"));
            var playerIdleLeft2 = _textureLoader.LoadTexture("PlayerIdleLeft/idle_left_2", Content.Load<Texture2D>("PlayerIdleLeft/idle_left_2"));
            var playerIdleLeft3 = _textureLoader.LoadTexture("PlayerIdleLeft/idle_left_3", Content.Load<Texture2D>("PlayerIdleLeft/idle_left_3"));
            var playerIdleLeft4 = _textureLoader.LoadTexture("PlayerIdleLeft/idle_left_4", Content.Load<Texture2D>("PlayerIdleLeft/idle_left_4"));
            var playerIdleLeft5 = _textureLoader.LoadTexture("PlayerIdleLeft/idle_left_5", Content.Load<Texture2D>("PlayerIdleLeft/idle_left_5"));
            var playerIdleLeft6 = _textureLoader.LoadTexture("PlayerIdleLeft/idle_left_6", Content.Load<Texture2D>("PlayerIdleLeft/idle_left_6"));
            var playerIdleLeft7 = _textureLoader.LoadTexture("PlayerIdleLeft/idle_left_7", Content.Load<Texture2D>("PlayerIdleLeft/idle_left_7"));
            var playerRunRight0 = _textureLoader.LoadTexture("PlayerRunRight/run_right_0", Content.Load<Texture2D>("PlayerRunRight/run_right_0"));
            var playerRunRight1 = _textureLoader.LoadTexture("PlayerRunRight/run_right_1", Content.Load<Texture2D>("PlayerRunRight/run_right_1"));
            var playerRunRight2 = _textureLoader.LoadTexture("PlayerRunRight/run_right_2", Content.Load<Texture2D>("PlayerRunRight/run_right_2"));
            var playerRunRight3 = _textureLoader.LoadTexture("PlayerRunRight/run_right_3", Content.Load<Texture2D>("PlayerRunRight/run_right_3"));
            var playerRunRight4 = _textureLoader.LoadTexture("PlayerRunRight/run_right_4", Content.Load<Texture2D>("PlayerRunRight/run_right_4"));
            var playerRunRight5 = _textureLoader.LoadTexture("PlayerRunRight/run_right_5", Content.Load<Texture2D>("PlayerRunRight/run_right_5"));
            var playerRunLeft0 = _textureLoader.LoadTexture("PlayerRunLeft/run_left_0", Content.Load<Texture2D>("PlayerRunLeft/run_left_0"));
            var playerRunLeft1 = _textureLoader.LoadTexture("PlayerRunLeft/run_left_1", Content.Load<Texture2D>("PlayerRunLeft/run_left_1"));
            var playerRunLeft2 = _textureLoader.LoadTexture("PlayerRunLeft/run_left_2", Content.Load<Texture2D>("PlayerRunLeft/run_left_2"));
            var playerRunLeft3 = _textureLoader.LoadTexture("PlayerRunLeft/run_left_3", Content.Load<Texture2D>("PlayerRunLeft/run_left_3"));
            var playerRunLeft4 = _textureLoader.LoadTexture("PlayerRunLeft/run_left_4", Content.Load<Texture2D>("PlayerRunLeft/run_left_4"));
            var playerRunLeft5 = _textureLoader.LoadTexture("PlayerRunLeft/run_left_5", Content.Load<Texture2D>("PlayerRunLeft/run_left_5"));
            var playerAttackRight0 = _textureLoader.LoadTexture("AttackRight/attack_right_0", Content.Load<Texture2D>("AttackRight/attack_right_0"));
            var playerAttackRight1 = _textureLoader.LoadTexture("AttackRight/attack_right_1", Content.Load<Texture2D>("AttackRight/attack_right_1"));
            var playerAttackRight2 = _textureLoader.LoadTexture("AttackRight/attack_right_2", Content.Load<Texture2D>("AttackRight/attack_right_2"));
            var playerAttackRight3 = _textureLoader.LoadTexture("AttackRight/attack_right_3", Content.Load<Texture2D>("AttackRight/attack_right_3"));
            var playerAttackLeft0 = _textureLoader.LoadTexture("AttackLeft/attack_left_0", Content.Load<Texture2D>("AttackLeft/attack_left_0"));
            var playerAttackLeft1 = _textureLoader.LoadTexture("AttackLeft/attack_left_1", Content.Load<Texture2D>("AttackLeft/attack_left_1"));
            var playerAttackLeft2 = _textureLoader.LoadTexture("AttackLeft/attack_left_2", Content.Load<Texture2D>("AttackLeft/attack_left_2"));
            var playerAttackLeft3 = _textureLoader.LoadTexture("AttackLeft/attack_left_3", Content.Load<Texture2D>("AttackLeft/attack_left_3"));
            var grassHandle = _textureLoader.LoadTexture("grassBlock", Content.Load<Texture2D>("grassBlock"));
            var backgroundHandle = _textureLoader.LoadTexture("backgroundColorForest", Content.Load<Texture2D>("backgroundColorForest"));
            #endregion

            _player = _world.AddGameObject()
                .AddComponent(new PlayerTag()) // A tag used to recognize the player
                .AddComponent(new Collider(100, 200)) // This game object can collide with any other collider object
                .AddComponent(new Transform { X = 0, Y = 0 }) // Can have a position in the world
                .AddComponent(new Movement { Speed = 300 }) // Can move
                .AddComponent(new Sprite(playerIdleRight0, 100, 200, zIndex: 1)) // Can be rendered on the world
                .AddComponent(new Direction()) // Can have a direction (this component is used to keep track of the last direction of the game object, usefull for Animation)
                .AddComponent(new Animation("idle_right") // Can have animations + Configuration of the followings
                    .AddClip("attack_right",
                        new OneShotClip("idle_right", 2)
                            .AddFrame(playerAttackRight0, 0.15f)
                            .AddFrame(playerAttackRight1, 0.1f)
                            .AddFrame(playerAttackRight2, 0.05f)
                            .AddFrame(playerAttackRight3, 0.15f)
                    )
                    .AddClip("attack_left",
                        new OneShotClip("idle_left", 2)
                            .AddFrame(playerAttackLeft0, 0.15f)
                            .AddFrame(playerAttackLeft1, 0.1f)
                            .AddFrame(playerAttackLeft2, 0.05f)
                            .AddFrame(playerAttackLeft3, 0.15f)
                    )
                    .AddClip("idle_right",
                        new LoopClip(1)
                            .AddFrame(playerIdleRight0, 0.05f)
                            .AddFrame(playerIdleRight1, 0.05f)
                            .AddFrame(playerIdleRight2, 0.05f)
                            .AddFrame(playerIdleRight3, 0.05f)
                            .AddFrame(playerIdleRight4, 0.05f)
                            .AddFrame(playerIdleRight5, 0.05f)
                            .AddFrame(playerIdleRight6, 0.05f)
                            .AddFrame(playerIdleRight7, 0.05f)
                    )
                    .AddClip("idle_left",
                        new LoopClip(1)
                            .AddFrame(playerIdleLeft0, 0.05f)
                            .AddFrame(playerIdleLeft1, 0.05f)
                            .AddFrame(playerIdleLeft2, 0.05f)
                            .AddFrame(playerIdleLeft3, 0.05f)
                            .AddFrame(playerIdleLeft4, 0.05f)
                            .AddFrame(playerIdleLeft5, 0.05f)
                            .AddFrame(playerIdleLeft6, 0.05f)
                            .AddFrame(playerIdleLeft7, 0.05f)
                    )
                    .AddClip("run_right",
                        new LoopClip(1)
                            .AddFrame(playerRunRight0, 0.05f)
                            .AddFrame(playerRunRight1, 0.05f)
                            .AddFrame(playerRunRight2, 0.05f)
                            .AddFrame(playerRunRight3, 0.05f)
                            .AddFrame(playerRunRight4, 0.05f)
                            .AddFrame(playerRunRight5, 0.05f)
                    )
                    .AddClip("run_left",
                        new LoopClip(1)
                            .AddFrame(playerRunLeft0, 0.05f)
                            .AddFrame(playerRunLeft1, 0.05f)
                            .AddFrame(playerRunLeft2, 0.05f)
                            .AddFrame(playerRunLeft3, 0.05f)
                            .AddFrame(playerRunLeft4, 0.05f)
                            .AddFrame(playerRunLeft5, 0.05f)
                    ));

            _grassHandle = _world.AddGameObject()
                .AddComponent(new Collider(100, 100))
                .AddComponent(new Transform { X = 300, Y = 0 })
                .AddComponent(new Sprite(grassHandle, 100, 100, zIndex: 1));

            _background = _world.AddGameObject()
                .AddComponent(new Transform { X = 0, Y = 0 })
                .AddComponent(new Sprite(backgroundHandle, GraphicsDevice.Viewport.Width * 2, GraphicsDevice.Viewport.Height * 2));

            _camera = _world.AddGameObject()
                .AddComponent(new Camera(0, 0, 1));
        }

        protected override void Update(GameTime gameTime)
        {
            var k = Keyboard.GetState();

            var movement = _player.GetComponent<Movement>()!;

            movement.DirX = (k.IsKeyDown(Keys.Right) ? 1 : 0) - (k.IsKeyDown(Keys.Left) ? 1 : 0);
            movement.DirY = (k.IsKeyDown(Keys.Down) ? 1 : 0) - (k.IsKeyDown(Keys.Up) ? 1 : 0);

            if (k.IsKeyDown(Keys.Space))
            {
                _world.Events.Publish(new PlayerAttackEvent(_player));
            }
            else
            {
                if (movement.DirX == 1)
                {
                    _world.Events.Publish(new PlayerRunEvent(_player, false));
                }
                if (movement.DirX == -1)
                {
                    _world.Events.Publish(new PlayerRunEvent(_player, true));
                }
                if (movement.DirX == 0)
                {
                    _world.Events.Publish(new PlayerIdleEvent(_player));
                }
            }

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

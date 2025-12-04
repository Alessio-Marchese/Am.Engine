# 🎮 **AM Engine**

AM Engine is a work-in-progress 2D engine written in C# and built as a personal learning project. It explores a minimal ECS core, an event-driven gameplay layer, and a rendering abstraction that can be adapted to different frameworks (MonoGame today, other renderers tomorrow). See the core `World` loop and rendering handler for the orchestration details: [Engine/Ecs/World.cs](Engine/Ecs/World.cs), [Engine/Rendering/RenderingHandler.cs](Engine/Rendering/RenderingHandler.cs).

## 🎯 Project goals
- Learn common game-development patterns while keeping the code modular and testable.
- Practice SOLID/Clean Architecture by separating data (components) from behavior (systems) and decoupling the engine from framework details.
- Grow a disciplined testing mindset around gameplay, events, and rendering orchestration.

## 🧩 Design philosophy
A core principle is **engine–framework decoupling**: the engine exposes interfaces and simple data models, while frameworks plug in through adapters. Rendering, texture loading, and registry concerns live behind `IRenderer`, `ITextureLoader`, and `ITextureRegistry`; the MonoGame adapter simply implements those contracts and can be swapped out without touching engine code. See [Engine/Rendering/Interfaces/IRenderer.cs](Engine/Rendering/Interfaces/IRenderer.cs), [Engine/Rendering/Abstract/TextureLoader.cs](Engine/Rendering/Abstract/TextureLoader.cs), [Engine/Rendering/Abstract/TextureRegistry.cs](Engine/Rendering/Abstract/TextureRegistry.cs), and the adapter in [MonoGameAdapter/MonoGameRenderer.cs](MonoGameAdapter/MonoGameRenderer.cs).

## 🧱 Core building blocks
Below is the progression from small pieces to the full architecture, with a quick primer on why each layer exists.

### GameObject & Components
Game objects are intentionally "dumb" containers: they only know their ID and which components they hold. This keeps identity and behavior separate so you can mix and match behaviors without inheritance chains.
- **GameObject** stores the component dictionary and exposes add/get/has helpers, behavior emerges from whichever components you attach. See [Engine/Ecs/GameObject.cs](Engine/Ecs/GameObject.cs).
- **Components** are lightweight data holders (no logic). Examples include `Transform` for position, `Movement` for velocity, `Sprite` for render info, `Camera` for view parameters, `Collider` for AABBs, `Direction` for last facing, and `Animation` for clip state. See [Engine/Ecs/Components](Engine/Ecs/Components).

### Systems
Systems are where all logic lives. They run every tick, scanning for entities with the components they need and then applying deterministic rules. This separation keeps data immutable by default and makes behavior easy to swap or extend.
- **MovementSystem** advances positions using movement vectors and delta time. See [Engine/Ecs/Systems/MovementSystem.cs](Engine/Ecs/Systems/MovementSystem.cs).
- **CameraFollowSystem** steers the camera toward the player tag, factoring viewport, zoom, offsets, and smoothing. See [Engine/Ecs/Systems/CameraFollowSystem.cs](Engine/Ecs/Systems/CameraFollowSystem.cs).
- **CollisionSystem** detects AABB overlaps and publishes `CollisionEvent` into the bus so reactions stay decoupled from detection. See [Engine/Ecs/Systems/CollisionSystem.cs](Engine/Ecs/Systems/CollisionSystem.cs).
- **AnimationSystem** walks clips forward based on frame durations and transitions one-shots into follow-up clips. See [Engine/Ecs/Systems/AnimationSystem.cs](Engine/Ecs/Systems/AnimationSystem.cs).

### World
The world orchestrates the simulation. It owns all entities, the registered systems, and the event bus. Each update it marches systems in order and then dispatches whatever events were raised. Query helpers (`With<T>`, `WithAll`) are used to recover GameObjects with specific components. See [Engine/Ecs/World.cs](Engine/Ecs/World.cs).

### Events
The event layer complements systems by handling logic that should run only when something specific happens, not every frame. Systems (or any publisher) raise events like collisions; subscribers react independently, keeping cross-cutting concerns decoupled.
- **EventBus** queues and dispatches published events on the next world tick. See [Engine/Ecs/Events/EventBus.cs](Engine/Ecs/Events/EventBus.cs).
- **Example of subscriber: PhysicsCollisionSubscriber** resolves overlaps by pushing entities apart and zeroing blocked movement axes when collisions occur. See [Engine/Ecs/Events/Subscribers/PhysicsCollisionSubscriber.cs](Engine/Ecs/Events/Subscribers/PhysicsCollisionSubscriber.cs).

### Rendering & textures
Rendering lives behind an abstraction so the engine stays portable. A dedicated handler translates ECS data into draw calls, while adapters own the framework-specific details (MonoGame today).
- **RenderingHandler** finds the active camera, gathers `Transform` + `Sprite` entities, sorts by Z-index, applies camera zoom/offset, and forwards draw calls to the injected `IRenderer`; it also delegates screen clearing so the handler remains framework-agnostic. See [Engine/Rendering/RenderingHandler.cs](Engine/Rendering/RenderingHandler.cs).
- **TextureLoader** and **TextureRegistry** manage texture lifecycles with opaque handles, letting adapters (MonoGame) bind real textures behind the scenes. See [Engine/Rendering/Abstract/TextureLoader.cs](Engine/Rendering/Abstract/TextureLoader.cs), [Engine/Rendering/Abstract/TextureRegistry.cs](Engine/Rendering/Abstract/TextureRegistry.cs), and [MonoGameAdapter/MonoGameRenderer.cs](MonoGameAdapter/MonoGameRenderer.cs).

## 🧪 Sandbox & tests
- **Sandbox** (MonoGame) wires everything together: it creates a world, registers systems/subscribers, loads textures through the registry/loader, and builds player/background entities with animation, collision, and camera following driven by keyboard input. See [Sandbox/Game1.cs](Sandbox/Game1.cs).
- **Tests** cover integration points such as movement, camera follow, collision events, animation playback, and rendering orchestration. See [Tests/Integration](Tests/Integration).

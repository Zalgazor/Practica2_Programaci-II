using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;
using TCEngine;

namespace TCGame
{
    class SuperAwesomeGame : Game
    {
        public void Init()
        {
            CreateBackground();
            CreateMainCharacter();
            CreatePeopleSpawner();
            CreateTanksSpawner();
            CreateOvniSpawner();
        }

        public void DeInit()
        {
        }

        public void Update(float _dt)
        {
        }

        private void CreateMainCharacter()
        {
            Actor mainCharacterActor = new Actor("MainCharacterActor");

            AnimatedSpriteComponent animatedSpriteComponent = mainCharacterActor.AddComponent<AnimatedSpriteComponent>("Textures/Player/Plane", 4u, 1u);
            animatedSpriteComponent.m_RenderLayer = RenderComponent.ERenderLayer.Middle;

            TransformComponent transformComponent = mainCharacterActor.AddComponent<TransformComponent>();
            transformComponent.Transform.Position = new Vector2f(500.0f, 300.0f);
            transformComponent.Transform.Rotation = 90.0f;

            mainCharacterActor.AddComponent<CollisionLayerComponent>(CollisionLayerComponent.ECollisionLayers.Player);
         
            Vector2f planeForward = new Vector2f(1.0f, 0.0f);
            List<CollisionLayerComponent.ECollisionLayers> enemyLayers = new List<CollisionLayerComponent.ECollisionLayers>();
            enemyLayers.Add(CollisionLayerComponent.ECollisionLayers.Enemy);

            // TODO (7): Add the missing components to the Main Character
            //   - CharacterControllerComponent
            //   - CannonComponent

            Vector2f cannonDirection = new Vector2f(0, 1);

            mainCharacterActor.AddComponent<CharacterControllerComponent>();
            mainCharacterActor.AddComponent<CannonComponent>(cannonDirection);

            TecnoCampusEngine.Get.Scene.CreateActor(mainCharacterActor);

            Actor planeCloudGasActor = new Actor("PlaneCloudGas");
            planeCloudGasActor.AddComponent<TransformComponent>();

            // TODO (7): Add the missing components to the Plane Cloud Gas
            //   - AnimatedSpriteComponent
            //   - ParentActorComponent

            planeCloudGasActor.AddComponent<AnimatedSpriteComponent>("Textures/FX/PlaneCloudGas", 4u, 1u);
            planeCloudGasActor.AddComponent<ParentActorComponent>(mainCharacterActor, new Vector2f (0,1));


            TecnoCampusEngine.Get.Scene.CreateActor(planeCloudGasActor);
        }

        private void CreatePeopleSpawner()
        {
            Actor peopleSpawner = new Actor("People Spawner");
            ActorSpawnerComponent<ActorPrefab> spawnerComponent = peopleSpawner.AddComponent<ActorSpawnerComponent<ActorPrefab>>();

            const float spawnLimitOffset = 50.0f;
            spawnerComponent.m_MinPosition = new Vector2f(spawnLimitOffset, spawnLimitOffset);
            spawnerComponent.m_MaxPosition = new Vector2f(TecnoCampusEngine.Get.ViewportSize.X - spawnLimitOffset, TecnoCampusEngine.Get.ViewportSize.Y - spawnLimitOffset);
            spawnerComponent.m_MinTime = 0.5f;
            spawnerComponent.m_MaxTime = 5f;
            spawnerComponent.Reset();

            for(int i = 1; i <= 3; ++i)
            {
                ActorPrefab peoplePrefab = new ActorPrefab("People0" + i);
                AnimatedSpriteComponent animatedSpriteComponent = peoplePrefab.AddComponent<AnimatedSpriteComponent>("Textures/People/People0"+i, 2u, 1u);
                animatedSpriteComponent.m_RenderLayer = RenderComponent.ERenderLayer.Back;
                peoplePrefab.AddComponent<TransformComponent>();
                peoplePrefab.AddComponent<CollisionLayerComponent>(CollisionLayerComponent.ECollisionLayers.Person);
                peoplePrefab.AddComponent<TargetableComponent>();
                spawnerComponent.AddActorPrefab(peoplePrefab);
            }

            TecnoCampusEngine.Get.Scene.CreateActor(peopleSpawner);
        }

        private void CreateTanksSpawner()
        {
            Actor tanksSpawner = new Actor("Tank Spawner");
            ActorSpawnerComponent<ActorPrefab> spawnerComponent = tanksSpawner.AddComponent<ActorSpawnerComponent<ActorPrefab>>();

            // TODO (8): Fix the spawn position for the tanks
            //    - They should spawn on the right side of the window

            spawnerComponent.m_MinPosition = new Vector2f(1180, 0);
            spawnerComponent.m_MaxPosition = new Vector2f(1194, 0);
            spawnerComponent.m_MinTime = 0.5f;
            spawnerComponent.m_MaxTime = 5f;
            spawnerComponent.Reset();

            Vector2f tankForward = new Vector2f(0.0f, 1.0f);
            List<CollisionLayerComponent.ECollisionLayers> tankEnemyLayers = new List<CollisionLayerComponent.ECollisionLayers>();
            tankEnemyLayers.Add(CollisionLayerComponent.ECollisionLayers.Person);
            float tankSpeed = 5.0f;
            Vector2f cannonDirection = new Vector2f(0, 1);

            for (int i = 1; i <= 2; ++i)
            {
                ActorPrefab tankPrefab = new ActorPrefab("Tank0" + i);
                SpriteComponent spriteComponent = tankPrefab.AddComponent<SpriteComponent>("Textures/Enemies/Tank0" + i);
                spriteComponent.m_RenderLayer = RenderComponent.ERenderLayer.Back;
                tankPrefab.AddComponent<TransformComponent>();

                // TODO (8): Add the Missing components to the Tank Prefab
                //   - ForwardMovementComponent
                //   - CannonComponent (remember to use the correct texture)

                
                tankPrefab.AddComponent<ForwardMovementComponent>(tankSpeed , tankForward);
                tankPrefab.AddComponent<CannonComponent>(cannonDirection);

                tankPrefab.AddComponent<OutOfWindowDestructionComponent>();
                tankPrefab.AddComponent<CollisionLayerComponent>(CollisionLayerComponent.ECollisionLayers.Enemy);
                tankPrefab.AddComponent<ExplosionComponent>();

                spawnerComponent.AddActorPrefab(tankPrefab);
            }

            TecnoCampusEngine.Get.Scene.CreateActor(tanksSpawner);
        }

        private void CreateOvniSpawner()
        {
            Actor ovniSpawner = new Actor("Ovni Spawner");
            ActorSpawnerComponent<ActorPrefab> spawnerComponent = ovniSpawner.AddComponent<ActorSpawnerComponent<ActorPrefab>>();

            // TODO (9): Fix the spawn position for the ovnis and its spawn rate
            //    - They should spawn on the right side of the window
            //    - They should spawn every 2 or 3 seconds aprox

            spawnerComponent.m_MinPosition = new Vector2f(TecnoCampusEngine.Get.ViewportSize.X + 1180, TecnoCampusEngine.Get.ViewportSize.Y - 825);
            spawnerComponent.m_MaxPosition = new Vector2f(TecnoCampusEngine.Get.ViewportSize.X + 1194, TecnoCampusEngine.Get.ViewportSize.Y - 825);
            spawnerComponent.m_MinTime = 2.0f;
            spawnerComponent.m_MaxTime = 3.0f;
            spawnerComponent.Reset();

            List<CollisionLayerComponent.ECollisionLayers> ovniTargetLayers = new List<CollisionLayerComponent.ECollisionLayers>();
            ovniTargetLayers.Add(CollisionLayerComponent.ECollisionLayers.Person);

            for (int i = 1; i <= 4; ++i)
            {
                ActorPrefab ovniPrefab = new ActorPrefab("Ovni0" + i);
                SpriteComponent spriteComponent = ovniPrefab.AddComponent<SpriteComponent>("Textures/Enemies/Ovni0" + i);
                spriteComponent.m_RenderLayer = RenderComponent.ERenderLayer.Front;
                ovniPrefab.AddComponent<TransformComponent>();

                ovniPrefab.AddComponent<CollisionLayerComponent>(CollisionLayerComponent.ECollisionLayers.Enemy);

                // TODO (9): Add the Missing components to the Ovni Prefab
                //   - PatrolMovementComponent
                //   - ExplosionComponent

                ovniPrefab.AddComponent<PatrolMovementComponent>();
                ovniPrefab.AddComponent<ExplosionComponent>();

                spawnerComponent.AddActorPrefab(ovniPrefab);
            }

            TecnoCampusEngine.Get.Scene.CreateActor(ovniSpawner);
        }

        private void CreateBackground()
        { 
            Actor backgroundActor = new Actor("Background");

            SpriteComponent spriteComponent = backgroundActor.AddComponent<SpriteComponent>("Textures/Background");
            spriteComponent.m_RenderLayer = RenderComponent.ERenderLayer.Background;

            TecnoCampusEngine.Get.Scene.CreateActor(backgroundActor);

        }

    }
}

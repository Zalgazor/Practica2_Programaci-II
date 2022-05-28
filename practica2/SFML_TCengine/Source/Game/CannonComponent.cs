using SFML.System;
using System.Collections.Generic;
using System.Diagnostics;
using TCEngine;

namespace TCGame
{
    public class CannonComponent : BaseComponent
    {

        private const float DEFAULT_FIRE_RATE = 2.0f;
        private const float DEFAULT_BULLET_SPEED = 200.0f;
        private const int DEFAULT_BULLETS_PER_SHOT = 1;
        private const float DEFAULT_MULTIPLE_BULLETS_OFFSET= 2.0f;
        private const float DEFAULT_FORWARD_BULLET_OFFSET = 50.0f;
        

        private List<CollisionLayerComponent.ECollisionLayers> m_ImpactLayers;
        private Vector2f m_CannonDirection;
        private float m_FireRate;
        private float m_TimeToShoot;
        private string m_BulletTextureName;
        private int m_BulletsPerShot = DEFAULT_BULLETS_PER_SHOT;
        private float m_BulletSpeed = DEFAULT_BULLET_SPEED;
        private float m_MultipleBulletsOffset = DEFAULT_MULTIPLE_BULLETS_OFFSET;
        private float m_ForwardBulletOffset = DEFAULT_FORWARD_BULLET_OFFSET;

        private bool m_AutomaticFire = true;

        public Vector2f CannonDirection
        {
            get => m_CannonDirection;
            set => m_CannonDirection = value;
        }

        public List<CollisionLayerComponent.ECollisionLayers> ImpactLayers
        {
            get => m_ImpactLayers;
            set => m_ImpactLayers= value;
        }

        public bool AutomaticFire
        {
            get => m_AutomaticFire;
            set => m_AutomaticFire = value;
        }

        public string BulletTextureName
        {
            get => m_BulletTextureName;
            set => m_BulletTextureName = value;
        }

        public float FireRate
        {
            get => m_FireRate;
            set
            {
                m_FireRate = value;
                m_TimeToShoot = m_FireRate;
            }
        }

        public int BulletsPerShot
        {
            get => m_BulletsPerShot;
            set => m_BulletsPerShot = value;
        }

        public float BulletSpeed
        {
            get => m_BulletSpeed;
            set => m_BulletSpeed = value;
        }

        public float MultipleBulletsOffset
        {
            get => m_MultipleBulletsOffset;
            set => m_MultipleBulletsOffset = value;
        }

        public float ForwardBulletOffset
        {
            get => m_ForwardBulletOffset;
            set => m_ForwardBulletOffset = value;
        }

        public CannonComponent(List<CollisionLayerComponent.ECollisionLayers> _impactLayers, Vector2f _cannonDirection)
        {
            m_ImpactLayers = _impactLayers;
            m_CannonDirection = _cannonDirection;
            m_FireRate = DEFAULT_FIRE_RATE;
            m_TimeToShoot = m_FireRate;
        }

        public override EComponentUpdateCategory GetUpdateCategory()
        {
            return EComponentUpdateCategory.Update;
        }

        public override void Update(float _dt)
        {
            base.Update(_dt);

            if( m_AutomaticFire)
            {
                m_TimeToShoot -= _dt;
                Shoot();
            }
            else if(m_TimeToShoot > 0.0f)
            {
                m_TimeToShoot -= _dt;
            }
        }

        public void Shoot()
        {
            if (m_TimeToShoot <= 0.0f)
            {
                TransformComponent transformComponent = Owner.GetComponent<TransformComponent>();
                Debug.Assert(transformComponent != null);

                for (int i = 0; i < m_BulletsPerShot; ++i)
                {
                    Actor missileActor = new Actor("MissileActor");
                    SpriteComponent spriteComponent = missileActor.AddComponent<SpriteComponent>(m_BulletTextureName);
                    spriteComponent.m_RenderLayer = RenderComponent.ERenderLayer.Front;

                    TransformComponent missileTransformComponent = missileActor.AddComponent<TransformComponent>();
                    missileTransformComponent.Transform.Position = CalculateBulletSpawnPosition(transformComponent.Transform.Position, i);

                    // TODO (2): Add the needed components
                    // - ForwardMovementComponent
                    // - BulletComponent
                    // - OutOfWindowDestructionComponent
                    // and add the actor to the Scene

                    missileActor.AddComponent<ForwardMovementComponent>(m_BulletSpeed, m_CannonDirection);
                    missileActor.AddComponent<BulletComponent>(m_ImpactLayers);
                    missileActor.AddComponent<OutOfWindowDestructionComponent>();

                    TecnoCampusEngine.Get.Scene.CreateActor(missileActor);
                }

                m_TimeToShoot = m_FireRate;
            }
        }

        private Vector2f CalculateBulletSpawnPosition(Vector2f _actorPosition, int _bulletIndex)
        {
            Vector2f bulletPosition = _actorPosition + m_CannonDirection * m_ForwardBulletOffset;
            if (m_BulletsPerShot > 1)
            {
                float multipleBulletsOffset = m_MultipleBulletsOffset * m_BulletsPerShot;
                float halfOffset = multipleBulletsOffset * 0.5f;
                float step = multipleBulletsOffset / (m_BulletsPerShot - 1);
                step = step * _bulletIndex;
                Vector2f bulletOffset = m_CannonDirection.Rotate(90) * (step - halfOffset);
                bulletPosition += bulletOffset;
            }
            return bulletPosition;
        }

        public override object Clone()
        {
            CannonComponent clonedComponent = new CannonComponent(m_ImpactLayers, m_CannonDirection);
            clonedComponent.AutomaticFire = m_AutomaticFire;
            clonedComponent.BulletTextureName = m_BulletTextureName;
            clonedComponent.BulletsPerShot = m_BulletsPerShot;
            return clonedComponent;
        }
    }
}

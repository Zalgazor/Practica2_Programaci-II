using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;
using System.Diagnostics;
using TCEngine;

namespace TCGame
{
    public class BulletComponent : BaseComponent
    {
        private List<CollisionLayerComponent.ECollisionLayers> m_ImpactLayers;

        public BulletComponent()
        {
            m_ImpactLayers = new List<CollisionLayerComponent.ECollisionLayers>();
        }

        public BulletComponent(List<CollisionLayerComponent.ECollisionLayers> _impactLayers)
        {
            m_ImpactLayers = _impactLayers;
        }


        public void AddImpactLayer(CollisionLayerComponent.ECollisionLayers _layer)
        {
            m_ImpactLayers.Add(_layer);
        }

        public override EComponentUpdateCategory GetUpdateCategory()
        {
            return EComponentUpdateCategory.Update;
        }

        public override void Update(float _dt)
        {
            base.Update(_dt);

            List<CollisionLayerComponent> collisionLayerComponents = TecnoCampusEngine.Get.Scene.GetAllComponents<CollisionLayerComponent>();
            foreach(CollisionLayerComponent collisionLayerComponent in collisionLayerComponents)
            {
                CollisionLayerComponent.ECollisionLayers layer = collisionLayerComponent.Layer;
                if (m_ImpactLayers.Contains(layer))
                {
                    if (IsActorInRange(collisionLayerComponent.Owner))
                    {
                        // TODO (1): Destroy both actors, the bullet actor and the collision layer actor
                    }
                }
            }
        }

        private bool IsActorInRange(Actor _actor)
        {
            // TODO (1): Implement this method. It returns true if this actor is touching the _actor passed by parameter
            return false;
        }

        public override object Clone()
        {
            BulletComponent clonedComponent = new BulletComponent(m_ImpactLayers);
            return clonedComponent;
        }
    }
}

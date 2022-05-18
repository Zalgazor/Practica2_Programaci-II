using SFML.System;
using TCEngine;

namespace TCGame
{
    public class ExplosionComponent : BaseComponent
    {

        private bool m_DisableExplosion = false;

        public bool DisableExplosion
        {
            set => m_DisableExplosion = value;
        }

        public ExplosionComponent()
        {
        }

        public override void OnActorDestroyed()
        {
            base.OnActorDestroyed();

            if( m_DisableExplosion == false)
            {
                // TODO (4): Create the Explosion actor add it to the scene. This actor has the next components:
                //  - TransformComponent
                //  - AnimatedSpriteComponent (you can Use the Explosion texture from the FX folder)
                //  - TimeToDieComponent
                //  - ForwardMovementComponent (optional) -> You can add it if you want, to add a very subtle movement
            }
        }

        public override EComponentUpdateCategory GetUpdateCategory()
        {
            return EComponentUpdateCategory.Update;
        }

        public override object Clone()
        {
            return new ExplosionComponent();
        }
    }
}

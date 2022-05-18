using SFML.System;
using System;
using TCEngine;

namespace TCGame
{
    public class OutOfWindowDestructionComponent : BaseComponent
    {
        public OutOfWindowDestructionComponent()
        {
        }

        public override EComponentUpdateCategory GetUpdateCategory()
        {
            return EComponentUpdateCategory.PostUpdate;
        }

        public override void Update(float _dt)
        {
            base.Update(_dt);

            TransformComponent transformComponent = Owner.GetComponent<TransformComponent>();
            Vector2f actorPosition = transformComponent.Transform.Position;
            Vector2f viewportSize = TecnoCampusEngine.Get.ViewportSize;
            if( actorPosition.Y > viewportSize.Y )
            {
                ExplosionComponent explosionComponent = Owner.GetComponent<ExplosionComponent>();
                if(explosionComponent != null)
                {
                    explosionComponent.DisableExplosion = true;
                }
                Owner.Destroy();
            }
        }

        public override object Clone()
        {
            return new OutOfWindowDestructionComponent();
        }
    }
}

using SFML.System;
using SFML.Window;
using TCEngine;

namespace TCGame
{
    public class CharacterControllerComponent : BaseComponent
    {

        private const float MOVEMENT_SPEED = 200f;
        private TransformComponent transformComponent;
        private CannonComponent cannonComponent;

        public CharacterControllerComponent()
        {
        }

        public override EComponentUpdateCategory GetUpdateCategory()
        {
            return BaseComponent.EComponentUpdateCategory.PreUpdate;
        }

        public override void OnActorCreated()
        {
            base.OnActorCreated();
            transformComponent = Owner.GetComponent<TransformComponent>();
            cannonComponent = Owner.GetComponent<CannonComponent>();
        }

        public override void Update(float _dt)
        {
            base.Update(_dt);

            
            // TODO (3): Implement the keyboard handling
            //   - Pressing W moves the Actor up
            //   - Pressing S moves the Actor down
            //   - Pressing A moves the Actor to the left
            //   - Pressing D moves the Actor to the right
            //   - Pressing Space shoots the cannon of this actor (only if this actor has a CannonComponent)
        }

        public void HandleKeyPressed(object _sender, KeyEventArgs _keyEvent, Actor actor)
        {
            if (_keyEvent.Code == Keyboard.Key.W)
            {
                Vector2f up = new Vector2f(0, 1);
                transformComponent.Transform.Position += up * MOVEMENT_SPEED;
            }
            else if (_keyEvent.Code == Keyboard.Key.A)
            {
                Vector2f left = new Vector2f(-1, 0);
                transformComponent.Transform.Position += left * MOVEMENT_SPEED;
            }
            else if (_keyEvent.Code == Keyboard.Key.S)
            {
                Vector2f down = new Vector2f(0, -1);
                transformComponent.Transform.Position += down * MOVEMENT_SPEED;
            }
            else if (_keyEvent.Code == Keyboard.Key.D)
            {
                Vector2f right = new Vector2f(1, 0);
                transformComponent.Transform.Position += right * MOVEMENT_SPEED;
            }
            else if (_keyEvent.Code == Keyboard.Key.Space)
            {
                try
                {
                    cannonComponent.Shoot();
                }
                catch
                {

                }
 
            }
        }
    }
}

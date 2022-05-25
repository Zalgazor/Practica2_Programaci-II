using SFML.System;
using SFML.Window;
using TCEngine;

namespace TCGame
{
    public class CharacterControllerComponent : BaseComponent
    {

        private const float MOVEMENT_SPEED = 200f;

        public CharacterControllerComponent()
        {
        }

        public override EComponentUpdateCategory GetUpdateCategory()
        {
            return BaseComponent.EComponentUpdateCategory.PreUpdate;
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
                actor.GetComponent<ForwardMovementComponent>(MOVEMENT_SPEED, up);
            }
            else if (_keyEvent.Code == Keyboard.Key.A)
            {
                Vector2f left = new Vector2f(-1, 0);
                actor.GetComponent<ForwardMovementComponent>(MOVEMENT_SPEED, left);
            }
            else if (_keyEvent.Code == Keyboard.Key.S)
            {
                Vector2f down = new Vector2f(0, -1);
                actor.GetComponent<ForwardMovementComponent>(MOVEMENT_SPEED, down);
            }
            else if (_keyEvent.Code == Keyboard.Key.D)
            {
                Vector2f right = new Vector2f(1, 0);
                actor.GetComponent<ForwardMovementComponent>(MOVEMENT_SPEED, right);
            }
            else if (_keyEvent.Code == Keyboard.Key.Space)
            {
                if ()
                {
                    actor.GetComponent<CannonComponent>().Shoot();
                }
            }
        }
    }
}

using SFML.System;
using System.Diagnostics;
using TCEngine;

namespace TCGame
{
    // TODO (5): Create the ForwardMovementComponent
    //  - Although you can be creative, it should have at least these two memebers:
    //    - float m_Speed and Vector2f m_Forward
    //  - The main idea of this component is to move the actor in the m_Forward direction and the speed defined
    //    by the m_Speed member
    class ForwardMovementComponent : BaseComponent
    {
        private Vector2f m_Forward;
        private float m_Speed;
        private const float DEFAULT_SPEED = 10.0f;
        private Vector2f DEFAULT_FORWARD_VECTOR = new Vector2f(0, 1);
        private TransformComponent m_TransformComponent;

        public Vector2f Forward
        {
            get => m_Forward;
            set => m_Forward = value;
        }

        public ForwardMovementComponent()
        {
            m_Forward = DEFAULT_FORWARD_VECTOR;
            m_Speed = DEFAULT_SPEED;
        }
        public ForwardMovementComponent(float m_Speed, Vector2f m_Forward)
        {
            this.m_Forward = m_Forward;
            this.m_Speed = m_Speed;
        }

        public override void OnActorCreated()
        {
            base.OnActorCreated();

            m_TransformComponent = Owner.GetComponent<TransformComponent>();
        }

        public override void Update(float _dt)
        {
            base.Update(_dt);

            TransformComponent transformComponent = Owner.GetComponent<TransformComponent>();
            Debug.Assert(transformComponent != null);

            Vector2f velocity = m_Forward * m_Speed;
            transformComponent.Transform.Position += velocity * _dt;
        }

        public override EComponentUpdateCategory GetUpdateCategory()
        {
            return EComponentUpdateCategory.Update;
        }

        public override object Clone()
        {
            ForwardMovementComponent clonedComponent = new ForwardMovementComponent(m_Speed, m_Forward);
            return clonedComponent;
        }
    }


}

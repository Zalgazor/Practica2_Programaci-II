using SFML.System;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using TCEngine;

namespace TCGame
{

    // TODO (6): Create a PatrolMovementComponent (or better name, as you wish)
    //  - This component must move the UFOs to patrol and look for people to trap
    //  - You can implement it as a FSM

    public class PatrolMovementComponent : BaseComponent
    {
        public Random rnd = new Random();
        private const float TIEMPO_MOVIMIENTO_ALEATORIO = 2.0f;
        private const float TIEMPO_Abducir = 2.0f;
        private List<Actor> humanActors = new List<Actor>();
        private const float DEFAULT_SPEED = 120.0f;
        private const float DEFAULT_RANGE = 1000f;
        public float m_OvniSpeed;
        public float m_OvniRange;

        private enum EState
        {
            MovimientoAleatorio,
            SeleccionarObjetivo,
            MoverseHaciaObjetivo,
            Abducir
        }

        private EState m_CurrentState;
        private float m_CurrentStateTime = 0.0f;
        private Actor target;
        private TransformComponent OvniTransformComponent;

        public PatrolMovementComponent(float _speed, float _range)
        {
            m_OvniSpeed = _speed;
            m_OvniRange = _range;
            m_CurrentState = EState.MovimientoAleatorio;
        }

        public PatrolMovementComponent()
        {
            m_OvniSpeed = DEFAULT_SPEED;
            m_OvniRange = DEFAULT_RANGE;
            m_CurrentState = EState.MovimientoAleatorio;
        }

        public override void OnActorCreated()
        {
            base.OnActorCreated();

            OvniTransformComponent = Owner.GetComponent<TransformComponent>();
        }
        public override void Update(float _dt)
        {
            base.Update(_dt);

            switch (m_CurrentState)
            {
                case EState.MovimientoAleatorio:
                    UpdateMovimientoAleatorio(_dt);
                    break;
                case EState.SeleccionarObjetivo:
                    UpdateSeleccionarObjetivo();
                    break;
                case EState.MoverseHaciaObjetivo:
                    UpdateMoverseHaciaObjetivo(_dt);
                    break;

                case EState.Abducir:
                    UpdateAbducir(_dt);
                    break;
            }
        }

        private void UpdateMovimientoAleatorio(float _dt)
        {
            m_CurrentStateTime += _dt;

            int _newPosX = rnd.Next(0, (int)TecnoCampusEngine.Get.Window.Size.X);
            int _newPosY = rnd.Next(0, (int)TecnoCampusEngine.Get.Window.Size.Y);
            Vector2f vector = new Vector2f(_newPosX, _newPosY);

            OvniTransformComponent.Transform.Position += ((Owner.GetPosition() - vector).Normal() * m_OvniSpeed * _dt);

            if (m_CurrentStateTime >= TIEMPO_MOVIMIENTO_ALEATORIO)
            {
                ChangeState(EState.SeleccionarObjetivo);
            }
        }

        private void UpdateSeleccionarObjetivo()
        {
            
            for (int i = 0; i > TecnoCampusEngine.Get.Scene.GetAllActors().Count; i++)
            {
                TargetableComponent targetableComponent = TecnoCampusEngine.Get.Scene.GetAllActors()[i].GetComponent<TargetableComponent>();
                if (targetableComponent != null)
                {
                    humanActors.Add(TecnoCampusEngine.Get.Scene.GetAllActors()[i]);
                }
            }

            if (HumanIsInRange())
            {
                target = ReturnNearestHumanActor();
                ChangeState(EState.MoverseHaciaObjetivo);
            }
            else
            {
                ChangeState(EState.MovimientoAleatorio);
            }


        }

        private void UpdateMoverseHaciaObjetivo(float _dt)
        {

            Vector2f targetDirection = (target.GetPosition() - Owner.GetPosition()).Normal();
            OvniTransformComponent.Transform.Position = OvniTransformComponent.Transform.Position + (targetDirection * m_OvniSpeed * _dt);

            if (OvniTransformComponent.Transform.Position == target.GetPosition())
            {
                ChangeState(EState.Abducir);
            }
        }

        private void UpdateAbducir(float _dt)
        {
            m_CurrentStateTime += _dt;

            if (m_CurrentStateTime >= TIEMPO_Abducir)
            {
                TecnoCampusEngine.Get.Scene.Destroy(target);
                ChangeState(EState.MovimientoAleatorio);
            }

        }

        private bool HumanIsInRange()
        {
            for (int i = 0; i < humanActors.Count; i++)
            {
                if (HumanIsNotOutOfWindow(humanActors[i]))
                {
                    if ((humanActors[i].GetPosition() - Owner.GetPosition()).Size() < m_OvniRange)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private Actor ReturnNearestHumanActor()
        {
            Actor nearestActor = new Actor("humanTarget");
            float range = m_OvniRange;

            for (int i = 0; i < humanActors.Count; i++)
            {
                if (HumanIsNotOutOfWindow(humanActors[i]))
                {
                    if ((humanActors[i].GetPosition() - Owner.GetPosition()).Size() < range)
                    {
                        range = humanActors[i].GetPosition().Size();
                        nearestActor = humanActors[i];
                    }
                }
            }

            return nearestActor;
        }

        private bool HumanIsNotOutOfWindow(Actor _human)
        {
            if (_human.GetPosition().X < TecnoCampusEngine.WINDOW_WIDTH && _human.GetPosition().Y < TecnoCampusEngine.WINDOW_HEIGHT)
            {
                return true;
            }

            return false;
        }

        private void ChangeState(EState _newState)
        {
            OnLeaveState(m_CurrentState);
            OnEnterState(_newState);

            m_CurrentState = _newState;
        }

        private void OnEnterState(EState _nextState)
        {
            switch (_nextState)
            {
                case EState.MovimientoAleatorio:
                    humanActors = null;
                    break;
                case EState.SeleccionarObjetivo:
                    break;
                case EState.MoverseHaciaObjetivo:
                    break;
                case EState.Abducir:
                    break;
                default:
                    break;
            }
        }
        private void OnLeaveState(EState _previousState)
        {
            switch (_previousState)
            {
                case EState.MovimientoAleatorio:
                    m_CurrentStateTime = 0.0f;
                    break;
                case EState.SeleccionarObjetivo:
                    break;
                case EState.MoverseHaciaObjetivo:
                    break;
                case EState.Abducir:
                    m_CurrentStateTime = 0.0f;
                    target = null;
                    break;
                default:
                    break;
            }
        }

        public override object Clone()
        {
            PatrolMovementComponent clonedComponent = new PatrolMovementComponent();
            return clonedComponent;
        }

        public override EComponentUpdateCategory GetUpdateCategory()
        {
            return EComponentUpdateCategory.Update;
        }
    }


}

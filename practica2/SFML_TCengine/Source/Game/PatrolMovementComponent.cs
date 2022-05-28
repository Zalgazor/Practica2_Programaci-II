using SFML.System;
using System.Diagnostics;
using System.Collections.Generic;
using TCEngine;

namespace TCGame
{
    public class PatrolMovementComponent : BaseComponent
    {
        private const float TIEMPO_MOVIMIENTO_ALEATORIO = 2.0f;
        private const float TIEMPO_Abducir = 2.0f;
        private List<Actor> humanActors = new List<Actor>();
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

        public PatrolMovementComponent()
        {
            m_CurrentState = EState.MovimientoAleatorio;
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

            if (m_CurrentStateTime >= TIEMPO_MOVIMIENTO_ALEATORIO)
            {
                ChangeState(EState.SeleccionarObjetivo);
            }
        }

        private void UpdateSeleccionarObjetivo()
        {
            for (int i = 0; i > TecnoCampusEngine.Get.Scene.GetAllActors().Count; i++)
            {
                if (TecnoCampusEngine.Get.Scene.GetAllActors()[i].GetComponent<TargetableComponent>().TargetableComponentConfirmation() == true)
                {
                    humanActors.Add(TecnoCampusEngine.Get.Scene.GetAllActors()[i]);
                }
            }

           for (int i = 0; i > humanActors.Count; i++)
            {
                if (humanActors[i] != null)
                {
                    target = humanActors[i];
                    break;
                }
            }
            
            ChangeState(EState.MoverseHaciaObjetivo);

        }

        private void UpdateMoverseHaciaObjetivo(float _dt)
        {

            Vector2f targetDirection = (target.GetPosition() - Owner.GetPosition()).Normal();
            OvniTransformComponent.Transform.Position = OvniTransformComponent.Transform.Position + (targetDirection * 5f * _dt);

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
                target.Destroy();
                ChangeState(EState.MovimientoAleatorio);
            }

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
            throw new System.NotImplementedException();
        }
    }

    // TODO (6): Create a PatrolMovementComponent (or better name, as you wish)
    //  - This component must move the UFOs to patrol and look for people to trap
    //  - You can implement it as a FSM

}

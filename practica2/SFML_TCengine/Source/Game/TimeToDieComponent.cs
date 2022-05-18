using SFML.System;
using System;
using System.Diagnostics;
using TCEngine;

namespace TCGame
{
    public class TimeToDieComponent : BaseComponent
    {
        private const int DEFAULT_TIME_TO_DIE = 5;

        private float m_TimeToDie;
        private float m_CurrentTime;

        private bool m_UseRandomTime = false;
        private Vector2f m_RandomTimeRange;

        public override EComponentUpdateCategory GetUpdateCategory()
        {
            return EComponentUpdateCategory.Update;
        }

        public TimeToDieComponent()
        {
            m_TimeToDie = DEFAULT_TIME_TO_DIE;
            m_CurrentTime = 0;
        }

        public TimeToDieComponent(float _timeToDie)
        {
            m_TimeToDie = _timeToDie;
            m_CurrentTime = 0;
        }

        public TimeToDieComponent(float _minTime, float _maxTime)
        {
            Debug.Assert(_minTime >= 0.0f, "_minTime cannot be a negative number");
            Debug.Assert(_minTime < _maxTime, "_maxTime mas be greater than _minTime");

            m_UseRandomTime = true;
            m_RandomTimeRange = new Vector2f(_minTime, _maxTime);
            m_CurrentTime = 0;
        }


        public override void OnActorCreated()
        {
            base.OnActorCreated();

            if(m_UseRandomTime)
            {
                Random randomGenerator = new Random();
                float randomValue = (float)randomGenerator.NextDouble();
                m_TimeToDie = m_RandomTimeRange.X * (1.0f - randomValue) + m_RandomTimeRange.Y * randomValue;
                Debug.Assert(m_TimeToDie >= m_RandomTimeRange.X);
            }
        }

        public override void Update(float _dt)
        {
            base.Update(_dt);

            m_CurrentTime += _dt;

            if( m_CurrentTime >= m_TimeToDie)
            {
                Owner.Destroy();
            }
        }

        public override object Clone()
        {
            TimeToDieComponent clonedComponent;
            if (m_UseRandomTime)
            {
                clonedComponent = new TimeToDieComponent(m_RandomTimeRange.X, m_RandomTimeRange.Y);
            }
            else
            {
                clonedComponent = new TimeToDieComponent(m_TimeToDie);
            }

            return clonedComponent;
        }
    }
}

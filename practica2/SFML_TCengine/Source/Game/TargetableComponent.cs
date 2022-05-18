using TCEngine;

namespace TCGame
{
    public class TargetableComponent : BaseComponent
    {
        private Actor m_ActorTargetting;

        public Actor ActorTargetting
        {
            get => m_ActorTargetting;
            set => m_ActorTargetting = value;
        }

        public override EComponentUpdateCategory GetUpdateCategory()
        {
            return EComponentUpdateCategory.Update;
        }

        public TargetableComponent()
        {
        }

        public override object Clone()
        {
            return new TargetableComponent();
        }
    }
}

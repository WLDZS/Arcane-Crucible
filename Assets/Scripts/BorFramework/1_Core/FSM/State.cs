namespace BorFramework
{
    public abstract class State : IState
    {
        protected StateMachine Machine { get; private set; }

        internal void Bind(StateMachine machine)
        {
            Machine = machine;
        }

        public virtual void OnEnter()
        {
        }

        public virtual void OnUpdate(float dt)
        {
        }

        public virtual void OnExit()
        {
        }
    }
}

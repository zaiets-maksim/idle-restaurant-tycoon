namespace Characters.PersonStateMachine
{
    public abstract class PersonBaseState
    {
        public abstract void Enter();
        public abstract void Exit();
        public virtual void Update() { }
    }
}
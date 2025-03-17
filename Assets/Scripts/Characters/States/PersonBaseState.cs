using System;
using System.Threading.Tasks;

namespace Characters.PersonStateMachine
{
    public abstract class PersonBaseState
    {
        protected TaskCompletionSource<bool> _tcs = new();
        
        public abstract void Enter();
        public abstract void Exit();
        public virtual void Update() { }
    }
}
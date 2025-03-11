using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Characters.PersonStateMachine
{
    [RequireComponent(typeof(PersonMover))]
    [RequireComponent(typeof(PersonAnimator))]
    public class PersonBehavior : MonoBehaviour
    {
        protected List<PersonBaseState> _states;
        protected PersonBaseState _currentState;
        protected List<PersonBaseState> CreateStates(params PersonBaseState[] states) => 
            new(states);

        public void ChangeState<T>() where T : PersonBaseState
        {
            var state = _states.FirstOrDefault(s  => s is T);

            if (state == _currentState)
                return;
            
            _currentState?.Exit();
            state.Enter();
            _currentState = state;
        }
    }
}

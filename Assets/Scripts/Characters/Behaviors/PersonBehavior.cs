using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Extensions;
using UnityEngine;

namespace Characters.PersonStateMachine
{
    [RequireComponent(typeof(PersonMover))]
    [RequireComponent(typeof(PersonRotator))]
    [RequireComponent(typeof(PersonAnimator))]
    public class PersonBehavior : MonoBehaviour
    {
        protected List<PersonBaseState> _states;
        protected PersonBaseState _currentState;

        public PersonBaseState CurrentState => _currentState;

        protected List<PersonBaseState> CreateStates(params PersonBaseState[] states) =>
            new(states);

        private bool _isTransitioning;

        public bool IsTransitioning => _isTransitioning;

        public async void ChangeState<T>() where T : PersonBaseState
        {
            if (_isTransitioning)
            {
                Debug.LogWarning($"Blocked state change to {typeof(T).Name} — transition in progress ({gameObject.GetInstanceID()})");
                return;
            }

            var state = _states.FirstOrDefault(s => s is T);
            if (state == _currentState)
                return;

            _isTransitioning = true;

            Debug.Log(Make.Colored($"To {state.GetType().Name} {gameObject.GetInstanceID()}", Color.yellow));

            _currentState?.Exit();

            await Task.Yield();

            state?.Enter();
            _currentState = state;

            Debug.Log(Make.Colored($"-> {_currentState.GetType().Name} {gameObject.GetInstanceID()}", Color.green));

            _isTransitioning = false;
        }
    }
}
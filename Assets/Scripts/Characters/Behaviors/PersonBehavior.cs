using System.Collections.Generic;
using System.Linq;
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

        public void ChangeState<T>() where T : PersonBaseState
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

            try
            {
                Debug.Log(Make.Colored($"To {state.GetType().Name} {gameObject.GetInstanceID()}", Color.yellow));

                _currentState?.Cancel();
                _currentState?.Exit();

                _currentState = state;
                _currentState.EnterSafe();

                Debug.Log(Make.Colored($"-> {_currentState.GetType().Name} {gameObject.GetInstanceID()}", Color.green));
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[PersonBehavior] State change to {typeof(T).Name} failed: {e}");
                _currentState = null;
            }
            finally
            {
                _isTransitioning = false;
            }
        }
    }
}
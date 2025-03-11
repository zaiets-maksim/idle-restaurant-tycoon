using System.Collections;
using System.Collections.Generic;
using Characters.PersonStateMachine;
using Characters.States;
using UnityEngine;

namespace Characters.Personal
{
    public class ChefBehavior : PersonBehavior
    {
        [SerializeField] private PersonMover _personMover;
        [SerializeField] private PersonAnimator _personAnimator;
        [SerializeField] private Chef _chef;
        
        private IEnumerator Start()
        {
            yield return null;
            
            _states = CreateStates(
                new FoodSearchState(this, _chef, _personMover, _personAnimator),
                new CookingState(this, transform, _personMover, _personAnimator)
            );

            ChangeState<FoodSearchState>();
        }

        private void Update()
        {
            _currentState?.Update();
        }
    }
}

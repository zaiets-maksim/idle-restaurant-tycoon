using System.Collections;
using Characters.Personal;
using Characters.PersonStateMachine;
using Characters.States;
using Characters.States.Chef;
using UnityEngine;

namespace Characters.Behaviors
{
    public class ChefBehavior : PersonBehavior
    {
        [SerializeField] private PersonMover _personMover;
        [SerializeField] private PersonAnimator _personAnimator;
        [SerializeField] private Chef _chef;
        [SerializeField] private DishHolder _dishHolder;

        private IEnumerator Start()
        {
            yield return null;
            
            _states = CreateStates(
                new IdleState(_personAnimator),
                new FoodSearchState(this, _chef, _personMover, _personAnimator),
                new CookingState(this, _chef, transform, _personMover, _personAnimator, _dishHolder),
                new DeliverAndServeState(this, _chef, transform, _personMover, _personAnimator, _dishHolder),
                new ReturnToSpawnState(this, _chef)
            );

            ChangeState<IdleState>();
        }

        private void Update()
        {
            _currentState?.Update();
        }
    }
}

using System.Collections;
using Characters.PersonStateMachine;
using Characters.States;
using Characters.States.Waiter;
using UnityEngine;

namespace Characters.Behaviors
{
    public class WaiterBehavior : PersonBehavior
    {
        [SerializeField] private PersonMover _personMover;
        [SerializeField] private PersonAnimator _personAnimator;
        [SerializeField] private Waiter _waiter;
        [SerializeField] private DishHolder _dishHolder;
    
        private IEnumerator Start()
        {
            yield return null;

            _states = CreateStates(
                new IdleState(_personAnimator),
                new DishHandlingState(this, _waiter, transform, _personMover, _personAnimator, _dishHolder),
                new OrderDeliveryState(this, _waiter, transform, _personMover, _personAnimator, _dishHolder),
                new LeaveHallState(this, _waiter)
            );

            ChangeState<IdleState>();
        }

        private void Update()
        {
            _currentState?.Update();
        }
    }
}

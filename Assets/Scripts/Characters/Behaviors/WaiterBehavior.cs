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
    
        private void OnEnable()
        {
            _states = CreateStates(
                new IdleState(_personAnimator),
                new DishHandlingState(this, _waiter, transform, _personMover, _personAnimator, _dishHolder, _orderStorageService, _purchasedItemRegistry),
                new OrderDeliveryState(this, _waiter, transform, _personMover, _personAnimator, _dishHolder, _orderStorageService),
                new ReturnToSpawnState(this, _waiter)
            );

            ChangeState<IdleState>();
        }

        private void Update()
        {
            _currentState?.Update();
        }
    }
}

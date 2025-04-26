using System.Collections;
using Characters;
using Characters.Customers;
using Characters.PersonStateMachine;
using Characters.States;
using Unity.VisualScripting;
using UnityEngine;

public class CustomerBehavior : PersonBehavior
{
    [SerializeField] private PersonMover _personMover;
    [SerializeField] private PersonAnimator _personAnimator;
    [SerializeField] private Customer _customer;

    private IEnumerator Start()
    {
        yield return null;
        
        _states = CreateStates(
            new IdleState(_personAnimator),
            new SeatAndOrderState(_personMover, _personAnimator, _customer, _purchasedItemRegistry),
            new EatAndPayState(this, _customer),
            new LeaveState(_customer)
        );

        ChangeState<SeatAndOrderState>();
    }

    private void Update()
    {
        _currentState?.Update();
    }
}
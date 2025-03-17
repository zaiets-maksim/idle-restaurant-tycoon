using System.Collections;
using Characters;
using Characters.Customers;
using Characters.PersonStateMachine;
using Characters.States;
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
            new SeatAndOrderState(this, transform, _personMover, _personAnimator, _customer),
            new EatAndPayState(this, transform, _personMover, _personAnimator, _customer),
            new LeaveState(this, transform, _personMover, _personAnimator)
        );

        ChangeState<SeatAndOrderState>();
    }

    private void Update()
    {
        _currentState?.Update();
    }
}
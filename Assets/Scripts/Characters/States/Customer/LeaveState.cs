using Characters.Customers;
using Characters.PersonStateMachine;
using UnityEngine;

internal class LeaveState : PersonBaseState
{
    private readonly Customer _customer;

    public LeaveState(Customer customer)
    {
        _customer = customer;
    }

    public override async void Enter()
    {
        _customer.LeaveChair();
        _customer.EnableAgent();
        await _customer.MoveToSpawn();
        Object.Destroy(_customer.gameObject);
    }

    public override void Exit()
    {
    }
}
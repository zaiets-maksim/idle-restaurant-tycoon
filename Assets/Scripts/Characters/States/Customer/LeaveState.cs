using System.Threading;
using System.Threading.Tasks;
using Characters.Customers;
using Characters.PersonStateMachine;
using Services.ActiveCustomersRegistry;
using UnityEngine;

internal class LeaveState : PersonBaseState
{
    private readonly Customer _customer;
    private readonly IActiveCustomersRegistry _activeCustomersRegistry;

    public LeaveState(Customer customer, IActiveCustomersRegistry activeCustomersRegistry)
    {
        _customer = customer;
        _activeCustomersRegistry = activeCustomersRegistry;
    }

    protected override async Task Enter(CancellationToken ct)
    {
        _customer.LeaveChair();
        _customer.EnableAgent();
        await _customer.MoveToSpawn();
        ct.ThrowIfCancellationRequested();
        _activeCustomersRegistry.Remove(_customer);
        Object.Destroy(_customer.gameObject);
    }

    public override void Exit()
    {
    }
}
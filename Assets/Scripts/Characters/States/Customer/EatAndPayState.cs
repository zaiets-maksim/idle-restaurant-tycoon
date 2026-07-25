using System.Threading;
using System.Threading.Tasks;
using Characters;
using Characters.Customers;
using Characters.PersonStateMachine;
using Extensions;
using UnityEngine;

internal class EatAndPayState : PersonBaseState
{
    private readonly Customer _customer;
    private readonly CustomerBehavior _customerBehavior;

    public EatAndPayState(CustomerBehavior customerBehavior, Customer customer)
    {
        _customerBehavior = customerBehavior;
        _customer = customer;
    }

    protected override async Task Enter(CancellationToken ct)
    {
        await _customer.Eat();
        ct.ThrowIfCancellationRequested();
        _customer.PayBill();

        await Task.Delay(1.ToMiliseconds(), ct);
        
        _customerBehavior.ChangeState<LeaveState>();
    }
    

    public override void Exit()
    {
    }
}
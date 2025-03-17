using System.Threading.Tasks;
using Characters;
using Characters.Customers;
using Characters.PersonStateMachine;
using UnityEngine;

internal class EatAndPayState : PersonBaseState
{
    private readonly Customer _customer;

    public EatAndPayState(CustomerBehavior customerBehavior, Transform transform, PersonMover personMover,
        PersonAnimator personAnimator, Customer customer)
    {
        _customer = customer;
    }

    public override async void Enter()
    {
        _tcs = new TaskCompletionSource<bool>();
        
        await _customer.Eat();
        _customer.PayBill();
    }
    

    public override void Exit()
    {
    }
}
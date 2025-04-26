using System.Threading.Tasks;
using Characters;
using Characters.Customers;
using Characters.PersonStateMachine;
using Cysharp.Threading.Tasks;
using tetris.Scripts.Extensions;
using UnityEngine;

internal class EatAndPayState : PersonBaseState
{
    private readonly Customer _customer;
    private readonly CustomerBehavior _customerBehavior;
    
    private UniTaskCompletionSource<bool> _tcs = new();


    public EatAndPayState(CustomerBehavior customerBehavior, Customer customer)
    {
        _customerBehavior = customerBehavior;
        _customer = customer;
    }

    public override async void Enter()
    {
        _tcs = new UniTaskCompletionSource<bool>();
        
        await _customer.Eat();
        _customer.PayBill();

        await UniTask.Delay(1.ToMiliseconds());
        
        _customerBehavior.ChangeState<LeaveState>();
    }
    

    public override void Exit()
    {
    }
}
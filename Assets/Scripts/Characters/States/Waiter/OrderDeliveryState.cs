using System.Threading.Tasks;
using Characters;
using Characters.Behaviors;
using Characters.PersonStateMachine;
using Characters.States;
using Infrastructure;
using Services.OrderStorageService;
using tetris.Scripts.Extensions;
using UnityEngine;

public class OrderDeliveryState : PersonBaseState
{
    private readonly WaiterBehavior _waiterBehavior;
    private readonly Transform _transform;
    private readonly PersonMover _personMover;
    private readonly PersonAnimator _personAnimator;
    private readonly DishHolder _dishHolder;
    private readonly Waiter _waiter;
    private readonly IOrderStorageService _orderStorageService;

    public OrderDeliveryState(WaiterBehavior waiterBehavior, Waiter waiter, Transform transform, PersonMover personMover, PersonAnimator personAnimator, DishHolder dishHolder)
    {
        _waiter = waiter;
        _dishHolder = dishHolder;
        _personAnimator = personAnimator;
        _personMover = personMover;
        _transform = transform;
        _waiterBehavior = waiterBehavior;
        _orderStorageService = ProjectContext.Instance?.OrderStorageService;

    }
    
    public override async void Enter()
    {
        _tcs = new TaskCompletionSource<bool>();
        
        await DeliverOrder();
        
        _waiterBehavior.ChangeState<IdleState>();
    }

    private async Task DeliverOrder()
    {
        var customer = _waiter.Order.Customer;
        
        var servingPoint = customer.Chair.ServingPoints[Random.Range(0, customer.Chair.ServingPoints.Length)];
        _personMover.StartMovingTo(servingPoint, () => _tcs.SetResult(true), true);
        await _tcs.Task;
        
        _dishHolder.Give(out var dish);
        customer.TakeDish(dish);
        _personAnimator.PutTheItem();
        _orderStorageService.Served(_waiter.Order);

        var time = _personAnimator.GetCurrentCLipLength();
        await Task.Delay(time.ToMiliseconds());
    }

    public override void Exit()
    {
        
    }
}

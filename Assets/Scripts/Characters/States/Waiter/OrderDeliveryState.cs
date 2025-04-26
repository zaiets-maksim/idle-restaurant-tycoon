using System.Threading.Tasks;
using Characters;
using Characters.Behaviors;
using Characters.PersonStateMachine;
using Characters.States.Waiter;
using Extensions;
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

    public OrderDeliveryState(WaiterBehavior waiterBehavior, Waiter waiter, Transform transform,
        PersonMover personMover, PersonAnimator personAnimator, DishHolder dishHolder,
        IOrderStorageService orderStorageService)
    {
        _waiter = waiter;
        _dishHolder = dishHolder;
        _personAnimator = personAnimator;
        _personMover = personMover;
        _transform = transform;
        _waiterBehavior = waiterBehavior;
        _orderStorageService = orderStorageService;
    }
    
    public override async void Enter()
    {
        await DeliverOrder();
        
        if(_waiter.Order != null || _waiter.TryGetNewOrder())
            _waiterBehavior.ChangeState<DishHandlingState>();
        else
            _waiterBehavior.ChangeState<ReturnToSpawnState>();
    }

    private async Task DeliverOrder()
    {
        var customer = _waiter.Order.Customer;
        var servingPoint = customer.Chair.ServingPoints[Random.Range(0, customer.Chair.ServingPoints.Length)];
        
        await TaskExtension.WaitFor(callback =>
        {
            _personMover.StartMovingTo(servingPoint, callback, true);
        });

        _dishHolder.GiveDish(out var dish);
        customer.TakeDish(dish);
        _personAnimator.PutTheItem();
        _waiter.Delivered();

        var time = _personAnimator.GetCurrentClipLength();
        await Task.Delay(time.ToMiliseconds());
    }

    public override void Exit()
    {
        
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Characters;
using Characters.Customers;
using Characters.PersonStateMachine;
using Infrastructure;
using Interactable;
using Services.PurchasedItemRegistry;
using tetris.Scripts.Extensions;
using UnityEngine;

internal class SeatAndOrderState : PersonBaseState
{
    private readonly IPurchasedItemRegistry _purchasedItemRegistry;
    private readonly Transform _transform;
    private readonly CustomerBehavior _customerBehavior;
    private readonly PersonMover _personMover;
    private readonly PersonAnimator _personAnimator;
    private readonly Customer _customer;

    public SeatAndOrderState(CustomerBehavior customerBehavior, Transform transform, PersonMover personMover, 
        PersonAnimator personAnimator, Customer customer)
    {
        _customer = customer;
        _personAnimator = personAnimator;
        _personMover = personMover;
        _transform = transform;
        _customerBehavior = customerBehavior;
        _purchasedItemRegistry = ProjectContext.Instance?.PurchasedItemRegistry;
    }

    public override async void Enter()
    {
        await TakeSeatAndOrder();
    }

    private async Task TakeSeatAndOrder()
    {
        if (HasFreeChair(out Chair chair))
        {
            _customer.AssignChair(chair);
            
            _personMover.StartMovingTo(chair.InteractionPoint, () => _tcs.SetResult(true));
            
            await _tcs.Task;
            _customer.DisableAgent();
            _personAnimator.SitDown();
            _transform.position = chair.transform.position + chair.transform.forward * 0.5f;
            await Task.Delay(chair.InteractionTime.ToMiliseconds());
            _customer.MakeOrder();
        }
    }
    
    private bool HasFreeChair(out Chair nearestChair)
    {
        List<Chair> chairs = _purchasedItemRegistry.HallItems
            .OfType<Chair>()
            .Where(x => !x.IsOccupied)
            .ToList();

        int count = chairs.Count;
        if (count > 0)
        {
            if (count > 1)
            {
                nearestChair = chairs[Random.Range(0, chairs.Count)];
                return true;
            }

            nearestChair = chairs.FirstOrDefault();
            return true;
        }

        nearestChair = null;
        return false;
    }

    public override void Exit()
    {
    }
}
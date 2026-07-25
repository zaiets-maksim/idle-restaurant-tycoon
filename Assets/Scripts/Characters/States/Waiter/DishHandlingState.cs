using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Characters.Behaviors;
using Characters.PersonStateMachine;
using Extensions;
using Infrastructure;
using Interactable;
using Services.OrderStorageService;
using Services.PurchasedItemRegistry;
using StaticData;
using UnityEngine;

namespace Characters.States.Waiter
{
    public class DishHandlingState : PersonBaseState
    {
        private readonly WaiterBehavior _waiterBehavior;
        private readonly Transform _transform;
        private readonly PersonMover _personMover;
        private readonly PersonAnimator _personAnimator;
        private readonly DishHolder _dishHolder;
        private readonly IOrderStorageService _orderStorageService;
        private readonly IPurchasedItemRegistry _purchasedItemRegistry;
        
        private IEnumerable<ServingTable> _servingTables;
        private Dish _dish;
        private readonly Characters.Waiter _waiter;

        public DishHandlingState(WaiterBehavior waiterBehavior, Characters.Waiter waiter, Transform transform, PersonMover personMover,
            PersonAnimator personAnimator, DishHolder dishHolder)
        {
            _waiter = waiter;
            _dishHolder = dishHolder;
            _personAnimator = personAnimator;
            _personMover = personMover;
            _transform = transform;
            _waiterBehavior = waiterBehavior;

            _orderStorageService = ProjectContext.Get<IOrderStorageService>();
            _purchasedItemRegistry = ProjectContext.Get<IPurchasedItemRegistry>();
        }

        protected override async Task Enter(CancellationToken ct)
        {
            _purchasedItemRegistry.CleanupDestroyed();
            await HandleOrder(ct);
            
            if (ct.IsCancellationRequested)
                _waiterBehavior.ChangeState<ReturnToSpawnState>();
            else
                _waiterBehavior.ChangeState<OrderDeliveryState>();
        }

        private async Task HandleOrder(CancellationToken ct)
        {
            _servingTables = _purchasedItemRegistry.KitchenItems
                .OfType<ServingTable>()
                .Where(x => x.TypeId == KitchenItemTypeId.ServingTable);

            if (HasServingTableWithDish(out ServingTable nearestServingTable))
            {
                await TaskExtension.WaitFor(callback =>
                {
                    _personMover.StartMovingTo(nearestServingTable.OrderCollectionPoint, callback);
                });

                ct.ThrowIfCancellationRequested();

                _dish = nearestServingTable.GetDish(_waiter.Order.DishTypeId);
                if (_dish == null)
                {
                    return;
                }
                
                _dishHolder.TakeDish(_dish);
                _personAnimator.PutTheItem();

                var time = _personAnimator.GetCurrentClipLength();
                await Task.Delay(time.ToMiliseconds(), ct);
            }
            else
            {
                _waiterBehavior.ChangeState<IdleState>();
            }
        }
        
        
        private bool HasServingTableWithDish(out ServingTable nearestServingTable)
        {
            if(_purchasedItemRegistry.KitchenItems
               .OfType<ServingTable>().Any(x => x.HasDish(_waiter.Order.DishTypeId)))
                
            _servingTables = _purchasedItemRegistry.KitchenItems
                .OfType<ServingTable>()
                .Where(x => x.HasDish(_waiter.Order.DishTypeId))
                .ToList();

            int count = _servingTables.Count();
            if (count > 0)
            {
                if (count > 1)
                {
                    nearestServingTable = TransformExtensions.NearestTo(_transform, _servingTables);
                    return true;
                }

                nearestServingTable = _servingTables.FirstOrDefault();
                return true;
            }

            nearestServingTable = null;
            return false;
        }

        public override void Exit()
        {
        }
    }
}

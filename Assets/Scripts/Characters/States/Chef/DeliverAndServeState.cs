using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Characters.Behaviors;
using Characters.PersonStateMachine;
using Extensions;
using Infrastructure;
using Services.PurchasedItemRegistry;
using UnityEngine;

namespace Characters.States.Chef
{
    public class DeliverAndServeState : PersonBaseState
    {
        private readonly IPurchasedItemRegistry _purchasedItemRegistry;

        private readonly ChefBehavior _chefBehavior;
        private readonly PersonAnimator _personAnimator;
        private readonly PersonMover _personMover;
        private readonly Transform _transform;
        private readonly DishHolder _dishHolder;

        private TaskCompletionSource<bool> _tcs = new();
        private List<ServingTable> _servingTables;
        private ServingTable _servingTable;

        public DeliverAndServeState(ChefBehavior chefBehavior, Transform transform, PersonMover personMover,
            PersonAnimator personAnimator, DishHolder dishHolder)
        {
            _dishHolder = dishHolder;
            _chefBehavior = chefBehavior;
            _purchasedItemRegistry = ProjectContext.Instance?.PurchasedItemRegistry;
            _personAnimator = personAnimator;
            _personMover = personMover;
            _transform = transform;
        }

        public override async void Enter()
        {
            _tcs = new TaskCompletionSource<bool>();
            await DeliverDish();
            await ServeDish();
            
            _chefBehavior.ChangeState<FoodSearchState>();
        }

        private async Task DeliverDish()
        {
            if (GetServingTable(out ServingTable servingTable))
            {
                _servingTable = servingTable;
                _servingTable.Occupy();
                _personMover.StartMovingTo(servingTable.InteractionPoint, () => _tcs.SetResult(true));
                await _tcs.Task;
            }
            else
            {
                _chefBehavior.ChangeState<IdleState>();
            }
        }
        
        private async Task ServeDish()
        {
            _servingTable.PlaceDish(_transform, _dishHolder.Dish);
            _personAnimator.PutTheItem();
            await Task.Delay(1300);
            _servingTable.Release();
        }

        private bool GetServingTable(out ServingTable nearestServingTable)
        {
            _servingTables = _purchasedItemRegistry.KitchenItems
                .OfType<ServingTable>()
                .Where(x => x.HasPlacementForDish)
                .ToList();

            int count = _servingTables.Count;
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
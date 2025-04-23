using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Characters.Behaviors;
using Characters.PersonStateMachine;
using Extensions;
using Infrastructure;
using Services.OrderStorageService;
using Services.PurchasedItemRegistry;
using tetris.Scripts.Extensions;
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
        private readonly IOrderStorageService _orderStorageService;
        private readonly Personal.Chef _chef;

        public DeliverAndServeState(ChefBehavior chefBehavior, Personal.Chef chef, Transform transform, PersonMover personMover,
            PersonAnimator personAnimator, DishHolder dishHolder)
        {
            _chef = chef;
            _dishHolder = dishHolder;
            _chefBehavior = chefBehavior;
            _purchasedItemRegistry = ProjectContext.Instance?.PurchasedItemRegistry;
            _orderStorageService = ProjectContext.Instance?.OrderStorageService;
            _personAnimator = personAnimator;
            _personMover = personMover;
            _transform = transform;
        }

        public override async void Enter()
        {
            _tcs = new TaskCompletionSource<bool>();

            await DeliverDish();
            await ServeDish();
            
            if(_chef.TryGetNewOrder())
                _chefBehavior.ChangeState<FoodSearchState>();
            else
                _chefBehavior.ChangeState<ReturnToSpawnState>();
        }

        private async Task DeliverDish()
        {
            if (GetServingTable(out ServingTable servingTable))
            {
                _servingTable = servingTable;
                _servingTable.Occupy();
                
                await TaskExtension.WaitFor(callback =>
                {
                    _personMover.StartMovingTo(servingTable.InteractionPoint, callback);
                });
            }
            else
            {
                _chefBehavior.ChangeState<ReturnToSpawnState>();
            }
        }
        
        private async Task ServeDish()
        {
            Debug.Log(_dishHolder.Dish);
            _servingTable.PlaceDish(_transform, _dishHolder.Dish);
            _personAnimator.PutTheItem();
            _chef.Cooked(_chef.Order);

            var time = _personAnimator.GetCurrentClipLength();
            await Task.Delay(time.ToMiliseconds());
            
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
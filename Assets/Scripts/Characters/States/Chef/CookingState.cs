using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Characters.Behaviors;
using Characters.PersonStateMachine;
using Extensions;
using Infrastructure;
using Interactable;
using Services.DataStorageService;
using Services.PurchasedItemRegistry;
using UnityEngine;

namespace Characters.States.Chef
{
    public class CookingState : PersonBaseState
    {
        private readonly IPurchasedItemRegistry _purchasedItemRegistry;
        private readonly Transform _transform;
        private readonly PersonMover _personMover;
        private readonly ChefBehavior _chefBehavior;
        private readonly DishHolder _dishHolder;

        private List<FoodStation> _foodStations;
        private readonly PersonAnimator _personAnimator;
        private readonly Personal.Chef _chef;
        private readonly IPersistenceProgressService _progress;

        public CookingState(ChefBehavior chefBehavior, Personal.Chef chef, Transform transform, PersonMover personMover, PersonAnimator personAnimator, 
            DishHolder dishHolder)
        {
            _chef = chef;
            _dishHolder = dishHolder;
            _purchasedItemRegistry = ProjectContext.Get<IPurchasedItemRegistry>();
            _progress = ProjectContext.Get<IPersistenceProgressService>();
            _personAnimator = personAnimator;
            _personMover = personMover;
            _transform = transform;
            _chefBehavior = chefBehavior;
        }
    
        protected override async Task Enter(CancellationToken ct)
        {
            _purchasedItemRegistry.CleanupDestroyed();

            await Cook(ct);

            if (ct.IsCancellationRequested)
            {
                _chefBehavior.ChangeState<IdleState>();
                return;
            }

            _personAnimator.Idle();
            _chefBehavior.ChangeState<DeliverAndServeState>();
        }

        private async Task Cook(CancellationToken ct)
        {
            if (GetFoodStation(out FoodStation foodStation))
            {
                foodStation.Occupy();
                
                await TaskExtension.WaitFor(callback =>
                {
                    _personMover.StartMovingTo(foodStation.InteractionPoint, callback);
                });

                ct.ThrowIfCancellationRequested();
                var dish = foodStation.MakeDish(_chef.Order.DishTypeId);
                _personAnimator.Cook();
                
                var time = TimeExtensions.RandomTime(5, 15) - _progress.PlayerData.ProgressData.Staff.Chef.CookingTimeDelay;
                time = Mathf.Max(time, 2f);
                
                await TaskExtension.WaitFor(callback =>
                {
                    _chef.ProgressIndicator.StartProgress(time, callback);
                });

                ct.ThrowIfCancellationRequested();
                _dishHolder.TakeDish(dish);
                foodStation.Release();
                _personAnimator.Idle();
            }
        }

        private bool GetFoodStation(out FoodStation nearestFoodStation)
        {
            _foodStations = _purchasedItemRegistry.KitchenItems
                .OfType<FoodStation>()
                .Where(x => !x.IsOccupied)
                .Where(x => x.DishTypeId.Contains(_chef.Order.DishTypeId))
                .ToList();
            
            int count = _foodStations.Count;
            if (count > 0)
            {
                if (count > 1)
                {
                    nearestFoodStation = TransformExtensions.NearestTo(_transform, _foodStations);
                    return true;
                }

                nearestFoodStation = _foodStations.FirstOrDefault();
                return true;
            }

            nearestFoodStation = null;
            return false;
        }
    
        public override void Exit()
        {
        }
    }
}

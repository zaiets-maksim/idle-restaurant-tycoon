using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Characters.Behaviors;
using Characters.PersonStateMachine;
using Extensions;
using Infrastructure;
using Interactable;
using Services.OrderStorageService;
using Services.PurchasedItemRegistry;
using tetris.Scripts.Extensions;
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
        TaskCompletionSource<bool> _tcs = new();
        private readonly PersonAnimator _personAnimator;
        private readonly Personal.Chef _chef;

        public CookingState(ChefBehavior chefBehavior, Personal.Chef chef, Transform transform, PersonMover personMover, PersonAnimator personAnimator, 
            DishHolder dishHolder)
        {
            _chef = chef;
            _dishHolder = dishHolder;
            _purchasedItemRegistry = ProjectContext.Instance?.PurchasedItemRegistry;
            _personAnimator = personAnimator;
            _personMover = personMover;
            _transform = transform;
            _chefBehavior = chefBehavior;
        }
    
        public override async void Enter()
        {
            _tcs = new TaskCompletionSource<bool>();
            await Cook();
            _personAnimator.Idle();
            _chefBehavior.ChangeState<DeliverAndServeState>();
        }

        private async Task Cook()
        {
            if (GetFoodStation(out FoodStation foodStation))
            {
                foodStation.Occupy();
                _personMover.StartMovingTo(foodStation.InteractionPoint, () => _tcs.SetResult(true));
                await _tcs.Task;

                var dish = foodStation.MakeDish(_chef.Order.DishTypeId);
                _personAnimator.Cook();
                int randomTime = TimeExtensions.RandomTime(5, 15).ToMiliseconds();
                await Task.Delay(randomTime);
                
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

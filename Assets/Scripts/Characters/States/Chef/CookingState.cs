using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Characters.Behaviors;
using Characters.PersonStateMachine;
using Cysharp.Threading.Tasks;
using Extensions;
using Infrastructure;
using Interactable;
using Services.DataStorageService;
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
        private readonly PersonAnimator _personAnimator;
        private readonly Personal.Chef _chef;
        private readonly IPersistenceProgressService _progress;
        
        private CancellationTokenSource _cts = new();
        

        public CookingState(ChefBehavior chefBehavior, Personal.Chef chef, Transform transform, PersonMover personMover, PersonAnimator personAnimator, 
            DishHolder dishHolder, IPurchasedItemRegistry purchasedItemRegistry, IPersistenceProgressService progress)
        {
            _chef = chef;
            _dishHolder = dishHolder;
            _purchasedItemRegistry = purchasedItemRegistry;
            _progress = progress;
            _personAnimator = personAnimator;
            _personMover = personMover;
            _transform = transform;
            _chefBehavior = chefBehavior;
        }
    
        public override async void Enter()
        {
            _cts = new CancellationTokenSource();

            await Cook();

            if (_cts.IsCancellationRequested)
            {
                Debug.Log(Make.Colored($"All dish stations are occupy now {_chef.gameObject.GetInstanceID()}", Color.red));
                Debug.Log(Make.Colored($"{_chefBehavior.IsTransitioning} {_chef.gameObject.GetInstanceID()}", Color.red));
                
                while (_chefBehavior.IsTransitioning)
                    await UniTask.Yield();
                
                Debug.Log(Make.Colored($"{_chefBehavior.IsTransitioning} {_chef.gameObject.GetInstanceID()}", Color.red));
                _chefBehavior.ChangeState<IdleState>();
                return;
            }

            _personAnimator.Idle();
            _chefBehavior.ChangeState<DeliverAndServeState>();
        }

        private async UniTask Cook()
        {
            if(_cts.IsCancellationRequested)
                return;
            
            if (GetFoodStation(out FoodStation foodStation))
            {
                foodStation.Occupy();
                
                await TaskExtension.WaitFor(callback =>
                {
                    _personMover.StartMovingTo(foodStation.InteractionPoint, callback);
                });

                var dish = foodStation.MakeDish(_chef.Order.DishTypeId);
                _personAnimator.Cook();
                
                var time = TimeExtensions.RandomTime(5, 15) - _progress.PlayerData.ProgressData.Staff.Chef.CookingTimeDelay;
                time = Mathf.Max(time, 2f);
                
                await TaskExtension.WaitFor(callback =>
                {
                    _chef.ProgressIndicator.StartProgress(time, callback);
                });

                _dishHolder.TakeDish(dish);
                foodStation.Release();
                _personAnimator.Idle();
            }
            else
            {
                _cts.Cancel();
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
            _cts.Cancel();
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Characters.Behaviors;
using Characters.PersonStateMachine;
using Cysharp.Threading.Tasks;
using Extensions;
using Interactable;
using Services.DataStorageService;
using Services.PurchasedItemRegistry;
using tetris.Scripts.Extensions;
using UnityEngine;

namespace Characters.States.Chef
{
    public class FoodSearchState : PersonBaseState
    {
        private readonly IPurchasedItemRegistry _purchasedItemRegistry;
        private readonly PersonMover _personMover;
        private readonly PersonAnimator _personAnimator;
        private readonly ChefBehavior _chefBehavior;
        private readonly Transform _transform;
        private readonly Personal.Chef _chef;

        private List<Fridge> _fridgesWithFood;
        UniTaskCompletionSource<bool> _tcs = new();
        private readonly IPersistenceProgressService _progress;

        public FoodSearchState(ChefBehavior chefBehavior, Personal.Chef chef, PersonMover personMover, 
            PersonAnimator personAnimator, IPurchasedItemRegistry purchasedItemRegistry,
            IPersistenceProgressService progress)
        {
            _personAnimator = personAnimator;
            _chefBehavior = chefBehavior;
            _personMover = personMover;
            _chef = chef;
            _transform = chef.transform;
            _purchasedItemRegistry = purchasedItemRegistry;
            _progress = progress;
        }

        public override async void Enter()
        {
            _tcs = new UniTaskCompletionSource<bool>();
            
            await GetSomeFood();
            _personAnimator.Idle();
            _chefBehavior.ChangeState<CookingState>();
        }

        private async UniTask GetSomeFood()
        {
            if (GetFridges(out Fridge fridge))
            {
                if (Random.value > 0.5f)
                {
                    await GetFoodFromStorage();
                    return;
                }

                await GetFoodFromFridge(fridge);
            }
            else
            {
                await GetFoodFromStorage();
            }
        }

        private async UniTask GetFoodFromFridge(Fridge fridge)
        {
            fridge.Occupy();
            
            await TaskExtension.WaitFor(callback =>
            {
                _personMover.StartMovingTo(fridge.InteractionPoint, callback);
            });

            fridge.Interact();
            _personAnimator.PutTheItem();
            
            var time = fridge.InteractionTime + fridge.DelayAfterClose - _progress.PlayerData.ProgressData.Staff.Chef.FoodSearchingTimeDelay;
            time = Mathf.Max(time, 2f);
            
            await TaskExtension.WaitFor(callback =>
            {
                _chef.ProgressIndicator.StartProgress(time, callback);
            });
        }

        private async UniTask GetFoodFromStorage()
        {
            if (GetRandomCrateInteractionPoint(out Transform point))
            {
                await TaskExtension.WaitFor(callback =>
                {
                    _personMover.StartMovingTo(point, callback);
                });

                _personAnimator.PickUp();

                var time = TimeExtensions.RandomTime(5, 15) - _progress.PlayerData.ProgressData.Staff.Chef.FoodSearchingTimeDelay;
                time = Mathf.Max(time, 2f);

                await TaskExtension.WaitFor(callback =>
                {
                    _chef.ProgressIndicator.StartProgress(time, callback);
                });
            }
        }

        private bool GetRandomCrateInteractionPoint(out Transform point)
        {
            point = _purchasedItemRegistry.StorageCrates
                .OrderBy(x => Random.value > 0.5f)
                .FirstOrDefault()?.InteractionPoint;
            return point;
        }

        private bool GetFridges(out Fridge nearestFridge)
        {
            _fridgesWithFood = _purchasedItemRegistry.KitchenItems
                .OfType<Fridge>()
                .Where(x => !x.IsOccupied)
                .ToList();

            int count = _fridgesWithFood.Count;
            if (count > 0)
            {
                if (count > 1)
                {
                    nearestFridge = TransformExtensions.NearestTo(_transform, _fridgesWithFood);
                    return true;
                }

                nearestFridge = _fridgesWithFood.FirstOrDefault();
                return true;
            }

            nearestFridge = null;
            return false;
        }

        public override void Exit()
        {
        }
    }
}
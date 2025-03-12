using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Characters.Behaviors;
using Characters.PersonStateMachine;
using Extensions;
using Infrastructure;
using Interactable;
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
        TaskCompletionSource<bool> _tcs = new();

        public FoodSearchState(ChefBehavior chefBehavior, Personal.Chef chef, PersonMover personMover, PersonAnimator personAnimator)
        {
            _personAnimator = personAnimator;
            _chefBehavior = chefBehavior;
            _personMover = personMover;
            _chef = chef;
            _transform = chef.transform;
            _purchasedItemRegistry = ProjectContext.Instance?.PurchasedItemRegistry;
        }

        public override async void Enter()
        {
            _tcs = new TaskCompletionSource<bool>();
            await GetSomeFood();
            _personAnimator.Idle();
            _chefBehavior.ChangeState<CookingState>();
        }

        private async Task GetSomeFood()
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

        private async Task GetFoodFromFridge(Fridge fridge)
        {
            fridge.Occupy();
            _personMover.StartMovingTo(fridge.InteractionPoint, () => _tcs.SetResult(true));

            await _tcs.Task;
            fridge.Interact();
            _personAnimator.PutTheItem();
            await Task.Delay((fridge.InteractionTime + fridge.DelayAfterClose).ToMiliseconds());
        }

        private async Task GetFoodFromStorage()
        {
            if (GetRandomCrateInteractionPoint(out Transform point))
            {
                _personMover.StartMovingTo(point, () => _tcs.SetResult(true));
                await _tcs.Task;
                _personAnimator.PickUp();
                int randomTime = TimeExtensions.RandomTime(5, 15).ToMiliseconds();
                await Task.Delay(randomTime);
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
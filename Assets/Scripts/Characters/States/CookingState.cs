using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Characters.Personal;
using Characters.PersonStateMachine;
using Extensions;
using Infrastructure;
using Interactable;
using Services.PurchasedItemRegistry;
using tetris.Scripts.Extensions;
using UnityEngine;

namespace Characters.States
{
    public class CookingState : PersonBaseState
    {
        private readonly IPurchasedItemRegistry _purchasedItemRegistry;
        private readonly Transform _transform;
        private readonly PersonMover _personMover;
        private ChefBehavior _chefBehavior;

        private List<FoodStation> _foodStations;
        readonly TaskCompletionSource<bool> _tcs = new();
        private readonly PersonAnimator _personAnimator;

        public CookingState(ChefBehavior chefBehavior, Transform transform, PersonMover personMover, PersonAnimator personAnimator)
        {
            _purchasedItemRegistry = ProjectContext.Instance?.PurchasedItemRegistry;
            _personAnimator = personAnimator;
            _personMover = personMover;
            _transform = transform;
            _chefBehavior = chefBehavior;
        }
    
        public override void Enter()
        {
            _ = Cook();
            _personAnimator.Idle();
        }

        public async Task Cook()
        {
            if (GetFoodStation(out FoodStation foodStation))
            {
                foodStation.Occupy();
                
                _personMover.StartMovingTo(foodStation.InteractionPoint, () => _tcs.SetResult(true));
                await _tcs.Task;
                _personAnimator.Cook();
                int randomTime = TimeExtensions.RandomTime(5, 15).ToMiliseconds();
                await Task.Delay(randomTime);
            }
        }

        private bool GetFoodStation(out FoodStation nearestFoodStation)
        {
            _foodStations = _purchasedItemRegistry.KitchenItems
                .OfType<FoodStation>()
                .Where(x => !x.IsOccupied)
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

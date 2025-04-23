using System.Linq;
using System.Threading.Tasks;
using Characters.Behaviors;
using Characters.States;
using Characters.States.Chef;
using Extensions;
using Infrastructure;
using Interactable;
using Services.OrderStorageService;
using Services.PurchasedItemRegistry;
using UnityEngine;

namespace Characters.Personal
{
    public class Chef : Employee, ICook
    {
        [SerializeField] private ChefBehavior _chefBehavior;
        [SerializeField] private PersonItemCollector _itemCollector;

        private IOrderStorageService _orderStorageService;
        private IPurchasedItemRegistry _purchasedItemRegistry;

        public ChefBehavior ChefBehavior => _chefBehavior;
        public bool IsIdle => _chefBehavior.CurrentState is IdleState | _chefBehavior.CurrentState is ReturnToSpawnState;
        public int Food => _itemCollector.Food;
        public bool HasFood => _itemCollector.Food > 0;
        public Order Order { get; private set; }


        public override void Start()
        {
            base.Start();
            _purchasedItemRegistry = ProjectContext.Instance?.PurchasedItemRegistry;
            _orderStorageService = ProjectContext.Instance?.OrderStorageService;
            _orderStorageService!.OnNewOrderReceived += TryChangeToCookingState;
            _progress!.PlayerData.ProgressData.Staff.Chef.OnSpeedUpdated += UpdateAgentSpeed;

            foreach (var kitchenItem in _purchasedItemRegistry!.KitchenItems)
                if (kitchenItem is FoodStation) 
                    kitchenItem.OnRelease += TryChangeToCookingState;
            
            _purchasedItemRegistry!.OnNewItemKitchenPurchased += kitchenItem =>
            {
                if(kitchenItem is FoodStation)
                    kitchenItem.OnRelease += TryChangeToCookingState;
            };
            
            UpdateAgentSpeed(_progress.PlayerData.ProgressData.Staff.Chef.Speed);
            _spawnPosition = transform.position;
        }

        private void TryChangeToCookingState(KitchenItem kitchenItem)
        {
            Debug.Log(Make.Colored($"TryChangeToCookingState {gameObject.GetInstanceID()}", Color.cyan));
            Debug.Log($"{_chefBehavior.CurrentState}");
            Debug.Log($"{Order}");
            Debug.Log("\n");

            if (IsIdle && Order != null && ((FoodStation)kitchenItem).DishTypeId.Any(id => id == Order.DishTypeId))
            {
                Debug.Log(Make.Colored($"-> FoodSearchState {gameObject.GetInstanceID()}", Color.yellow));
                _chefBehavior.ChangeState<FoodSearchState>();
            }
        }

        private void OnDestroy()
        {
            _orderStorageService!.OnNewOrderReceived -= TryChangeToCookingState;
            _progress!.PlayerData.ProgressData.Staff.Waiter.OnSpeedUpdated -= UpdateAgentSpeed;
        }

        private void TryChangeToCookingState(Order order)
        {
            if (!IsIdle)
                return;

            if (TryGetNewOrder())
            {
                Debug.Log(Make.Colored($"-> FoodSearchState {gameObject.GetInstanceID()}", Color.yellow));
                _chefBehavior.ChangeState<FoodSearchState>();
            }
        }

        public override void PerformDuties()
        {
            
        }

        public void Cook()
        {
            
        }
        
        
        public bool TryGetNewOrder()
        {
            if(Order != null)
                return false;
            
            // Debug.Log($"{gameObject.name} - {_orderStorageService.HasOrders()}");
            if (_orderStorageService.HasOrders())
            {
                Order = _orderStorageService.GetOrder();
                // Debug.Log($"{gameObject.name} want to take {Order.DishTypeId}");
                
                return true;
            }
            
            return false;
        }

        public void Cooked(Order order)
        {
            Order = null;
            _orderStorageService.Cooked(order);
        }
    }
}

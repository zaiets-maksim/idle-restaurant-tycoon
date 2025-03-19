using Characters.Behaviors;
using Characters.States;
using Characters.States.Chef;
using Infrastructure;
using Services.OrderStorageService;
using UnityEngine;

namespace Characters.Personal
{
    public class Chef : Employee, ICook
    {
        [SerializeField] private ChefBehavior _chefBehavior;
        [SerializeField] private PersonItemCollector _itemCollector;

        private IOrderStorageService _orderStorageService;

        public bool IsIdle => _chefBehavior.CurrentState is IdleState;
        public int Food => _itemCollector.Food;
        public bool HasFood => _itemCollector.Food > 0;
        public Order Order { get; private set; }


        public override void Start()
        {
            base.Start();
            _orderStorageService = ProjectContext.Instance?.OrderStorageService;
            _orderStorageService!.OnNewOrderReceived += TryChangeToCookingState;
            
            _progress!.PlayerData.ProgressData.Staff.Chef.OnSpeedUpdated += UpdateAgentSpeed;
            UpdateAgentSpeed(_progress.PlayerData.ProgressData.Staff.Chef.Speed);
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
            
            if (GotNewOrder()) 
                _chefBehavior.ChangeState<FoodSearchState>();
        }

        public override void PerformDuties()
        {
            
        }

        public void Cook()
        {
            
        }
        
        
        public bool GotNewOrder()
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

using System;
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
        }

        private void TryChangeToCookingState(Order order)
        {
            if (IsIdle)
            {
                UpdateOrder();
                _chefBehavior.ChangeState<FoodSearchState>();
            }
        }

        public override void PerformDuties()
        {
            
        }

        public void Cook()
        {
            
        }

        public bool HasOrders() => _orderStorageService.HasOrders();
        
        public void UpdateOrder() => Order = _orderStorageService.GetOrder();
    }
}

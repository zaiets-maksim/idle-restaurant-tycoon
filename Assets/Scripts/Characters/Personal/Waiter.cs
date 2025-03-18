using System;
using Characters.Behaviors;
using Characters.States;
using Characters.States.Waiter;
using Infrastructure;
using Services.OrderStorageService;
using UnityEngine;

namespace Characters
{
    public class Waiter : Employee, IServiceWorker
    {
        [SerializeField] private WaiterBehavior _waiterBehavior;
        
        private IOrderStorageService _orderStorageService;

        public bool IsIdle => _waiterBehavior.CurrentState is IdleState;
        public Order Order { get; private set; }

        public override void Start()
        {
            base.Start();
            _orderStorageService = ProjectContext.Instance?.OrderStorageService;
            _orderStorageService!.OnOrderCooked += TryChangeToDishHandlingState;
        }

        public Waiter()
        {
            
        }

        private void TryChangeToDishHandlingState(Order order)
        {
            if (IsIdle)
                _waiterBehavior.ChangeState<DishHandlingState>();
        }

        public void GetOrder()
        {
            if(_orderStorageService.HasOrdersForServe())
                Order = _orderStorageService.GetOrderForServe();
        }

        public override void PerformDuties()
        {
            
        }

        public void ServeCustomer()
        {
        }

        public void TakeOrder()
        {
        }
    
        public void ServeFood()
        {
        }

        public void CalculateBill()
        {
        }
    }
}

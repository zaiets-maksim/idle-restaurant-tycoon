using System.Threading.Tasks;
using Characters.Behaviors;
using Characters.States;
using Characters.States.Waiter;
using Extensions;
using Infrastructure;
using Services.OrderStorageService;
using UnityEngine;

namespace Characters
{
    public class Waiter : Employee, IServiceWorker
    {
        [SerializeField] private WaiterBehavior _waiterBehavior;
        
        private IOrderStorageService _orderStorageService;

        public bool IsIdle => _waiterBehavior.CurrentState is IdleState or LeaveHallState;
        public Order Order { get; private set; }

        public override void Start()
        {
            base.Start();
            _orderStorageService = ProjectContext.Instance?.OrderStorageService;
            _orderStorageService!.OnOrderCooked += TryChangeToDishHandlingState;

            _progress!.PlayerData.ProgressData.Staff.Waiter.OnSpeedUpdated += UpdateAgentSpeed;
            UpdateAgentSpeed(_progress.PlayerData.ProgressData.Staff.Waiter.Speed);
            _spawnPosition = transform.position;
        }

        private void OnDestroy()
        {
            _orderStorageService!.OnOrderCooked -= TryChangeToDishHandlingState;
            _progress!.PlayerData.ProgressData.Staff.Waiter.OnSpeedUpdated -= UpdateAgentSpeed;
        }

        private void TryChangeToDishHandlingState(Order order)
        {
            if (!IsIdle)
                return;
            
            if(GotNewOrder())
                _waiterBehavior.ChangeState<DishHandlingState>();
        }

        public async Task LeaveHall()
        {
            await TaskExtension.WaitFor(callback =>
            {
                _personMover.StartMovingTo(_spawnPosition, callback);
            });
        }
        
        public bool GotNewOrder()
        {
            if(Order != null)
                return false;
            
            Debug.Log($"{gameObject.name} - {_orderStorageService.HasOrdersForServe()}");
            if(_orderStorageService.HasOrdersForServe())
            {
                Order = _orderStorageService.GetOrderForServe();
                Debug.Log($"{gameObject.name} want to take {Order.DishTypeId}");
                
                return true;
            }
            
            return false;
        }
        
        public void Delivered()
        {
            Order = null;
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

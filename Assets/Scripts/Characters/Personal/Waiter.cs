using System.Threading.Tasks;
using Characters.Behaviors;
using Characters.States;
using Characters.States.Waiter;
using Cysharp.Threading.Tasks;
using Extensions;
using Infrastructure;
using Services.OrderStorageService;
using UnityEngine;

namespace Characters
{
    public class Waiter : Employee, IServiceWorker
    {
        [SerializeField] private WaiterBehavior _waiterBehavior;

        public WaiterBehavior WaiterBehavior => _waiterBehavior;
        public bool IsIdle => _waiterBehavior.CurrentState is IdleState or ReturnToSpawnState;
        public Order Order { get; private set; }

        public override void Start()
        {
            base.Start();
            _orderStorageService.OnOrderCooked += TryChangeToDishHandlingState;
            _progress.PlayerData.ProgressData.Staff.Waiter.OnSpeedUpdated += UpdateAgentSpeed;
            
            UpdateAgentSpeed(_progress.PlayerData.ProgressData.Staff.Waiter.Speed);
            _spawnPosition = transform.position;
        }

        private void OnDestroy()
        {
            _orderStorageService.OnOrderCooked -= TryChangeToDishHandlingState;
            _progress.PlayerData.ProgressData.Staff.Waiter.OnSpeedUpdated -= UpdateAgentSpeed;
        }

        private void TryChangeToDishHandlingState(Order order)
        {
            if (!IsIdle)
                return;
            
            if(Order != null && HasSameOrderType(order) || TryGetNewOrder())
                _waiterBehavior.ChangeState<DishHandlingState>();
        }

        public new async UniTask MoveToSpawn()
        {
            await TaskExtension.WaitFor(callback =>
            {
                _personMover.StartMovingTo(_spawnPosition, callback);
            });
        }

        public bool HasSameOrderType(Order order) => Order.DishTypeId == order.DishTypeId;

        public bool TryGetNewOrder()
        {
            Debug.Log($"{gameObject.name} ({gameObject.GetInstanceID()}) - {_orderStorageService.HasOrdersForServe()}");
            if(_orderStorageService.HasOrdersForServe())
            {
                Order = _orderStorageService.GetOrderForServe();
                Debug.Log($"{gameObject.name} ({gameObject.GetInstanceID()}) got order: {Order.DishTypeId}");
                
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

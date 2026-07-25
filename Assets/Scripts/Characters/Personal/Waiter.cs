using System.Threading.Tasks;
using Characters.Behaviors;
using Characters.States;
using Characters.States.Waiter;
using Extensions;
using Infrastructure;
using Services.OrderStorageService;
using Services.ProgressEventService;
using UnityEngine;

namespace Characters
{
    public class Waiter : Employee, IServiceWorker
    {
        [SerializeField] private WaiterBehavior _waiterBehavior;
        private IOrderStorageService _orderStorageService;
        private IProgressEventService _progressEventService;

        public WaiterBehavior WaiterBehavior => _waiterBehavior;
        public bool IsIdle => _waiterBehavior.CurrentState is IdleState or ReturnToSpawnState;
        public Order Order { get; private set; }

        public override void Start()
        {
            base.Start();
            _orderStorageService = ProjectContext.Get<IOrderStorageService>();
            _progressEventService = ProjectContext.Get<IProgressEventService>();
            _orderStorageService.OnOrderCooked += TryChangeToDishHandlingState;

            _progressEventService.OnWaiterSpeedUpdated += UpdateAgentSpeed;
            UpdateAgentSpeed(_progress.PlayerData.ProgressData.Staff.Waiter.Speed);
            _spawnPosition = transform.position;
        }

        private void OnDisable()
        {
            if (_orderStorageService != null)
                _orderStorageService.OnOrderCooked -= TryChangeToDishHandlingState;
            if (_progressEventService != null)
                _progressEventService.OnWaiterSpeedUpdated -= UpdateAgentSpeed;
        }

        private void TryChangeToDishHandlingState(Order order)
        {
            if (!IsIdle)
                return;
            
            if(Order != null && HasSameOrderType(order) || TryGetNewOrder())
                _waiterBehavior.ChangeState<DishHandlingState>();
        }

        public new async Task MoveToSpawn()
        {
            await TaskExtension.WaitFor(callback =>
            {
                _personMover.StartMovingTo(_spawnPosition, callback);
            });
        }

        public bool HasSameOrderType(Order order) => Order.DishTypeId == order.DishTypeId;

        public bool TryGetNewOrder()
        {
            if(_orderStorageService.HasOrdersForServe())
            {
                Order = _orderStorageService.GetOrderForServe();
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

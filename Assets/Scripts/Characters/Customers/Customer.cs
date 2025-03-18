using System.Threading.Tasks;
using Extensions;
using Infrastructure;
using Interactable;
using Services.CurrencyService;
using Services.OrderStorageService;
using Services.StaticDataService;
using StaticData;
using StaticData.Configs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Characters.Customers
{
    [RequireComponent(typeof(CustomerBehavior))]
    public class Customer : Person
    {
        [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
        [SerializeField] private CustomerBehavior _customerBehavior;
        [SerializeField] private Transform _dishPoint;
        [SerializeField] private PersonMover _personMover;
        
        private readonly IStaticDataService _staticData;
        private readonly BalanceStaticData _balance;
        private readonly ICurrencyService _currencyService;
        private readonly IOrderStorageService _orderStorageService;
        private DishTypeId _dishTypeId;
        private float _mealDuration;
        private Dish _dish;
        private readonly TaskCompletionSource<bool> _tcs;
        private Vector3 _lastPosition;
        private Vector3 _spawnPosition;

        public bool IsAwaiting => _customerBehavior.CurrentState is SeatAndOrderState && _dishTypeId != DishTypeId.Unknown;
        public Transform DishPoint => _dishPoint;
        public Chair Chair { get; private set; }

        public Customer()
        {
            _tcs = new TaskCompletionSource<bool>();
            
            _staticData = ProjectContext.Instance?.StaticData;
            _orderStorageService = ProjectContext.Instance?.OrderStorageService;
            _balance = _staticData?.Balance();
            _currencyService = ProjectContext.Instance?.CurrencyService;
            _progress = ProjectContext.Instance?.Progress;
        }

        public override void Start()
        {
            base.Start();    
            _spawnPosition = transform.position;
        }
        
        public override void PerformDuties()
        {
        }

        public async void LeaveRestaurant()
        {
            await TaskExtension.WaitFor(callback =>
            {
                _personMover.StartMovingTo(_spawnPosition, callback);
            });
            
            Destroy(gameObject);
        }
        
        public void AssignChair(Chair chair)
        {
            Chair = chair;
            chair.Occupy();
        }

        public void MakeOrder()
        {
            DishTypeId[] dishTypeIds = _staticData.GetDishTypeIds();
            _dishTypeId = dishTypeIds[Random.Range(0, dishTypeIds.Length)];
            _orderStorageService.NewOrder(new Order(_dishTypeId, this));
        }
        
        public async Task Eat()
        {
            await TaskExtension.WaitFor(callback =>
            {
                ProgressIndicator.StartProgress(_mealDuration, callback);
            });
            
            Destroy(_dish.gameObject);
        }

        public void PayBill()
        {
            _currencyService.AddMoney(_dish.Price);
        }
        
        public void SetAppearance(CustomerAppearance appearance)
        {
            _skinnedMeshRenderer.sharedMesh = appearance.Mesh;
            _skinnedMeshRenderer.materials = appearance.Materials;
        }

        public void SetRandomCharacteristics()
        {
            float randomSpeed = Random.Range(_balance.Customers.Speed.x, _balance.Customers.Speed.y);
            _navMeshAgent.speed = randomSpeed;
            float coefficient = randomSpeed / _balance.Customers.DefaultSpeed;
            _personAnimator.SetSpeed(coefficient);
            Debug.Log(_progress.PlayerData.ProgressData.Customers.EatingTimeDelay);
            _mealDuration = Random.Range(_balance.Customers.MealDurationInterval.x, _balance.Customers.MealDurationInterval.y) - 
                _progress.PlayerData.ProgressData.Customers.EatingTimeDelay;
        }

        public void SetObjectName(string name) => 
            gameObject.name = name;

        public void TakeDish(Dish dish)
        {
            _dish = dish;
            
            dish.transform.SetPositionAndRotation(_dishPoint.position, _dishPoint.rotation);
            dish.transform.SetParent(transform);
            
            _customerBehavior.ChangeState<EatAndPayState>();
        }

        public void SetPosition(Vector3 position)
        {
            _lastPosition = position;
            transform.position = position;
        }

        public void LeaveChair()
        {
            transform.position = _lastPosition;
            Chair.Release();
        }
    }
}
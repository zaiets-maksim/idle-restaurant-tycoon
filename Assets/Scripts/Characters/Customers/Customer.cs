using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Extensions;
using Interactable;
using Services.CurrencyService;
using Services.OrderStorageService;
using Services.StaticDataService;
using StaticData;
using StaticData.Configs;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using Random = UnityEngine.Random;

namespace Characters.Customers
{
    [RequireComponent(typeof(CustomerBehavior))]
    public class Customer : Person
    {
        [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
        [SerializeField] private CustomerBehavior _customerBehavior;
        [SerializeField] private Transform _dishPoint;
        [FormerlySerializedAs("_tipNotifier")] [SerializeField] private BillNotifier billNotifier;

        private IStaticDataService _staticData;
        private ICurrencyService _currencyService;

        private BalanceStaticData _balance;
        private DishTypeId _dishTypeId;
        private float _mealDuration;
        private Dish _dish;
        private Vector3 _lastPosition;

        public bool IsAwaiting => _customerBehavior.CurrentState is SeatAndOrderState && _dishTypeId != DishTypeId.Unknown;
        public Transform DishPoint => _dishPoint;
        public Chair Chair { get; private set; }

        [Inject]
        public void Constructor(IStaticDataService staticData, ICurrencyService currencyService)
        {
            _staticData = staticData;
            _currencyService = currencyService;
            _balance = _staticData.Balance();
        }

        public override void Start()
        {
            base.Start();
            _spawnPosition = transform.position;
        }
        
        public override void PerformDuties()
        {
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
        
        public async UniTask Eat()
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
            billNotifier.ShowFloatingBill(_dish.Price);
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
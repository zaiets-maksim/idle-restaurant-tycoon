using System.Threading.Tasks;
using Infrastructure;
using Interactable;
using Services.CurrencyService;
using Services.OrderStorageService;
using Services.StaticDataService;
using StaticData;
using StaticData.Configs;
using tetris.Scripts.Extensions;
using UnityEngine;

namespace Characters.Customers
{
    [RequireComponent(typeof(CustomerBehavior))]
    public class Customer : Person
    {
        [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
        [SerializeField] private PersonAnimator _personAnimator;
        [SerializeField] private CustomerBehavior _customerBehavior;
        [SerializeField] private Transform _dishPoint;
        
        private readonly IStaticDataService _staticData;
        private readonly BalanceStaticData _balance;
        private readonly ICurrencyService _currencyService;
        private readonly IOrderStorageService _orderStorageService;
        private DishTypeId _dishTypeId;
        private float _mealDuration;
        public bool IsAwaiting => _customerBehavior.CurrentState is SeatAndOrderState && _dishTypeId != DishTypeId.Unknown;
        public Transform DishPoint => _dishPoint;
        public Chair Chair { get; private set; }

        public Customer()
        {
            _staticData = ProjectContext.Instance?.StaticData;
            _orderStorageService = ProjectContext.Instance?.OrderStorageService;
            _balance = _staticData?.Balance();
            _currencyService = ProjectContext.Instance?.CurrencyService;
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
        
        public async Task Eat(Dish dish)
        {
            Debug.Log(_mealDuration.ToMiliseconds());
            await Task.Delay(_mealDuration.ToMiliseconds());
        }

        public void PayBill(Dish dish)
        {
            _currencyService.AddMoney(dish.Price);
            Destroy(dish.gameObject);
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
            _mealDuration = Random.Range(_balance.Customers.MealDurationInterval.x, _balance.Customers.MealDurationInterval.y);
        }

        public void SetObjectName(string name) => 
            gameObject.name = name;

        public async void TakeDish(Dish dish)
        {
            dish.transform.SetPositionAndRotation(_dishPoint.position, _dishPoint.rotation);
            dish.transform.SetParent(transform);
            
            await Eat(dish);
            PayBill(dish);
        }
    }
}
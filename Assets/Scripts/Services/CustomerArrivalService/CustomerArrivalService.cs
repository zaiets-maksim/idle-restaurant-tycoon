using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Characters.Customers;
using Infrastructure;
using Services.ActiveCustomersRegistry;
using Services.Factories.CharacterFactory;
using Services.ItemBuyingService;
using Services.PurchasedItemRegistry;
using Services.StaticDataService;
using StaticData;
using StaticData.Levels;
using StaticData.TypeId;
using Unity.VisualScripting;
using UnityEngine;

namespace Services.CustomerArrivalService
{
    public class CustomerArrivalService : ICustomerArrivalService
    {
        private readonly CoroutineRunner _coroutineRunner;
        private readonly IStaticDataService _staticData;
        private readonly IItemBuyingService _itemBuyingService;
        private readonly IPurchasedItemRegistry _purchasedItemRegistry;
        private readonly ICharacterFactory _characterFactory;
        private readonly LevelStaticData _levelStaticData;
        private readonly BalanceStaticData _balance;
        private readonly IActiveCustomersRegistry _activeCustomersRegistry;

        private Coroutine _spawningCoroutine;
        private int _customersQuantity;
        private readonly List<CharacterData> _customersPointsForSpawn;

        public CustomerArrivalService(IStaticDataService staticData, IItemBuyingService itemBuyingService,
            IPurchasedItemRegistry purchasedItemRegistry, ICharacterFactory characterFactory, IActiveCustomersRegistry activeCustomersRegistry)
        {
            _coroutineRunner = CoroutineRunner.instance;
            _staticData = staticData;
            _itemBuyingService = itemBuyingService;
            _purchasedItemRegistry = purchasedItemRegistry;
            _characterFactory = characterFactory;
            _levelStaticData = _staticData.LevelConfig();
            _balance = _staticData.Balance();
            _customersPointsForSpawn = _levelStaticData.CharactersData
                    .Where(x => x.TypeId == CharacterTypeId.Customer)
                    .ToList();
            _activeCustomersRegistry = activeCustomersRegistry;
        }

        public void StartSpawning()
        {
            if (_spawningCoroutine != null)
                _coroutineRunner.StopCoroutine(_spawningCoroutine);

            _spawningCoroutine = _coroutineRunner.StartCoroutine(SpawnCustomer());
        }

        private IEnumerator SpawnCustomer()
        {
            while (true)
            {
                int maxCustomers = _itemBuyingService.GetNextAvailableOrder(HallItemTypeId.Chair) - 1;

                if (_purchasedItemRegistry.HasFreeChair())
                {
                    var point = _customersPointsForSpawn[Random.Range(0, _customersPointsForSpawn.Count)];
                    var customer = _characterFactory.Create<Customer>(point.TypeId, point.Position, point.Rotation, null);
                    var randomAppearance = _characterFactory.GetRandomCustomerAppearance();
                    customer.SetAppearance(randomAppearance);
                    customer.SetObjectName(randomAppearance.TypeId.ToString());
                    customer.SetRandomCharacteristics();
                    
                    _activeCustomersRegistry.Add(customer);
                }

                yield return new WaitForSeconds(Random.Range(_balance.Customers.SpawnInterval.x,
                    _balance.Customers.SpawnInterval.y));
            }
        }

        public void Stop()
        {
            if (_spawningCoroutine != null)
                _coroutineRunner.StopCoroutine(_spawningCoroutine);

            _spawningCoroutine = null;
        }
    }

    public interface ICustomerArrivalService
    {
        void StartSpawning();
        void Stop();
    }
}
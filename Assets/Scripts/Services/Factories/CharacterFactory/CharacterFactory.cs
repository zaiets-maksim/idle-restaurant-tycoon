using Characters;
using Characters.Customers;
using Services.StaticDataService;
using StaticData.Configs;
using StaticData.TypeId;
using UnityEngine;

namespace Services.Factories.CharacterFactory
{
    public class CharacterFactory : Factory, ICharacterFactory
    {
        private readonly IStaticDataService _staticData;

        public CharacterFactory(IStaticDataService staticData)
        {
            _staticData = staticData;
        }

        public T Create<T>(CharacterTypeId typeId, Vector3 position, Vector3 rotation, Transform parent) where T : Person
        {
            var config = _staticData.ForCharacter(typeId);
            var customer = InstantiateOnActiveScene<T>(config.Prefab, position, rotation, parent);
            return customer;
        }

        public CustomerAppearance GetRandomCustomerAppearance()
        {
            CustomerTypeId[] customerTypeId = _staticData.GetCustomerTypeIdsInAppearance();
            CustomerTypeId randomTypeId = customerTypeId[Random.Range(0, customerTypeId.Length)];
            
            return _staticData.ForCharacterAppearance(randomTypeId);
        }
    }

    public interface ICharacterFactory
    {
        T Create<T>(CharacterTypeId typeId, Vector3 position, Vector3 rotation, Transform parent = null) where T : Person;
        CustomerAppearance GetRandomCustomerAppearance();
    }
}
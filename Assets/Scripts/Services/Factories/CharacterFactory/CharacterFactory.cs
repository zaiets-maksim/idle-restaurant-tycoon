using Characters;
using Characters.Customers;
using Connect4.Scripts.Services.Factories;
using Services.StaticDataService;
using StaticData.Configs;
using StaticData.TypeId;
using UnityEngine;
using Zenject;

namespace Services.Factories.CharacterFactory
{
    public class CharacterFactory : Factory, ICharacterFactory
    {
        private readonly IStaticDataService _staticData;

        public CharacterFactory(IInstantiator instantiator, IStaticDataService staticDataService) : base(instantiator)
        {
            _instantiator = instantiator;
            _staticData = staticDataService;
        }

        public T Create<T>(CharacterTypeId typeId, Vector3 position, Vector3 eulerAngles, Transform parent) where T : Person
        {
            var config = _staticData.ForCharacter(typeId);
            var person = InstantiateOnActiveScene<T>(config.Prefab, position, eulerAngles, parent);
            person.Initialize(config);
            return person;
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
        T Create<T>(CharacterTypeId typeId, Vector3 position, Vector3 eulerAngles, Transform parent = null) where T : Person;
        CustomerAppearance GetRandomCustomerAppearance();
    }
}
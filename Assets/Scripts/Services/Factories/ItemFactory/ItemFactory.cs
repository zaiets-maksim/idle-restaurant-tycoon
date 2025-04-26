using Connect4.Scripts.Services.Factories;
using Interactable;
using Services.StaticDataService;
using StaticData;
using StaticData.TypeId;
using UnityEngine;
using Zenject;

namespace Services.Factories.ItemFactory
{
    public class ItemFactory : Factory, IItemFactory
    {
        private readonly IStaticDataService _staticData;
        
        public ItemFactory(IInstantiator instantiator, IStaticDataService staticDataService) : base(instantiator)
        {
            _instantiator = instantiator;
            _staticData = staticDataService;
        }
    
        public KitchenItem Create(KitchenItemTypeId typeId, Vector3 position, Vector3 eulerAngles, Transform parent)
        {
            var config = _staticData.ForKitchenItem(typeId);
            var kitchenItem = InstantiateOnActiveScene<KitchenItem>(config.Prefab, position, eulerAngles, parent);
            return kitchenItem;
        }
    
        public HallItem Create(HallItemTypeId typeId, Vector3 position, Vector3 eulerAngles, Transform parent)
        {
            var config = _staticData.ForHallItem(typeId);
            var kitchenItem = InstantiateOnActiveScene<HallItem>(config.Prefab, position, eulerAngles, parent);
            return kitchenItem;
        }

        public Dish Create(DishTypeId typeId, Vector3 position, Vector3 eulerAngles, Transform parent)
        {
            var config = _staticData.ForDish(typeId);
            var dish = InstantiateOnActiveScene<Dish>(config.Prefab, position, eulerAngles, parent);
            dish.Initialize(config);
            return dish;
        }
    }

    public interface IItemFactory
    {
        KitchenItem Create(KitchenItemTypeId typeId, Vector3 position, Vector3 eulerAngles, Transform parent);
        HallItem Create(HallItemTypeId typeId, Vector3 position, Vector3 eulerAngles, Transform parent);
        Dish Create(DishTypeId typeId, Vector3 position, Vector3 eulerAngles, Transform parent);
    }
}
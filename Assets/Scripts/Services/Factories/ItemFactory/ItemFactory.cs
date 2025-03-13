using Interactable;
using Services.StaticDataService;
using StaticData;
using StaticData.TypeId;
using UnityEngine;

namespace Services.Factories.ItemFactory
{
    public class ItemFactory : Factory, IItemFactory
    {
        private readonly IStaticDataService _staticData;

        public ItemFactory(IStaticDataService staticData)
        {
            _staticData = staticData;
        }
    
        public KitchenItem Create(KitchenItemTypeId typeId, Vector3 position, Vector3 rotation, Transform parent)
        {
            var config = _staticData.ForKitchenItem(typeId);
            var kitchenItem = InstantiateOnActiveScene<KitchenItem>(config.Prefab, position, rotation, parent);
            return kitchenItem;
        }
    
        public HallItem Create(HallItemTypeId typeId, Vector3 position, Vector3 rotation, Transform parent)
        {
            var config = _staticData.ForHallItem(typeId);
            var kitchenItem = InstantiateOnActiveScene<HallItem>(config.Prefab, position, rotation, parent);
            return kitchenItem;
        }
    }

    public interface IItemFactory
    {
        KitchenItem Create(KitchenItemTypeId typeId, Vector3 position, Vector3 rotation, Transform parent);
        HallItem Create(HallItemTypeId typeId, Vector3 position, Vector3 rotation, Transform parent);
    }
}
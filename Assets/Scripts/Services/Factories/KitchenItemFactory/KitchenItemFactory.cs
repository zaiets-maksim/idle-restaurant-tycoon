using Interactable;
using Services.Factories;
using Services.StaticDataService;
using StaticData;
using UnityEngine;

public class KitchenItemFactory : Factory, IKitchenItemFactory
{
    private readonly IStaticDataService _staticData;

    public KitchenItemFactory(IStaticDataService staticData)
    {
        _staticData = staticData;
    }
    
    public KitchenItem Create(KitchenItemTypeId typeId, Vector3 position, Vector3 rotation, Transform parent)
    {
        var config = _staticData.ForKitchenItem(typeId);
        var kitchenItem = InstantiateOnActiveScene<KitchenItem>(config.Prefab, position, rotation, parent);
        return kitchenItem;
    }
}

public interface IKitchenItemFactory
{
    public KitchenItem Create(KitchenItemTypeId typeId, Vector3 position, Vector3 rotation, Transform parent);
}

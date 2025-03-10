using System.Linq;
using Services.DataStorageService;
using Services.PurchasedItemRegistry;
using Services.SaveLoad;
using Services.StaticDataService;
using StaticData;

namespace Services.ItemBuyingService
{
    public class ItemBuyingService : IItemBuyingService
    {
        private readonly IPersistenceProgressService _progress;
        private readonly IStaticDataService _staticData;
        private readonly ISaveLoadService _saveLoad;
        private readonly IKitchenItemFactory _kitchenItemFactory;
        private readonly IPurchasedItemRegistry _purchasedItemRegistry;

        public ItemBuyingService(IPersistenceProgressService progress, IStaticDataService staticData,
            ISaveLoadService saveLoad, IKitchenItemFactory kitchenItemFactory, IPurchasedItemRegistry purchasedItemRegistry)
        {
            _purchasedItemRegistry = purchasedItemRegistry;
            _kitchenItemFactory = kitchenItemFactory;
            _saveLoad = saveLoad;
            _staticData = staticData;
            _progress = progress;
        }

        public void BuyKitchenItem(KitchenItemTypeId typeId)
        {
            int purchasedCount = _progress.PlayerData.ProgressData.GetPurchasedCount(typeId);
            var levelStaticData = _staticData.LevelConfig();

            var nextItem = levelStaticData.KitchenItemsData
                .Where(item => item.TypeId == typeId)
                .OrderBy(item => item.PurchaseOrder)
                .ElementAtOrDefault(purchasedCount);
            
            if (nextItem == null)
                return;

            // if (you have money)
            var kitchenItem = _kitchenItemFactory.Create(typeId, nextItem.Position, nextItem.Rotation, null);
            _purchasedItemRegistry.AddKitchenItem(kitchenItem);
            _progress.PlayerData.ProgressData.BuyKitchenItem(nextItem);
            _saveLoad.SaveProgress();
        }
    }
}
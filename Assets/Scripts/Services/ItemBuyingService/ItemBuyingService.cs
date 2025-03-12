using System.Collections.Generic;
using System.Linq;
using Services.DataStorageService;
using Services.PurchasedItemRegistry;
using Services.SaveLoad;
using Services.StaticDataService;
using StaticData;
using StaticData.Levels;

namespace Services.ItemBuyingService
{
    public class ItemBuyingService : IItemBuyingService
    {
        private readonly IPersistenceProgressService _progress;
        private readonly IStaticDataService _staticData;
        private readonly ISaveLoadService _saveLoad;
        private readonly IKitchenItemFactory _kitchenItemFactory;
        private readonly IPurchasedItemRegistry _purchasedItemRegistry;
        private readonly LevelStaticData _levelStaticData;

        public ItemBuyingService(IPersistenceProgressService progress, IStaticDataService staticData,
            ISaveLoadService saveLoad, IKitchenItemFactory kitchenItemFactory, IPurchasedItemRegistry purchasedItemRegistry)
        {
            _purchasedItemRegistry = purchasedItemRegistry;
            _kitchenItemFactory = kitchenItemFactory;
            _saveLoad = saveLoad;
            _staticData = staticData;
            _progress = progress;
            _levelStaticData = _staticData.LevelConfig();
        }

        public bool GetAvailableItemCount(KitchenItemTypeId typeId, out int count)
        {
            count = 0;
            int purchasedCount = _progress.PlayerData.ProgressData.GetPurchasedCount(typeId);
    
            var nextItem = _levelStaticData.KitchenItemsData
                .Where(item => item.TypeId == typeId)
                .OrderBy(item => item.PurchaseOrder)
                .Skip(purchasedCount)
                .FirstOrDefault();

            var total = _levelStaticData.KitchenItemsData.Count(item => item.TypeId == typeId);

            if (nextItem == null)
                return false;

            count = total - nextItem.PurchaseOrder + 1;
            
            return true;
        }

        public void BuyKitchenItem(KitchenItemTypeId typeId)
        {
            int purchasedCount = _progress.PlayerData.ProgressData.GetPurchasedCount(typeId);

            var nextItem = _levelStaticData.KitchenItemsData
                .Where(item => item.TypeId == typeId)
                .OrderBy(item => item.PurchaseOrder)
                .ElementAtOrDefault(purchasedCount);
            
            if (nextItem == null)
                return;
            
            var kitchenItem = _kitchenItemFactory.Create(typeId, nextItem.Position, nextItem.Rotation, null);
            _purchasedItemRegistry.AddKitchenItem(kitchenItem);
            _progress.PlayerData.ProgressData.BuyKitchenItem(nextItem);
            _saveLoad.SaveProgress();
        }

        public int GetNextAvailableOrder(KitchenItemTypeId typeId) =>
            _progress.PlayerData.ProgressData.PurchasedKitchenItems
                .Where(item => item.TypeId == typeId)
                .Max(item => (int?)item.PurchaseOrder + 1) ?? 0;

        public List<KitchenData> GetAvailableKitchenItemsForPurchase()
        {
            var purchasedItems = _progress.PlayerData.ProgressData.PurchasedKitchenItems;
            var levelStaticData = _staticData.LevelConfig();

            return levelStaticData.KitchenItemsData
                .Where(item => !purchasedItems
                .Any(purchased => purchased.TypeId == item.TypeId && purchased.PurchaseOrder == item.PurchaseOrder))
                .ToList();
        }
    }
}
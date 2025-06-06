using System;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Services.DataStorageService;
using Services.Factories.CharacterFactory;
using Services.Factories.ItemFactory;
using Services.PurchasedItemRegistry;
using Services.SaveLoad;
using Services.StaticDataService;
using Services.SurfaceUpdaterService;
using StaticData;
using StaticData.Levels;
using StaticData.TypeId;

namespace Services.ItemBuyingService
{
    public class ItemBuyingService : IItemBuyingService
    {
        private readonly IPersistenceProgressService _progress;
        private readonly IStaticDataService _staticData;
        private readonly ISaveLoadService _saveLoad;
        private readonly IItemFactory _itemFactory;
        private readonly IPurchasedItemRegistry _purchasedItemRegistry;
        private readonly LevelStaticData _levelStaticData;
        private readonly ICharacterFactory _characterFactory;
        private readonly ISurfaceUpdaterService _surfaceUpdaterService;

        public ItemBuyingService(IPersistenceProgressService progress, IStaticDataService staticData,
            ISaveLoadService saveLoad, IItemFactory itemFactory,
            IPurchasedItemRegistry purchasedItemRegistry, ICharacterFactory characterFactory,
            ISurfaceUpdaterService surfaceUpdaterService)
        {
            _surfaceUpdaterService = surfaceUpdaterService;
            _characterFactory = characterFactory;
            _purchasedItemRegistry = purchasedItemRegistry;
            _itemFactory = itemFactory;
            _saveLoad = saveLoad;
            _staticData = staticData;
            _progress = progress;
            _levelStaticData = _staticData.LevelConfig();
        }

        public event Action<HallItemTypeId> OnHallItemPurchased;

        public bool CanPurchaseChair(out int availableChairs)
        {
            int tableOrder = GetNextAvailableOrder(HallItemTypeId.Table) - 1;
            int chairOrder = GetNextAvailableOrder(HallItemTypeId.Chair) - 1;

            availableChairs = tableOrder * 4 - chairOrder;

            return availableChairs > 0;
        }

        public int GetAvailableStuffCount(CharacterTypeId typeId) =>
            _staticData.LevelConfig().CharactersData.Count(x => x.TypeId == typeId) -
            _progress.PlayerData.ProgressData.PurchasedStuff.Count(item => item.TypeId == typeId);

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

        public bool GetAvailableItemCount(HallItemTypeId typeId, out int count)
        {
            count = 0;
            int purchasedCount = _progress.PlayerData.ProgressData.GetPurchasedCount(typeId);

            var nextItem = _levelStaticData.HallItemsData
                .Where(item => item.TypeId == typeId)
                .OrderBy(item => item.PurchaseOrder)
                .Skip(purchasedCount)
                .FirstOrDefault();

            var total = _levelStaticData.HallItemsData.Count(item => item.TypeId == typeId);

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

            var kitchenItem = _itemFactory.Create(typeId, nextItem.Position, nextItem.Rotation, null);
            _purchasedItemRegistry.AddKitchenItem(kitchenItem);
            _progress.PlayerData.ProgressData.BuyKitchenItem(nextItem);
            _saveLoad.SaveProgress();
            _surfaceUpdaterService.Bake();
        }

        public void BuyHallItem(HallItemTypeId typeId)
        {
            int purchasedCount = _progress.PlayerData.ProgressData.GetPurchasedCount(typeId);

            var nextItem = _levelStaticData.HallItemsData
                .Where(item => item.TypeId == typeId)
                .OrderBy(item => item.PurchaseOrder)
                .ElementAtOrDefault(purchasedCount);

            if (nextItem == null)
                return;

            var kitchenItem = _itemFactory.Create(typeId, nextItem.Position, nextItem.Rotation, null);
            _purchasedItemRegistry.AddHallItem(kitchenItem);
            _progress.PlayerData.ProgressData.BuyHallItem(nextItem);
            _saveLoad.SaveProgress();

            OnHallItemPurchased?.Invoke(typeId);
            _surfaceUpdaterService.Bake();
        }

        public void BuyStuff(CharacterTypeId typeId)
        {
            var stuff = _levelStaticData.CharactersData
                .Where(item => item.TypeId == typeId).ToList();

            var count = _purchasedItemRegistry.Stuff.Count(x => x.CharacterTypeId == typeId);
            var characterData = stuff[count];

            var character = _characterFactory.Create<Person>(typeId, characterData.Position, characterData.Rotation);
            _purchasedItemRegistry.AddStuff(character);
            _progress.PlayerData.ProgressData.BuyStuff(characterData);
            _saveLoad.SaveProgress();
        }

        public int GetNextAvailableOrder(CharacterTypeId typeId) =>
            _progress.PlayerData.ProgressData.PurchasedStuff.Count(item => item.TypeId == typeId) + 1;

        public int GetNextAvailableOrder(KitchenItemTypeId typeId) =>
            _progress.PlayerData.ProgressData.PurchasedKitchenItems
                .Where(item => item.TypeId == typeId)
                .Max(item => (int?)item.PurchaseOrder + 1) ?? 1;

        public int GetNextAvailableOrder(HallItemTypeId typeId) =>
            _progress.PlayerData.ProgressData.PurchasedHallItems
                .Where(item => item.TypeId == typeId)
                .Max(item => (int?)item.PurchaseOrder + 1) ?? 1;


        public List<KitchenData> GetAvailableKitchenItemsForPurchase()
        {
            var purchasedItems = _progress.PlayerData.ProgressData.PurchasedKitchenItems;
            var levelStaticData = _staticData.LevelConfig();

            return levelStaticData.KitchenItemsData
                .Where(item => !purchasedItems
                    .Any(purchased => purchased.TypeId == item.TypeId && purchased.PurchaseOrder == item.PurchaseOrder))
                .ToList();
        }

        public List<HallData> GetAvailableHallItemsForPurchase()
        {
            var purchasedItems = _progress.PlayerData.ProgressData.PurchasedHallItems;
            var levelStaticData = _staticData.LevelConfig();

            return levelStaticData.HallItemsData
                .Where(item => !purchasedItems
                    .Any(purchased => purchased.TypeId == item.TypeId && purchased.PurchaseOrder == item.PurchaseOrder))
                .ToList();
        }
    }
}
using System;
using System.Collections.Generic;
using StaticData;
using StaticData.Levels;
using StaticData.TypeId;

namespace Services.ItemBuyingService
{
    public interface IItemBuyingService
    {
        event Action<HallItemTypeId> OnHallItemPurchased;
        bool CanPurchaseChair(out int availableChairs);
        
        void BuyKitchenItem(KitchenItemTypeId typeId);
        void BuyHallItem(HallItemTypeId typeId);
        void BuyStuff(CharacterTypeId typeId);
        
        List<KitchenData>  GetAvailableKitchenItemsForPurchase();
        List<HallData> GetAvailableHallItemsForPurchase();
        
        bool GetAvailableItemCount(KitchenItemTypeId typeId, out int count);
        bool GetAvailableItemCount(HallItemTypeId typeId, out int count);

        int GetNextAvailableOrder(CharacterTypeId typeId);
        int GetNextAvailableOrder(KitchenItemTypeId typeId);
        int GetNextAvailableOrder(HallItemTypeId typeId);
        int GetAvailableStuffCount(CharacterTypeId configTypeId);
    }
}

using System.Collections.Generic;
using StaticData;
using StaticData.Levels;

namespace Services.ItemBuyingService
{
    public interface IItemBuyingService
    {
        void BuyKitchenItem(KitchenItemTypeId typeId);

        List<KitchenData> GetAvailableKitchenItemsForPurchase();
        bool GetAvailableItemCount(KitchenItemTypeId typeId, out int count);
        int GetNextAvailableOrder(KitchenItemTypeId typeId);
    }
}

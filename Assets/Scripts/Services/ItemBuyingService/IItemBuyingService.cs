using System.Collections.Generic;
using StaticData;
using StaticData.Levels;

namespace Services.ItemBuyingService
{
    public interface IItemBuyingService
    {
        void BuyKitchenItem(KitchenItemTypeId typeId);

        List<KitchenData> GetAvailableKitchenItemsForPurchase();
    }
}

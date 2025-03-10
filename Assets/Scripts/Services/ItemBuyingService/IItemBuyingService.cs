using StaticData;

namespace Services.ItemBuyingService
{
    public interface IItemBuyingService
    {
        void BuyKitchenItem(KitchenItemTypeId typeId);
    }
}

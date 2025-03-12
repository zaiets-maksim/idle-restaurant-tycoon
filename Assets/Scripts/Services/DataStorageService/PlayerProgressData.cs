using System;
using System.Collections.Generic;
using System.Linq;
using StaticData;
using StaticData.Levels;

namespace Services.DataStorageService
{
    [Serializable]
    public class PlayerProgressData
    {
        public int Money;
        public int Stars;
        public bool HasProgress => PurchasedKitchenItems.Count > 0;

        public List<KitchenData> PurchasedKitchenItems = new();

        public void BuyKitchenItem(KitchenData data) => 
            PurchasedKitchenItems.Add(data);

        public int GetPurchasedCount(KitchenItemTypeId typeId)
        {
            var count = PurchasedKitchenItems.Count(x => x.TypeId == typeId);
            return count;
        }
    }
}
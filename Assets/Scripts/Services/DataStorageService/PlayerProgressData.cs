using System;
using System.Collections.Generic;
using System.Linq;
using StaticData;
using StaticData.Levels;
using StaticData.TypeId;

namespace Services.DataStorageService
{
    [Serializable]
    public class PlayerProgressData
    {
        public int Money;
        public int Stars;
        
        public List<HallData> PurchasedHallItems = new();
        public List<KitchenData> PurchasedKitchenItems = new();
        
        public bool HasProgress => PurchasedKitchenItems.Count > 0;

        public void BuyKitchenItem(KitchenData data) => 
            PurchasedKitchenItems.Add(data);
        
        public void BuyHallItem(HallData data) => 
            PurchasedHallItems.Add(data);
        
        public int GetPurchasedCount(KitchenItemTypeId typeId) => 
            PurchasedKitchenItems.Count(x => x.TypeId == typeId);

        public int GetPurchasedCount(HallItemTypeId typeId) => 
            PurchasedHallItems.Count(x => x.TypeId == typeId);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using StaticData;
using UnityEngine;

namespace Services.DataStorageService
{
    [Serializable]
    public class PlayerProgressData
    {
        public bool HasProgress => PurchasedKitchenItems.Count > 0;

        public List<KitchenItemInfo> PurchasedKitchenItems = new();

        public void BuyKitchenItem(KitchenData data)
        {
            PurchasedKitchenItems.Add(new KitchenItemInfo
            {
                TypeId = data.TypeId,
                PurchaseOrder = data.PurchaseOrder,
                Position = data.Position,
                Rotation = data.Rotation,
                Parent = data.Parent
            });
        }

        public int GetPurchasedCount(KitchenItemTypeId typeId)
        {
            var count = PurchasedKitchenItems.Count(x => x.TypeId == typeId);
            return count;
        }
    }

    [Serializable]
    public class KitchenItemInfo
    {
        public KitchenItemTypeId TypeId;
        public int PurchaseOrder;
        public Vector3 Position;
        public Vector3 Rotation;
        public Transform Parent;
    }
}
using System.Collections.Generic;
using Crates;
using Interactable;
using UnityEngine;

namespace Services.PurchasedItemRegistry
{
    public class PurchasedItemRegistry : IPurchasedItemRegistry
    {
        public List<KitchenItem> KitchenItems { get; } = new();
        public List<HallItem> HallItems { get; } = new();
        public List<Crate> StorageCrates { get; } = new();

        public void AddKitchenItems(List<KitchenItem> kitchenItems) => 
            KitchenItems.AddRange(kitchenItems);

        public void AddKitchenItem(KitchenItem kitchenItem) => 
            KitchenItems.Add(kitchenItem);

        public void AddHallItem(HallItem hallItem) => 
            HallItems.Add(hallItem);

        public void AddStorageCrates() => 
            StorageCrates.AddRange(Object.FindObjectsOfType<Crate>());
    }
    

    public interface IPurchasedItemRegistry
    {
        void AddKitchenItem(KitchenItem kitchenItem);
        void AddHallItem(HallItem hallItem);

        List<KitchenItem> KitchenItems { get; }
        List<HallItem> HallItems { get; }
        List<Crate> StorageCrates { get; }
        
        void AddKitchenItems(List<KitchenItem> kitchenItems);
        void AddStorageCrates();
    }
}
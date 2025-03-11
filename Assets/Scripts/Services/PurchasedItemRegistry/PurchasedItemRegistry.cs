using System;
using System.Collections.Generic;
using Crates;
using Interactable;
using Unity.VisualScripting;
using Object = UnityEngine.Object;

namespace Services.PurchasedItemRegistry
{
    public class PurchasedItemRegistry : IPurchasedItemRegistry
    {
        public List<KitchenItem> KitchenItems { get; } = new();
        public List<Crate> StorageCrates { get; } = new();

        public void AddKitchenItems(List<KitchenItem> kitchenItems) => 
            KitchenItems.AddRange(kitchenItems);

        public void AddKitchenItem(KitchenItem kitchenItem) => 
            KitchenItems.Add(kitchenItem);

        public void AddStorageCrates() => 
            StorageCrates.AddRange(Object.FindObjectsOfType<Crate>());
    }

    public interface IPurchasedItemRegistry
    {
        void AddKitchenItem(KitchenItem kitchenItem);

        List<KitchenItem> KitchenItems { get; }
        List<Crate> StorageCrates { get; }
        void AddKitchenItems(List<KitchenItem> kitchenItems);
        void AddStorageCrates();
    }
}
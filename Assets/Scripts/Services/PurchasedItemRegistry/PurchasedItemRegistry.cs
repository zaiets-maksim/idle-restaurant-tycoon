using System.Collections.Generic;
using Interactable;

namespace Services.PurchasedItemRegistry
{
    public class PurchasedItemRegistry : IPurchasedItemRegistry
    {
        public List<KitchenItem> KitchenItems { get; } = new();

        public void AddKitchenItem(KitchenItem kitchenItem) => 
            KitchenItems.Add(kitchenItem);
    }

    public interface IPurchasedItemRegistry
    {
        void AddKitchenItem(KitchenItem kitchenItem);

        List<KitchenItem> KitchenItems { get; }
    }
}
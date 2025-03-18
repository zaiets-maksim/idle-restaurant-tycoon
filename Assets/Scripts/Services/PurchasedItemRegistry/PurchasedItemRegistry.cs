using System.Collections.Generic;
using System.Linq;
using Characters;
using Crates;
using Interactable;
using StaticData.TypeId;
using UnityEngine;

namespace Services.PurchasedItemRegistry
{
    public class PurchasedItemRegistry : IPurchasedItemRegistry
    {
        public List<KitchenItem> KitchenItems { get; } = new();
        public List<HallItem> HallItems { get; } = new();
        public List<Person> Stuff { get; } = new();
        public List<Crate> StorageCrates { get; } = new();

        public void AddKitchenItems(List<KitchenItem> kitchenItems) => 
            KitchenItems.AddRange(kitchenItems);

        public void AddKitchenItem(KitchenItem kitchenItem) => 
            KitchenItems.Add(kitchenItem);

        public void AddHallItem(HallItem hallItem) => 
            HallItems.Add(hallItem);
        
        public void AddStuff(Person person) => 
            Stuff.Add(person);

        public void AddStorageCrates() => 
            StorageCrates.AddRange(Object.FindObjectsOfType<Crate>());

        public bool HasFreeChair() =>
            HallItems.Any(x => x.TypeId == HallItemTypeId.Chair && !x.IsOccupied);
    }
    

    public interface IPurchasedItemRegistry
    {
        void AddKitchenItem(KitchenItem kitchenItem);
        void AddHallItem(HallItem hallItem);
        void AddStuff(Person person);
        
        List<KitchenItem> KitchenItems { get; }
        List<HallItem> HallItems { get; }
        List<Crate> StorageCrates { get; }
        List<Person> Stuff { get; }

        void AddKitchenItems(List<KitchenItem> kitchenItems);
        void AddStorageCrates();
        bool HasFreeChair();
 
    }
}
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
        private const float PriceIncreaseRatePercent = 25f;
        private const float SpeedIncreaseRatePercent = 10f;
        
        public Staff Staff = new();
        public Meals Meals = new();
        public Customers Customers = new();
        public List<Upgrade> Upgrades;
        public List<HallData> PurchasedHallItems = new();
        public List<KitchenData> PurchasedKitchenItems = new();
        
        public int Money;
        public int Stars;
        
        public bool HasProgress => PurchasedKitchenItems.Count > 0;


        public PlayerProgressData()
        {
            Upgrades = new List<Upgrade>
            {
                new("Meal", $"Raising prices by {Meals.PriceMultiplier + PriceIncreaseRatePercent}%",
                    new List<int> { 1000, 2000, 3000, 5000 }, RaisePrices),
                
                new("Customers", $"Reduce eating time by {2f}sec", new List<int> { 1500, 2500, 4500, 6500, 8000 }, () =>
                {
                    ReduceTime(ref Customers.EatingTimeDelay, 2f);
                }),
                new("Chef", $"Reduce food searching time by {2f}sec", new List<int> { 2000, 2500, 3000 }, () =>
                {
                    ReduceTime(ref Staff.Chef.FoodSearchingTimeDelay, 2f);
                }),
                new("Chef", $"Reduce cooking time by {2f}sec", new List<int> { 2500, 3000, 3500 }, () =>
                {
                    ReduceTime(ref Staff.Chef.CookingTimeDelay, 2f);
                }),
            };
        }

        
        public void ReduceTime(ref float time, float delay) => time -= delay;
        
        public void IncreaseSpeed(ref float speed) => speed += SpeedIncreaseRatePercent;
        
        public void RaisePrices() => Meals.PriceMultiplier += PriceIncreaseRatePercent;

        public void BuyUpgrade(Upgrade upgrade)
        {
            int index = Upgrades.IndexOf(upgrade);
            Upgrades[index].Prices.RemoveAt(0);
        }

        public void BuyKitchenItem(KitchenData data) => 
            PurchasedKitchenItems.Add(data);
        
        public void BuyHallItem(HallData data) => 
            PurchasedHallItems.Add(data);
        
        public int GetPurchasedCount(KitchenItemTypeId typeId) => 
            PurchasedKitchenItems.Count(x => x.TypeId == typeId);

        public int GetPurchasedCount(HallItemTypeId typeId) => 
            PurchasedHallItems.Count(x => x.TypeId == typeId);
    }

    [Serializable]
    public class Upgrade
    {
        public string UpgradeType;
        public string Description;
        public List<int> Prices;
        public Action Action;

        public Upgrade(string upgradeType, string description, List<int> prices, Action action)
        {
            Action = action;
            Prices = prices;
            Description = description;
            UpgradeType = upgradeType;
        }
    }
    
    [Serializable]
    public class Customers
    {
        public float EatingTimeDelay;
    }
    
    [Serializable]
    public class Meals
    {
        public float PriceMultiplier;
    }

    [Serializable]
    public class Staff
    {
        public Chef Chef = new Chef();
        public Waiter Waiter = new Waiter();
    }

    [Serializable]
    public class Chef : Worker
    {
        public float FoodSearchingTimeDelay;
        public float CookingTimeDelay;
    }

    [Serializable]
    public class Waiter : Worker
    {
        
    }
    
    [Serializable]
    public class Worker
    {
        public float Speed = 3.5f;
    }
}
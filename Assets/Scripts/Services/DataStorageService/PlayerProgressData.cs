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
        private const float SpeedIncreaseRatePercent = 10f;
        private const float PriceIncreaseRate = 5f;

        public Staff Staff = new();
        public Meals Meals = new();
        public Customers Customers = new();
        public List<Upgrade> Upgrades;
        public List<HallData> PurchasedHallItems = new();
        public List<KitchenData> PurchasedKitchenItems = new();
        public List<CharacterData> PurchasedStuff = new();
        
        public int Money;
        public int Stars;
        
        public bool HasProgress => PurchasedKitchenItems.Count > 0 || PurchasedHallItems.Count > 0 || PurchasedStuff.Count > 0;


        public PlayerProgressData()
        {
            Upgrades = new List<Upgrade>
            {
                new("Meal", $"Raising prices by {PriceIncreaseRate}%", new List<int> { 1000, 2000, 3000, 5000 }, () =>
                    {
                        RaisePrices();
                        Meals.UpdatePriceMultiplier();
                    }),
                
                new("Customers", $"Reduce eating time by {2f}sec", new List<int> { 1500, 2500, 4500, 6500, 8000 }, () =>
                {
                    ReduceTime(ref Customers.EatingTimeDelay, 2f);
                    Customers.UpdateEatingTimeDelay();
                }),
                new("Chef", $"Reduce food searching time by {2f}sec", new List<int> { 2000, 2500, 3000 }, () =>
                {
                    ReduceTime(ref Staff.Chef.FoodSearchingTimeDelay, 2f);
                    Staff.Chef.UpdateFoodSearchingTime();
                }),
                new("Chef", $"Reduce cooking time by {2f}sec", new List<int> { 2500, 3000, 3500 }, () =>
                {
                    ReduceTime(ref Staff.Chef.CookingTimeDelay, 2f);
                    Staff.Chef.UpdateCookingTime();
                }),
                new("Chef", $"Increase speed by {SpeedIncreaseRatePercent}%", new List<int> { 500, 750, 1000, 1250, 1500 }, () =>
                {
                    IncreaseSpeed(ref Staff.Chef.Speed);
                    Staff.Chef.UpdateSpeed();
                }),
                new("Waiter", $"Increase speed by {SpeedIncreaseRatePercent}%", new List<int> { 500, 750, 1000, 1250, 1500 }, () =>
                {
                    IncreaseSpeed(ref Staff.Waiter.Speed);
                    Staff.Waiter.UpdateSpeed();
                })
            };
        }

        
        public void ReduceTime(ref float time, float delay) => time -= delay;
        
        public void IncreaseSpeed(ref float speed) => speed += speed / SpeedIncreaseRatePercent;
        
        public void RaisePrices() => Meals.PriceMultiplier += PriceIncreaseRate;

        public void BuyUpgrade(Upgrade upgrade)
        {
            int index = Upgrades.IndexOf(upgrade);
            Upgrades[index].Prices.RemoveAt(0);
        }

        public void BuyKitchenItem(KitchenData data) => 
            PurchasedKitchenItems.Add(data);
        
        public void BuyHallItem(HallData data) => 
            PurchasedHallItems.Add(data);

        public void BuyStuff(CharacterData characterData) => 
            PurchasedStuff.Add(characterData);

        public int GetPurchasedCount(CharacterTypeId typeId) => 
            PurchasedStuff.Count(x => x.TypeId == typeId);
        
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
        public event Action<float> OnEatingTimeDelayUpdated;
        
        public float EatingTimeDelay;
        
        public void UpdateEatingTimeDelay() => OnEatingTimeDelayUpdated?.Invoke(EatingTimeDelay);
    }
    
    [Serializable]
    public class Meals
    {
        public event Action<float> OnPriceMultiplierUpdated;
        
        public float PriceMultiplier = 1f;

        public void UpdatePriceMultiplier() => OnPriceMultiplierUpdated?.Invoke(PriceMultiplier);
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
        public event Action<float> OnFoodSearchingTimeUpdated;
        public event Action<float> OnCookingTimeDUpdated;
        
        
        public float FoodSearchingTimeDelay;
        public float CookingTimeDelay;

        public void UpdateFoodSearchingTime() => OnFoodSearchingTimeUpdated?.Invoke(FoodSearchingTimeDelay);
        public void UpdateCookingTime() => OnCookingTimeDUpdated?.Invoke(CookingTimeDelay);
    }

    [Serializable]
    public class Waiter : Worker
    {
        
    }
    
    [Serializable]
    public class Worker
    {
        public event Action<float> OnSpeedUpdated;

        public float Speed = 3.5f;
        public void UpdateSpeed() => OnSpeedUpdated?.Invoke(Speed);

    }
}
using System;
using Services.DataStorageService;
using Services.SaveLoad;

namespace Services.CurrencyService
{
    public class CurrencyService : ICurrencyService
    {
        public event Action<int> OnMoneyChanged;
        public event Action<int> OnAddMoney;
    
        public event Action<int> OnStarsChanged;
        public event Action<int> OnAddStars;
    
        private readonly IPersistenceProgressService _progress;
        private readonly ISaveLoadService _saveLoad;
        public int Money { get; private set; }
        public int Stars { get; private set; }

        public CurrencyService(IPersistenceProgressService persistenceProgressService, ISaveLoadService saveLoad)
        {
            _saveLoad = saveLoad;
            _progress = persistenceProgressService;
        }

        public bool CanAffordWithMoney(int cost) => Money >= cost;
        public bool CanAffordWithStars(int cost) => Stars >= cost;
        
        public void AddMoney(int amount)
        {
            Money += amount;
        
            OnAddMoney?.Invoke(amount);
            UpdateMoney(Money);
        }

        public void AddStars(int amount)
        {
            Stars += amount;
        
            OnAddStars?.Invoke(amount);
            UpdateStars(Stars);
        }

        public void RemoveMoney(int amount)
        {
            Money -= amount;
        
            UpdateMoney(Money);
        }

        public void RemoveStars(int amount)
        {
            Stars -= amount;
            UpdateStars(Stars);
        }

        public void Init()
        {
            Money = _progress.PlayerData.ProgressData.Money;
            Stars = _progress.PlayerData.ProgressData.Stars;
            UpdateMoney(Money);
            UpdateMoney(Stars);
        }

        private void UpdateMoney(int money)
        {
            OnMoneyChanged?.Invoke(money);
            _progress.PlayerData.ProgressData.Money = money;
            _saveLoad.SaveProgress();
        }
    
        private void UpdateStars(int stars)
        {
            OnStarsChanged?.Invoke(stars);
            _progress.PlayerData.ProgressData.Stars = stars;
            _saveLoad.SaveProgress();
        }
    }

    public interface ICurrencyService
    {
        event Action<int> OnMoneyChanged;
        event Action<int> OnAddMoney;
        public event Action<int> OnStarsChanged;
        public event Action<int> OnAddStars;
    
        int Money { get; }
        int Stars { get; }
        
        bool CanAffordWithMoney(int cost);
        bool CanAffordWithStars(int cost);
        void AddMoney(int amount);
        void RemoveMoney(int amount);
        void AddStars(int amount);
        void RemoveStars(int amount);
        void Init();
    }
}
using System;
using Services.DataStorageService;

namespace Services.ProgressEventService
{
    public class ProgressEventService : IProgressEventService, IDisposable
    {
        public event Action<float> OnChefSpeedUpdated;
        public event Action<float> OnWaiterSpeedUpdated;
        public event Action<float> OnChefCookingTimeUpdated;
        public event Action<float> OnChefFoodSearchingTimeUpdated;
        public event Action<float> OnCustomerEatingTimeUpdated;
        public event Action<float> OnPriceMultiplierUpdated;

        private readonly IPersistenceProgressService _progress;
        private bool _subscribed;

        public ProgressEventService(IPersistenceProgressService progress)
        {
            _progress = progress;
        }

        public void Initialize()
        {
            if (_subscribed)
                return;

            _subscribed = true;
            _progress.PlayerData.ProgressData.Staff.Chef.OnSpeedUpdated += OnChefSpeed;
            _progress.PlayerData.ProgressData.Staff.Waiter.OnSpeedUpdated += OnWaiterSpeed;
            _progress.PlayerData.ProgressData.Staff.Chef.OnCookingTimeDUpdated += OnChefCookingTime;
            _progress.PlayerData.ProgressData.Staff.Chef.OnFoodSearchingTimeUpdated += OnChefFoodSearchingTime;
            _progress.PlayerData.ProgressData.Customers.OnEatingTimeDelayUpdated += OnCustomerEatingTime;
            _progress.PlayerData.ProgressData.Meals.OnPriceMultiplierUpdated += OnPriceMultiplier;
        }

        private void OnChefSpeed(float speed) => OnChefSpeedUpdated?.Invoke(speed);
        private void OnWaiterSpeed(float speed) => OnWaiterSpeedUpdated?.Invoke(speed);
        private void OnChefCookingTime(float time) => OnChefCookingTimeUpdated?.Invoke(time);
        private void OnChefFoodSearchingTime(float time) => OnChefFoodSearchingTimeUpdated?.Invoke(time);
        private void OnCustomerEatingTime(float time) => OnCustomerEatingTimeUpdated?.Invoke(time);
        private void OnPriceMultiplier(float multiplier) => OnPriceMultiplierUpdated?.Invoke(multiplier);

        public void Dispose()
        {
            if (!_subscribed)
                return;

            _progress.PlayerData.ProgressData.Staff.Chef.OnSpeedUpdated -= OnChefSpeed;
            _progress.PlayerData.ProgressData.Staff.Waiter.OnSpeedUpdated -= OnWaiterSpeed;
            _progress.PlayerData.ProgressData.Staff.Chef.OnCookingTimeDUpdated -= OnChefCookingTime;
            _progress.PlayerData.ProgressData.Staff.Chef.OnFoodSearchingTimeUpdated -= OnChefFoodSearchingTime;
            _progress.PlayerData.ProgressData.Customers.OnEatingTimeDelayUpdated -= OnCustomerEatingTime;
            _progress.PlayerData.ProgressData.Meals.OnPriceMultiplierUpdated -= OnPriceMultiplier;
        }
    }

    public interface IProgressEventService
    {
        event Action<float> OnChefSpeedUpdated;
        event Action<float> OnWaiterSpeedUpdated;
        event Action<float> OnChefCookingTimeUpdated;
        event Action<float> OnChefFoodSearchingTimeUpdated;
        event Action<float> OnCustomerEatingTimeUpdated;
        event Action<float> OnPriceMultiplierUpdated;
        void Initialize();
    }
}

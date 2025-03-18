using Infrastructure;
using Services.DataStorageService;
using StaticData.Configs;
using UnityEngine;

namespace Interactable
{
    public class Dish : MonoBehaviour
    {
        [SerializeField] private DishTypeId _dishTypeId;
        [SerializeField] private int _price;
        
        private readonly IPersistenceProgressService _progress;

        public DishTypeId DishTypeId => _dishTypeId;

        public int Price => _price;

        public Dish()
        {
            _progress = ProjectContext.Instance?.Progress;
        }

        public void Initialize(DishConfig config)
        {
            _dishTypeId = config.TypeId;
            _price = (int)(config.Price * _progress.PlayerData.ProgressData.Meals.PriceMultiplier);
        }
    }
}

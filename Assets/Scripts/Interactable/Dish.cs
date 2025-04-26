using Infrastructure;
using Services.DataStorageService;
using StaticData.Configs;
using UnityEngine;
using Zenject;

namespace Interactable
{
    public class Dish : MonoBehaviour
    {
        [SerializeField] private DishTypeId _dishTypeId;
        [SerializeField] private int _price;
        
        private IPersistenceProgressService _progress;

        public DishTypeId DishTypeId => _dishTypeId;

        public int Price => _price;

        [Inject]
        public void Constructor(IPersistenceProgressService progress)
        {
            _progress = progress;
        }

        
        public void Initialize(DishConfig config)
        {
            _dishTypeId = config.TypeId;
            _price = (int)(config.Price * _progress.PlayerData.ProgressData.Meals.PriceMultiplier);
        }
    }
}

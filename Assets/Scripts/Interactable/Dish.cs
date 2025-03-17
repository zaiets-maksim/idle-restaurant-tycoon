using StaticData.Configs;
using UnityEngine;

namespace Interactable
{
    public class Dish : MonoBehaviour
    {
        [SerializeField] private DishTypeId _dishTypeId;
        [SerializeField] private int _price;

        public DishTypeId DishTypeId => _dishTypeId;

        public int Price => _price;


        public void Initialize(DishConfig config)
        {
            _dishTypeId = config.TypeId;
            _price = config.Price;
        }
    }
}

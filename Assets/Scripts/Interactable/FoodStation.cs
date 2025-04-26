using Infrastructure;
using Services.Factories.ItemFactory;
using Services.StaticDataService;
using UnityEngine;
using Zenject;

namespace Interactable
{
    public class FoodStation : KitchenItem
    {
        [SerializeField] private Transform _dishPrefab;
        [SerializeField] private Transform _placeForDish;
        [SerializeField] private DishTypeId[] _dishTypeId;
        
        private IItemFactory _itemFactory;

        public DishTypeId[] DishTypeId => _dishTypeId;

        [Inject]
        public void Constructor(IItemFactory itemFactory)
        {
            _itemFactory = itemFactory;
        }
        
        public override void Interact()
        {
            
        }

        public Dish MakeDish(DishTypeId dishTypeId)
        {
            var dish = _itemFactory.Create(dishTypeId, _placeForDish.position, _placeForDish.rotation.eulerAngles, null);
            dish.transform.SetParent(transform);

            return dish;
        }
    }
}

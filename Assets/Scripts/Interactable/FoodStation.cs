using Infrastructure;
using Services.Factories.ItemFactory;
using Services.StaticDataService;
using UnityEngine;

namespace Interactable
{
    public class FoodStation : KitchenItem
    {
        [SerializeField] private Transform _dishPrefab;
        [SerializeField] private Transform _placeForDish;
        [SerializeField] private DishTypeId[] _dishTypeId;
        
        private readonly IItemFactory _itemFactory;
        private readonly IStaticDataService _staticData;
        
        public DishTypeId[] DishTypeId => _dishTypeId;

        public FoodStation()
        {
            _itemFactory = ProjectContext.Instance?.ItemFactory;
            _staticData = ProjectContext.Instance?.StaticData;
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

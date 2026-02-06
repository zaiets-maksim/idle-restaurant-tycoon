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
        
        private IItemFactory _itemFactory => ProjectContext.Get<IItemFactory>();
        private IStaticDataService _staticData => ProjectContext.Get<IStaticDataService>();
        
        public DishTypeId[] DishTypeId => _dishTypeId;
        
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

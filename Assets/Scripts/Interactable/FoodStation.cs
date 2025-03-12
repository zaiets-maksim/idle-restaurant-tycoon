using UnityEngine;

namespace Interactable
{
    public class FoodStation : KitchenItem
    {
        [SerializeField] private Transform _dishPrefab;
        [SerializeField] private Transform _placeForDish;
        public override void Interact()
        {
            
        }

        public Transform MakeDish()
        {
            var dish = Instantiate(_dishPrefab, _placeForDish.position, _placeForDish.rotation);
            dish.SetParent(transform);

            return dish;
        }
    }
}

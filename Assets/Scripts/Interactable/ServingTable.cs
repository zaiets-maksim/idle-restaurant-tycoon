using System.Collections.Generic;
using System.Linq;
using Interactable;
using UnityEngine;

public class ServingTable : KitchenItem
{
    [SerializeField] private List<DishPlacement> _placementsForDish;
    [SerializeField] private Transform _orderCollectionPoint;

    private DishPlacement _nearestDishPlacement;
    private IEnumerable<DishPlacement> _dishPlacements;

    public bool HasPlacementForDish => _placementsForDish.Any(x => !x.IsOccupied);
    public Transform OrderCollectionPoint => _orderCollectionPoint;

    public override void Interact()
    {
        
    }

    public void PlaceDish(Transform person, Dish dish)
    {
        _nearestDishPlacement = _placementsForDish
            .Where(x => !x.IsOccupied)
            .OrderBy(x => (x.transform.position - person.position).sqrMagnitude)
            .FirstOrDefault();
        
        _nearestDishPlacement?.Place(dish);
    }

    public bool HasDish(DishTypeId typeId) => 
        _placementsForDish.Any(x => x.Dish.DishTypeId == typeId);

    public bool HasDishNew(DishTypeId typeId) => 
        _placementsForDish.Any(x => x.HasDishTypeId(typeId));

    public Dish GetDish(DishTypeId dishTypeId)
    {
        _dishPlacements = _placementsForDish.Where(x => x.HasDishTypeId(dishTypeId));
        Dish dish = _dishPlacements.FirstOrDefault()?.Get();
        
        return dish;
    }
}

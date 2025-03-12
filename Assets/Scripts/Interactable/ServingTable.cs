using System.Collections.Generic;
using System.Linq;
using Interactable;
using UnityEngine;

public class ServingTable : KitchenItem
{
    [SerializeField] private List<DishPlacement> _placementsForDish;
    private DishPlacement _nearestDishPlacement;

    public bool HasPlacementForDish => _placementsForDish.Any(x => !x.IsOccupied);
    
    public override void Interact()
    {
        
    }

    public void PlaceDish(Transform person, Transform dish)
    {
        _nearestDishPlacement = _placementsForDish
            .Where(x => !x.IsOccupied)
            .OrderBy(x => (x.transform.position - person.position).sqrMagnitude)
            .FirstOrDefault();
        
        _nearestDishPlacement?.Place(dish);
    }
}

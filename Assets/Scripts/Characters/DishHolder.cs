using Extensions;
using Interactable;
using UnityEngine;

public class DishHolder : MonoBehaviour
{
    [SerializeField] private Transform _pointForDish;

    public bool HasDish => Dish != null;
    public Dish Dish { get; private set; }

    public void TakeDish(Dish dish)
    {
        Dish = dish;
        Dish.transform.SetParent(_pointForDish.parent);
        Dish.transform.SetPositionAndRotation(_pointForDish.position, _pointForDish.rotation);
    }

    public void GiveDish(out Dish dish)
    {
        dish = Dish;
        Dish = null;
    }
}

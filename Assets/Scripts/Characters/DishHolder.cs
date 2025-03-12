using UnityEngine;

public class DishHolder : MonoBehaviour
{
    [SerializeField] private Transform _pointForDish;

    public bool HasDish => Dish != null;
    public Transform Dish { get; private set; }

    public void TakeDish(Transform dish)
    {
        dish.SetParent(_pointForDish.parent);
        dish.SetPositionAndRotation(_pointForDish.position, _pointForDish.rotation);
        Dish = dish;
    }

    private void Give(out Transform dish)
    {
        dish = Dish;
        Dish = null;
    }
}

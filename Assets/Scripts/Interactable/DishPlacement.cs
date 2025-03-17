using Interactable;
using UnityEngine;

public class DishPlacement : MonoBehaviour
{
    [SerializeField] private Dish _dish;
    
    public Dish Dish => _dish;
    public bool IsOccupied { get; private set; }

    private void Occupy() => IsOccupied = true;

    private void Release() => IsOccupied = false;

    public bool HasDishTypeId(DishTypeId dishTypeId) => 
        IsOccupied && _dish.DishTypeId == dishTypeId;

    public void Place(Dish dish)
    {
        Occupy();
        _dish = dish;
        _dish.transform.position = transform.position;
        _dish.transform.rotation = transform.rotation;
        _dish.transform.parent = transform;
    }

    public Dish Get()
    {
        Release();
        var dishToReturn = _dish; 
        _dish = null;
        return dishToReturn;
    }
}

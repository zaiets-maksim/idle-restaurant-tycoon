using UnityEngine;

public class DishPlacement : MonoBehaviour
{
    [SerializeField] private Transform _dish;
    
    public Transform Dish => _dish;
    public bool IsOccupied { get; private set; }

    private void Occupy() => IsOccupied = true;

    private void Release() => IsOccupied = false;
    
    public void Place(Transform dish)
    {
        Occupy();
        _dish = dish;
        _dish.transform.position = transform.position;
        _dish.transform.rotation = transform.rotation;
        _dish.parent = transform;
    }

    public Transform Get()
    {
        Release();
        return _dish;
    }
}

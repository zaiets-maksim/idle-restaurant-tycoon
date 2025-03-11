namespace Interactable
{
    public class FoodStation : KitchenItem
    {
        public bool IsOccupied { get; private set; }

        public void Occupy() => IsOccupied = true;

        public void Release() => IsOccupied = false;
        
        public override void Interact()
        {
            
        }
    }
}

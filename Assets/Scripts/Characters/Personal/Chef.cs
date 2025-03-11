using UnityEngine;

namespace Characters.Personal
{
    public class Chef : Employee, ICook
    {
        [SerializeField] private ChefBehavior _chefBehavior;
        [SerializeField] private PersonItemCollector _itemCollector;
        // [SerializeField] private 

        public int Food => _itemCollector.Food;
        public bool HasFood => _itemCollector.Food > 0;

        public override void PerformDuties()
        {
            
        }

        public void CheckQuality()
        {
        
        }

        public void Cook()
        {
        
        }
    }
}

using UnityEngine;

namespace Characters
{
    public class PersonItemCollector : MonoBehaviour
    {
        public int Food { get; private set; }
        public int Dishes { get; private set; }

        public void CollectFood(int amount) => 
            Food += amount;
    
        public void CollectDishes(int amount) => 
            Dishes += amount;
    }
}

using UnityEngine;
using UnityEngine.AI;

namespace Characters
{
    public abstract class Person : MonoBehaviour, IPerson
    {
        
        public string Name { get; set; }
        public abstract void PerformDuties();
    }
}

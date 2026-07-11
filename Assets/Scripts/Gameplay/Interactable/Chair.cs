using UnityEngine;

namespace Interactable
{
    public class Chair : HallItem
    {
        [SerializeField] private Transform[] _servingPoints;
        
        public Transform[] ServingPoints => _servingPoints;

        public override void Interact()
        {
            
        }
    }
}

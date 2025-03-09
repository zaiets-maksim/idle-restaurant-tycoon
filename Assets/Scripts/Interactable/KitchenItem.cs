using StaticData;
using UnityEngine;

namespace Interactable
{
    public class KitchenItem : MonoBehaviour, IInteractable
    {
        [SerializeField] private KitchenItemTypeId _typeId;

        public KitchenItemTypeId TypeId => _typeId;


        public void Interact()
        {
            
        }
    }
}

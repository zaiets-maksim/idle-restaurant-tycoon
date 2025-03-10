using StaticData;
using UnityEngine;

namespace Interactable
{
    public class KitchenItem : MonoBehaviour, IInteractable
    {
        [SerializeField] private KitchenItemTypeId _typeId;
        [SerializeField] private Transform _interactionPoint;

        public Transform InteractionPoint => _interactionPoint;
        public KitchenItemTypeId TypeId => _typeId;

        public void Interact()
        {
            
        }
    }
}

using StaticData;
using UnityEngine;

namespace Interactable
{
    public abstract class KitchenItem : MonoBehaviour, IInteractable
    {
        [SerializeField] protected KitchenItemTypeId _typeId;
        [SerializeField] protected Transform _interactionPoint;
        [SerializeField] protected float _interactionTime = 2f;
        
        public KitchenItemTypeId TypeId => _typeId;
        public Transform InteractionPoint => _interactionPoint;
        
        public bool IsOccupied { get; private set; }

        public virtual void Occupy() => IsOccupied = true;

        public virtual void Release() => IsOccupied = false;
        
        public float InteractionTime
        {
            get => _interactionTime;
            protected set => _interactionTime = value;
        }

        public abstract void Interact();
    }
}

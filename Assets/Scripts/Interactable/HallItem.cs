using StaticData.TypeId;
using UnityEngine;

namespace Interactable
{
    public abstract class HallItem : MonoBehaviour, IInteractable
    {
        [SerializeField] protected HallItemTypeId _typeId;
        [SerializeField] protected Transform _interactionPoint;
        [SerializeField] protected float _interactionTime = 2f;
        
        public HallItemTypeId TypeId => _typeId;
        public Transform InteractionPoint => _interactionPoint;
        
        public bool IsOccupied { get; private set; }

        public void Occupy() => IsOccupied = true;

        public void Release() => IsOccupied = false;
        
        public float InteractionTime
        {
            get => _interactionTime;
            protected set => _interactionTime = value;
        }

        public abstract void Interact();
    }
}

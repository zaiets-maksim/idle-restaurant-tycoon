using UnityEngine;

namespace Characters
{
    public class PersonAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        private static readonly int IsWalking = Animator.StringToHash("Walk");
        private static readonly int IsWalkingWithFood = Animator.StringToHash("WalkWithFood");
        private static readonly int IsIdling = Animator.StringToHash("Idle");
        private static readonly int IsPickingUp = Animator.StringToHash("PickUp");
        private static readonly int IsCooking = Animator.StringToHash("Cook");
        private static readonly int IsPuttingTheItem = Animator.StringToHash("PutTheItem");

        public void Idle() => _animator.SetTrigger(IsIdling);

        public void Walk() => _animator.SetTrigger(IsWalking);

        public void WalkWithFood() => _animator.SetTrigger(IsWalkingWithFood);

        public void PickUp() => _animator.SetTrigger(IsPickingUp);

        public void Cook() => _animator.SetTrigger(IsCooking);

        public void PutTheItem() => _animator.SetTrigger(IsPuttingTheItem);
    }
}

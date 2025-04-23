using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(Animator))]

    public class PersonAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        private static readonly int IsWalking = Animator.StringToHash("Walk");
        private static readonly int IsWalkingWithFood = Animator.StringToHash("WalkWithFood");
        private static readonly int IsIdling = Animator.StringToHash("Idle");
        private static readonly int IsPickingUp = Animator.StringToHash("PickUp");
        private static readonly int IsCooking = Animator.StringToHash("Cook");
        private static readonly int IsPuttingTheItem = Animator.StringToHash("PutTheItem");
        private static readonly int IsSittingDown = Animator.StringToHash("SitDown");
        private static readonly int IsStandingUp = Animator.StringToHash("StandUp");

        public void SetSpeed(float speed) => _animator.speed = speed;

        public float GetCurrentClipLength()
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            var time =  stateInfo.normalizedTime;
            return time / Time.timeScale;
        }

        public void Idle() => _animator.SetTrigger(IsIdling);
        public void Walk() => _animator.SetTrigger(IsWalking);
        public void WalkWithFood() => _animator.SetTrigger(IsWalkingWithFood);
        public void PickUp() => _animator.SetTrigger(IsPickingUp);
        public void Cook() => _animator.SetTrigger(IsCooking);
        public void PutTheItem() => _animator.SetTrigger(IsPuttingTheItem);
        public void SitDown() => _animator.Play(IsSittingDown, 0, 1f);
        public void StandUp() => _animator.SetTrigger(IsStandingUp);
    }
}

using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(Animator))]

    public class PersonAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float _defaultFadeDuration = 0.15f;

        private static readonly int IsWalking = Animator.StringToHash("Walk");
        private static readonly int IsWalkingWithFood = Animator.StringToHash("WalkWithFood");
        private static readonly int IsIdling = Animator.StringToHash("Idle");
        private static readonly int IsPickingUp = Animator.StringToHash("PickUp");
        private static readonly int IsCooking = Animator.StringToHash("Cook");
        private static readonly int IsPuttingTheItem = Animator.StringToHash("PutTheItem");
        private static readonly int IsSittingDown = Animator.StringToHash("SitDown");
        private static readonly int IsStandingUp = Animator.StringToHash("StandUp");

        public void SetWalkSpeed(float speed)
        {
            _animator.speed = speed;
        }

        public void ResetSpeed()
        {
            _animator.speed = 1f;
        }

        public float GetCurrentClipLength()
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            return stateInfo.length;
        }

        public void Idle() => _animator.CrossFadeInFixedTime(IsIdling, _defaultFadeDuration);
        public void Walk() => _animator.CrossFadeInFixedTime(IsWalking, _defaultFadeDuration);
        public void WalkWithFood() => _animator.CrossFadeInFixedTime(IsWalkingWithFood, _defaultFadeDuration);
        public void PickUp()
        {
            ResetSpeed();
            _animator.CrossFadeInFixedTime(IsPickingUp, _defaultFadeDuration);
        }
        public void Cook()
        {
            ResetSpeed();
            _animator.CrossFadeInFixedTime(IsCooking, _defaultFadeDuration);
        }
        public void PutTheItem()
        {
            ResetSpeed();
            _animator.CrossFadeInFixedTime(IsPuttingTheItem, _defaultFadeDuration);
        }
        public void SitDown()
        {
            ResetSpeed();
            _animator.CrossFadeInFixedTime(IsSittingDown, _defaultFadeDuration);
        }
        public void StandUp()
        {
            ResetSpeed();
            _animator.CrossFadeInFixedTime(IsStandingUp, _defaultFadeDuration);
        }
    }
}

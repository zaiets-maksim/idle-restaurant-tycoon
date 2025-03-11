using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Characters
{
    public class PersonMover : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private PersonRotator _personRotator;
        [SerializeField] private PersonAnimator _animator;

        private Coroutine _moveToCoroutine;

        public void StartMovingTo(Transform target, Action callback = null)
        {
            if (_moveToCoroutine != null)
                StopCoroutine(_moveToCoroutine);

            _moveToCoroutine = StartCoroutine(MoveTo(target, callback));
        }

        public void StopMoving()
        {
            if (_moveToCoroutine != null)
            {
                StopCoroutine(_moveToCoroutine);
                _moveToCoroutine = null;
            }
            
            _animator.Idle();
            _navMeshAgent.isStopped = true;
        }

        private IEnumerator MoveTo(Transform target, Action callback = null)
        {
            _personRotator.Enable();
            _navMeshAgent.SetDestination(target.position);
            _navMeshAgent.isStopped = false;
            _animator.Walk();

            while (!HasArrived())
                yield return null;

            StopMoving();
            _personRotator.StartRotationTo(target, () =>
            {
                _personRotator.Disable();
                callback?.Invoke();
            });
            
            yield return null;
        }

        private bool HasArrived() => _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance &&
                                     !_navMeshAgent.pathPending;
    }
}
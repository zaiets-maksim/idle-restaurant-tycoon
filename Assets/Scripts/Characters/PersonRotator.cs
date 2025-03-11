using System;
using Extensions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Characters
{
    public class PersonRotator : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private float _rotationSpeed = 7f;

        private Quaternion _targetRotation;
        private Coroutine _lookAtCoroutine;
        private Vector3 _direction;
        
        public bool IsRotating { get; private set; }
        public bool IsActive { get; private set; }

        
        public void Enable() => IsActive = true;

        public void Disable() => IsActive = false;

        private void Update()
        {
            if (_navMeshAgent.velocity != Vector3.zero)
                _navMeshAgent.transform.eulerAngles =
                    new Vector3(0, Quaternion.LookRotation(_navMeshAgent.velocity).eulerAngles.y, 0);
        }

        public void StartRotationTo(Transform target, Action rollback)
        {
            if (_lookAtCoroutine != null)
            {
                StopCoroutine(_lookAtCoroutine);
                IsRotating = false;
            }
            
            _lookAtCoroutine = StartCoroutine(TransformExtensions.RotateTo(transform, target, _rotationSpeed, rollback));
            IsRotating = true;
        }

        public void StopRotating()
        {
            if (_lookAtCoroutine != null)
            {
                StopCoroutine(_lookAtCoroutine);
                _lookAtCoroutine = null;
                IsRotating = false;
            }
        }
    }
}
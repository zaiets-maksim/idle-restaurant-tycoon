using UI.ProgressIndicator;
using UnityEngine;
using UnityEngine.AI;

namespace Characters
{
    public abstract class Person : MonoBehaviour, IPerson
    {
        [SerializeField] protected NavMeshAgent _navMeshAgent;
        [SerializeField] protected ProgressIndicator _progressIndicator;

        public ProgressIndicator ProgressIndicator => _progressIndicator;

        public string Name { get; set; }
        public abstract void PerformDuties();

        public void EnableAgent()
        {
            _navMeshAgent.enabled = true;
            _navMeshAgent.isStopped = false;
        }
        
        public void DisableAgent()
        {
            _navMeshAgent.isStopped = true;
            _navMeshAgent.enabled = false;
        }
    }
}

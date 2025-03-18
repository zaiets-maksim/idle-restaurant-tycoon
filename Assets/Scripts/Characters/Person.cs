using Infrastructure;
using Services.DataStorageService;
using UI.ProgressIndicator;
using UnityEngine;
using UnityEngine.AI;

namespace Characters
{
    public abstract class Person : MonoBehaviour, IPerson
    {
        [SerializeField] protected NavMeshAgent _navMeshAgent;
        [SerializeField] protected ProgressIndicator _progressIndicator;
        [SerializeField] protected PersonAnimator _personAnimator;

        protected IPersistenceProgressService _progress;

        public ProgressIndicator ProgressIndicator => _progressIndicator;

        public string Name { get; set; }
        

        public virtual void Start()
        {
            _navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
            _progress = ProjectContext.Instance?.Progress;
        }

        public abstract void PerformDuties();

        protected void UpdateAgentSpeed(float speed)
        {
            _navMeshAgent.speed = speed;
            _personAnimator.SetSpeed(speed / 3.5f);
        }

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

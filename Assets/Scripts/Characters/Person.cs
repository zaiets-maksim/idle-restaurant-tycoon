using System.Threading.Tasks;
using Extensions;
using Infrastructure;
using Services.DataStorageService;
using StaticData.Configs;
using StaticData.TypeId;
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
        [SerializeField] protected PersonMover _personMover;
        [SerializeField] private CharacterTypeId _characterTypeId;
        
        protected Vector3 _spawnPosition;
        protected IPersistenceProgressService _progress;

        public CharacterTypeId CharacterTypeId => _characterTypeId;
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
        
        public async Task MoveToSpawn()
        {
            await TaskExtension.WaitFor(callback =>
            {
                _personMover.StartMovingTo(_spawnPosition, callback);
            });
        }

        public void Initialize(CharacterConfig config)
        {
            _characterTypeId = config.TypeId;
        }
    }
}

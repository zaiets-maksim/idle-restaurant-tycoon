using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Extensions;
using Infrastructure;
using Services.DataStorageService;
using Services.OrderStorageService;
using StaticData.Configs;
using StaticData.TypeId;
using UI.ProgressIndicator;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

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
        protected IOrderStorageService _orderStorageService;

        public CharacterTypeId CharacterTypeId => _characterTypeId;
        public ProgressIndicator ProgressIndicator => _progressIndicator;

        public string Name { get; set; }

        [Inject]
        public void Constructor(IPersistenceProgressService progress, IOrderStorageService orderStorageService)
        {
            _orderStorageService = orderStorageService;
            _progress = progress;
        }

        public virtual void Start()
        {
            _navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
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
        
        public async UniTask MoveToSpawn()
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

using Services.DataStorageService;
using Services.SaveLoad;
using UnityEngine;

namespace Infrastructure.StateMachine.States
{
    public class LoadProgressState : GameStateEntity
    {
        private readonly ISaveLoadService _saveLoadService;
        private readonly IPersistenceProgressService _progress;
        private readonly IStateMachine _stateMachine;

        public LoadProgressState(ProjectContext projectContext)
        {
            _stateMachine = projectContext.StateMachine;
            _progress = projectContext.Progress;
            _saveLoadService = projectContext.SaveLoad;
        }
    
        public override void Enter()
        {
            LoadOrCreatePlayerData();
            _stateMachine.Enter<LoadMenuState>();
        }
        
        public override void Exit()
        {
            
        }
        
        private PlayerData LoadOrCreatePlayerData() =>
            _progress.PlayerData =
                _saveLoadService.LoadProgress()
                ?? CreateNew();

        private PlayerData CreateNew()
        {
            PlayerData playerData = new PlayerData
            {
                
            };
            return playerData;
        }
    }
}
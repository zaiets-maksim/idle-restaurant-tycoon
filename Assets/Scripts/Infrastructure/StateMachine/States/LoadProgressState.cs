using Services.CurrencyService;
using Services.DataStorageService;
using Services.ProgressEventService;
using Services.SaveLoad;
using UnityEngine;

namespace Infrastructure.StateMachine.States
{
    public class LoadProgressState : GameStateEntity
    {
        private readonly ISaveLoadService _saveLoadService;
        private readonly IPersistenceProgressService _progress;
        private readonly IStateMachine _stateMachine;
        private readonly ICurrencyService _currencyService;
        private readonly IProgressEventService _progressEventService;

        public LoadProgressState(ProjectContext projectContext)
        {
            _stateMachine = ProjectContext.Get<IStateMachine>();
            _progress = ProjectContext.Get<IPersistenceProgressService>();
            _saveLoadService = ProjectContext.Get<ISaveLoadService>();
            _currencyService = ProjectContext.Get<ICurrencyService>();
            _progressEventService = ProjectContext.Get<IProgressEventService>();
        }
    
        public override void Enter()
        {
            LoadOrCreatePlayerData();
            _progressEventService.Initialize();
            _saveLoadService.SaveProgress();
            _currencyService.Init();
            _stateMachine.Enter<LoadLevelState>();
        }
        
        public override void Exit()
        {
        }
        
        private PlayerData LoadOrCreatePlayerData()
        {
            _progress.PlayerData =
                _saveLoadService.LoadProgress()
                ?? CreateNew();
            
            _progress.PlayerData.ProgressData.InitActions();
            
            return _progress.PlayerData;
        }

        private PlayerData CreateNew()
        {
            PlayerData playerData = new PlayerData
            {
            };
            return playerData;
        }
    }
}
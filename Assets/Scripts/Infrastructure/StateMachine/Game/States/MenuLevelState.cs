using Connect4.Scripts.Infrastructure;
using PiratesIdle.Scripts.Infrastructure;
using Services.Factories.UIFactory;
using Services.WindowService;
using StaticData.Configs;
using Zenject;

namespace Infrastructure.StateMachine.Game.States
{
    public class MenuLevelState : IPayloadedState<string>, IGameState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly ILoadingCurtain _loadingCurtain;
        private readonly IUIFactory _uiFactory;
        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly IWindowService _windowService;

        [Inject]
        public MenuLevelState(IStateMachine<IGameState> gameStateMachine, ISceneLoader sceneLoader,
            ILoadingCurtain loadingCurtain, IWindowService windowService)
        {
            _windowService = windowService;
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
        }

        public void Enter(string payload)
        {
            _loadingCurtain.SetAnimationSpeed(1.5f);
            _loadingCurtain.Show();
            _sceneLoader.Load(payload, OnLevelLoad);
        }

        public void Exit()
        {
            _loadingCurtain.Hide();
        }

        protected virtual void OnLevelLoad()
        {
            InitGameWorld();
            _gameStateMachine.Enter<GameLoopState>();
        }

        private void InitGameWorld()
        {
            _windowService.Open(WindowTypeId.Menu);
        }
        
    }
}
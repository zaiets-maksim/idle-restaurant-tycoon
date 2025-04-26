using System.Collections.Generic;
using Characters;
using Connect4.Scripts.Infrastructure;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.Game.States;
using Interactable;
using PiratesIdle.Scripts.Infrastructure;
using Services.CustomerArrivalService;
using Services.DataStorageService;
using Services.Factories.CharacterFactory;
using Services.Factories.ItemFactory;
using Services.Factories.UIFactory;
using Services.PurchasedItemRegistry;
using Services.SurfaceUpdaterService;
using Services.WindowService;
using StaticData.Levels;
using Zenject;

namespace Friction_balance.Scripts.Infrastructure.StateMachine.Game.States
{
    public class LoadLevelState : IPayloadedState<string>, IGameState
    {
        private readonly IStateMachine<IGameState> _gameStateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly ILoadingCurtain _loadingCurtain;
        private readonly IUIFactory _uiFactory;
        private readonly IWindowService _windowService;
        private readonly ICharacterFactory _characterFactory;
        private readonly IItemFactory _itemFactory;
        private readonly IPersistenceProgressService _progress;
        private readonly IPurchasedItemRegistry _purchasedItemRegistry;
        private readonly ISurfaceUpdaterService _surfaceUpdaterService;
        private readonly ICustomerArrivalService _customerArrivalService;
        
        private List<KitchenData> _purchasedKitchenItems;
        private List<HallData> _purchasedHallItems;
        private List<CharacterData> _purchasedStuff;
        private KitchenItem _kitchenItem;
        private HallItem _hallItem;
        private Person _person;

        [Inject]
        public LoadLevelState(IStateMachine<IGameState> gameStateMachine, ISceneLoader sceneLoader,
            ILoadingCurtain loadingCurtain, IUIFactory uiFactory, IWindowService windowService, ICharacterFactory characterFactory,
            IItemFactory itemFactory, IPersistenceProgressService progress, IPurchasedItemRegistry purchasedItemRegistry, ISurfaceUpdaterService surfaceUpdaterService,
            ICustomerArrivalService customerArrivalService)
        {
            _customerArrivalService = customerArrivalService;
            _surfaceUpdaterService = surfaceUpdaterService;
            _purchasedItemRegistry = purchasedItemRegistry;
            _progress = progress;
            _itemFactory = itemFactory;
            _characterFactory = characterFactory;
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _uiFactory = uiFactory;
            _windowService = windowService;

        }
        
        public void Enter(string payload)
        {
            _loadingCurtain.SetAnimationSpeed(1f);
            _loadingCurtain.Show();
            _sceneLoader.Load(payload, OnLevelLoad);
        }

        public void Exit()
        {
            _loadingCurtain.Hide();
        }

        protected virtual void OnLevelLoad()
        {
            _uiFactory.CreateHud();
            _uiFactory.CreatePopUpMarket();
            InitGameWorld();
            _gameStateMachine.Enter<GameLoopState>();
        }

        private void InitGameWorld()
        {
            _surfaceUpdaterService.Init();
        
            _purchasedKitchenItems = _progress.PlayerData.ProgressData.PurchasedKitchenItems;
            _purchasedHallItems = _progress.PlayerData.ProgressData.PurchasedHallItems;
            _purchasedStuff = _progress.PlayerData.ProgressData.PurchasedStuff;
        
            foreach (var item in _purchasedKitchenItems)
            {
                _kitchenItem = _itemFactory.Create(item.TypeId, item.Position, item.Rotation, item.Parent);
                _purchasedItemRegistry.AddKitchenItem(_kitchenItem);
                _surfaceUpdaterService.UpdateCommon();
            }
        
            foreach (var item in _purchasedHallItems)
            {
                _hallItem = _itemFactory.Create(item.TypeId, item.Position, item.Rotation, item.Parent);
                _purchasedItemRegistry.AddHallItem(_hallItem);
                _surfaceUpdaterService.UpdateCommon();
            }
        
            foreach (var person in _purchasedStuff)
            {
                _person = _characterFactory.Create<Person>(person.TypeId, person.Position, person.Rotation, person.Parent);
                _purchasedItemRegistry.AddStuff(_person);
            }
        
            _purchasedItemRegistry.AddStorageCrates();
            _customerArrivalService.StartSpawning();
        }
    }
}
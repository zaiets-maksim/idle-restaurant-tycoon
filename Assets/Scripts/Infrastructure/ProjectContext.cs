using System;
using System.Collections.Generic;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using Services.DataStorageService;
using Services.Factories.UIFactory;
using Services.ItemBuyingService;
using Services.PurchasedItemRegistry;
using Services.SaveLoad;
using Services.SceneLoader;
using Services.StaticDataService;
using Services.SurfaceUpdaterService;
using Services.WindowService;
using Unity.VisualScripting;
using UnityEngine;

namespace Infrastructure
{
    public class ProjectContext : MonoBehaviour
    {
        public static ProjectContext Instance { get; private set; }

        private IStaticDataService _staticData;
        private ISceneLoader _sceneLoader;
        private ISaveLoadService _saveLoad;
        private IPersistenceProgressService _progress;
        private IUIFactory _uiFactory;
        private IWindowService _windowService;
        private GameStateFactory _gameStateFactory;
        private IStateMachine _stateMachine;
        private IItemBuyingService _itemBuyingService;
        private IKitchenItemFactory _kitchenItemFactory;
        private IPurchasedItemRegistry _purchasedItemRegistry;
        private ISurfaceUpdaterService _surfaceUpdaterService;
        

        public IStaticDataService StaticData => _staticData;
        public ISceneLoader SceneLoader => _sceneLoader;
        public ISaveLoadService SaveLoad => _saveLoad;
        public IPersistenceProgressService Progress => _progress;
        public IUIFactory UIFactory => _uiFactory;
        public IWindowService WindowService => _windowService;
        public GameStateFactory GameStateFactory => _gameStateFactory;
        public IStateMachine StateMachine => _stateMachine;
        public IItemBuyingService ItemBuyingService => _itemBuyingService;
        public IKitchenItemFactory KitchenItemFactory => _kitchenItemFactory;
        public IPurchasedItemRegistry PurchasedItemRegistry => _purchasedItemRegistry;
        public ISurfaceUpdaterService SurfaceUpdaterService => _surfaceUpdaterService;

        
        
        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                
                InstallBindings();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void InstallBindings()
        {
            Debug.Log("Installer");

            BindMonoServices();
            BindServices();

            BindGameStateMachine();
            BindGameStates();

            BootstrapGame();
        }

        private void BindMonoServices()
        {
        }

        private void BindServices()
        {
            BindStaticDataService();
            _progress = new PersistenceProgressService();
            _saveLoad = new SaveLoadService(_progress);
            _sceneLoader = new SceneLoader(CoroutineRunner.instance, _staticData);
            _uiFactory = new UIFactory(_staticData);
            _windowService = new WindowService(_uiFactory);
            _kitchenItemFactory = new KitchenItemFactory(_staticData);
            _itemBuyingService = new ItemBuyingService(_progress, _staticData, _saveLoad, _kitchenItemFactory, _purchasedItemRegistry);
            _purchasedItemRegistry = new PurchasedItemRegistry();
            _surfaceUpdaterService = new SurfaceUpdaterService();
        }


        private void BindStaticDataService()
        {
            _staticData = new StaticDataService();
            _staticData.LoadData();
        }

        private void BindGameStateMachine()
        {
            _gameStateFactory = new GameStateFactory();
            _stateMachine = new StateMachine.StateMachine(_gameStateFactory);
        }

        private void BindGameStates()
        {
            _gameStateFactory.InitStates(BuildStatesRegister());

            Dictionary<Type, GameStateEntity> BuildStatesRegister() =>
                new()
                {
                    { typeof(LoadMenuState), new LoadMenuState(this) },
                    { typeof(LoadLevelState), new LoadLevelState(this) },
                    { typeof(BootStrapState), new BootStrapState(this) },
                    { typeof(LoadProgressState), new LoadProgressState(this) }
                };
        }

        private void BootstrapGame()
        {
            _stateMachine.Enter<BootStrapState>();
        }
    }
}
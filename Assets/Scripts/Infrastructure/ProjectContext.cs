using System;
using System.Collections.Generic;
using Infrastructure.DI;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using Services.ActiveCustomersRegistry;
using Services.CurrencyService;
using Services.CustomerArrivalService;
using Services.DataStorageService;
using Services.Factories.CharacterFactory;
using Services.Factories.ItemFactory;
using Services.Factories.UIFactory;
using Services.ItemBuyingService;
using Services.OrderStorageService;
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

        [SerializeField] private TestService _testService;
        
        private DiContainer _container;
        private GameStateFactory _gameStateFactory;

        public DiContainer Container => _container;
        
        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                _container = new DiContainer(transform);
                InstallBindings();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public static T Get<T>() => Instance.Container.Resolve<T>();
        
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
            _container.Bind<ITestService>().FromComponentInNewPrefab(_testService).AsLazy();
            // _container.Bind<ITestService>().FromNewComponentOnNewGameObject<TestService>().AsLazy();
        }

        private void BindServices()
        {
            // StaticDataService
            var staticData = new StaticDataService();
            staticData.LoadData();
            _container.Bind<IStaticDataService>().FromInstance(staticData).AsSingle();

            // Progress & SaveLoad
            var progress = new PersistenceProgressService();
            _container.Bind<IPersistenceProgressService>().FromInstance(progress).AsSingle();
            _container.Bind<ISaveLoadService>().FromInstance(new SaveLoadService(progress)).AsSingle();

            // SceneLoader
            _container.Bind<ISceneLoader>()
                .FromInstance(new SceneLoader(CoroutineRunner.instance, staticData))
                .AsSingle();

            // Factories
            _container.Bind<IUIFactory>().FromInstance(new UIFactory(staticData)).AsSingle();
            _container.Bind<IItemFactory>().FromInstance(new ItemFactory(staticData)).AsSingle();
            _container.Bind<ICharacterFactory>().FromInstance(new CharacterFactory(staticData)).AsSingle();

            // Registries
            var purchasedItemRegistry = new PurchasedItemRegistry();
            _container.Bind<IPurchasedItemRegistry>().FromInstance(purchasedItemRegistry).AsSingle();
            
            var activeCustomersRegistry = new ActiveCustomersRegistry();
            _container.Bind<IActiveCustomersRegistry>().FromInstance(activeCustomersRegistry).AsSingle();

            // Services
            _container.Bind<IWindowService>()
                .FromInstance(new WindowService(_container.Resolve<IUIFactory>()))
                .AsSingle();

            _container.Bind<IItemBuyingService>()
                .FromInstance(new ItemBuyingService(
                    progress,
                    staticData,
                    _container.Resolve<ISaveLoadService>(),
                    _container.Resolve<IItemFactory>(),
                    purchasedItemRegistry,
                    _container.Resolve<ICharacterFactory>()))
                .AsSingle();

            _container.Bind<ISurfaceUpdaterService>().To<SurfaceUpdaterService>().AsSingle();
            
            _container.Bind<ICurrencyService>()
                .FromInstance(new CurrencyService(progress, _container.Resolve<ISaveLoadService>()))
                .AsSingle();

            _container.Bind<ICustomerArrivalService>()
                .FromInstance(new CustomerArrivalService(
                    staticData,
                    _container.Resolve<IItemBuyingService>(),
                    purchasedItemRegistry,
                    _container.Resolve<ICharacterFactory>(),
                    activeCustomersRegistry))
                .AsSingle();

            _container.Bind<IOrderStorageService>().To<OrderStorageService>().AsSingle();
        }

        private void BindGameStateMachine()
        {
            _gameStateFactory = new GameStateFactory();
            _container.Bind<GameStateFactory>().FromInstance(_gameStateFactory).AsSingle();
            
            var stateMachine = new StateMachine.StateMachine(_gameStateFactory);
            _container.Bind<IStateMachine>().FromInstance(stateMachine).AsSingle();
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
            _container.Resolve<IStateMachine>().Enter<BootStrapState>();
        }
    }
}
using Connect4.Scripts.Infrastructure;
using Friction_balance.Scripts.Infrastructure.StateMachine.Game.States;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.Game;
using Infrastructure.StateMachine.Game.States;
using PiratesIdle.Scripts.Infrastructure;
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
using Services.StaticDataService;
using Services.SurfaceUpdaterService;
using Services.WindowService;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private LoadingCurtain _curtain;

        public override void InstallBindings()
        {
            Debug.Log("Installer");

            BindMonoServices();
            BindServices();

            BindGameStateMachine();
            BindGameStates();

            BootstrapGame(); 
        }

        private void BindServices()
        {
            BindStaticDataService();
            Container.BindInterfacesTo<UIFactory>().AsSingle();
            Container.BindInterfacesTo<SaveLoadService>().AsSingle();
            Container.BindInterfacesTo<WindowService>().AsSingle();
            Container.BindInterfacesTo<PersistenceProgressService>().AsSingle();
            Container.BindInterfacesTo<ItemBuyingService>().AsSingle();
            Container.BindInterfacesTo<ItemFactory>().AsSingle();
            Container.BindInterfacesTo<PurchasedItemRegistry>().AsSingle();
            Container.BindInterfacesTo<SurfaceUpdaterService>().AsSingle();
            Container.BindInterfacesTo<CurrencyService>().AsSingle();
            Container.BindInterfacesTo<CharacterFactory>().AsSingle();
            Container.BindInterfacesTo<CustomerArrivalService>().AsSingle();
            Container.BindInterfacesTo<OrderStorageService>().AsSingle();
            Container.BindInterfacesTo<ActiveCustomersRegistry>().AsSingle();
        }

        private void BindMonoServices()
        {
            Container.Bind<ILoadingCurtain>().FromMethod(() => Container.InstantiatePrefabForComponent<ILoadingCurtain>(_curtain)).AsSingle();

            BindSceneLoader();
        }

        private void BindSceneLoader()
        {
            ISceneLoader sceneLoader = new SceneLoader();
            Container.Bind<ISceneLoader>().FromInstance(sceneLoader).AsSingle();
        }

        private void BindStaticDataService()
        {
            IStaticDataService staticDataService = new StaticDataService();
            staticDataService.LoadData();
            Container.Bind<IStaticDataService>().FromInstance(staticDataService).AsSingle();
        }

        private void BindGameStateMachine()
        {
            Container.Bind<GameStateFactory>().AsSingle();
            Container.BindInterfacesTo<GameStateMachine>().AsSingle();
        }

        private void BindGameStates()
        {
            Container.Bind<BootstrapState>().AsSingle();
            Container.Bind<LoadProgressState>().AsSingle();
            Container.Bind<LoadLevelState>().AsSingle();
            Container.Bind<MenuLevelState>().AsSingle();
            Container.Bind<GameLoopState>().AsSingle();
        }

        private void BootstrapGame() => 
            Container.Resolve<IStateMachine<IGameState>>().Enter<BootstrapState>();
    }
}
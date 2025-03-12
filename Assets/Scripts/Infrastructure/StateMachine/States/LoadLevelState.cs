using System.Collections.Generic;
using Infrastructure;
using Interactable;
using Services.DataStorageService;
using Services.Factories.UIFactory;
using Services.PurchasedItemRegistry;
using Services.SceneLoader;
using Services.SurfaceUpdaterService;
using StaticData.Levels;

public class LoadLevelState : GameStateEntity
{
    private readonly ISceneLoader _sceneLoader;
    private readonly IUIFactory _uiFactory;
    private List<KitchenData> _purchasedKitchenItems = new();
    private readonly IKitchenItemFactory _kitchenItemFactory;
    private readonly IPersistenceProgressService _progress;
    private readonly IPurchasedItemRegistry _purchasedItemRegistry;
    private readonly ISurfaceUpdaterService _surfaceUpdaterService;

    private KitchenItem _kitchenItem;

    public LoadLevelState(ProjectContext projectContext)
    {
        _sceneLoader = projectContext?.SceneLoader;
        _uiFactory = projectContext?.UIFactory;
        _kitchenItemFactory = projectContext?.KitchenItemFactory;
        _progress = projectContext?.Progress;
        _purchasedItemRegistry = projectContext?.PurchasedItemRegistry;
        _surfaceUpdaterService = projectContext?.SurfaceUpdaterService;
    }

    public override void Enter()
    {
        _sceneLoader.Load(SceneTypeId.Gameplay, OnLevelLoad);
    }

    public override void OnLevelLoad()
    {
        _uiFactory.CreateHud();
        _uiFactory.CreatePopUpMarket();
        InitGameWorld();
    }

    private void InitGameWorld()
    {
        _surfaceUpdaterService.Init();
        
        _purchasedKitchenItems = _progress.PlayerData.ProgressData.PurchasedKitchenItems;
        foreach (var item in _purchasedKitchenItems)
        {
            _kitchenItem = _kitchenItemFactory.Create(item.TypeId, item.Position, item.Rotation, item.Parent);
            _purchasedItemRegistry.AddKitchenItem(_kitchenItem);
            _surfaceUpdaterService.Update();
        }
        
        _purchasedItemRegistry.AddStorageCrates();
    }

    public override void Tick()
    {
    }

    public override void Exit()
    {
    }
}
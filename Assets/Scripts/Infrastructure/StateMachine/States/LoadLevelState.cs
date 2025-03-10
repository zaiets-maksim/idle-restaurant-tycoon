using System.Collections.Generic;
using Infrastructure;
using Interactable;
using Services.DataStorageService;
using Services.Factories.UIFactory;
using Services.PurchasedItemRegistry;
using Services.SceneLoader;

public class LoadLevelState : GameStateEntity
{
    private readonly ISceneLoader _sceneLoader;
    private readonly IUIFactory _uiFactory;
    private List<KitchenItemInfo> _purchasedKitchenItems = new();
    private readonly IKitchenItemFactory _kitchenItemFactory;
    private readonly IPersistenceProgressService _progress;
    private readonly IPurchasedItemRegistry _purchasedItemRegistry;
    
    private KitchenItem _kitchenItem;

    public LoadLevelState(ProjectContext projectContext)
    {
        _sceneLoader = projectContext?.SceneLoader;
        _uiFactory = projectContext?.UIFactory;
        _kitchenItemFactory = projectContext?.KitchenItemFactory;
        _progress = projectContext?.Progress;
        _purchasedItemRegistry = projectContext?.PurchasedItemRegistry;
    }

    public override void Enter()
    {
        _sceneLoader.Load(SceneTypeId.Gameplay, OnLevelLoad);
    }

    public override void OnLevelLoad()
    {
        _uiFactory.CreateHud();
        InitGameWorld();
    }

    private void InitGameWorld()
    {
        _purchasedKitchenItems = _progress.PlayerData.ProgressData.PurchasedKitchenItems;
        foreach (var item in _purchasedKitchenItems)
        {
            _kitchenItem = _kitchenItemFactory.Create(item.TypeId, item.Position, item.Rotation, item.Parent);
            _purchasedItemRegistry.AddKitchenItem(_kitchenItem);
        }
    }

    public override void Tick()
    {
    }

    public override void Exit()
    {
    }
}
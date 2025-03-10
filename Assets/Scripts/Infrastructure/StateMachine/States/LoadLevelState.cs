using System.Collections.Generic;
using Infrastructure;
using Services.DataStorageService;
using Services.Factories.UIFactory;
using Services.SceneLoader;

public class LoadLevelState : GameStateEntity
{
    private readonly ISceneLoader _sceneLoader;
    private readonly IUIFactory _uiFactory;
    private List<KitchenItemInfo> _purchasedKitchenItems = new();
    private readonly IKitchenItemFactory _kitchenItemFactory;
    private readonly IPersistenceProgressService _progress;

    public LoadLevelState(ProjectContext projectContext)
    {
        _sceneLoader = projectContext?.SceneLoader;
        _uiFactory = projectContext?.UIFactory;
        _kitchenItemFactory = projectContext?.KitchenItemFactory;
        _progress = projectContext?.Progress;
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
            _kitchenItemFactory.Create(item.TypeId, item.Position, item.Rotation, item.Parent);
    }

    public override void Tick()
    {
    }

    public override void Exit()
    {
    }
}
using System.Collections.Generic;
using Characters;
using Infrastructure;
using Interactable;
using Services.CustomerArrivalService;
using Services.DataStorageService;
using Services.Factories.CharacterFactory;
using Services.Factories.ItemFactory;
using Services.Factories.UIFactory;
using Services.PurchasedItemRegistry;
using Services.SceneLoader;
using Services.SurfaceUpdaterService;
using StaticData.Levels;

public class LoadLevelState : GameStateEntity
{
    private readonly ISceneLoader _sceneLoader;
    private readonly IUIFactory _uiFactory;
    private readonly ICharacterFactory _characterFactory;
    private readonly IItemFactory _itemFactory;
    private readonly IPersistenceProgressService _progress;
    private readonly IPurchasedItemRegistry _purchasedItemRegistry;
    private readonly ISurfaceUpdaterService _surfaceUpdaterService;
    private readonly ICustomerArrivalService _customerArrivalService;

    private KitchenItem _kitchenItem;
    private HallItem _hallItem;
    private Person _person;

    private List<KitchenData> _purchasedKitchenItems;
    private List<HallData> _purchasedHallItems;
    private List<CharacterData> _purchasedStuff;

    public LoadLevelState(ProjectContext projectContext)
    {
        _sceneLoader = projectContext?.SceneLoader;
        _uiFactory = projectContext?.UIFactory;
        _characterFactory = projectContext?.CharacterFactory;
        _itemFactory = projectContext?.ItemFactory;
        _progress = projectContext?.Progress;
        _purchasedItemRegistry = projectContext?.PurchasedItemRegistry;
        _surfaceUpdaterService = projectContext?.SurfaceUpdaterService;
        _customerArrivalService = projectContext?.CustomerArrivalService;
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

    public override void Tick()
    {
    }

    public override void Exit()
    {
    }
}
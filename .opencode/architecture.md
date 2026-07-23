# Architecture

## Boot Sequence
```
ProjectBootstrap.Init() [RuntimeInitializeOnLoadMethod]
  └─> Instantiate ProjectContext prefab from Resources
        └─> ProjectContext.Awake()
              ├─> Create DiContainer
              ├─> InstallBindings() — wire all services
              └─> StateMachine.Enter<BootStrapState>()
                    └─> LoadProgressState — load/create PlayerData
                          └─> LoadLevelState — load Gameplay scene, restore saved items, spawn customers
```

## DI Container
Custom lightweight container in `Infrastructure/DI/`.

### Registration (in ProjectContext.InstallBindings)
```csharp
Container.Bind<IStaticDataService>().FromInstance(StaticDataService).AsSingle();
Container.Bind<ISceneLoader>().FromNewComponentOnNewGameObject<SceneLoader>().AsSingle().AsLazy();
Container.Bind<IItemFactory>().FromInstance(ItemFactory).AsSingle();
// etc.
```

### Resolution
```csharp
var service = ProjectContext.Get<T>();
```

### Binding API (BindingBuilder.cs)
| Method | Purpose |
|---|---|
| `Bind<T>()` | Start binding for interface/base type |
| `To<T>()` | Concrete implementation |
| `AsSingle()` | Singleton lifetime |
| `AsLazy()` | Create on first resolve |
| `FromInstance(obj)` | Bind existing instance |
| `FromComponentInNewPrefab(prefab)` | Instantiate prefab, get component |
| `FromNewComponentOnNewGameObject<T>()` | New GameObject with component |

## State Machine (Game Level)
Located in `Infrastructure/StateMachine/`.

| State | Purpose |
|---|---|
| `BootStrapState` | Load Loader scene |
| `LoadProgressState` | Load/save PlayerData, init CurrencyService |
| `LoadLevelState` | Load Gameplay scene, restore purchases, start spawning |

Flow: `BootStrapState` -> `LoadProgressState` -> `LoadLevelState`

## State Machine (Character AI)
Located in `Characters/Behaviors/`. Base class: `PersonBehavior` with `PersonBaseState`.

| Behavior | States |
|---|---|
| `CustomerBehavior` | Idle -> SeatAndOrder -> EatAndPay -> Leave |
| `ChefBehavior` | Idle -> FoodSearch -> Cooking -> DeliverAndServe -> ReturnToSpawn |
| `WaiterBehavior` | Idle -> DishHandling -> OrderDelivery -> ReturnToSpawn |

## Event System
Events are C# events/delegates on services:
- `CurrencyService.OnMoneyChanged`, `OnStarsChanged`
- `OrderStorageService.OnNewOrderReceived`, `OnOrderCooked`
- `KitchenItem.OnRelease` (station freed)
- `ItemBoughtEvent` (kitchen/hall/staff purchased)

## Persistence Flow
```
PlayerData (root)
  ├─> Money, Stars
  └─> PlayerProgressData
        ├─> PurchasedKitchenItems (List<KitchenItemTypeId>)
        ├─> PurchasedHallItems (List<HallItemTypeId>)
        ├─> PurchasedStaff (List<CharacterTypeId>)
        ├─> Upgrades (Dictionary<UpgradeTypeId, int> — level per upgrade)
        └─> StaffStats (speed, cooking time, eating time modifiers)

SaveLoadService.Save(PlayerData) -> JSON -> PlayerPrefs
SaveLoadService.Load() -> JSON from PlayerPrefs -> PlayerData (or create default)
```

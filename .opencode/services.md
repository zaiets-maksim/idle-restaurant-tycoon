# Services Reference

All services registered in `ProjectContext.InstallBindings()`. Access via `ProjectContext.Get<T>()`.

## Core

| Service | Interface | File | Purpose |
|---|---|---|---|
| `StaticDataService` | `IStaticDataService` | `Services/StaticDataService/StaticDataService.cs` | Loads all ScriptableObject configs from `Resources/StaticData/`. Provides `GetKitchenItem()`, `GetHallItem()`, `GetCharacter()`, `GetDish()`, `GetBalance()`, etc. |
| `SceneLoader` | `ISceneLoader` | `Services/SceneLoader/SceneLoader.cs` | Async scene loading. `Load(sceneName, onLoaded)` |
| `SaveLoadService` | `ISaveLoadService` | `Services/SaveLoad/SaveLoadService.cs` | `Save(PlayerData)`, `Load()` — JSON <-> PlayerPrefs |
| `CurrencyService` | `ICurrencyService` | `Services/CurrencyService/CurrencyService.cs` | `Money`, `Stars`, `AddMoney()`, `SpendMoney()`, `AddStars()`. Events: `OnMoneyChanged`, `OnStarsChanged`. Auto-saves on change. |

## Gameplay

| Service | Interface | File | Purpose |
|---|---|---|---|
| `ItemBuyingService` | `IItemBuyingService` | `Services/ItemBuyingService/ItemBuyingService.cs` | `BuyKitchenItem()`, `BuyHallItem()`, `BuyStaff()`. Checks availability, delegates to factories, saves progress. |
| `OrderStorageService` | `IOrderStorageService` | `Services/OrderStorageService/OrderStorageService.cs` | Order queue: `AddOrder()`, `CookOrder()`, `ServeOrder()`. Events: `OnNewOrderReceived`, `OnOrderCooked` |
| `CustomerArrivalService` | `ICustomerArrivalService` | `Services/CustomerArrivalService/CustomerArrivalService.cs` | Coroutine-based customer spawner. Uses `BalanceStaticData` for intervals. |
| `PurchasedItemRegistry` | `IPurchasedItemRegistry` | `Services/PurchasedItemRegistry/PurchasedItemRegistry.cs` | Tracks all instantiated kitchen items, hall items, staff. `IsKitchenItemAvailable()`, `IsHallItemAvailable()`, etc. |
| `ActiveCustomersRegistry` | `IActiveCustomersRegistry` | `Services/ActiveCustomersRegistry/ActiveCustomersRegistry.cs` | Tracks currently active customers |
| `SurfaceUpdaterService` | `ISurfaceUpdaterService` | `Services/SurfaceUpdaterService/SurfaceUpdaterService.cs` | Rebuilds NavMesh surfaces (kitchen, hall, common) |

## UI

| Service | Interface | File | Purpose |
|---|---|---|---|
| `WindowService` | `IWindowService` | `Services/WindowService/WindowService.cs` | UI window management |

## Factories

| Factory | File | Purpose |
|---|---|---|
| `ItemFactory` | `Services/Factories/Factory.cs` + `ItemFactory/ItemFactory.cs` | Creates `KitchenItem`, `HallItem`, `Dish` instances |
| `CharacterFactory` | `Services/Factories/CharacterFactory/CharacterFactory.cs` | Creates `Person`/`Customer` with random appearances |
| `UIFactory` | `Services/Factories/UIFactory/UIFactory.cs` | Creates Hud, PopUpMarket, market list elements |

## Event Dispatchers

| Dispatcher | File | Purpose |
|---|---|---|
| `KitchenEventDispatcher` | `Services/EventDispatchers/KitchenEventDispatcher.cs` | Routes kitchen-related events |
| `HallEventDispatcher` | `Services/EventDispatchers/HallEventDispatcher.cs` | Routes hall-related events |

## Adding a New Service
1. Create interface `IXxxService` and implementation `XxxService`
2. Implement `IInitializable` (setup) and `IDisposable` (cleanup) if needed
3. Add binding in `ProjectContext.InstallBindings()`
4. Access via `ProjectContext.Get<IXxxService>()`

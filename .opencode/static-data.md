# Static Data & Configs

## Location
All configs in `Assets/Resources/StaticData/` as ScriptableObject `.asset` files.
Loaded at startup by `StaticDataService` from `Resources/StaticData/`.

## TypeId Enums

### KitchenItemTypeId
`Scripts/StaticData/TypeId/KitchenItemTypeId.cs`
| Value | Description |
|---|---|
| `BurgerStation` | Produces burgers |
| `DishStation` | Produces dinners |
| `Dishwashing` | Washing station |
| `ServingTable` | Serving area |
| `SilverwareTable` | Silverware storage |
| `Fridge` | Food storage |

### HallItemTypeId
`Scripts/StaticData/TypeId/HallItemTypeId.cs`
| Value | Description |
|---|---|
| `Table` | Dining table |
| `Chair` | Customer seat |

### CharacterTypeId
`Scripts/StaticData/TypeId/CharacterTypeId.cs`
| Value | Description |
|---|---|
| `Customer` | Paying guest |
| `Chef` | Kitchen staff |
| `Waiter` | Service staff |

### DishTypeId
`Scripts/StaticData/TypeId/DishTypeId.cs`
| Value | Description |
|---|---|
| `Burger` | Burger dish |
| `Dinner` | Dinner dish |
| `Stew` | Stew dish |

## Config ScriptableObjects

### BalanceStaticData
Customer spawn interval, speed range, meal duration timing.

### KitchenItemStaticData
Dictionary<KitchenItemTypeId, KitchenItemConfig> — prefab + cost per kitchen item.

### HallItemStaticData
Dictionary<HallItemTypeId, HallItemConfig> — prefab + cost per hall item.

### CharacterStaticData
Dictionary<CharacterTypeId, CharacterConfig> — prefab + market data per character type.

### DishStaticData
Dictionary<DishTypeId, DishConfig> — prefab + price per dish.

### LevelStaticData
Defines all kitchen items, hall items, storage, and character positions for the level.

### WindowsStaticData
UI window configurations.

## Accessing Configs
```csharp
var staticData = ProjectContext.Get<IStaticDataService>();
var kitchenConfig = staticData.GetKitchenItem(KitchenItemTypeId.BurgerStation);
var balance = staticData.GetBalance();
var dishConfig = staticData.GetDish(DishTypeId.Burger);
```

## PlayerProgressData (Runtime Save Data)
```
PlayerProgressData
  ├─> PurchasedKitchenItems: List<KitchenItemTypeId>
  ├─> PurchasedHallItems: List<HallItemTypeId>
  ├─> PurchasedStaff: List<CharacterTypeId>
  ├─> Upgrades: Dictionary<UpgradeTypeId, int>  (upgrade level)
  ├─> Money: int
  ├─> Stars: int
  └─> StaffStats
        ├─> SpeedMultiplier: float
        ├─> CookingTimeMultiplier: float
        └─> EatingTimeMultiplier: float
```

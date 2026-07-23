# Idle Restaurant Tycoon - Agent Instructions

## Project Overview
- **Type:** Unity idle/tycoon restaurant management game
- **Engine:** Unity 2022.3.62f3 (LTS), C# (.NET Standard 2.1)
- **Architecture:** Service Locator + custom DI container (NO Zenject, NO UniTask)
- **Third-party:** DOTween (animations), NavMesh Components, KayKit assets, Ultimate Animated Character Pack

## Build & Run
- Open project folder in Unity 2022.3.62f3
- Play: Open `Assets/Scenes/Loader.unity` (index 0), press Play
- Build scenes: `Loader.unity` (0) -> `Gameplay.unity` (1)
- Target: Android (min SDK 26) + Standalone

## Code Conventions
- **DI:** Custom `DiContainer` in `Infrastructure/DI/`. Bindings in `ProjectContext.InstallBindings()`
- **Service access:** `ProjectContext.Get<T>()` (static service locator)
- **State machines:** Game-level in `Infrastructure/StateMachine/`, character-level in `Characters/Behaviors/`
- **Data:** ScriptableObject configs in `Resources/StaticData/`, loaded by `StaticDataService`
- **Persistence:** `PlayerData` -> JSON -> `PlayerPrefs` via `SaveLoadService`
- **No comments** in code unless explicitly asked

## Key Entry Points
| File | Role |
|---|---|
| `Scripts/Infrastructure/ProjectBootstrap.cs` | App start, loads ProjectContext prefab |
| `Scripts/Infrastructure/ProjectContext.cs` | DI container setup, service bindings, state machine boot |
| `Scripts/Infrastructure/DI/DiContainer.cs` | Custom DI container |
| `Scripts/Infrastructure/DI/BindingBuilder.cs` | Fluent binding API |

## File Structure Quick Reference
```
Assets/Scripts/
├── Characters/          # AI: Customer, Chef, Waiter + Behaviors/States
├── Extensions/          # C# extension methods
├── Gameplay/            # Interactable objects (KitchenItem, HallItem, Chair, Table, Dish, Crates)
├── Infrastructure/      # Core: DI, StateMachine, Bootstrap, ProjectContext
├── Services/            # 20+ services (see .opencode/services.md)
├── StaticData/          # TypeId enums + ScriptableObject configs
└── UI/                  # Views, PopUpMarket, Buttons, Cheats
```

## Important Patterns
- All services implement `IInitializable` for setup and `IDisposable` for cleanup
- Character AI uses `PersonBehavior` base class with state pattern (`PersonBaseState`)
- Kitchen items use occupy/release pattern for station assignment
- Customer flow: Spawn -> FindChair -> Order -> ChefCooks -> WaiterServes -> Eat -> Pay -> Leave
- Events used extensively: `OnMoneyChanged`, `OnStarsChanged`, `OnNewOrderReceived`, `OnOrderCooked`

## Constraints
- Do NOT use Zenject or UniTask
- Do NOT add unnecessary comments
- Follow existing code style and patterns
- All runtime assets loaded from `Resources/` folder

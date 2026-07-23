# Characters & AI

## Class Hierarchy
```
MonoBehaviour
  └─> Person (abstract) — NavMeshAgent, ProgressIndicator, PersonAnimator, PersonMover
        ├─> Employee (abstract) — staff base
        │     ├─> Chef
        │     ├─> Waiter
        │     └─> Dishwasher
        └─> Customer
```

## Core Components per Character
| Component | Purpose |
|---|---|
| `PersonMover` | Wraps NavMeshAgent, handles movement to target |
| `PersonAnimator` | Controls Animator states (walk, idle, cook, eat, etc.) |
| `PersonRotator` | Handles character rotation |
| `ProgressIndicator` | Shows progress bar for cooking/eating |
| `DishHolder` | Holds a dish model (for Chef/Waiter delivery) |
| `PersonItemCollector` | Collects items (food) |

## AI State Machines

### Customer (`CustomerBehavior`)
```
IdleState
  └─> SeatAndOrderState
        - Find free chair (via PurchasedItemRegistry)
        - Walk to chair
        - Sit (disable NavMeshAgent)
        - Wait, then place random order
        └─> EatAndPayState
              - Wait for dish delivery (OrderStorageService)
              - Eat with progress timer
              - Pay (CurrencyService.AddMoney)
              └─> LeaveState
                    - Stand up, release chair
                    - Walk to exit, destroy
```

### Chef (`ChefBehavior`)
```
IdleState
  └─> FoodSearchState
        - Listen for OnNewOrderReceived
        - Find free FoodStation (KitchenItem.Occupy)
        - Walk to station
        └─> CookingState
              - Cook dish with progress timer
              - Create Dish at placement point
              └─> DeliverAndServeState
                    - Pick up dish
                    - Find customer with matching order
                    - Walk to customer, deliver
                    └─> ReturnToSpawnState
                          - Return to spawn point
                          - Back to IdleState
```

### Waiter (`WaiterBehavior`)
```
IdleState
  └─> DishHandlingState
        - Listen for OnOrderCooked
        - Pick up cooked dish
        └─> OrderDeliveryState
              - Find customer with order
              - Walk to customer, deliver
              └─> ReturnToSpawnState
                    - Return to spawn point
                    - Back to IdleState
```

## Character Factory
`CharacterFactory.CreateCharacter(type)`:
- Loads prefab from Resources
- Instantiates on active scene
- Assigns random appearance from `CharacterStaticData`
- Sets up NavMeshAgent, Animator, behavior

## NavMesh Integration
- Characters use `NavMeshAgent` for pathfinding
- `SurfaceUpdaterService` rebuilds NavMesh when items are purchased
- Kitchen and Hall have separate NavMesh surfaces

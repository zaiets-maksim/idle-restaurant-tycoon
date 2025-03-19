using System.Threading.Tasks;
using Characters;
using Characters.Behaviors;
using Characters.PersonStateMachine;
using Characters.States;

public class LeaveHallState : PersonBaseState
{
    private readonly WaiterBehavior _waiterBehavior;
    private readonly Waiter _waiter;

    public LeaveHallState(WaiterBehavior waiterBehavior, Waiter waiter)
    {
        _waiterBehavior = waiterBehavior;
        _waiter = waiter;
    }
    
    public override async void Enter()
    {
        _tcs = new TaskCompletionSource<bool>();

        await _waiter.LeaveHall();
        
        _waiterBehavior.ChangeState<IdleState>();
    }

    public override void Exit()
    {
        
    }
}

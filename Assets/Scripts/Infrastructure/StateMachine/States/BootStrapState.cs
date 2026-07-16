using Infrastructure;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;

public class BootStrapState : GameStateEntity
{
    private readonly IStateMachine _stateMachine;

    public BootStrapState(ProjectContext projectContext)
    {
        _stateMachine = ProjectContext.Get<IStateMachine>();
    }
    
    public override void Enter()
    {
        _stateMachine.Enter<LoadProgressState>();
    }

    public override void OnLevelLoad()
    {
    }

    public override void Tick()
    {
    }

    public override void Exit()
    {
    }
}

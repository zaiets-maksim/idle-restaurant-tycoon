using Infrastructure;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using Services.SceneLoader;
using StudentHistory.Scripts;

public class BootStrapState : GameStateEntity
{
    private readonly IStateMachine _stateMachine;
    private ISceneLoader _sceneLoader;

    public BootStrapState(ProjectContext projectContext)
    {
        _sceneLoader = ProjectContext.Get<ISceneLoader>();
        _stateMachine = ProjectContext.Get<IStateMachine>();
    }
    
    public override void Enter()
    {
        _sceneLoader.Load(SceneTypeId.Initial, OnLevelLoad);
    }

    public override void OnLevelLoad()
    {
        _stateMachine.Enter<LoadProgressState>();
    }


    public override void Tick()
    {
    }

    public override void Exit()
    {
    }
}

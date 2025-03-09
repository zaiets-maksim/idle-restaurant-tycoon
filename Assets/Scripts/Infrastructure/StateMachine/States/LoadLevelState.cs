using Infrastructure;
using Services.Factories.UIFactory;
using Services.SceneLoader;
using StudentHistory.Scripts;

public class LoadLevelState : GameStateEntity
{
    private readonly ISceneLoader _sceneLoader;
    private readonly IUIFactory _uiFactory;

    public LoadLevelState(ProjectContext projectContext)
    {
        _sceneLoader = projectContext.SceneLoader;
        _uiFactory = projectContext.UIFactory;
    }

    public override void Enter()
    {
        _sceneLoader.Load(SceneTypeId.Gameplay, OnLevelLoad);
    }

    public override void OnLevelLoad()
    {
        _uiFactory.CreateHud();
    }

    public override void Tick()
    {
    }

    public override void Exit()
    {
    }
}
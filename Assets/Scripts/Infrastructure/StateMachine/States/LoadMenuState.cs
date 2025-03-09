using Infrastructure;
using Services.SceneLoader;
using Services.WindowService;
using StaticData.Configs;
using StudentHistory.Scripts;

public class LoadMenuState : GameStateEntity
{
    // private readonly IGameFactory _gameFactory;
    private readonly IWindowService _windowService;
    private readonly ISceneLoader _sceneLoader;

    public LoadMenuState(ProjectContext projectContext)
    {
        _sceneLoader = projectContext.SceneLoader;
        _windowService = projectContext.WindowService;
        // _gameFactory = gameFactory;
    }

    public override void Enter()
    {
        _sceneLoader.Load(SceneTypeId.MainMenu, OnLevelLoad);
    }

    public override void OnLevelLoad()
    {
        _windowService.Open(WindowTypeId.Menu);
    }


    public override void Tick()
    {
    }

    public override void Exit()
    {
    }
}
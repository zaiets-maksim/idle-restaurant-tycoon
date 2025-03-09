using UnityEngine;

namespace StudentHistory.Scripts.UI
{
    public class BackToPreviousScene : MonoBehaviour
    {
        // [SerializeField] private Button _button;
        //
        // private IStateMachine _stateMachine;
        // private ISceneLoader _sceneLoader;
        //
        // [Inject]
        // public void Constructor(IStateMachine stateMachine, ISceneLoader sceneLoader)
        // {
        //     _sceneLoader = sceneLoader;
        //     _stateMachine = stateMachine;
        // }
        //
        // private void Start() => 
        //     _button.onClick.AddListener(GoToState);
        //
        // private void GoToState()
        // {
        //     Debug.Log(_sceneLoader.LastSceneTypeId);
        //     _stateMachine.Enter(GameState.LoadLevelState, _sceneLoader.LastSceneTypeId);
        // }
    }
}

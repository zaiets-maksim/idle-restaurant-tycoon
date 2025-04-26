using Infrastructure;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.Game.States;
using Services.DataStorageService;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Buttons
{
    public class MenuButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        
        private IStateMachine<IGameState> _stateMachine;

        [Inject]
        public void Constructor(IStateMachine<IGameState> stateMachine)
        {
            _stateMachine = stateMachine;
        }
        
        private void Start() => _button.onClick.AddListener(ToMenu);
        
        private void ToMenu() => _stateMachine.Enter<MenuLevelState, string>("MainMenu");
    }
}
using Infrastructure;
using Infrastructure.StateMachine;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class MenuButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        
        private readonly IStateMachine _stateMachine;
        
        public MenuButton()
        {
            _stateMachine = ProjectContext.Instance?.StateMachine;
        }
        
        private void Start() => _button.onClick.AddListener(ToMenu);
        
        private void ToMenu() => _stateMachine.Enter<LoadMenuState>();
    }
}
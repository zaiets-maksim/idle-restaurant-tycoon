using Friction_balance.Scripts.Infrastructure.StateMachine.Game.States;
using Infrastructure;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.Game.States;
using Services.DataStorageService;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Buttons
{
    public class PlayButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Text _text;
        private IStateMachine<IGameState> _stateMachine;
        private IPersistenceProgressService _progress;
        
        [Inject]
        public void Constructor(IStateMachine<IGameState> stateMachine, IPersistenceProgressService progress)
        {
            _progress = progress;
            _stateMachine = stateMachine;
        }

        private void Start()
        {
            InitNameButton();
            _button.onClick.AddListener(Play);
        }

        private void InitNameButton()
        {
            if (_progress.PlayerData.ProgressData.HasProgress)
                _text.text = "Resume";
        }

        private void Play()
        {
            _stateMachine.Enter<LoadLevelState, string>("Gameplay");
        }
    }
}
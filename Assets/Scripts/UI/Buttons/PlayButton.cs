using Infrastructure;
using Infrastructure.StateMachine;
using Services.DataStorageService;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class PlayButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Text _text;
        
        private readonly IStateMachine _stateMachine;
        private readonly IPersistenceProgressService _progress;

        public PlayButton()
        {
            _stateMachine = ProjectContext.Instance?.StateMachine;
            _progress = ProjectContext.Instance?.Progress;
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
            _stateMachine.Enter<LoadLevelState>();
        }
    }
}

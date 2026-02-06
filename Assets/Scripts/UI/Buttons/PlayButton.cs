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
        
        private IStateMachine _stateMachine => ProjectContext.Get<IStateMachine>();
        private IPersistenceProgressService _progress => ProjectContext.Get<IPersistenceProgressService>();
        
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

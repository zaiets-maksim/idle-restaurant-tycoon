using UnityEngine;
using UnityEngine.UI;

namespace StudentHistory.Scripts.UI.Buttons
{
    public class ExitGameButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
    
        private void Start() => _button.onClick.AddListener(Exit);

        private void Exit() => Application.Quit();
    }
}

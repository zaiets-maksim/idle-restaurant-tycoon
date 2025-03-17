using UnityEngine;
using UnityEngine.UI;

namespace UI.Cheats
{
    public class AccelerateTimeButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private float _coefficient;

        private bool _isAccelerated;
        
        private void Start()
        {
            _button.onClick.AddListener(SwitchAccelerate);
        }

        private void SwitchAccelerate()
        {
            if (_isAccelerated)
                Return();
            else
                Accelerate();
        }

        private void Accelerate()
        {
            Time.timeScale = _coefficient;
            _isAccelerated = true;
        }

        private void Return()
        {
            Time.timeScale = 1f;
            _isAccelerated = false;
        }
    }
}

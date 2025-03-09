using project01a.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace StudentHistory.Scripts.UI.Buttons
{
    // [RequireComponent(typeof(ButtonClickResponse))]
    [RequireComponent(typeof(ButtonClickVisualizer))]
    public class ClosePopUpWindow : MonoBehaviour
    {
        // [SerializeField] private Button _button;
        //
        // private IWindowService _windowService;
        //
        // private void Start() => 
        //     _button.onClick.AddListener(Open);
        //
        // private void Open() => 
        //     _windowService.ClosePopUp();
    }
}

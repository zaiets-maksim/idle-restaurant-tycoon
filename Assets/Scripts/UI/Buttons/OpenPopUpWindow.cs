//
// namespace StudentHistory.Scripts.UI.Buttons
// {
//     [RequireComponent(typeof(ButtonClickResponse))]
//     [RequireComponent(typeof(ButtonClickVisualizer))]
//     public class OpenPopUpWindow : MonoBehaviour
//     {
//         [SerializeField] private Button _button;
//         [SerializeField] private PopUpWindowTypeId _popUpWindowTypeId;
//         
//         private IWindowService _windowService;
//
//         [Inject]
//         public void Constructor(IWindowService windowService) => 
//             _windowService = windowService;
//
//         private void Start() => 
//             _button.onClick.AddListener(Open);
//
//         private void Open() => 
//             _windowService.OpenPopUp(_popUpWindowTypeId);
//     }
// }

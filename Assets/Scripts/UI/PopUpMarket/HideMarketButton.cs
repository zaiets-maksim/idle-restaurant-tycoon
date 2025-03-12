using UnityEngine;
using UnityEngine.UI;

namespace UI.PopUpMarket
{
    public class HideMarketButton : MonoBehaviour
    {
        [SerializeField] private PopUpMarket _popUpMarket;
        [SerializeField] private Button _button;

        private void Start() => _button.onClick.AddListener(Hide);

        private void Hide() => _popUpMarket.Hide();
    }
}

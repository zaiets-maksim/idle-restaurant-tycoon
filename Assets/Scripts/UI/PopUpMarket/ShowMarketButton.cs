using Infrastructure;
using Services.Factories.UIFactory;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PopUpMarket
{
    public class ShowMarketButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        
        private readonly IUIFactory _uiFactory;
        private PopUpMarket _popUpMarket;

        public ShowMarketButton()
        {
            _uiFactory = ProjectContext.Instance?.UIFactory;
        }

        private void Start()
        {
            _popUpMarket = _uiFactory.PopUpMarket;
            _popUpMarket.InitShowButton(this);
            _button.onClick.AddListener(Show);
        }

        private void Show()
        {
            _popUpMarket.Show();
        }
    }
}

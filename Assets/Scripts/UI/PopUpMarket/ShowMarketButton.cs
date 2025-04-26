using Services.Factories.UIFactory;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.PopUpMarket
{
    public class ShowMarketButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        
        private IUIFactory _uiFactory;
        
        private PopUpMarket _popUpMarket;

        [Inject]
        public void Constructor(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
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

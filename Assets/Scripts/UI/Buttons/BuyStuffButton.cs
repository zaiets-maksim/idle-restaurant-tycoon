using Infrastructure;
using Services.CurrencyService;
using Services.ItemBuyingService;
using StaticData.TypeId;
using UI.PopUpMarket;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace UI.Buttons
{
    public class BuyStuffButton : MonoBehaviour
    {
        [SerializeField] private StuffElement _stuffElement;

        [SerializeField] private Button _button;
        [SerializeField] private CharacterTypeId _typeId;
        [SerializeField] private Image _buttonImage;
        [SerializeField] private Material _grayScaleMaterial;
        
        private IItemBuyingService _itemBuyingService;
        private ICurrencyService _currencyService;

        [Inject]
        public void Constructor(IItemBuyingService itemBuyingService, ICurrencyService currencyService)
        {
            _currencyService = currencyService;
            _itemBuyingService = itemBuyingService;
        }
        
        private void Start()
        {
            _button.onClick.AddListener(BuyStuff);
        }

        public void Initialize(CharacterTypeId typeId)
        {
            _typeId = typeId;
        }
        
        public void Active()
        {
            _button.interactable = true;
            _buttonImage.material = null;
        }
        
        public void Lock()
        {
            _button.interactable = false;
            _buttonImage.material = _grayScaleMaterial;
        }

        private void BuyStuff()
        {
            if (_currencyService.CanAffordWithMoney(_stuffElement.ActualPrice))
            {
                _currencyService.RemoveMoney(_stuffElement.ActualPrice);
                _itemBuyingService.BuyStuff(_typeId);
                _stuffElement.UpdateAvailableCount();
            }
            else
            {
                // money are not enough
            }
        }
    }
}

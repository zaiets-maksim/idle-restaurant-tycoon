using Infrastructure;
using Services.CurrencyService;
using Services.ItemBuyingService;
using StaticData;
using UI.PopUpMarket;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class BuyKitchenItemButton : MonoBehaviour
    {
        [SerializeField] private KitchenItemElement _kitchenItemElement;
        [SerializeField] private Button _button;
        [SerializeField] private KitchenItemTypeId _typeId;
        [SerializeField] private Image _buttonImage;
        [SerializeField] private Material _grayScaleMaterial;
        
        private readonly IItemBuyingService _itemBuyingService;
        private readonly ICurrencyService _currencyService;

        private BuyKitchenItemButton()
        {
            _itemBuyingService = ProjectContext.Instance?.ItemBuyingService;
            _currencyService = ProjectContext.Instance?.CurrencyService;
        }

        private void Start()
        {
            _button.onClick.AddListener(BuyKitchenItem);
        }

        public void Initialize(KitchenItemTypeId typeId)
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

        private void BuyKitchenItem()
        {
            if (_currencyService.CanAffordWithMoney(_kitchenItemElement.Price))
            {
                _currencyService.RemoveMoney(_kitchenItemElement.Price);
                _itemBuyingService.BuyKitchenItem(_typeId);
                _kitchenItemElement.UpdateAvailableCount(_typeId);
            }
            else
            {
                // money are not enough
            }
        }
    }
}

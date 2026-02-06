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
        
        private IItemBuyingService _itemBuyingService => ProjectContext.Get<IItemBuyingService>();
        private ICurrencyService _currencyService => ProjectContext.Get<ICurrencyService>();

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
            if (_currencyService.CanAffordWithMoney(_kitchenItemElement.ActualPrice))
            {
                _currencyService.RemoveMoney(_kitchenItemElement.ActualPrice);
                _itemBuyingService.BuyKitchenItem(_typeId);
                _kitchenItemElement.UpdateAvailableCount();
            }
            else
            {
                // money are not enough
            }
        }
    }
}

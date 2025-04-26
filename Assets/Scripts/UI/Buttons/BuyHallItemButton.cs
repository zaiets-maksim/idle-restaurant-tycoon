using Infrastructure;
using Services.CurrencyService;
using Services.ItemBuyingService;
using StaticData.TypeId;
using UI.PopUpMarket;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Buttons
{
    public class BuyHallItemButton : MonoBehaviour
    {
        [SerializeField] private HallItemElement _hallItemElement;
        [SerializeField] private Button _button;
        [SerializeField] private HallItemTypeId _typeId;
        [SerializeField] private Image _buttonImage;
        [SerializeField] private Material _grayScaleMaterial;

        private IItemBuyingService _itemBuyingService;
        private ICurrencyService _currencyService;

        [Inject]
        public void Constructor(IItemBuyingService itemBuyingService, ICurrencyService currencyService)
        {
            _itemBuyingService = itemBuyingService;
            _currencyService = currencyService;
        }
        
        private void Start()
        {
            _button.onClick.AddListener(TryBuyHallItem);
        }

        public void Initialize(HallItemTypeId typeId)
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

        private void TryBuyHallItem()
        {
            if (_typeId == HallItemTypeId.Chair)
            {
                if (_itemBuyingService.CanPurchaseChair(out int chairs) && CanAffordWithMoney(_hallItemElement.ActualPrice))
                {
                    PurchaseHallItem();
                    return;
                }

                return;
            }

            if (CanAffordWithMoney(_hallItemElement.ActualPrice))
                PurchaseHallItem();
        }

        private bool CanAffordWithMoney(int price)
        {
            return _currencyService.CanAffordWithMoney(price);
        }

        private void PurchaseHallItem()
        {
            _currencyService.RemoveMoney(_hallItemElement.ActualPrice);
            _itemBuyingService.BuyHallItem(_typeId);
            _hallItemElement.UpdateAfterPurchase();
        }
    }
}
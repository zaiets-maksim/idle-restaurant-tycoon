using Services.ItemBuyingService;
using StaticData.TypeId;
using UI.Buttons;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PopUpMarket
{
    public class HallItemElement : ItemElement
    {
        [SerializeField] private BuyHallItemButton _buyHallItemButton;
        [SerializeField] private Text _needable;

        private HallItemTypeId _hallItemTypeId;
        public void Initialize(HallItemConfig config, IItemBuyingService itemBuyingService)
        {
            _itemBuyingService = itemBuyingService;
            _itemBuyingService.OnHallItemPurchased += TryUpdateElement;
            _icon.sprite = config.MarketItem.Icon;
            _name.text = config.MarketItem.Name;
            _description.text = config.MarketItem.Description;
            _defaultPrice = config.MarketItem.Price;
            UpdatePrice(_defaultPrice * _itemBuyingService.GetNextAvailableOrder(config.TypeId));
            _buyHallItemButton.Initialize(config.TypeId);
            _defaultNameColor = _name.color;
            _hallItemTypeId = config.TypeId;
        
            if (_hallItemTypeId == HallItemTypeId.Chair)
            {
                UpdateAvailableForChair();
                return;
            }
        
            if (_itemBuyingService.GetAvailableItemCount(config.TypeId, out int count))
                SetAvailable(count);
            else
                MakeLock();
        }

        private void TryUpdateElement(HallItemTypeId purchasedItemTypeId)
        {
            if (_hallItemTypeId == HallItemTypeId.Chair && purchasedItemTypeId == HallItemTypeId.Table) 
                UpdateAfterPurchase();   
        }

        private void SetAvailable(int count)
        {
            _needable.gameObject.SetActive(false);
            _available.text = $"Available: {count}";
            _available.gameObject.SetActive(true);
        }

        private void SetNeedable(HallItemTypeId typeId)
        {
            _available.gameObject.SetActive(false);
            _needable.text = $"Need: {typeId}";
            _needable.gameObject.SetActive(true);
        }

        private void UpdateAvailableForChair()
        {
            if (_itemBuyingService.CanPurchaseChair(out int availableChairs))
            {
                if (availableChairs > 0)
                {
                    MakeActive();
                    SetAvailable(availableChairs);
                    UpdatePrice(_defaultPrice * _itemBuyingService.GetNextAvailableOrder(_hallItemTypeId));
                }
            }
            else
            {
                MakeLock();
                SetNeedable(HallItemTypeId.Table);
            }
        }

        public void MakeActive()
        {
            _buyHallItemButton.Active();
            _icon.material = null;
            _currencyImage.material = null;
            _name.color = _defaultNameColor;
        }

        public void MakeLock()
        {
            _buyHallItemButton.Lock();
            SetAvailable(0);
            _priceText.text = "0";
            _icon.material = _grayScaleMaterial;
            _currencyImage.material = _grayScaleMaterial;
            _name.color = Color.white;
        }
        
        public void UpdateAfterPurchase()
        {
            if (_hallItemTypeId == HallItemTypeId.Chair)
            {
                UpdateAvailableForChair();
                return;
            }

            if (_itemBuyingService.GetAvailableItemCount(_hallItemTypeId, out int count))
            {
                SetAvailable(count);
                UpdatePrice(_itemBuyingService.GetNextAvailableOrder(_hallItemTypeId) * _defaultPrice);
            }
            else
                MakeLock();
        }
    }
}
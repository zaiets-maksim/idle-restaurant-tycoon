using Services.ItemBuyingService;
using StaticData;
using StaticData.Configs;
using UI.Buttons;
using UnityEngine;

namespace UI.PopUpMarket
{
    public class KitchenItemElement : ItemElement
    {
        [SerializeField] private BuyKitchenItemButton _buyKitchenItemButton;

        private KitchenItemTypeId _kitchenItemTypeId;

        public void Initialize(KitchenItemConfig config, IItemBuyingService itemBuyingService)
        {
            _itemBuyingService = itemBuyingService;
            _icon.sprite = config.MarketItem.Icon;
            _name.text = config.MarketItem.Name;
            _description.text = config.MarketItem.Description;
            _defaultPrice = config.MarketItem.Price;
            UpdatePrice(_defaultPrice * _itemBuyingService.GetNextAvailableOrder(config.TypeId));
            _buyKitchenItemButton.Initialize(config.TypeId);
            _defaultNameColor = _name.color;
            _kitchenItemTypeId = config.TypeId;

            if (_itemBuyingService.GetAvailableItemCount(config.TypeId, out int count))
            {
                _available.text = $"Available: {count}";
            }
            else
            {
                MakeLock();
                return;
            }
        }

        public void MakeActive()
        {
            _buyKitchenItemButton.Active();
            _icon.material = null;
            _currencyImage.material = null;
            _name.color = _defaultNameColor;
        }

        public void MakeLock()
        {
            _buyKitchenItemButton.Lock();
            _available.text = $"Available: 0";
            _priceText.text = "0";
            _icon.material = _grayScaleMaterial;
            _currencyImage.material = _grayScaleMaterial;
            _name.color = Color.white;
        }

        public void UpdateAvailableCount()
        {
            if (_itemBuyingService.GetAvailableItemCount(_kitchenItemTypeId, out int count))
            {
                _available.text = $"Available: {count}";
                UpdatePrice(_itemBuyingService.GetNextAvailableOrder(_kitchenItemTypeId) * _defaultPrice);
            }
            else
                MakeLock();
        }
    }
}
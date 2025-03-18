using Services.ItemBuyingService;
using StaticData.Configs;
using StaticData.TypeId;
using UI.Buttons;
using UnityEngine;

namespace UI.PopUpMarket
{
    public class StuffElement : ItemElement
    {
        [SerializeField] private BuyStuffButton buyStuffButton;
        
        private IItemBuyingService _itemBuyingService;

        private CharacterTypeId _characterTypeId;

        public void Initialize(CharacterConfig config, IItemBuyingService itemBuyingService)
        {
            _characterTypeId = config.TypeId;
            _itemBuyingService = itemBuyingService;
            _icon.sprite = config.MarketItem.Icon;
            _name.text = config.MarketItem.Name;
            _description.text = config.MarketItem.Description;
            _defaultPrice = config.MarketItem.Price;
            
            int boughtCount = _itemBuyingService.GetNextAvailableOrder(_characterTypeId);
            int remainingAmount = _itemBuyingService.GetAvailableStuffCount(_characterTypeId);
            
            UpdatePrice(_defaultPrice * boughtCount);
            buyStuffButton.Initialize(config.TypeId);
            _defaultNameColor = _name.color;
            _characterTypeId = config.TypeId;

            if (remainingAmount > 0)
            {
                _available.text = $"Available: {boughtCount}";
            }
            else
            {
                MakeLock();
                return;
            }
        }

        public void MakeActive()
        {
            buyStuffButton.Active();
            _icon.material = null;
            _currencyImage.material = null;
            _name.color = _defaultNameColor;
        }

        public void MakeLock()
        {
            buyStuffButton.Lock();
            _available.text = $"Available: 0";
            _priceText.text = "0";
            _icon.material = _grayScaleMaterial;
            _currencyImage.material = _grayScaleMaterial;
            _name.color = Color.white;
        }

        public void UpdateAvailableCount()
        {
            int boughtCount = _itemBuyingService.GetNextAvailableOrder(_characterTypeId);
            int remainingAmount = _itemBuyingService.GetAvailableStuffCount(_characterTypeId);
            
            if (boughtCount > 0)
            {
                _available.text = $"Available: {remainingAmount}";
                UpdatePrice(boughtCount * _defaultPrice);
            }
            else
                MakeLock();
        }
    }
}

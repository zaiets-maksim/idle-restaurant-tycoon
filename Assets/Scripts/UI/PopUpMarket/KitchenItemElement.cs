using Services.ItemBuyingService;
using StaticData;
using StaticData.Configs;
using UI.Buttons;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PopUpMarket
{
    public class KitchenItemElement : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _currencyImage;
        [SerializeField] private Text _name;
        [SerializeField] private Text _description;
        [SerializeField] private Text _priceText;
        [SerializeField] private BuyKitchenItemButton _buyKitchenItemButton;
        [SerializeField] private Text _available;
        [SerializeField] private Material _grayScaleMaterial;
        
        private IItemBuyingService _itemBuyingService;
        private KitchenItemTypeId _kitchenItemTypeId;
        
        private Color _defaultNameColor;
        private int _price;

        public int Price => _price;

        public void Initialize(KitchenItemConfig config, IItemBuyingService itemBuyingService)
        {
            _itemBuyingService = itemBuyingService;
            _icon.sprite = config.MarketItem.Icon;
            _name.text = config.MarketItem.Name;
            _description.text = config.MarketItem.Description;
            _price = config.MarketItem.Price;
            _priceText.text = (_price * _itemBuyingService.GetNextAvailableOrder(config.TypeId)).ToString();
            _buyKitchenItemButton.Initialize(config.TypeId);
            _defaultNameColor = _name.color;

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
            _priceText.text = (_itemBuyingService.GetNextAvailableOrder(_kitchenItemTypeId) * _price).ToString();
            _icon.material = _grayScaleMaterial;
            _currencyImage.material = _grayScaleMaterial;
            _name.color = Color.white;
        }

        public float GetHeight()
        {
            float height = _rectTransform.rect.height;
            return height;
        }

        public void UpdateAvailableCount(KitchenItemTypeId kitchenItemTypeId)
        {
            _kitchenItemTypeId = kitchenItemTypeId;

            if (_itemBuyingService.GetAvailableItemCount(_kitchenItemTypeId, out int count))
            {
                _available.text = $"Available: {count}";
                _price = _itemBuyingService.GetNextAvailableOrder(kitchenItemTypeId) * _price;
                _priceText.text = _price.ToString();
            }
            else
                MakeLock();
        }
    }
}
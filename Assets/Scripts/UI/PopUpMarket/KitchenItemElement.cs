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
        [SerializeField] private Text _name;
        [SerializeField] private Text _description;
        [SerializeField] private Text _price;
        [SerializeField] private BuyKitchenItemButton _buyKitchenItemButton;
        [SerializeField] private Text _available;

        public void Initialize(KitchenItemConfig config)
        {
            _icon.sprite = config.MarketItem.Icon;
            _name.text = config.MarketItem.Name;
            _description.text = config.MarketItem.Description;
            _price.text = config.MarketItem.Price.ToString();
            _buyKitchenItemButton.Initialize(config.TypeId);
            // _available.text = $"Available: {}";
        }

        public void MakeActive()
        {
            _buyKitchenItemButton.Active();
        }

        public void MakeLock()
        {
            _buyKitchenItemButton.Lock();
        }

        public float GetHeight()
        {
            float height = _rectTransform.rect.height;
            return height;
        }
    }
}

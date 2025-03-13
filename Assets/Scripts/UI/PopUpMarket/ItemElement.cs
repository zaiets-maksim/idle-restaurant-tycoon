using Services.ItemBuyingService;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PopUpMarket
{
    public class ItemElement : MonoBehaviour
    {
        [SerializeField] protected RectTransform _rectTransform;
        [SerializeField] protected Image _icon;
        [SerializeField] protected Image _currencyImage;
        [SerializeField] protected Text _name;
        [SerializeField] protected Text _description;
        [SerializeField] protected Text _priceText;
        [SerializeField] protected Text _available;
        [SerializeField] protected Material _grayScaleMaterial;
        
        protected IItemBuyingService _itemBuyingService;
        
        protected Color _defaultNameColor;
        protected int _defaultPrice;
        protected int _actualPrice;

        public int ActualPrice => _actualPrice;

        protected void UpdatePrice(int price)
        {
            _actualPrice = price;
            _priceText.text = price.ToString();
        }

        public float GetHeight() => _rectTransform.rect.height;
    }
}

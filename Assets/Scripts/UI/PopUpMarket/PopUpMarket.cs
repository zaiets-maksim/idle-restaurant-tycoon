using System.Collections.Generic;
using System.Linq;
using Extensions;
using Infrastructure;
using Services.Factories.UIFactory;
using Services.ItemBuyingService;
using Services.StaticDataService;
using StaticData;
using StaticData.Levels;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PopUpMarket
{
    public class PopUpMarket : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectScrollView;
        [SerializeField] private RectTransform _content;
        [SerializeField] private VerticalLayoutGroup _verticalLayoutGroup;
    
        private readonly IUIFactory _uiFactory;
        private readonly IItemBuyingService _itemBuyingService;
        private readonly IStaticDataService _staticData;
    
        private List<KitchenData> _availableKitchenItems;
        private List<KitchenItemElement> _elements = new();
        private ShowMarketButton _showMarketButton;

        public PopUpMarket()
        {
            _uiFactory = ProjectContext.Instance?.UIFactory;
            _itemBuyingService = ProjectContext.Instance?.ItemBuyingService;
            _staticData = ProjectContext.Instance?.StaticData;
        }

        private void Start()
        {
            Fill();
            _rectScrollView.sizeDelta = new Vector2(_rectScrollView.sizeDelta.x, -250f);
        }

        public void RemoveAvailableKitchenItem(KitchenItemTypeId typeId, int purchaseOrder)
        {
            var itemToRemove = _availableKitchenItems
                .FirstOrDefault(item => item.TypeId == typeId && item.PurchaseOrder == purchaseOrder);

            if (itemToRemove != null)
                _availableKitchenItems.Remove(itemToRemove);
        }

        private void Fill()
        {
            _availableKitchenItems = _itemBuyingService.GetAvailableKitchenItemsForPurchase();
        
            foreach (var availableKitchenItem in _availableKitchenItems)
            {
                var kitchenItemElement = _uiFactory.CreateKitchenItemElement();
                var config = _staticData.ForKitchenItem(availableKitchenItem.TypeId);
            
                kitchenItemElement.Initialize(config);
                _elements.Add(kitchenItemElement);
                kitchenItemElement.transform.SetParent(_content);
                kitchenItemElement.transform.localScale = Vector3.one;
                AddContentHeight(kitchenItemElement.GetHeight() + _verticalLayoutGroup.spacing);
            }
        }

        public void Show()
        {
            _rectScrollView.AnimateOverTime(
                rect => rect.sizeDelta.y,
                (rect, value) => rect.sizeDelta = new Vector2(rect.sizeDelta.x, value),
                1250f,
                0.25f);

            _showMarketButton.gameObject.SetActive(false);
        }

        public void Hide()
        {
            _rectScrollView.AnimateOverTime(
                rect => rect.sizeDelta.y,
                (rect, value) => rect.sizeDelta = new Vector2(rect.sizeDelta.x, value),
                -250f,
                0.25f);
            
            _showMarketButton.gameObject.SetActive(true);
        }

        private void AddContentHeight(float height)
        {
            Vector2 size = _content.sizeDelta;
            size.y += height;
            _content.sizeDelta = size;
        }

        public void InitShowButton(ShowMarketButton showMarketButton) => 
            _showMarketButton = showMarketButton;
    }
}
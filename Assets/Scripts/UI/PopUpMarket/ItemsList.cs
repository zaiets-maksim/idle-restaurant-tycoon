using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using Services.Factories.UIFactory;
using Services.ItemBuyingService;
using Services.StaticDataService;
using StaticData;
using StaticData.Levels;
using StaticData.TypeId;
using UnityEngine;
using Zenject;

namespace UI.PopUpMarket
{
    public class ItemsList : MonoBehaviour
    {
        [SerializeField] private PopUpMarket _popUpMarket;
    
        private IUIFactory _uiFactory;
        private IItemBuyingService _itemBuyingService;
        private IStaticDataService _staticData;
    
        private List<KitchenData> _availableKitchenItems;
        private List<HallData> _availableHallItems;
        private List<KitchenItemElement> _kitchenElements = new();
        private List<HallItemElement> _hallElements = new();

        [Inject]
        public void Constructor(IUIFactory uiFactory, IItemBuyingService itemBuyingService, IStaticDataService staticData)
        {
            _staticData = staticData;
            _itemBuyingService = itemBuyingService;
            _uiFactory = uiFactory;
        }

        public void Fill()
        {
            _availableKitchenItems = _itemBuyingService.GetAvailableKitchenItemsForPurchase();
            _availableHallItems = _itemBuyingService.GetAvailableHallItemsForPurchase();
        
            foreach (KitchenItemTypeId typeId in Enum.GetValues(typeof(KitchenItemTypeId)))
            {
                if(typeId == KitchenItemTypeId.Unknown)
                    continue;
                
                var kitchenItemElement = _uiFactory.CreateKitchenItemElement();
                var config = _staticData.ForKitchenItem(typeId);
                
                kitchenItemElement.Initialize(config, _itemBuyingService);
                _kitchenElements.Add(kitchenItemElement);
                kitchenItemElement.transform.SetParent(_popUpMarket.Content);
                kitchenItemElement.transform.localScale = Vector3.one;
                _popUpMarket.AddContentHeight(kitchenItemElement.GetHeight() + _popUpMarket.VerticalLayoutGroup.spacing);
            }
            
            foreach (HallItemTypeId typeId in Enum.GetValues(typeof(HallItemTypeId)))
            {
                if(typeId == HallItemTypeId.Unknown)
                    continue;
                
                var hallItemElement = _uiFactory.CreateHallItemElement();
                var config = _staticData.ForHallItem(typeId);
                
                hallItemElement.Initialize(config, _itemBuyingService);
                _hallElements.Add(hallItemElement);
                hallItemElement.transform.SetParent(_popUpMarket.Content);
                hallItemElement.transform.localScale = Vector3.one;
                _popUpMarket.AddContentHeight(hallItemElement.GetHeight() + _popUpMarket.VerticalLayoutGroup.spacing);
            }
        }
    
        public void RemoveAvailableKitchenItem(KitchenItemTypeId typeId, int purchaseOrder)
        {
            var itemToRemove = _availableKitchenItems
                .FirstOrDefault(item => item.TypeId == typeId && item.PurchaseOrder == purchaseOrder);

            if (itemToRemove != null)
                _availableKitchenItems.Remove(itemToRemove);
        }
    }
}

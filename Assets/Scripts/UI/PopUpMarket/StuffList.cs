using System;
using System.Collections.Generic;
using Infrastructure;
using Services.Factories.UIFactory;
using Services.ItemBuyingService;
using Services.StaticDataService;
using StaticData.Levels;
using StaticData.TypeId;
using UnityEngine;
using Zenject;

namespace UI.PopUpMarket
{
    public class StuffList : MonoBehaviour
    {
        [SerializeField] private PopUpMarket _popUpMarket;

        private IUIFactory _uiFactory;
        private IItemBuyingService _itemBuyingService;
        private IStaticDataService _staticData;
        
        private List<CharacterData> _availableStuffData;

        [Inject]
        public void Constructor(IUIFactory uiFactory, IItemBuyingService itemBuyingService, IStaticDataService staticData)
        {
            _staticData = staticData;
            _itemBuyingService = itemBuyingService;
            _uiFactory = uiFactory;
        }

        public void Fill()
        {
            foreach (CharacterTypeId typeId in Enum.GetValues(typeof(CharacterTypeId)))
            {
                if (typeId == CharacterTypeId.Unknown || typeId == CharacterTypeId.Customer)
                    continue;

                var stuffElement = _uiFactory.CreateStuffElement();
                var config = _staticData.ForCharacter(typeId);

                stuffElement.Initialize(config, _itemBuyingService);

                stuffElement.transform.SetParent(_popUpMarket.Content);
                stuffElement.transform.localScale = Vector3.one;
                _popUpMarket.AddContentHeight(stuffElement.GetHeight() + _popUpMarket.VerticalLayoutGroup.spacing);
            }
        }
    }
}
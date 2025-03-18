using System;
using System.Collections.Generic;
using Infrastructure;
using Services.Factories.UIFactory;
using Services.ItemBuyingService;
using Services.StaticDataService;
using StaticData.Levels;
using StaticData.TypeId;
using UnityEngine;

namespace UI.PopUpMarket
{
    public class StuffList : MonoBehaviour
    {
        [SerializeField] private PopUpMarket _popUpMarket;

        private readonly IUIFactory _uiFactory;
        private readonly IItemBuyingService _itemBuyingService;
        private readonly IStaticDataService _staticData;

        private List<CharacterData> _availableStuffData;

        public StuffList()
        {
            _uiFactory = ProjectContext.Instance?.UIFactory;
            _itemBuyingService = ProjectContext.Instance?.ItemBuyingService;
            _staticData = ProjectContext.Instance?.StaticData;
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
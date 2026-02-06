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

        private IUIFactory _uiFactory => ProjectContext.Get<IUIFactory>();
        private IItemBuyingService _itemBuyingService => ProjectContext.Get<IItemBuyingService>();
        private IStaticDataService _staticData => ProjectContext.Get<IStaticDataService>();

        private List<CharacterData> _availableStuffData;

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
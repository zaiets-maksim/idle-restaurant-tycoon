using StaticData.Configs;
using UI;
using UI.PopUpMarket;
using UnityEngine;

namespace Services.Factories.UIFactory
{
    public interface IUIFactory
    {
        RectTransform CreateWindow(WindowTypeId windowTypeId);
        void CreateUiRoot();
        void CreateHud();
        PopUpMarket CreatePopUpMarket();
        KitchenItemElement CreateKitchenItemElement();
        HallItemElement CreateHallItemElement();

        PopUpMarket PopUpMarket { get; }
        KitchenItemElement KitchenItemElement { get; }
    }
}
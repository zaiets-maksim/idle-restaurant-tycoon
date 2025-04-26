using Connect4.Scripts.Services.Factories;
using Services.StaticDataService;
using StaticData.Configs;
using UI.PopUpMarket;
using UnityEngine;
using Zenject;

namespace Services.Factories.UIFactory
{
    public class UIFactory : Factory, IUIFactory
    {
        private const string UiRootPath = "Prefabs/UI/UiRoot";
        private const string HudPath = "Prefabs/UI/Hud";
        private const string PopUpMarketPath = "Prefabs/UI/PopUpMarket/PopUpMarket";
        private const string KitchenItemElementPath = "Prefabs/UI/PopUpMarket/KitchenItemElement";
        private const string HallItemElementPath = "Prefabs/UI/PopUpMarket/HallItemElement";
        private const string UpgradeElementPath = "Prefabs/UI/PopUpMarket/UpgradeElement";
        private const string StuffElementPath = "Prefabs/UI/PopUpMarket/StuffElement";

        private readonly IStaticDataService _staticData;
        
        private Transform _uiRoot;
        
        public PopUpMarket PopUpMarket { get; private set; }
        public KitchenItemElement KitchenItemElement { get; private set; }
        public HallItemElement HallItemElement { get; private set; }
        public UpgradeElement UpgradeElement { get; private set; }
        public StuffElement StuffElement { get; private set; }

        public UIFactory(IInstantiator instantiator, IStaticDataService staticDataService) : base(instantiator)
        {
            _instantiator = instantiator;
            _staticData = staticDataService;
        }

        public void CreateUiRoot() => _uiRoot = InstantiateOnActiveScene(UiRootPath).transform;

        public void CreateHud() => InstantiateOnActiveScene(HudPath);

        public PopUpMarket CreatePopUpMarket()
        {
            PopUpMarket = InstantiateOnActiveScene(PopUpMarketPath).GetComponent<PopUpMarket>();
            return PopUpMarket;
        }
        
        public StuffElement CreateStuffElement()
        {
            StuffElement = InstantiateOnActiveScene(StuffElementPath).GetComponent<StuffElement>();
            return StuffElement;
        }
        
        public UpgradeElement CreateUpgradeElement()
        {
            UpgradeElement = InstantiateOnActiveScene(UpgradeElementPath).GetComponent<UpgradeElement>();
            return UpgradeElement;
        }

        public KitchenItemElement CreateKitchenItemElement()
        {
            KitchenItemElement = InstantiateOnActiveScene(KitchenItemElementPath).GetComponent<KitchenItemElement>();
            return KitchenItemElement;
        }
        
        public HallItemElement CreateHallItemElement()
        {
            HallItemElement = InstantiateOnActiveScene(HallItemElementPath).GetComponent<HallItemElement>();
            return HallItemElement;
        }

        public RectTransform CreateWindow(WindowTypeId windowTypeId)
        {
            WindowConfig config = _staticData.ForWindow(windowTypeId);
            var window = InstantiatePrefab(config.Prefab, _uiRoot).GetComponent<RectTransform>();
            
            return window;
        }
    }
}
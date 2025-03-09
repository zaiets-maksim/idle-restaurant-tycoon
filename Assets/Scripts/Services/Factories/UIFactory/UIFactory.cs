using StaticData.Configs;
using UnityEngine;

namespace Services.Factories.UIFactory
{
    public class UIFactory : Factory, IUIFactory
    {
        private const string UiRootPath = "UI/UiRoot";
        private const string HudPath = "UI/Hud";

        private readonly IStaticDataService _staticData;

        private Transform _uiRoot;

        public UIFactory(IStaticDataService staticDataService)
        {
            _staticData = staticDataService;
        }

        public void CreateUiRoot() => _uiRoot = InstantiateOnActiveScene(UiRootPath).transform;
        

        public void CreateHud() => InstantiateOnActiveScene(HudPath);

        public RectTransform CreateWindow(WindowTypeId windowTypeId)
        {
            WindowConfig config = _staticData.ForWindow(windowTypeId);
            var window = Object.Instantiate(config.Prefab, _uiRoot).GetComponent<RectTransform>();
            
            return window;
        }
    }
}
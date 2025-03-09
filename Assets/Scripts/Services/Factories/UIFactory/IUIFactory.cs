using StaticData.Configs;
using UnityEngine;

namespace Services.Factories.UIFactory
{
    public interface IUIFactory
    {
        RectTransform CreateWindow(WindowTypeId windowTypeId);
        void CreateUiRoot();
        void CreateHud();
    }
}
using Services.Factories.UIFactory;
using StaticData.Configs;
using UnityEngine;

namespace Services.WindowService
{
    public class WindowService : IWindowService
    {
        private readonly IUIFactory _uiFactory;

        private RectTransform _currentWindow;
        private RectTransform _popUpCurrentWindow;

        public WindowService(IUIFactory uiFactory) =>
            _uiFactory = uiFactory;


        public void Open(WindowTypeId windowTypeId)
        {
            if (_currentWindow != null)
                Object.Destroy(_currentWindow.gameObject);

            _uiFactory.CreateUiRoot();
            _currentWindow = _uiFactory.CreateWindow(windowTypeId);
        }

        public void Close()
        {
            if (_currentWindow != null)
                Object.Destroy(_currentWindow.gameObject);
        }
    }

    public interface IWindowService
    {
        void Open(WindowTypeId windowTypeId);
        void Close();
    }
}
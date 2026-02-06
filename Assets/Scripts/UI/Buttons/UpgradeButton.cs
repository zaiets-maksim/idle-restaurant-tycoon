using System;
using Infrastructure;
using Services.CurrencyService;
using Services.DataStorageService;
using Services.SaveLoad;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class UpgradeButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _buttonImage;

        private ICurrencyService _currencyService => ProjectContext.Get<ICurrencyService>();
        private IPersistenceProgressService _progress => ProjectContext.Get<IPersistenceProgressService>();
        private ISaveLoadService _saveLoad => ProjectContext.Get<ISaveLoadService>();
        
        private Upgrade _upgrade;
        private Material _grayScaleMaterial;
        private PopUpMarket.PopUpMarket _popUpMarket;
        
        private void Start()
        {
            _button.onClick.AddListener(Upgrade);
        }

        public void Initialize(Upgrade upgrade, PopUpMarket.PopUpMarket popUpMarket)
        {
            _upgrade = upgrade;
            _popUpMarket = popUpMarket;
        }

        public void Active()
        {
            _button.interactable = true;
            _buttonImage.material = null;
        }

        public void Lock()
        {
            _button.interactable = false;
            _buttonImage.material = _grayScaleMaterial;
        }

        private void Upgrade()
        {
            if (_currencyService.CanAffordWithMoney(_upgrade.Prices[0]))
            {
                _currencyService.RemoveMoney(_upgrade.Prices[0]);
                _upgrade.Action?.Invoke();
                _progress.PlayerData.ProgressData.BuyUpgrade(_upgrade);
                _saveLoad.SaveProgress();
                
                _popUpMarket.ShowUpgrades();
            }
            else
            {
                
            }
        }
    }
}
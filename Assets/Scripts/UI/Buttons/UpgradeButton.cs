using System;
using Infrastructure;
using Services.CurrencyService;
using Services.DataStorageService;
using Services.SaveLoad;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Buttons
{
    public class UpgradeButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _buttonImage;

        private ICurrencyService _currencyService;
        private IPersistenceProgressService _progress;
        private ISaveLoadService _saveLoad;
        
        private Upgrade _upgrade;
        private Material _grayScaleMaterial;
        private PopUpMarket.PopUpMarket _popUpMarket;

        [Inject]
        public void Constructor(ICurrencyService currencyService, IPersistenceProgressService progress, ISaveLoadService saveLoad)
        {
            _saveLoad = saveLoad;
            _progress = progress;
            _currencyService = currencyService;
        }
        
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
using Services.DataStorageService;
using UI.Buttons;
using UnityEngine;

namespace UI.PopUpMarket
{
    public class UpgradeElement : ItemElement
    {
        [SerializeField] private UpgradeButton _upgradeButton;
        
        private Color _defaultNameColor;

        public void Initialize(Upgrade upgrade, PopUpMarket popUpMarket)
        {
            _defaultNameColor = _name.color;
            _name.text = upgrade.UpgradeType;
            _description.text = upgrade.Description;
            _upgradeButton.Initialize(upgrade, popUpMarket);

            if (upgrade.Prices.Count == 0)
            {
                MakeLock();
                return;
            }

            _priceText.text = upgrade.Prices[0].ToString();
        }
    
        public void MakeActive()
        {
            _upgradeButton.Active();
            _currencyImage.material = null;
            _name.color = _defaultNameColor;
        }

        public void MakeLock()
        {
            _upgradeButton.Lock();
            _priceText.text = "0";
            _currencyImage.material = _grayScaleMaterial;
            _name.color = Color.white;
        }
    }
}

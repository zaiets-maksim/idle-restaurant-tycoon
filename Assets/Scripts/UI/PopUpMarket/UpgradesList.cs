using System.Collections.Generic;
using Services.DataStorageService;
using Services.Factories.UIFactory;
using UI.PopUpMarket;
using UnityEngine;
using Zenject;

public class UpgradesList : MonoBehaviour
{
    [SerializeField] private PopUpMarket _popUpMarket;
    
    private IPersistenceProgressService _progress;
    private IUIFactory _uiFactory;
    private List<Upgrade> _availableUpgrades;


    [Inject]
    public void Constructor(IUIFactory uiFactory, IPersistenceProgressService progress)
    {
        _uiFactory = uiFactory;
        _progress = progress;
    }
    
    public void Fill()
    {
        _availableUpgrades = _progress.PlayerData.ProgressData.Upgrades;
        
        foreach (var upgrade in _availableUpgrades)
        {
            var upgradeElement = _uiFactory.CreateUpgradeElement();
            upgradeElement.Initialize(upgrade, _popUpMarket);
            upgradeElement.transform.SetParent(_popUpMarket.Content);
            upgradeElement.transform.localScale = Vector3.one;
            _popUpMarket.AddContentHeight(upgradeElement.GetHeight() + _popUpMarket.VerticalLayoutGroup.spacing);
        }
    }
}

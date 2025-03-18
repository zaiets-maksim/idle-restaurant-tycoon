using System.Collections.Generic;
using Infrastructure;
using Services.DataStorageService;
using Services.Factories.UIFactory;
using UI.PopUpMarket;
using UnityEngine;

public class UpgradesList : MonoBehaviour
{
    [SerializeField] private PopUpMarket _popUpMarket;
    
    private readonly IPersistenceProgressService _progress;
    private readonly IUIFactory _uiFactory;
    private List<Upgrade> _availableUpgrades;

    public UpgradesList()
    {
        _uiFactory = ProjectContext.Instance?.UIFactory;
        _progress = ProjectContext.Instance?.Progress;
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

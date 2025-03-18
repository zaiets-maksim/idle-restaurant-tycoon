using System.Collections;
using System.Collections.Generic;
using UI.PopUpMarket;
using UnityEngine;
using UnityEngine.UI;

public class OpenUpgradesButton : MonoBehaviour
{
    [SerializeField] private PopUpMarket _popUpMarket;
    [SerializeField] private Button _button;

    private void Start()
    {
        _button.onClick.AddListener(Show);
    }

    private void Show() => _popUpMarket.ShowUpgrades();
}

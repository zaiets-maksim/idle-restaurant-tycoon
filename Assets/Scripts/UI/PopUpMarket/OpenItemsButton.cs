using UI.PopUpMarket;
using UnityEngine;
using UnityEngine.UI;

public class OpenItemsButton : MonoBehaviour
{
    [SerializeField] private PopUpMarket _popUpMarket;
    [SerializeField] private Button _button;

    private void Start()
    {
        _button.onClick.AddListener(Show);
    }

    private void Show() => _popUpMarket.ShowItems();
}

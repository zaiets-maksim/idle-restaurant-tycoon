using Infrastructure;
using Services.DataStorageService;
using Services.ItemBuyingService;
using StaticData;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class ButKitchenItem : MonoBehaviour
    {
        [SerializeField] private Button _button;
        
        private readonly IPersistenceProgressService _progress;
        private readonly IItemBuyingService _itemBuyingService;

        private ButKitchenItem()
        {
            _progress = ProjectContext.Instance?.Progress;
            _itemBuyingService = ProjectContext.Instance?.ItemBuyingService;
        }

        private void Start()
        {
            _button.onClick.AddListener(BuySomething);
        }

        private void BuySomething() => _itemBuyingService.BuyKitchenItem(KitchenItemTypeId.DishStation);
    }
}

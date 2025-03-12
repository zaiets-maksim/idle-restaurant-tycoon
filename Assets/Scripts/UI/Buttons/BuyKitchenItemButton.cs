using Infrastructure;
using Services.ItemBuyingService;
using StaticData;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class BuyKitchenItemButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private KitchenItemTypeId _typeId;
        
        private readonly IItemBuyingService _itemBuyingService;

        private BuyKitchenItemButton()
        {
            _itemBuyingService = ProjectContext.Instance?.ItemBuyingService;
        }

        private void Start()
        {
            _button.onClick.AddListener(BuyKitchenItem);
        }

        public void Initialize(KitchenItemTypeId typeId)
        {
            _typeId = typeId;
        }
        
        public void Active()
        {
            _button.interactable = true;
        }
        
        public void Lock()
        {
            _button.interactable = false;
        }

        private void BuyKitchenItem() => _itemBuyingService.BuyKitchenItem(_typeId);
    }
}

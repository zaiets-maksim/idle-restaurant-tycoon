using Infrastructure;
using Services.ItemBuyingService;
using StaticData;
using UI.PopUpMarket;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class BuyKitchenItemButton : MonoBehaviour
    {
        [SerializeField] private KitchenItemElement _kitchenItemElement;
        [SerializeField] private Button _button;
        [SerializeField] private KitchenItemTypeId _typeId;
        [SerializeField] private Image _buttonImage;
        [SerializeField] private Material _grayScaleMaterial;
        
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
            _buttonImage.material = null;
        }
        
        public void Lock()
        {
            _button.interactable = false;
            _buttonImage.material = _grayScaleMaterial;
        }

        private void BuyKitchenItem()
        {
            _itemBuyingService.BuyKitchenItem(_typeId);
            _kitchenItemElement.UpdateAvailableCount(_typeId);
        }
    }
}

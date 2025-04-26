using DG.Tweening;
using Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PopUpMarket
{
    public class PopUpMarket : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectScrollView;
        [SerializeField] private RectTransform _content;
        [SerializeField] private VerticalLayoutGroup _verticalLayoutGroup;
        [SerializeField] private Transform _bottom;

        [SerializeField] private ItemsList _itemsList;
        [SerializeField] private UpgradesList _upgradesList;
        [SerializeField] private StuffList _stuffList;
        
        
        private ShowMarketButton _showMarketButton;
        public RectTransform Content => _content;
        public VerticalLayoutGroup VerticalLayoutGroup => _verticalLayoutGroup;

        private void Start()
        {
            _itemsList.Fill();
            
            _rectScrollView.sizeDelta = new Vector2(_rectScrollView.sizeDelta.x, -250f);
            _bottom.position = new Vector3(_bottom.position.x, -125f, _bottom.position.z);
        }

        public void ShowItems()
        {
            ClearContent();
            _itemsList.Fill();
        }

        public void ShowUpgrades()
        {
            ClearContent();
            _upgradesList.Fill();
        }

        public void ShowStuff()
        {
            ClearContent();
            _stuffList.Fill();
        }

        private void ClearContent()
        {
            _content.sizeDelta = new Vector2(_content.sizeDelta.x, 0f);
            foreach (Transform child in _content.transform) 
                Destroy(child.gameObject);
        }

        public void Show()
        {
            _rectScrollView.DOSizeDelta(new Vector2(_rectScrollView.sizeDelta.x, 1250f), 0.15f);
            _rectScrollView.DOAnchorPosY(125f, 0.15f);
            _bottom.DOMoveY(0f, 0.15f);

            _showMarketButton.gameObject.SetActive(false);
        }

        public void Hide()
        {
            _rectScrollView.DOSizeDelta(new Vector2(_rectScrollView.sizeDelta.x, -250f), 0.15f);
            _rectScrollView.anchoredPosition = new Vector2(_rectScrollView.anchoredPosition.x, 0f);
            _bottom.DOMoveY(-125f, 0.15f);
            
            _showMarketButton.gameObject.SetActive(true);
        }

        public void AddContentHeight(float height)
        {
            Vector2 size = _content.sizeDelta;
            size.y += height;
            _content.sizeDelta = size;
        }

        public void InitShowButton(ShowMarketButton showMarketButton) => 
            _showMarketButton = showMarketButton;
    }
}
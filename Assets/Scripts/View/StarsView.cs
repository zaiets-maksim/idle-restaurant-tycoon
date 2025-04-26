using DG.Tweening;
using Extensions;
using Infrastructure;
using Services.CurrencyService;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace View
{
    public class StarsView : MonoBehaviour
    {
        [SerializeField] private Text _text;
    
        private ICurrencyService _currencyService;

        [Inject]
        public void Constructor(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }
        
        private void Start()
        {
            UpdateText(_currencyService.Stars);
        }

        private void OnEnable()
        {
            _currencyService.OnStarsChanged += UpdateText;
        }

        private void OnDisable()
        {
            _currencyService.OnStarsChanged -= UpdateText;
        }

        private void UpdateText(int amount)
        {
            transform.DOKill();
            transform.localScale = Vector3.one;
            transform.DOScale(Vector3.one * 1.1f, 0.1f)
                .SetLoops(2, LoopType.Yoyo);
            
            _text.text = amount.ToString();
        }
    }
}

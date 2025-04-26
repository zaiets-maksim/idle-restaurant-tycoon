using System;
using DG.Tweening;
using Extensions;
using Infrastructure;
using Services.CurrencyService;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace View
{
    public class MoneyView : MonoBehaviour
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
            UpdateText(_currencyService.Money);
        }

        private void OnEnable()
        {
            _currencyService.OnMoneyChanged += UpdateText;
        }

        private void OnDisable()
        {
            _currencyService.OnMoneyChanged -= UpdateText;
        }

        private void UpdateText(int amount)
        {
            transform.DOKill();
            transform.localScale = Vector3.one;
            transform.DOScale(Vector3.one * 1.1f, 0.1f)
                .SetLoops(2, LoopType.Yoyo);
            
            _text.text = amount >= 1000
                ? $"{(amount >= 10000 ? amount / 1000 : (amount / 1000f)):0.#}K"
                : amount.ToString();
        }
    }
}

using System;
using Extensions;
using Infrastructure;
using Services.CurrencyService;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class MoneyView : MonoBehaviour
    {
        [SerializeField] private Text _text;
    
        private readonly ICurrencyService _currencyService;
    
        public MoneyView()
        {
            _currencyService = ProjectContext.Instance?.CurrencyService;
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
            transform.AnimatePingPong(t => t.localScale, 
                (t, value) => t.localScale = value, 
                Vector3.one * 1.1f, 
                0.1f
            );
            
            _text.text = amount.ToString();
        }
    }
}
